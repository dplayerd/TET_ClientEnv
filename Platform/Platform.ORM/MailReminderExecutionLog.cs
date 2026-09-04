namespace Platform.ORM
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary> 提醒信件排程執行紀錄 </summary>
    [Table("MailReminderExecutionLog")]
    public partial class MailReminderExecutionLog
    {
        /// <summary> 系統識別碼 </summary>
        [Key]
        public Guid ID { get; set; }

        /// <summary> 提醒類型 </summary>
        [Required]
        [StringLength(64)]
        public string ReminderType { get; set; }

        /// <summary> 執行日期 </summary>
        [Column(TypeName = "date")]
        public DateTime ExecuteDate { get; set; }

        /// <summary> 開始時間 </summary>
        public DateTime StartedAt { get; set; }

        /// <summary> 完成時間；執行中尚未完成時為 Null </summary>
        public DateTime? FinishedAt { get; set; }

        /// <summary> 執行狀態 </summary>
        [Required]
        [StringLength(16)]
        public string Status { get; set; }

        /// <summary> 本次產生信件數 </summary>
        public int MailCount { get; set; }

        /// <summary> 執行訊息 </summary>
        public string Message { get; set; }

        /// <summary> 建立者 </summary>
        [Required]
        [StringLength(64)]
        public string CreateUser { get; set; }

        /// <summary> 建立時間 </summary>
        public DateTime CreateDate { get; set; }
    }
}
