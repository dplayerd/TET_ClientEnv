using Platform.AbstractionClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ScoringInfo.Models.ToolTips
{
    /// <summary> Tab4 ToolTip </summary>
    public class Tab4ToolTip
    {
        public Tab4ToolTip(List<KeyTextModel> keyTexts)
        {
            this.Correctness = keyTexts.Where(obj => obj.Key == nameof(Correctness)).FirstOrDefault()?.Text;
            this.Contribution = keyTexts.Where(obj => obj.Key == nameof(Contribution)).FirstOrDefault()?.Text;
        }

        /// <summary> 作業正確性 </summary>
        public string Correctness { get; set; }

        /// <summary> 人員備齊貢獻度 </summary>
        public string Contribution { get; set; }
    }
}
