using Platform.AbstractionClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_Evaluation.Models.ToolTips
{
    /// <summary> Tab1 ToolTip </summary>
    public class ToolTip
    {
        public ToolTip(List<KeyTextModel> keyTexts)
        {
            this.IsEvaluate = keyTexts.Where(obj => obj.Key == nameof(IsEvaluate)).FirstOrDefault()?.Text;
            this.BU = keyTexts.Where(obj => obj.Key == nameof(BU)).FirstOrDefault()?.Text;
            this.ServiceFor = keyTexts.Where(obj => obj.Key == nameof(ServiceFor)).FirstOrDefault()?.Text;
            this.BelongTo = keyTexts.Where(obj => obj.Key == nameof(BelongTo)).FirstOrDefault()?.Text;
            this.POSource = keyTexts.Where(obj => obj.Key == nameof(POSource)).FirstOrDefault()?.Text;
            this.AssessmentItem = keyTexts.Where(obj => obj.Key == nameof(AssessmentItem)).FirstOrDefault()?.Text;
            this.PriceDeflator = keyTexts.Where(obj => obj.Key == nameof(PriceDeflator)).FirstOrDefault()?.Text;
            this.PaymentTerm = keyTexts.Where(obj => obj.Key == nameof(PaymentTerm)).FirstOrDefault()?.Text;
            this.Cooperation = keyTexts.Where(obj => obj.Key == nameof(Cooperation)).FirstOrDefault()?.Text;
            this.Advantage = keyTexts.Where(obj => obj.Key == nameof(Advantage)).FirstOrDefault()?.Text;
            this.Improved = keyTexts.Where(obj => obj.Key == nameof(Improved)).FirstOrDefault()?.Text;
            this.Comment = keyTexts.Where(obj => obj.Key == nameof(Comment)).FirstOrDefault()?.Text;
            this.Remark = keyTexts.Where(obj => obj.Key == nameof(Remark)).FirstOrDefault()?.Text;
        }


        /// <summary> 評鑑與否 </summary>
        public string IsEvaluate { get; set; }

        /// <summary> 評鑑單位 </summary>
        public string BU { get; set; }

        /// <summary> 服務對象 </summary>
        public string ServiceFor { get; set; }

        /// <summary> 受評供應商 </summary>
        public string BelongTo { get; set; }

        /// <summary> PO Source </summary>
        public string POSource { get; set; }

        /// <summary> 評鑑項目 </summary>
        public string AssessmentItem { get; set; }

        /// <summary> 價格競爭力 </summary>
        public string PriceDeflator { get; set; }

        /// <summary> 付款條件 </summary>
        public string PaymentTerm { get; set; }

        /// <summary> 配合度 </summary>
        public string Cooperation { get; set; }

        /// <summary> 優點、滿意、值得鼓勵之處 </summary>
        public string Advantage { get; set; }

        /// <summary> 不滿意、期望改善之處 </summary>
        public string Improved { get; set; }

        /// <summary> 客戶評論與其他補充說明 </summary>
        public string Comment { get; set; }

        /// <summary> 備註 </summary>
        public string Remark { get; set; }
    }
}
