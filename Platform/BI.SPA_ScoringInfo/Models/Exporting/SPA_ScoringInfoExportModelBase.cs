using BI.Shared.Utils;
using Platform.AbstractionClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ScoringInfo.Models.Exporting
{
    public class SPA_ScoringInfoExportModelBase
    {
        public SPA_ScoringInfoExportModelBase(SPA_ScoringInfoModel main)
        {
            this.ID = main.ID;
            this.Period = main.Period;
            this.BU = main.BU;
            this.ServiceItem = main.ServiceItem;
            this.ServiceFor = main.ServiceFor;
            this.BelongTo = main.BelongTo;
            this.POSource = main.POSource;
            this.MOCount = main.MOCount;
            this.TELLoss = main.TELLoss;
            this.CustomerLoss = main.CustomerLoss;
            this.Accident = main.Accident;
            this.WorkerCount = main.WorkerCount;
            this.Correctness = main.Correctness;
            this.Contribution = main.Contribution;
            this.SelfTraining = main.SelfTraining;
            this.SelfTrainingRemark = main.SelfTrainingRemark;
            this.Cooperation = main.Cooperation;
            this.Complain = main.Complain;
            this.Advantage = main.Advantage;
            this.Improved = main.Improved;
            this.Comment = main.Comment;

            DatePeriod DatePeriod = PeriodUtil.ParsePeriod(this.Period);
            this.PeriodStart = DatePeriod.StartDate?.ToString("yyyy-MM-dd");
            this.PeriodEnd = DatePeriod.EndDate?.ToString("yyyy-MM-dd");
        }


        #region 基本欄位
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
        #endregion

        #region Program
        /// <summary> 評鑑期間 (起始) </summary>
        public string PeriodStart { get; set; }

        /// <summary> 評鑑期間 (結束) </summary>
        public string PeriodEnd { get; set; }
        #endregion
    }
}
