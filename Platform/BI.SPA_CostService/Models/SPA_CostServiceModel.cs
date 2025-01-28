using BI.Shared.Utils;
using Platform.AbstractionClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BI.SPA_CostService.Utils;
using BI.SPA_CostService.Enums;

namespace BI.SPA_CostService.Models
{
    public class SPA_CostServiceModel
    {
        #region 原生欄位
        /// <summary> 系統辨識碼 </summary>
        public Guid? ID { get; set; }

        /// <summary> 評鑑期間 </summary>
        public string Period { get; set; }

        /// <summary> 填寫人員 </summary>
        public string Filler { get; set; }

        /// <summary> 審核狀態 </summary>
        public string ApproveStatus { get; set; }

        /// <summary> 建立人員 </summary>
        public string CreateUser { get; set; }

        /// <summary> 新增時間 </summary>
        public DateTime CreateDate { get; set; }

        /// <summary> 最後更新人員 </summary>
        public string ModifyUser { get; set; }

        /// <summary> 最後更新時間 </summary>
        public DateTime ModifyDate { get; set; }
        #endregion

        #region Program
        /// <summary> 填寫入全名 </summary>
        public string FillerFullName { get; set; }
        #endregion


        #region  OtherTable
        /// <summary> 明細表 </summary>
        public List<SPA_CostServiceDetailModel> DetailList { get; set; } = new List<SPA_CostServiceDetailModel>();

        /// <summary> 簽核清單 </summary>
        public List<SPA_CostServiceApprovalModel> ApprovalList { get; set; } = new List<SPA_CostServiceApprovalModel>();
        #endregion


        #region Program
        /// <summary> 評鑑期間 </summary>
        private DatePeriod DatePeriod { get { return PeriodUtil.ParsePeriod(this.Period); } }

        /// <summary> 評鑑期間 (起始) </summary>
        public string PeriodStart { get { return this.DatePeriod.StartDate?.ToString("yyyy-MM-dd"); } }

        /// <summary> 評鑑期間 (結束) </summary>
        public string PeriodEnd { get { return this.DatePeriod.EndDate?.ToString("yyyy-MM-dd"); } }

        /// <summary> 審核狀態 Text </summary>
        public string ApprovalStatusEnum
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.ApproveStatus))
                    return "未送出";

                return ApprovalUtils.ParseApprovalStatus(this.ApproveStatus).ToText();
            }
        }
        #endregion
    }
}
