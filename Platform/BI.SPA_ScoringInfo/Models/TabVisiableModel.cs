using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ScoringInfo.Models
{
    /// <summary> 頁籤是否可視 </summary>
    public class TabVisiableModel
    {
        /// <summary> 人力盤點 </summary>
        public bool Tab1 { get; set; } = false;

        /// <summary> 施工達交狀況盤點 </summary>
        public bool Tab2 { get; set; } = false;

        /// <summary> 施工正確性 </summary>
        public bool Tab3 { get; set; } = false;

        /// <summary> 作業正確性 & 人員備齊貢獻度 </summary>
        public bool Tab4 { get; set; } = false;

        /// <summary> 自訓能力 </summary>
        public bool Tab5 { get; set; } = false;

        /// <summary> 服務 </summary>
        public bool Tab6 { get; set; } = false;

        /// <summary> 附件 </summary>
        public bool Tab7 { get; set; } = false;
    }
}
