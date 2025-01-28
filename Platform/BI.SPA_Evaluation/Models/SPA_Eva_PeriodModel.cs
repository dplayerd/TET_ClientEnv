using BI.Shared.Utils;
using BI.SPA_ScoringInfo.Models;
using BI.SPA_Violation.Models;
using Platform.AbstractionClass;
using Platform.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using BI.SPA_Evaluation.Enums;
using BI.SPA_CostService.Models;

namespace BI.SPA_Evaluation.Models
{
    /// <summary> 組合 SPA_PeriodModel 、 SPA_ScoringInfo 、 SPA_Violation </summary>
    public class SPA_Eva_PeriodModel
    {
        #region 原生欄位
        /// <summary> ID </summary>
        public Guid? ID { get; set; }

        /// <summary> 評鑑期間 </summary>
        public string Period { get; set; }

        /// <summary> 狀態 </summary>
        public string Status { get; set; }

        /// <summary> CreateUser </summary>
        public string CreateUser { get; set; }

        /// <summary> CreateDate </summary>
        public DateTime CreateDate { get; set; }

        /// <summary> ModifyUser </summary>
        public string ModifyUser { get; set; }

        /// <summary> ModifyDate </summary>
        public DateTime ModifyDate { get; set; }
        #endregion


        #region  OtherTable
        /// <summary> 該評鑑期間中的 SPA評鑑計分資料主檔 清單 </summary>
        public List<SPA_ScoringInfoModel> ScoringInfoList { get; set; } = new List<SPA_ScoringInfoModel>();

        /// <summary> 該評鑑期間中的 SPA違規紀錄資料主檔 清單 </summary>
        public List<SPA_ViolationModel> ViolationList { get; set; } = new List<SPA_ViolationModel>();

        /// <summary> 該評鑑期間中的 Cost&Service 主檔 清單 </summary>
        public List<SPA_CostServiceModel> CostServiceList { get; set; } = new List<SPA_CostServiceModel>();
        #endregion


        #region Program
        /// <summary> 評鑑期間 </summary>
        private DatePeriod DatePeriod { get { return PeriodUtil.ParsePeriod(this.Period); } }

        /// <summary> 評鑑期間 (起始) </summary>
        public string PeriodStart { get { return this.DatePeriod.StartDate?.ToString("yyyy-MM-dd"); } }

        /// <summary> 評鑑期間 (結束) </summary>
        public string PeriodEnd { get { return this.DatePeriod.EndDate?.ToString("yyyy-MM-dd"); } }

        /// <summary> 是否全都審核通過了 </summary>
        public bool IsAllApproved
        {
            get
            {
                // 如果沒有值，就視為沒審核通過
                if (!this.ScoringInfoList.Any() || !this.ViolationList.Any())
                    return false;

                // 檢查是否全都通過了
                var hasNotCompleted_ScoringInfo = this.ScoringInfoList.Where(obj => obj.ApproveStatus != ApprovalStatus.Completed.ToText()).Any();
                var hasNotCompleted_Violation = this.ViolationList.Where(obj => obj.ApproveStatus != ApprovalStatus.Completed.ToText()).Any();

                if (hasNotCompleted_ScoringInfo || hasNotCompleted_Violation)
                    return false;

                return true;
            }
        }
        #endregion
    }
}
