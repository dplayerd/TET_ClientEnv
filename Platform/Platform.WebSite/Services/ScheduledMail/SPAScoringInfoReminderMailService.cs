using Newtonsoft.Json;
using Platform.Infra;
using Platform.Messages;
using Platform.Messages.Enums;
using Platform.Messages.Models;
using Platform.ORM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Platform.WebSite.Services.ScheduledMail
{
    /// <summary>
    /// 需求十四：SPA評鑑計分資料填寫提醒信件寄送排程。
    /// </summary>
    public class SPAScoringInfoReminderMailService : IReminderMailService
    {
        /// <summary>
        /// 執行紀錄使用的提醒信類型。
        /// </summary>
        private const string ReminderType = "SPAScoringInfoReminder";

        /// <summary>
        /// SPA評鑑單位參數類型。
        /// </summary>
        private const string ParameterTypeBU = "SPA評鑑單位";

        /// <summary>
        /// SPA評鑑項目參數類型。
        /// </summary>
        private const string ParameterTypeServiceItem = "SPA評鑑項目";

        private readonly IMailQueueService _mailQueueService;

        /// <summary>
        /// 建立 SPA 計分資料填寫提醒信服務。
        /// </summary>
        public SPAScoringInfoReminderMailService()
            : this(new MailQueueService())
        {
        }

        /// <summary>
        /// 建立 SPA 計分資料填寫提醒信服務，允許測試或其他模組替換信件佇列服務。
        /// </summary>
        /// <param name="mailQueueService">信件佇列服務。</param>
        public SPAScoringInfoReminderMailService(IMailQueueService mailQueueService)
        {
            this._mailQueueService = mailQueueService;
        }

        /// <summary>
        /// 產生超過 CalInfoRemindDay 仍未送審的 SPA 計分資料提醒信。
        /// </summary>
        /// <param name="userID">建立人員代號。</param>
        /// <param name="cDate">排程執行時間。</param>
        /// <returns>提醒信產生結果。</returns>
        public ReminderMailGenerateResult Generate(string userID, DateTime cDate)
        {
            var result = new ReminderMailGenerateResult() { ReminderType = ReminderType };
            var remindDays = ScheduledMailConfig.ReadPositiveInt("CalInfoRemindDay");
            var cutoffDate = cDate.AddDays(-1 * remindDays);

            using (PlatformContextModel context = new PlatformContextModel())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    // 防止 Edge 或 Windows 排程重複呼叫時，於同一天重複建立同一批提醒信。
                    if (ReminderMailExecutionHelper.IsCompleted(context, ReminderType, cDate))
                    {
                        result.IsSkipped = true;
                        result.Messages.Add("Today has already completed.");
                        return result;
                    }

                    // 依只提醒主檔已建立、尚未送審且已超過設定天數的 SPA 計分資料。
                    var scoringInfoList =
                        (from item in context.TET_SPA_ScoringInfo
                         where
                            item.ApproveStatus == null &&
                            item.CreateDate <= cutoffDate
                         orderby item.CreateDate
                         select item).ToList();

                    result.SourceCount = scoringInfoList.Count;

                    // ApproverSetup 存放的是參數 ID，需先轉回 BU/評鑑項目文字後才能與計分資料主檔比對。
                    var buMap = context.TET_Parameters
                        .Where(obj => obj.Type == ParameterTypeBU)
                        .ToDictionary(obj => obj.ID, obj => obj.Item);

                    var serviceItemMap = context.TET_Parameters
                        .Where(obj => obj.Type == ParameterTypeServiceItem)
                        .ToDictionary(obj => obj.ID, obj => obj.Item);

                    var setupList = context.TET_SPA_ApproverSetup.ToList();
                    var setupMap = setupList
                        .Select(obj => new
                        {
                            Setup = obj,
                            BUText = buMap.ContainsKey(obj.BUID) ? buMap[obj.BUID] : null,
                            ServiceItemText = serviceItemMap.ContainsKey(obj.ServiceItemID) ? serviceItemMap[obj.ServiceItemID] : null,
                        })
                        .Where(obj => obj.BUText != null && obj.ServiceItemText != null)
                        .GroupBy(obj => $"{obj.BUText}___{obj.ServiceItemText}")
                        .ToDictionary(obj => obj.Key, obj => obj.First().Setup);

                    // 先彙整所有可能的填寫人與確認人，再一次查詢使用者信箱，降低資料庫往返次數。
                    var userIDs = new HashSet<string>();
                    foreach (var setup in setupList)
                    {
                        foreach (var item in ParseUserIDList(setup.InfoFill))
                            userIDs.Add(item);

                        if (!string.IsNullOrWhiteSpace(setup.InfoConfirm))
                            userIDs.Add(setup.InfoConfirm);
                    }

                    var userMap =
                        (from item in context.Users
                         where userIDs.Contains(item.UserID) && item.IsEnabled == "Y"
                         select item).ToDictionary(obj => obj.UserID, obj => obj.EMail);

                    var mails = new List<MailPoolWithCCModel>();

                    foreach (var scoringInfo in scoringInfoList)
                    {
                        var setupKey = $"{scoringInfo.BU}___{scoringInfo.ServiceItem}";
                        if (!setupMap.TryGetValue(setupKey, out TET_SPA_ApproverSetup setup))
                        {
                            result.Messages.Add($"No approver setup for {scoringInfo.BU}/{scoringInfo.ServiceItem}.");
                            continue;
                        }

                        var receivers = ParseUserIDList(setup.InfoFill)
                            .Where(userMap.ContainsKey)
                            .Select(obj => userMap[obj])
                            .Where(obj => !string.IsNullOrWhiteSpace(obj))
                            .Distinct()
                            .ToList();

                        var ccs = new List<string>();
                        if (!string.IsNullOrWhiteSpace(setup.InfoConfirm) &&
                            userMap.TryGetValue(setup.InfoConfirm, out string ccEmail) &&
                            !string.IsNullOrWhiteSpace(ccEmail))
                        {
                            ccs.Add(ccEmail);
                        }

                        if (!receivers.Any())
                        {
                            result.Messages.Add($"No receiver email for scoring info {scoringInfo.ID}.");
                            continue;
                        }

                        mails.Add(new MailPoolWithCCModel()
                        {
                            Receivers = receivers,
                            CCs = ccs,
                            Subject = "您所負責的供應商SPA評鑑計分資料尚未完成送審作業，請撥空進行處理，謝謝。",
                            Body = BuildBody(),
                            Priority = MailPriorityEnum.Default,
                        });
                    }

                    // 收件人或設定資料不完整時整批失敗，避免產生部分提醒信造成資料與執行紀錄不一致。
                    if (result.Messages.Any())
                        throw new InvalidOperationException(string.Join(Environment.NewLine, result.Messages));

                    this._mailQueueService.EnqueueBatch(context, mails, userID, cDate);

                    result.MailCount = mails.Count;
                    // 待發信清單與完成紀錄共用同一個交易；任一段失敗都會整批 Rollback。
                    ReminderMailExecutionHelper.AddCompletedLog(
                        context,
                        ReminderType,
                        cDate,
                        DateTime.Now,
                        result.MailCount,
                        $"SourceCount={result.SourceCount}",
                        userID);

                    context.SaveChanges();
                    transaction.Commit();
                }
            }

            return result;
        }

        /// <summary>
        /// 解析使用者代號清單。InfoFill 目前以 JSON 陣列儲存，保留純文字相容處理。
        /// </summary>
        /// <param name="json">使用者代號 JSON 陣列或單一使用者代號。</param>
        /// <returns>使用者代號清單。</returns>
        private static List<string> ParseUserIDList(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new List<string>();

            try
            {
                return JsonConvert.DeserializeObject<List<string>>(json) ?? new List<string>();
            }
            catch
            {
                return new List<string>() { json };
            }
        }

        /// <summary>
        /// 建立 SPA 計分資料填寫提醒信本文。
        /// </summary>
        /// <returns>HTML 信件本文。</returns>
        private static string BuildBody()
        {
            var pageUrl = $"{GetEmailRootUrl()}/SPA_ScoringInfo/Index";

            return
$@"您好,<br/>
請點選「<a href=""{pageUrl}"" target=""_blank"">供應商SPA評鑑計分資料維護</a>」進行處理，謝謝";
        }

        /// <summary>
        /// 取得系統網址根路徑，用於產生信件中的功能連結。
        /// </summary>
        /// <returns>系統網址根路徑。</returns>
        private static string GetEmailRootUrl()
        {
            var config = EmailConfig.GetDefault();
            if (config != null && !string.IsNullOrWhiteSpace(config.EmailRootUrl))
                return config.EmailRootUrl.TrimEnd('/');

            return ScheduledMailConfig.ReadString("EmailRootUrl").TrimEnd('/');
        }
    }
}
