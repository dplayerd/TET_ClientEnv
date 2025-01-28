using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ScoringInfo.Models
{
    /// <summary> 人力盤點 </summary>
    public class SPA_ScoringInfoModule1Model
    {
        #region 原生欄位
        /// <summary> 系統辨識碼 </summary>
        public Guid? ID { get; set; }

        /// <summary> 評鑑計分資料系統辨識碼 </summary>
        public Guid SIID { get; set; }

        /// <summary> 資料來源 </summary>
        public string Source { get; set; }

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

        /// <summary> 建立人員 </summary>
        public string CreateUser { get; set; }

        /// <summary> 新增時間 </summary>
        public DateTime CreateDate { get; set; }

        /// <summary> 最後更新人員 </summary>
        public string ModifyUser { get; set; }

        /// <summary> 最後更新時間 </summary>
        public DateTime ModifyDate { get; set; }
        #endregion


        #region Program
        #endregion
    }
}
