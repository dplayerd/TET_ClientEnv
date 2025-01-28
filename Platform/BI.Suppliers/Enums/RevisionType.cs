using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Suppliers.Enums
{
    /// <summary> 是否有異動銀行資訊 </summary>
    public enum RevisionType
    {
        /// <summary> 空白 </summary>
        Empty,

        /// <summary> 未變更 </summary>
        Same,

        /// <summary> 已變更 </summary>
        Changed,
    }


    /// <summary> 是否有異動銀行資訊擴充 </summary>
    public static class RevisionTypeExtension
    {
        /// <summary> 轉換為文字
        /// </summary>
        /// <param name="enm"> RevisionType </param>
        /// <returns></returns>
        public static string ToText(this RevisionType enm)
        {
            switch (enm)
            {
                case RevisionType.Same:
                    return "1";

                case RevisionType.Changed:
                    return "2";

                case RevisionType.Empty:
                default:
                    return null;
            }
        }
    }
}
