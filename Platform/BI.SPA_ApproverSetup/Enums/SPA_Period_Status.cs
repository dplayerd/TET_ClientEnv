using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ApproverSetup.Enums
{
    /// <summary> 評鑑期間 的 評鑑狀態 </summary>
    public enum SPA_Period_Status
    {
        /// <summary> 空白 </summary>
        Empty = 0,

        /// <summary> 未開始 </summary>
        Ready = 1,

        /// <summary> 進行中 </summary>
        Executing = 2,

        /// <summary> 已完成 </summary>
        Completed = 3,
    }


    /// <summary> 評鑑狀態擴充 </summary>
    public static class SPA_Period_StatusExtension
    {
        /// <summary> 轉換為文字
        /// </summary>
        /// <param name="enm"> 評鑑狀態 </param>
        /// <returns></returns>
        public static string ToText(this SPA_Period_Status enm)
        {
            switch (enm)
            {
                case SPA_Period_Status.Ready:
                    return "未開始";

                case SPA_Period_Status.Executing:
                    return "進行中";

                case SPA_Period_Status.Completed:
                    return "已完成";

                default:
                    return null;
            }
        }
    }
}
