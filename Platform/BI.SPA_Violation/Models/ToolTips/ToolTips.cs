using Platform.AbstractionClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_Violation.Models.ToolTips
{
    /// <summary> Tab1 ToolTip </summary>
    public class ViolationToolTip
    {
        public ViolationToolTip(List<KeyTextModel> keyTexts)
        {
            this.Date = keyTexts.Where(obj => obj.Key == nameof(Date)).FirstOrDefault()?.Text;
            this.BelongTo = keyTexts.Where(obj => obj.Key == nameof(BelongTo)).FirstOrDefault()?.Text;
            this.BU = keyTexts.Where(obj => obj.Key == nameof(BU)).FirstOrDefault()?.Text;
            this.AssessmentItem = keyTexts.Where(obj => obj.Key == nameof(AssessmentItem)).FirstOrDefault()?.Text;
            this.MiddleCategory = keyTexts.Where(obj => obj.Key == nameof(MiddleCategory)).FirstOrDefault()?.Text;
            this.SmallCategory = keyTexts.Where(obj => obj.Key == nameof(SmallCategory)).FirstOrDefault()?.Text;
            this.CustomerName = keyTexts.Where(obj => obj.Key == nameof(CustomerName)).FirstOrDefault()?.Text;
            this.CustomerPlant = keyTexts.Where(obj => obj.Key == nameof(CustomerPlant)).FirstOrDefault()?.Text;
            this.CustomerDetail = keyTexts.Where(obj => obj.Key == nameof(CustomerDetail)).FirstOrDefault()?.Text;
            this.Description = keyTexts.Where(obj => obj.Key == nameof(Description)).FirstOrDefault()?.Text;
        }


        /// <summary> 日期 </summary>
        public string Date { get; set; }

        /// <summary> 受評供應商 </summary>
        public string BelongTo { get; set; }

        /// <summary> 評鑑單位 </summary>
        public string BU { get; set; }

        /// <summary> 評鑑項目 </summary>
        public string AssessmentItem { get; set; }

        /// <summary> 中分類 </summary>
        public string MiddleCategory { get; set; }

        /// <summary> 小分類 </summary>
        public string SmallCategory { get; set; }

        /// <summary> 客戶名稱 </summary>
        public string CustomerName { get; set; }

        /// <summary> 客戶廠別 </summary>
        public string CustomerPlant { get; set; }

        /// <summary> 客戶細分 </summary>
        public string CustomerDetail { get; set; }

        /// <summary> 違規事件說明 </summary>
        public string Description { get; set; }
    }
}
