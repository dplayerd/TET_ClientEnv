using Platform.AbstractionClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.PaymentSuppliers.Models
{
    /// <summary> 供應商審核設定 </summary>
    public class ApprovalValidConfig : ValidateConfig
    {
        /// <summary> 欄位英文名稱 </summary>
        public string ColumnName { get { return base.Name; } set { base.Name = value; } }

        /// <summary> 欄位標題 </summary>
        public string ColumnTitle { get { return base.Title; } set { base.Title = value; } }

        /// <summary> 是否一開始為鎖定 </summary>
        public bool Disabled { get; set; }
        
        /// <summary> 開放關卡 </summary>
        public string EditAtLevel { get; set; }
    }
}

