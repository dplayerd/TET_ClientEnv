using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ScoringInfo.Models.Exporting
{
    /// <summary> 匯出資料用 Model </summary>
    public class SPA_CostServiceExportModel
    {
        #region 原生欄位
        /// <summary> Period </summary>
        public string Period { get; set; }

        /// <summary> Filler </summary>
        public string Filler { get; set; }

        /// <summary> Source </summary>
        public string Source { get; set; }

        /// <summary> IsEvaluate </summary>
        public string IsEvaluate { get; set; }

        /// <summary> BU </summary>
        public string BU { get; set; }

        /// <summary> ServiceFor </summary>
        public string ServiceFor { get; set; }

        /// <summary> BelongTo </summary>
        public string BelongTo { get; set; }

        /// <summary> POSource </summary>
        public string POSource { get; set; }

        /// <summary> AssessmentItem </summary>
        public string AssessmentItem { get; set; }

        /// <summary> PriceDeflator </summary>
        public string PriceDeflator { get; set; }

        /// <summary> PaymentTerm </summary>
        public string PaymentTerm { get; set; }

        /// <summary> Cooperation </summary>
        public string Cooperation { get; set; }

        /// <summary> Advantage </summary>
        public string Advantage { get; set; }

        /// <summary> Improved </summary>
        public string Improved { get; set; }

        /// <summary> Comment </summary>
        public string Comment { get; set; }

        /// <summary> Remark </summary>
        public string Remark { get; set; }
        #endregion


        #region Program
        /// <summary> 填寫入全名 </summary>
        public string FillerFullName { get; set; }

        /// <summary> 評鑑期間 (起始) </summary>
        public string PeriodStart { get; set; }

        /// <summary> 評鑑期間 (結束) </summary>
        public string PeriodEnd { get; set; } 
        #endregion
    }
}
