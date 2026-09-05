using System.Collections.Generic;

namespace Platform.Messages
{
    /// <summary> 提醒信產生結果，供排程 WebAPI 回傳本次執行狀態 </summary>
    public class ReminderMailGenerateResult
    {
        #region 屬性
        /// <summary> 提醒信類型，用來區分不同排程及防止同日重複執行 </summary>
        public string ReminderType { get; set; }

        /// <summary> 是否因今日已完成而略過執行 </summary>
        public bool IsSkipped { get; set; }

        /// <summary> 本次實際寫入待發信清單的信件筆數 </summary>
        public int MailCount { get; set; }

        /// <summary> 本次符合提醒條件的來源資料筆數 </summary>
        public int SourceCount { get; set; }

        /// <summary> 本次執行過程中的補充訊息或錯誤原因 </summary>
        public List<string> Messages { get; set; } = new List<string>();
        #endregion
    }
}
