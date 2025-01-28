using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_Violation.Enums
{
    /// <summary> 關卡名稱 </summary>
    public enum ApprovalLevel
    {
        /// <summary> 空白 </summary>
        Empty,

        /// <summary> 第一關審核者 </summary>
        Level_1,

        /// <summary> 第二關審核者 </summary>
        Level_2,

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
                case ApprovalLevel.Level_1:
                    return "Level_1";

                case ApprovalLevel.Level_2:
                    return "Level_2";

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
                case ApprovalLevel.Level_1:
                    return "第一關審核者";

                case ApprovalLevel.Level_2:
                    return "第二關審核者";

                    case ApprovalLevel.QSM:
                    return "QSM";

                case ApprovalLevel.Empty:
                default:
                    return null;
            }
        }
    }
}
