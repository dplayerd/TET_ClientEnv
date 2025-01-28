using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ScoringInfo.Models
{
    /// <summary> 作業正確性&人員備齊貢獻度 </summary>
    public class SPA_ScoringInfoModule4Model
    {
        #region 原生欄位
        /// <summary> 系統辨識碼 </summary>
        public Guid? ID { get; set; }

        /// <summary> 評鑑計分資料系統辨識碼 </summary>
        public Guid SIID { get; set; }

        /// <summary> 時間 </summary>
        public DateTime Date { get; set; }

        /// <summary> 地點 </summary>
        public string Location { get; set; }

        /// <summary> 造成財損 </summary>
        public string IsDamage { get; set; }

        /// <summary> 事件說明 </summary>
        public string Description { get; set; }

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
        public string DateText { get { return this.Date.ToString("yyyy-MM-dd"); } }
        #endregion
    }
}
