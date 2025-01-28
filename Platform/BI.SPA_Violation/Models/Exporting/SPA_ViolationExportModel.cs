using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_Violation.Models.Exporting
{
    public class SPA_ViolationExportModel
    {
        #region 原生欄位

        /// <summary> Period </summary>
        public string Period { get; set; }

        /// <summary> Date </summary>
        public DateTime Date { get; set; }

        /// <summary> BelongTo </summary>
        public string BelongTo { get; set; }

        /// <summary> BU </summary>
        public string BU { get; set; }

        /// <summary> AssessmentItem </summary>
        public string AssessmentItem { get; set; }

        /// <summary> MiddleCategory </summary>
        public string MiddleCategory { get; set; }

        /// <summary> SmallCategory </summary>
        public string SmallCategory { get; set; }

        /// <summary> CustomerName </summary>
        public string CustomerName { get; set; }

        /// <summary> CustomerPlant </summary>
        public string CustomerPlant { get; set; }

        /// <summary> CustomerDetail </summary>
        public string CustomerDetail { get; set; }

        /// <summary> Description </summary>
        public string Description { get; set; }

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
