using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA.Models
{
    public class TET_SupplierSPAModel
    {
        #region 原生欄位
        /// <summary> ID </summary>
        public Guid? ID { get; set; }

        /// <summary> BelongTo </summary>
        public string BelongTo { get; set; }

        /// <summary> Period </summary>
        public string Period { get; set; }

        /// <summary> BU </summary>
        public string BU { get; set; }

        /// <summary> ServiceFor </summary>
        public string ServiceFor { get; set; }

        /// <summary> AssessmentItem </summary>
        public string AssessmentItem { get; set; }

        /// <summary> AssessmentItemID </summary>
        public string AssessmentItemID { get; set; }

        /// <summary> PerformanceLevel </summary>
        public string PerformanceLevel { get; set; }

        /// <summary> TotalScore </summary>
        public string TotalScore { get; set; }

        /// <summary> TScore </summary>
        public string TScore { get; set; }

        /// <summary> DScore </summary>
        public string DScore { get; set; }

        /// <summary> QScore </summary>
        public string QScore { get; set; }

        /// <summary> CScore </summary>
        public string CScore { get; set; }

        /// <summary> SScore </summary>
        public string SScore { get; set; }

        /// <summary> Comment </summary>
        public string SPAComment { get; set; }

        /// <summary> ApproveStatus </summary>
        public string ApproveStatus { get; set; }

        /// <summary> CreateUser </summary>
        public string CreateUser { get; set; }

        /// <summary> CreateDate </summary>
        public DateTime CreateDate { get; set; }

        /// <summary> ModifyUser </summary>
        public string ModifyUser { get; set; }

        /// <summary> ModifyDate </summary>
        public DateTime ModifyDate { get; set; }
        #endregion

        #region 外部資料表
        /// <summary> 簽核歷程 </summary>
        public List<TET_SupplierSPAApprovalModel> ApprovalList { get; set; } = new List<TET_SupplierSPAApprovalModel>();
        #endregion
    }
}

