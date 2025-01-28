using Platform.AbstractionClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ScoringInfo.Models.ToolTips
{
    /// <summary> Tab2 ToolTip </summary>
    public class Tab2ToolTip
    {
        public Tab2ToolTip(List<KeyTextModel> keyTexts)
        {
            this.ServiceFor = keyTexts.Where(obj => obj.Key == nameof(ServiceFor)).FirstOrDefault()?.Text;
            this.WorkItem = keyTexts.Where(obj => obj.Key == nameof(WorkItem)).FirstOrDefault()?.Text;
            this.MachineName = keyTexts.Where(obj => obj.Key == nameof(MachineName)).FirstOrDefault()?.Text;
            this.MachineNo = keyTexts.Where(obj => obj.Key == nameof(MachineNo)).FirstOrDefault()?.Text;
            this.OnTime = keyTexts.Where(obj => obj.Key == nameof(OnTime)).FirstOrDefault()?.Text;
            this.Remark = keyTexts.Where(obj => obj.Key == "Tab2_Remark").FirstOrDefault()?.Text;
        }


        /// <summary> 服務對象 </summary>
        public string ServiceFor { get; set; }

        /// <summary> 作業項目 </summary>
        public string WorkItem { get; set; }

        /// <summary> 承攬機台名稱 </summary>
        public string MachineName { get; set; }

        /// <summary> 機台Serial No. </summary>
        public string MachineNo { get; set; }

        /// <summary> 是否準時交付 </summary>
        public string OnTime { get; set; }

        /// <summary> 備註 </summary>
        public string Remark { get; set; }
    }
}
