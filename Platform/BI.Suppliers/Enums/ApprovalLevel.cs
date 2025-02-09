using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Suppliers.Enums
{
    /// <summary> 關卡名稱 </summary>
    public enum ApprovalLevel
    {
        /// <summary> 空白 </summary>
        Empty,

        /// <summary> 供應商表單審核 </summary>
        User_GL,

        /// <summary> 供應商表單初審 </summary>
        SRI_SS,

        /// <summary> 供應商表單覆審 </summary>
        SRI_SS_GL,

        /// <summary> ACC初審 </summary>
        ACC_First,

        /// <summary> ACC複審 </summary>
        ACC_Second,

        /// <summary> ACC覆核 </summary>
        ACC_Last
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
                case ApprovalLevel.User_GL:
                    return "User_GL";

                case ApprovalLevel.SRI_SS:
                    return "PQM_SS";

                case ApprovalLevel.SRI_SS_GL:
                    return "PQM_SS_GL";

                case ApprovalLevel.ACC_First:
                    return "ACC初審";

                case ApprovalLevel.ACC_Second:
                    return "ACC複審";

                case ApprovalLevel.ACC_Last:
                    return "ACC覆核";

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
                case ApprovalLevel.User_GL:
                    return "User's Supervisor";

                case ApprovalLevel.SRI_SS:
                    return "採購_SS";

                case ApprovalLevel.SRI_SS_GL:
                    return "採購_SS's Supervisor";

                case ApprovalLevel.ACC_First:
                    return "ACC初審";

                case ApprovalLevel.ACC_Second:
                    return "ACC複審";

                case ApprovalLevel.ACC_Last:
                    return "ACC覆核";

                case ApprovalLevel.Empty:
                default:
                    return null;
            }
        }
    }
}
