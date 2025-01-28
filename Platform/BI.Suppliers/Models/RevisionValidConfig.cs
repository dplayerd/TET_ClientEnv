using Platform.AbstractionClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Suppliers.Models
{
    /// <summary> 供應商改版審核設定 </summary>
    public class RevisionValidConfig : ValidateConfig
    {
        /// <summary> 是否允許編輯 </summary>
        public bool CanEdit { get; set; }
    }
}

