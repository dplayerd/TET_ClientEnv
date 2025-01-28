using BI.Shared.Utils;
using BI.SPA_ScoringInfo.Enums;
using BI.SPA_ScoringInfo.Utils;
using Platform.AbstractionClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ScoringInfo.Models
{
    /// <summary> 供應商SPA評鑑計分資料維護 </summary>
    public class SPA_ScoringInfoModel
    {
        #region 原生欄位
        /// <summary> 系統辨識碼 </summary>
        public Guid? ID { get; set; }

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

        /// <summary> 審核狀態 </summary>
        public string ApproveStatus { get; set; }

        /// <summary> MO次數 </summary>
        public string MOCount { get; set; }

        /// <summary> TEL財損 </summary>
        public string TELLoss { get; set; }

        /// <summary> 客戶財損 </summary>
        public string CustomerLoss { get; set; }

        /// <summary> 人身事故 </summary>
        public string Accident { get; set; }

        /// <summary> 出工人數 </summary>
        public int? WorkerCount { get; set; }

        /// <summary> 作業正確性 </summary>
        public string Correctness { get; set; }

        /// <summary> 人員備齊貢獻度 </summary>
        public string Contribution { get; set; }

        /// <summary> 供應商自訓程度 </summary>
        public string SelfTraining { get; set; }

        /// <summary> 供應商自訓程度備註 </summary>
        public string SelfTrainingRemark { get; set; }

        /// <summary> 配合度 </summary>
        public string Cooperation { get; set; }

        /// <summary> 客戶抱怨 </summary>
        public string Complain { get; set; }

        /// <summary> 優點、滿意、值得鼓勵之處 </summary>
        public string Advantage { get; set; }

        /// <summary> 不滿意、期望改善之處 </summary>
        public string Improved { get; set; }

        /// <summary> 客戶評論與其他補充說明 </summary>
        public string Comment { get; set; }

        /// <summary> 建立人員 </summary>
        public string CreateUser { get; set; }

        /// <summary> 新增時間 </summary>
        public DateTime CreateDate { get; set; }

        /// <summary> 最後更新人員 </summary>
        public string ModifyUser { get; set; }

        /// <summary> 最後更新時間 </summary>
        public DateTime ModifyDate { get; set; }
        #endregion


        #region OtherTable
        /// <summary> 簽核清單 </summary>
        public List<SPA_ScoringInfoApprovalModel> ApprovalList { get; set; } = new List<SPA_ScoringInfoApprovalModel>();

        /// <summary> 附件清單 </summary>
        public List<SPA_ScoringInfoAttachmentModel> AttachmentList { get; set; } = new List<SPA_ScoringInfoAttachmentModel>();

        /// <summary> 人力盤點清單 </summary>
        public List<SPA_ScoringInfoModule1Model> Module1List { get; set; } = new List<SPA_ScoringInfoModule1Model>();

        /// <summary> 施工達交狀況盤點清單 </summary>
        public List<SPA_ScoringInfoModule2Model> Module2List { get; set; } = new List<SPA_ScoringInfoModule2Model>();

        /// <summary> 施工正確性清單 </summary>
        public List<SPA_ScoringInfoModule3Model> Module3List { get; set; } = new List<SPA_ScoringInfoModule3Model>();

        /// <summary> 作業正確性&人員備齊貢獻度清單 </summary>
        public List<SPA_ScoringInfoModule4Model> Module4List { get; set; } = new List<SPA_ScoringInfoModule4Model>();
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
