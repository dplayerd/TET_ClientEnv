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
        /// 表示本次排程已開始執行，尚未確認成功或失敗。
        /// </summary>
        internal const string StatusRunning = "Running";

        /// <summary>
        /// 表示本次排程已完成且待發信清單已寫入成功。
        /// </summary>
        internal const string StatusCompleted = "Completed";

        /// <summary>
        /// 表示本次排程執行失敗，錯誤原因會記錄於 Message。
        /// </summary>
        internal const string StatusFailed = "Failed";

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
        /// 新增執行中紀錄。此紀錄會先寫入資料庫，確保後續失敗時仍可回寫錯誤原因。
        /// </summary>
        /// <param name="context">資料庫內容。</param>
        /// <param name="reminderType">提醒信類型。</param>
        /// <param name="startedAt">排程開始時間。</param>
        /// <param name="userID">建立人員代號。</param>
        /// <returns>執行紀錄。</returns>
        internal static MailReminderExecutionLog AddRunningLog(PlatformContextModel context, string reminderType, DateTime startedAt, string userID)
        {
            var log = new MailReminderExecutionLog()
            {
                ID = Guid.NewGuid(),
                ReminderType = reminderType,
                ExecuteDate = startedAt.Date,
                StartedAt = startedAt,
                Status = StatusRunning,
                MailCount = 0,
                Message = "排程開始執行。",
                CreateUser = userID,
                CreateDate = startedAt,
            };

            context.MailReminderExecutionLogs.Add(log);

            return log;
        }

        /// <summary>
        /// 將執行紀錄更新為完成。呼叫端會在同一個交易中同時寫入待發信清單與完成紀錄。
        /// </summary>
        /// <param name="log">執行紀錄。</param>
        /// <param name="finishedAt">排程完成時間。</param>
        /// <param name="mailCount">本次建立信件數。</param>
        /// <param name="message">補充訊息。</param>
        internal static void MarkCompleted(MailReminderExecutionLog log, DateTime finishedAt, int mailCount, string message)
        {
            log.FinishedAt = finishedAt;
            log.Status = StatusCompleted;
            log.MailCount = mailCount;
            log.Message = message;
        }

        /// <summary>
        /// 將執行紀錄更新為失敗。此方法使用新的資料庫內容呼叫，避免主交易 Rollback 時一併清掉失敗原因。
        /// </summary>
        /// <param name="context">資料庫內容。</param>
        /// <param name="id">執行紀錄識別碼。</param>
        /// <param name="finishedAt">失敗時間。</param>
        /// <param name="mailCount">失敗前已計算出的信件數。</param>
        /// <param name="message">錯誤原因。</param>
        internal static void MarkFailed(PlatformContextModel context, Guid id, DateTime finishedAt, int mailCount, string message)
        {
            var log = context.MailReminderExecutionLogs.Find(id);
            if (log == null)
                return;

            log.FinishedAt = finishedAt;
            log.Status = StatusFailed;
            log.MailCount = mailCount;
            log.Message = message;
        }
    }
}
