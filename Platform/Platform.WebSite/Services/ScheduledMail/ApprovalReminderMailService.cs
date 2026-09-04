using Platform.Infra;
using Platform.Messages;
using Platform.Messages.Enums;
using Platform.Messages.Models;
using Platform.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Platform.WebSite.Services.ScheduledMail
{
    /// <summary> 審核提醒信件寄送排程 </summary>
    public class ApprovalReminderMailService : IReminderMailService
    {
        /// <summary>
        /// 執行紀錄使用的提醒信類型。
        /// </summary>
        private const string ReminderType = "ApprovalReminder";

        private readonly IMailQueueService _mailQueueService;

        /// <summary>
        /// 建立審核提醒信服務。
        /// </summary>
        public ApprovalReminderMailService()
            : this(new MailQueueService())
        {
        }

        /// <summary>
        /// 建立審核提醒信服務，允許測試或其他模組替換信件佇列服務。
        /// </summary>
        /// <param name="mailQueueService">信件佇列服務。</param>
        public ApprovalReminderMailService(IMailQueueService mailQueueService)
        {
            this._mailQueueService = mailQueueService;
        }

        /// <summary>
        /// 產生超過 ApprovalRemindDays 仍未審核的提醒信。
        /// </summary>
        /// <param name="userID">建立人員代號。</param>
        /// <param name="cDate">排程執行時間。</param>
        /// <returns>提醒信產生結果。</returns>
        public ReminderMailGenerateResult Generate(string userID, DateTime cDate)
        {
            var result = new ReminderMailGenerateResult() { ReminderType = ReminderType };
            Guid? executionLogID = null;

            using (PlatformContextModel context = new PlatformContextModel())
            {
                // 防止 Edge 或 Windows 排程重複呼叫時，於同一天重複建立同一批提醒信。
                if (ReminderMailExecutionHelper.IsCompleted(context, ReminderType, cDate))
                {
                    result.IsSkipped = true;
                    result.Messages.Add("Today has already completed.");
                    return result;
                }

                var executionLog = ReminderMailExecutionHelper.AddRunningLog(context, ReminderType, cDate, userID);
                executionLogID = executionLog.ID;
                context.SaveChanges();

                try
                {
                    var remindDays = ScheduledMailConfig.ReadPositiveInt("ApprovalRemindDays");
                    var cutoffDate = cDate.AddDays(-1 * remindDays);

                    using (var transaction = context.Database.BeginTransaction())
                    {
                        // 只提醒尚未審核且審核開始時間已超過設定天數的待審資料。
                        var approvalList =
                            (from item in context.vwApprovalList
                             where
                                item.Result == null &&
                                item.CreateDate <= cutoffDate
                             orderby item.Approver, item.CreateDate
                             select item).ToList();

                        result.SourceCount = approvalList.Count;

                        var approverList = approvalList.Select(obj => obj.Approver).Distinct().ToList();
                        var userList =
                            (from item in context.Users
                             where approverList.Contains(item.UserID) && item.IsEnabled == "Y"
                             select item).ToList();

                        var userMap = userList.ToDictionary(obj => obj.UserID, obj => obj.EMail);
                        var mails = new List<MailPoolWithCCModel>();

                        foreach (var group in approvalList.GroupBy(obj => obj.Approver))
                        {
                            if (!userMap.TryGetValue(group.Key, out string email) || string.IsNullOrWhiteSpace(email))
                            {
                                result.Messages.Add($"Approver {group.Key} has no email.");
                                continue;
                            }


                            var mailBody = BuildBody(group.ToList(), remindDays);
                            var mailModel = new MailPoolWithCCModel()
                            {
                                Receivers = new List<string>() { email },
                                CCs = new List<string>(),
                                Subject = $"您尚有超過{remindDays}天的簽核尚未完成，請撥空進行簽核，謝謝。",
                                Body = mailBody,
                                Priority = MailPriorityEnum.Default,
                            };

                            mails.Add(mailModel);
                        }

                        // 收件人資料不完整時整批失敗，避免產生部分提醒信造成資料與執行紀錄不一致。
                        if (result.Messages.Any())
                            throw new InvalidOperationException(string.Join(Environment.NewLine, result.Messages));

                        result.MailCount = mails.Count;
                        this._mailQueueService.EnqueueBatch(context, mails, userID, cDate);

                        // 待發信清單與完成紀錄共用同一個交易；任一段失敗都會整批 Rollback。
                        ReminderMailExecutionHelper.MarkCompleted(executionLog, DateTime.Now, result.MailCount, $"SourceCount={result.SourceCount}");

                        context.SaveChanges();
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    WriteFailedLog(executionLogID, result, ex);
                    throw;
                }
            }

            return result;
        }

        /// <summary>
        /// 回寫失敗紀錄。使用獨立 Context，避免主交易 Rollback 時一併清除錯誤原因。
        /// </summary>
        /// <param name="executionLogID">執行紀錄識別碼。</param>
        /// <param name="result">本次產生結果。</param>
        /// <param name="ex">失敗例外。</param>
        private static void WriteFailedLog(Guid? executionLogID, ReminderMailGenerateResult result, Exception ex)
        {
            if (!executionLogID.HasValue)
                return;

            using (PlatformContextModel failContext = new PlatformContextModel())
            {
                ReminderMailExecutionHelper.MarkFailed(
                    failContext,
                    executionLogID.Value,
                    DateTime.Now,
                    result.MailCount,
                    ex.ToString());

                failContext.SaveChanges();
            }
        }

        /// <summary>
        /// 建立審核提醒信本文。
        /// </summary>
        /// <param name="approvalList">同一位審核人的逾期待審清單。</param>
        /// <param name="remindDays">提醒天數。</param>
        /// <returns>HTML 信件本文。</returns>
        private static string BuildBody(List<vwApprovalList> approvalList, int remindDays)
        {
            var dicTypeAndCount =
                approvalList.GroupBy(obj => obj.Type).ToDictionary(obj => obj.Key, obj => obj.Count());

            var pageUrl = $"{GetEmailRootUrl()}/SupplierApproval/Index";
            var rows = string.Join(
                string.Empty,
                dicTypeAndCount.Select(obj =>
                    $@"<tr>
                        <td>{HttpUtility.HtmlEncode(obj.Key)}</td>
                        <td>{HttpUtility.HtmlEncode(obj.Value)}</td>
                    </tr>"));

            return
$@"您好,<br/><br/>
您尚有超過{remindDays}天的簽核尚未完成，請點選「<a href=""{pageUrl}"" target=""_blank"">待審清單</a>」進行簽核，謝謝<br/>
<br/>
<table border=""1"" cellpadding=""6"" cellspacing=""0"">
    <thead>
        <tr>
            <th>未簽核單據</th>
            <th>未簽核筆數</th>
        </tr>
    </thead>
    <tbody>
        {rows}
    </tbody>
</table>
<br/>
<br/>
上述清單表格只會列出超過 {remindDays} 天，尚未簽核的資料
";
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
