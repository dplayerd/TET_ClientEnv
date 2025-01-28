using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ScoringInfo.Models.Exporting
{
    /// <summary> 匯出資料用 Model - Tab5 </summary>
    public class SPA_ScoringInfoExportTab5Model : SPA_ScoringInfoExportModelBase
    {
        public SPA_ScoringInfoExportTab5Model(SPA_ScoringInfoModel main) : base(main)
        {
            this.SelfTraining = main.SelfTraining;
            this.SelfTrainingRemark = main.SelfTrainingRemark;
        }

        #region 基本欄位
        /// <summary> 供應商自訓程度 </summary>
        public string SelfTraining { get; set; }

        /// <summary> 供應商自訓程度備註 </summary>
        public string SelfTrainingRemark { get; set; }
        #endregion
    }
}
