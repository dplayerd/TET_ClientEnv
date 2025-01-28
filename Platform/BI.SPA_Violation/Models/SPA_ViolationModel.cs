using BI.Shared.Utils;
using BI.SPA_Violation.Enums;
using BI.SPA_Violation.Utils;
using Platform.AbstractionClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_Violation.Models
{
    public class SPA_ViolationModel
    {
        #region 原生欄位
        /// <summary> ID </summary>
        public Guid? ID { get; set; }

        /// <summary> 評鑑期間 </summary>
        public string Period { get; set; }

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


        #region  OtherTable
        /// <summary> 明細表 </summary>
        public List<SPA_ViolationDetailModel> DetailList { get; set; } = new List<SPA_ViolationDetailModel>();

        /// <summary> 簽核清單 </summary>
        public List<SPA_ViolationApprovalModel> ApprovalList { get; set; } = new List<SPA_ViolationApprovalModel>();
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
