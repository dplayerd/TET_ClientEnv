using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_Evaluation.Models.Exporting
{
    /// <summary> 匯出資料用 Model </summary>
    public class SPA_EvaluationExportModel
    {
        #region 原生欄位
        /// <summary> 系統辨識碼 </summary>
        public Guid ID { get; set; }

        /// <summary> 評鑑期間 </summary>
        public string Period { get; set; }

        /// <summary> 評鑑單位 </summary>
        public string BU { get; set; }

        /// <summary> 評鑑項目 </summary>
        public string ServiceItem { get; set; }

        /// <summary> 服務對象 </summary>
        public string ServiceFor { get; set; }

        /// <summary> 受評供應商 </summary>
        public string BelongTo { get; set; }

        /// <summary> PO Source </summary>
        public string POSource { get; set; }

        /// <summary> Performance Level </summary>
        public string PerformanceLevel { get; set; }

        /// <summary> Total Score </summary>
        public string TotalScore { get; set; }

        /// <summary> Technology Score </summary>
        public string TScore { get; set; }

        /// <summary> Delivery Score </summary>
        public string DScore { get; set; }

        /// <summary> Quality Score </summary>
        public string QScore { get; set; }

        /// <summary> Cost Score </summary>
        public string CScore { get; set; }

        /// <summary> Service Score </summary>
        public string SScore { get; set; }

        /// <summary> TScore1 </summary>
        public string TScore1 { get; set; }

        /// <summary> TScore2 </summary>
        public string TScore2 { get; set; }

        /// <summary> DScore1 </summary>
        public string DScore1 { get; set; }

        /// <summary> DScore2 </summary>
        public string DScore2 { get; set; }

        /// <summary> QScore1 </summary>
        public string QScore1 { get; set; }

        /// <summary> QScore2 </summary>
        public string QScore2 { get; set; }

        /// <summary> CScore1 </summary>
        public string CScore1 { get; set; }

        /// <summary> CScore2 </summary>
        public string CScore2 { get; set; }

        /// <summary> SScore1 </summary>
        public string SScore1 { get; set; }

        /// <summary> SScore2 </summary>
        public string SScore2 { get; set; }
        #endregion


        #region Program
        /// <summary> 評鑑期間 (起始) </summary>
        public string PeriodStart { get; set; }

        /// <summary> 評鑑期間 (結束) </summary>
        public string PeriodEnd { get; set; } 
        #endregion
    }
}
