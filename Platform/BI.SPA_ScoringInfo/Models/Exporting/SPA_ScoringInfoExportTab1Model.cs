using BI.SPA_ScoringInfo.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ScoringInfo.Models.Exporting
{
    /// <summary> 匯出資料用 Model - Tab1 </summary>
    public class SPA_ScoringInfoExportTab1Model : SPA_ScoringInfoExportModelBase
    {
        public SPA_ScoringInfoExportTab1Model(SPA_ScoringInfoModule1Model module1, SPA_ScoringInfoModel main) : base(main)
        {
            this.Type = module1.Type;
            this.Supplier = module1.Supplier;
            this.EmpName = module1.EmpName;
            this.MajorJob = module1.MajorJob;
            this.IsIndependent = module1.IsIndependent;
            this.SkillLevel = module1.SkillLevel;
            this.EmpStatus = module1.EmpStatus;
            this.TELSeniorityY = module1.TELSeniorityY;
            this.TELSeniorityM = module1.TELSeniorityM;
            this.Remark = module1.Remark;
        }


        #region 原生欄位
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

        /// <summary> 派工至TEL的年資(年) </summary>
        public string TELSeniorityY { get; set; }

        /// <summary> 派工至TEL的年資(月) </summary>
        public string TELSeniorityM { get; set; }

        /// <summary> 備註 </summary>
        public string Remark { get; set; }
        #endregion


        #region Program
        #endregion
    }
}
