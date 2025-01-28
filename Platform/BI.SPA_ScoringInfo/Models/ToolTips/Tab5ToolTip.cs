using Platform.AbstractionClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ScoringInfo.Models.ToolTips
{
    /// <summary> Tab5  ToolTip </summary>
    public class Tab5ToolTip
    {
        public Tab5ToolTip(List<KeyTextModel> keyTexts)
        {
            this.SelfTraining = keyTexts.Where(obj => obj.Key == nameof(SelfTraining)).FirstOrDefault()?.Text;
            this.SelfTrainingRemark = keyTexts.Where(obj => obj.Key == nameof(SelfTrainingRemark)).FirstOrDefault()?.Text;
        }

        /// <summary> 供應商自訓程度 </summary>
        public string SelfTraining { get; set; }

        /// <summary> 供應商自訓程度備註 </summary>
        public string SelfTrainingRemark { get; set; }
    }
}
