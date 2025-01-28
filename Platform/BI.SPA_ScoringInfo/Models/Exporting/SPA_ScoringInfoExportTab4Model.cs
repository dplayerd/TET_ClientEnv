using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ScoringInfo.Models.Exporting
{
    /// <summary> 匯出資料用 Model - Tab4 </summary>
    public class SPA_ScoringInfoExportTab4Model : SPA_ScoringInfoExportModelBase  
    {
        public SPA_ScoringInfoExportTab4Model( SPA_ScoringInfoModel main) : base(main)
        {
            this.Correctness = main.Correctness;
            this.Contribution = main.Contribution;
        }

        #region 基本欄位
        /// <summary> 作業正確性 </summary>
        public string Correctness { get; set; }

        /// <summary> 人員備齊貢獻度 </summary>
        public string Contribution { get; set; }
        #endregion
    }
}
