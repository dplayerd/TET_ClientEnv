using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.AbstractionClass
{
    /// <summary> 郵件內容 </summary>
    public class EMailContent
    {
        /// <summary> 標題 </summary>
        public string Title { get; set; }

        /// <summary> 內文 </summary>
        public string Body { get; set; }
    }
}
