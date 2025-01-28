using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Suppliers.Models
{
    /// <summary> 供應商驗證設定 </summary>
    public class ValidConfig
    {
        /// <summary> 欄位英文名稱 </summary>
        public string ColumnName { get; set; }

        /// <summary> 欄位標題 </summary>
        public string ColumnTitle { get; set; }

        /// <summary> 新增時是否必填 </summary>
        public bool RequiredOnCreate { get; set; }
        
        /// <summary> 編輯時是否必填 </summary>
        public bool RequiredOnModify { get; set; }
        
        /// <summary> 新增時是否顯示 </summary>
        public bool ShowOnCreate { get; set; }
        
        /// <summary> 編輯時是否顯示 </summary>
        public bool ShowOnModify { get; set; }
    }
}
