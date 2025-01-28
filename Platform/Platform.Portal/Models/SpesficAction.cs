using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Portal.Models
{
    /// <summary> 特殊功能權限 </summary>
    public class SpesficAction
    {
        /// <summary> 主代碼 </summary>
        public Guid ID { get; set; }

        /// <summary> 功能名稱 (20 碼) </summary>
        public string FunctionCode { get; set; }

        /// <summary> 是否允許 </summary>
        public bool IsAllow { get; set; }
    }
}
