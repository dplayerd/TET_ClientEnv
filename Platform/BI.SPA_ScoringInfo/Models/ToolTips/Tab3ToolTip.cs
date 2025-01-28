using Platform.AbstractionClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ScoringInfo.Models.ToolTips
{
    /// <summary> Tab3 ToolTip </summary>
    public class Tab3ToolTip
    {
        public Tab3ToolTip(List<KeyTextModel> keyTexts)
        {
            this.WorkerCount = keyTexts.Where(obj => obj.Key == nameof(WorkerCount)).FirstOrDefault()?.Text;
            this.Date = keyTexts.Where(obj => obj.Key == "Tab3_Date").FirstOrDefault()?.Text;
            this.Location = keyTexts.Where(obj => obj.Key == "Tab3_Location").FirstOrDefault()?.Text;
            this.TELLoss = keyTexts.Where(obj => obj.Key == nameof(TELLoss)).FirstOrDefault()?.Text;
            this.CustomerLoss = keyTexts.Where(obj => obj.Key == nameof(CustomerLoss)).FirstOrDefault()?.Text;
            this.Accident = keyTexts.Where(obj => obj.Key == nameof(Accident)).FirstOrDefault()?.Text;
            this.Description = keyTexts.Where(obj => obj.Key == "Tab3_Description").FirstOrDefault()?.Text;
        }

        /// <summary> 出工人數 </summary>
        public string WorkerCount { get; set; }

        /// <summary> 時間 </summary>
        public string Date { get; set; }

        /// <summary> 地點 </summary>
        public string Location { get; set; }

        /// <summary> TEL財損 </summary>
        public string TELLoss { get; set; }

        /// <summary> 客戶財損 </summary>
        public string CustomerLoss { get; set; }

        /// <summary> 人身事故 </summary>
        public string Accident { get; set; }

        /// <summary> 事件說明 </summary>
        public string Description { get; set; }
    }
}
