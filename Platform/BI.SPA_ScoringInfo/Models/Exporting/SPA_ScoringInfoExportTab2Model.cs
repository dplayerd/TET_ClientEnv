using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ScoringInfo.Models.Exporting
{
    /// <summary> 匯出資料用 Model - Tab2 </summary>
    public class SPA_ScoringInfoExportTab2Model : SPA_ScoringInfoExportModelBase
    {
        public SPA_ScoringInfoExportTab2Model(SPA_ScoringInfoModule2Model item, SPA_ScoringInfoModel main) : base(main)
        {
            this.ServiceFor = item.ServiceFor;
            this.WorkItem = item.WorkItem;
            this.MachineName = item.MachineName;
            this.MachineNo = item.MachineNo;
            this.OnTime = item.OnTime;
            this.Remark = item.Remark;
        }

        #region 原生欄位
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
        #endregion
    }
}
