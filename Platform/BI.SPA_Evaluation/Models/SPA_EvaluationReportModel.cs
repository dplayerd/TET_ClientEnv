using BI.Shared.Utils;
using Platform.AbstractionClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_Evaluation.Models
{
    /// <summary> SPA 績效評鑑報告 </summary>
    public class SPA_EvaluationReportModel
    {
        #region 原生欄位
        /// <summary> 系統辨識碼 </summary>
        public Guid ID { get; set; }

        /// <summary> 評鑑期間 </summary>
        public string Period { get; set; }

        /// <summary> 評鑑單位 </summary>
        public string BU { get; set; }

        /// <summary> 建立人員 </summary>
        public string CreateUser { get; set; }

        /// <summary> 新增時間 </summary>
        public DateTime CreateDate { get; set; }

        /// <summary> 最後更新人員 </summary>
        public string ModifyUser { get; set; }

        /// <summary> 最後更新時間 </summary>
        public DateTime ModifyDate { get; set; }
        #endregion


        #region Other Table
        /// <summary> 附件清單 </summary>
        public List<SPA_EvaluationReportAttachmentModel> AttachmentList { get; set; } = new List<SPA_EvaluationReportAttachmentModel>();
        #endregion


        #region Program
        /// <summary> 目前登入者是否為 QSM </summary>
        public bool IsQSM { get; set; }

        /// <summary> 評鑑期間 </summary>
        private DatePeriod DatePeriod { get { return PeriodUtil.ParsePeriod(this.Period); } }

        /// <summary> 評鑑期間 (起始) </summary>
        public string PeriodStart { get { return this.DatePeriod.StartDate?.ToString("yyyy-MM-dd"); } }

        /// <summary> 評鑑期間 (結束) </summary>
        public string PeriodEnd { get { return this.DatePeriod.EndDate?.ToString("yyyy-MM-dd"); } }

        /// <summary> 是否可編輯 </summary>
        public bool CanEdit { get; set; }
        #endregion


        #region 檔案上傳
        /// <summary> 本次上傳的檔案 </summary>
        public List<FileContent> UploadFiles { get; set; } = new List<FileContent>();
        #endregion
    }
}
