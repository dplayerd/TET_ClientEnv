using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_CostService.Enums
{
    /// <summary> 關卡名稱 </summary>
    public enum ApprovalLevel
    {
        /// <summary> 空白 </summary>
        Empty,

        /// <summary> 供應商表單覆審 </summary>
        SRI_SS_GL,

        /// <summary> BU 人員 </summary>
        BU,

        /// <summary> QSM </summary>
        QSM
    }

    /// <summary> 關卡名稱擴充 </summary>
    public static class ApprovalLevelExtension
    {
        /// <summary> 轉換為儲存用的文字
        /// </summary>
        /// <param name="enm"> 關卡名稱 </param>
        /// <returns></returns>
        public static string ToText(this ApprovalLevel enm)
        {
            switch (enm)
            {
                case ApprovalLevel.SRI_SS_GL:
                    return "PQM_SS_GL";

                case ApprovalLevel.BU:
                    return "BU 人員";

                case ApprovalLevel.QSM:
                    return "QSM";

                case ApprovalLevel.Empty:
                default:
                    return null;
            }
        }

        /// <summary> 轉換為顯示用的文字
        /// </summary>
        /// <param name="enm"> 關卡名稱 </param>
        /// <returns></returns>
        public static string ToDisplayText(this ApprovalLevel enm)
        {
            switch (enm)
            {
                case ApprovalLevel.SRI_SS_GL:
                    return "採購_SS's Supervisor";

                case ApprovalLevel.BU:
                    return "BU 人員";

                    case ApprovalLevel.QSM:
                    return "QSM";

                case ApprovalLevel.Empty:
                default:
                    return null;
            }
        }
    }
}
