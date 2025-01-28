using Platform.AbstractionClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ScoringInfo.Models.ToolTips
{
    /// <summary> Tab1 ToolTip </summary>
    public class Tab1ToolTip
    {
        public Tab1ToolTip(List<KeyTextModel> keyTexts)
        {
            this.Type = keyTexts.Where(obj => obj.Key == nameof(Type)).FirstOrDefault()?.Text;
            this.Supplier = keyTexts.Where(obj => obj.Key == nameof(Supplier)).FirstOrDefault()?.Text;
            this.EmpName = keyTexts.Where(obj => obj.Key == nameof(EmpName)).FirstOrDefault()?.Text;
            this.MajorJob = keyTexts.Where(obj => obj.Key == nameof(MajorJob)).FirstOrDefault()?.Text;
            this.IsIndependent = keyTexts.Where(obj => obj.Key == nameof(IsIndependent)).FirstOrDefault()?.Text;
            this.SkillLevel = keyTexts.Where(obj => obj.Key == nameof(SkillLevel)).FirstOrDefault()?.Text;
            this.EmpStatus = keyTexts.Where(obj => obj.Key == nameof(EmpStatus)).FirstOrDefault()?.Text;
            this.TELSeniority = keyTexts.Where(obj => obj.Key == nameof(TELSeniority)).FirstOrDefault()?.Text;
            this.Remark = keyTexts.Where(obj => obj.Key == "Tab1_Remark").FirstOrDefault()?.Text;
        }


        /// <summary> 本社/協力廠商 </summary>
        public string Type { get; set; }

        /// <summary> 供應商名稱 </summary>
        public string Supplier { get; set; }

        /// <summary> 員工姓名 </summary>
        public string EmpName { get; set; }

        /// <summary> 主要負責作業 </summary>
        public string MajorJob { get; set; }

        /// <summary> 能否獨立作業 </summary>
        public string IsIndependent { get; set; }

        /// <summary> Skill Level </summary>
        public string SkillLevel { get; set; }

        /// <summary> 員工狀態 </summary>
        public string EmpStatus { get; set; }

        /// <summary> 派工至TEL的年資 </summary>
        public string TELSeniority { get; set; }

        /// <summary> 備註 </summary>
        public string Remark { get; set; }
    }
}
