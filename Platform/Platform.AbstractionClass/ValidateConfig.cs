using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.AbstractionClass
{
    /// <summary> 驗證設定 </summary>
    public class ValidateConfig
    {
        /// <summary> 欄位英文名稱 </summary>
        public string Name { get; set; }

        /// <summary> 欄位標題 </summary>
        public string Title { get; set; }

        /// <summary> 是否必填 </summary>
        public bool Required { get; set; }

        /// <summary> 是否允許編輯 </summary>
        public bool CanEdit { get; set; }
    }
}
