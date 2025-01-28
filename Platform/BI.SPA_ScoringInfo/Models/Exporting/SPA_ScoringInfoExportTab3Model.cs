using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ScoringInfo.Models.Exporting
{
    /// <summary> 匯出資料用 Model - Tab3 </summary>
    public class SPA_ScoringInfoExportTab3Model : SPA_ScoringInfoExportModelBase
    {
        public SPA_ScoringInfoExportTab3Model(SPA_ScoringInfoModule3Model item, SPA_ScoringInfoModel main) : base(main)
        {
            this.MOCount_Main = main.MOCount;
            this.TELLoss_Main = main.TELLoss;
            this.CustomerLoss_Main = main.CustomerLoss;
            this.Accident_Main = main.Accident;
            this.WorkerCount_Main = main.WorkerCount;


            this.Date = item.Date;
            this.Location = item.Location;
            this.TELLoss = item.TELLoss;
            this.CustomerLoss = item.CustomerLoss;
            this.Accident = item.Accident;
            this.Description = item.Description;
        }


        #region 基本欄位 - Main
        /// <summary> MO次數 </summary>
        public string MOCount_Main { get; set; }

        /// <summary> TEL財損 </summary>
        public string TELLoss_Main { get; set; }

        /// <summary> 客戶財損 </summary>
        public string CustomerLoss_Main { get; set; }

        /// <summary> 人身事故 </summary>
        public string Accident_Main { get; set; }

        /// <summary> 出工人數 </summary>
        public int? WorkerCount_Main { get; set; }
        #endregion


        #region 基本欄位 - Module3
        /// <summary> 時間 </summary>
        public DateTime Date { get; set; }

        /// <summary> 地點 </summary>
        public string Location { get; set; }

        /// <summary> TEL財損 </summary>
        public string TELLoss { get; set; }

        /// <summary> 客戶財損 </summary>
        public string CustomerLoss { get; set; }

        /// <summary> 人身事故 </summary>
        public string Accident { get; set; }

        /// <summary> 事件說明 </summary>
        public string Description { get; set; }
        #endregion
    }
}
