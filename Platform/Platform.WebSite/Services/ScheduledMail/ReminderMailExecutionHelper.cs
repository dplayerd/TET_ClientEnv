using Platform.ORM;
using System;
using System.Linq;

namespace Platform.WebSite.Services.ScheduledMail
{
    /// <summary>
    /// 提醒信排程執行紀錄共用處理。
    /// </summary>
    internal static class ReminderMailExecutionHelper
    {
        /// <summary>
        /// 表示本次排程已完成且待發信清單已寫入成功。
        /// </summary>
        internal const string StatusCompleted = "Completed";

        /// <summary>
        /// 檢查指定提醒類型在指定日期是否已完成，避免 Windows 排程或人工重打造成重複信件。
        /// </summary>
        /// <param name="context">資料庫內容。</param>
        /// <param name="reminderType">提醒信類型。</param>
        /// <param name="executeDate">本次執行日期。</param>
        /// <returns>今日已完成時回傳 true。</returns>
        internal static bool IsCompleted(PlatformContextModel context, string reminderType, DateTime executeDate)
        {
            var date = executeDate.Date;

            return context.MailReminderExecutionLogs.Any(obj =>
                obj.ReminderType == reminderType &&
                obj.ExecuteDate == date &&
                obj.Status == StatusCompleted);
        }

        /// <summary>
        /// 新增完成紀錄。呼叫端會在同一個交易中同時寫入待發信清單與完成紀錄。
        /// </summary>
        /// <param name="context">資料庫內容。</param>
        /// <param name="reminderType">提醒信類型。</param>
        /// <param name="startedAt">排程開始時間。</param>
        /// <param name="finishedAt">排程完成時間。</param>
        /// <param name="mailCount">本次建立信件數。</param>
        /// <param name="message">補充訊息。</param>
        /// <param name="userID">建立人員代號。</param>
        internal static void AddCompletedLog(PlatformContextModel context, string reminderType, DateTime startedAt, DateTime finishedAt, int mailCount, string message, string userID)
        {
            context.MailReminderExecutionLogs.Add(new MailReminderExecutionLog()
            {
                ID = Guid.NewGuid(),
                ReminderType = reminderType,
                ExecuteDate = startedAt.Date,
                StartedAt = startedAt,
                FinishedAt = finishedAt,
                Status = StatusCompleted,
                MailCount = mailCount,
                Message = message,
                CreateUser = userID,
                CreateDate = finishedAt,
            });
        }
    }
}
