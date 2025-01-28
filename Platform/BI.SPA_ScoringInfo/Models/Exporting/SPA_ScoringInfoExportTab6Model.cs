using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ScoringInfo.Models.Exporting
{
    /// <summary> 匯出資料用 Model - Tab6 </summary>
    public class SPA_ScoringInfoExportTab6Model : SPA_ScoringInfoExportModelBase
    {
        public SPA_ScoringInfoExportTab6Model(SPA_ScoringInfoModule4Model item, SPA_ScoringInfoModel main) : base(main)
        {
            this.Cooperation = main.Cooperation;
            this.Complain = main.Complain;
            this.Advantage = main.Advantage;
            this.Improved = main.Improved;
            this.Comment = main.Comment;

            this.Date = item.Date;
            this.Location = item.Location;
            this.IsDamage = item.IsDamage;
            this.Description = item.Description;
        }

        #region 基本欄位 - Main
        /// <summary> 配合度 </summary>
        public string Cooperation { get; set; }

        /// <summary> 客戶抱怨 </summary>
        public string Complain { get; set; }

        /// <summary> 優點、滿意、值得鼓勵之處 </summary>
        public string Advantage { get; set; }

        /// <summary> 不滿意、期望改善之處 </summary>
        public string Improved { get; set; }

        /// <summary> 客戶評論與其他補充說明 </summary>
        public string Comment { get; set; }
        #endregion


        #region 基本欄位 - Module4
        /// <summary> 時間 </summary>
        public DateTime Date { get; set; }

        /// <summary> 地點 </summary>
        public string Location { get; set; }

        /// <summary> 造成財損 </summary>
        public string IsDamage { get; set; }

        /// <summary> 事件說明 </summary>
        public string Description { get; set; }
        #endregion
    }
}
