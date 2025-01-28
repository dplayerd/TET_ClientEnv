using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ApproverSetup.Models
{

    /// <summary> SPA評鑑期間 </summary>
    public class TET_SPA_PeriodModel
    {
        #region 原生欄位
        /// <summary> ID </summary>
        public Guid ID { get; set; }

        /// <summary> 評鑑期間 </summary>
        public string Period { get; set; }

        /// <summary> 評鑑狀態 </summary>
        public string Status { get; set; }

        /// <summary> 建立人員 </summary>
        public string CreateUser { get; set; }

        /// <summary> 新增時間 </summary>
        public DateTime CreateDate { get; set; }

        /// <summary> 最後更新人員 </summary>
        public string ModifyUser { get; set; }

        /// <summary> 最後更新時間 </summary>
        public DateTime ModifyDate { get; set; }
        #endregion
    }
}
