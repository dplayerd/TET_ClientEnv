using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ScoringInfo.Models
{
    /// <summary> 施工達交狀況盤點 </summary>
    public class TET_SPA_ScoringInfoModule2Model
    {
        #region 原生欄位
        /// <summary> 系統辨識碼 </summary>
        public Guid ID { get; set; }

        /// <summary> 評鑑計分資料系統辨識碼 </summary>
        public Guid SIID { get; set; }

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
