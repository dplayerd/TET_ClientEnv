using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ApproverSetup.Enums
{
    /// <summary> PO 來源 </summary>
    public enum POSource
    {
        Local = 0,

        Factory = 1
    }


    /// <summary> PO 來源擴充 </summary>
    public static class POSourceExtension
    {
        /// <summary> 轉換為文字
        /// </summary>
        /// <param name="enm"> PO 來源 </param>
        /// <returns></returns>
        public static string ToText(this POSource enm)
        {
            return enm.ToString();
        }
    }
}
