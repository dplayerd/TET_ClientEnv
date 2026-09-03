using System;

namespace Platform.WebSite.Services.ScheduledMail
{
    /// <summary>
    /// 提醒信排程服務介面。
    /// </summary>
    public interface IReminderMailService
    {
        /// <summary>
        /// 依指定時間產生提醒信，並以批次方式寫入待發信清單。
        /// </summary>
        /// <param name="userID">建立人員代號，排程通常帶入 System。</param>
        /// <param name="cDate">本次排程判斷日期時間。</param>
        /// <returns>提醒信產生結果。</returns>
        ReminderMailGenerateResult Generate(string userID, DateTime cDate);
    }
}
