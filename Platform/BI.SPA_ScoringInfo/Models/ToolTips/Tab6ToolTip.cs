using Platform.AbstractionClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ScoringInfo.Models.ToolTips
{
    /// <summary> Tab6 ToolTip </summary>
    public class Tab6ToolTip
    {
        public Tab6ToolTip(List<KeyTextModel> keyTexts)
        {
            this.Cooperation = keyTexts.Where(obj => obj.Key == nameof(Cooperation)).FirstOrDefault()?.Text;
            this.Advantage = keyTexts.Where(obj => obj.Key == nameof(Advantage)).FirstOrDefault()?.Text;
            this.Improved = keyTexts.Where(obj => obj.Key == nameof(Improved)).FirstOrDefault()?.Text;
            this.Comment = keyTexts.Where(obj => obj.Key == "Tab6_Comment").FirstOrDefault()?.Text;
            this.Date = keyTexts.Where(obj => obj.Key == "Tab6_Date").FirstOrDefault()?.Text;
            this.Location = keyTexts.Where(obj => obj.Key == "Tab6_Location").FirstOrDefault()?.Text;
            this.IsDamage = keyTexts.Where(obj => obj.Key == nameof(IsDamage)).FirstOrDefault()?.Text;
            this.Description = keyTexts.Where(obj => obj.Key == "Tab6_Description").FirstOrDefault()?.Text;
        }


        /// <summary> 配合度 </summary>
        public string Cooperation { get; set; }

        /// <summary> 優點、滿意、值得鼓勵之處 </summary>
        public string Advantage { get; set; }

        /// <summary> 不滿意、期望改善之處 </summary>
        public string Improved { get; set; }

        /// <summary> 客戶評論與其他補充說明 </summary>
        public string Comment { get; set; }

        /// <summary> 時間 </summary>
        public string Date { get; set; }

        /// <summary> 地點 </summary>
        public string Location { get; set; }

        /// <summary> 造成財損 </summary>
        public string IsDamage { get; set; }

        /// <summary> 事件說明 </summary>
        public string Description { get; set; }
    }
}
