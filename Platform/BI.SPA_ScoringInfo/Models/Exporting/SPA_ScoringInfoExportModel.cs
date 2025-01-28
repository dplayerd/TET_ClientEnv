using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ScoringInfo.Models.Exporting
{
    /// <summary> 匯出報表用資料 </summary>
    public class SPA_ScoringInfoExportModel
    {
        public List<SPA_ScoringInfoExportModelBase> BaseList { get; set; } = new List<SPA_ScoringInfoExportModelBase>();
        public List<SPA_ScoringInfoExportTab1Model> Tab1List { get; set; } = new List<SPA_ScoringInfoExportTab1Model>();
        public List<SPA_ScoringInfoExportTab2Model> Tab2List { get; set; } = new List<SPA_ScoringInfoExportTab2Model>();
        public List<SPA_ScoringInfoExportTab3Model> Tab3List { get; set; } = new List<SPA_ScoringInfoExportTab3Model>();
        public List<SPA_ScoringInfoExportTab4Model> Tab4List { get; set; } = new List<SPA_ScoringInfoExportTab4Model>();
        public List<SPA_ScoringInfoExportTab5Model> Tab5List { get; set; } = new List<SPA_ScoringInfoExportTab5Model>();
        public List<SPA_ScoringInfoExportTab6Model> Tab6List { get; set; } = new List<SPA_ScoringInfoExportTab6Model>();
    }
}
