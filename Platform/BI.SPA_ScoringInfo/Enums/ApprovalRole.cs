using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ScoringInfo.Enums
{
    /// <summary> 審核角色 </summary>
    public enum ApprovalRole
    {
        /// <summary> 空白 </summary>
        Empty,

        /// <summary> 供應商表單審核 </summary>
        User_GL,

        /// <summary> 供應商表單初審 </summary>
        SRI_SS,

        /// <summary> 供應商表單審核 </summary>
        SRI_SS_Approval,

        /// <summary> 供應商表單覆審 </summary>
        SRI_SS_GL,

        /// <summary> ACC初審 </summary>
        ACC_First,

        /// <summary> ACC複審 </summary>
        ACC_Second,

        /// <summary> ACC覆核 </summary>
        ACC_Last,

        /// <summary> QSM </summary>
        QSM,

        /// <summary> QSM_GL </summary>
        QSM_GL,
    }

    /// <summary> 審核角色擴充 </summary>
    public static class ApprovalRoleExtension
    {
        /// <summary> 轉換為文字
        /// </summary>
        /// <param name="enm"> 審核角色 </param>
        /// <returns></returns>
        public static string ToText(this ApprovalRole enm)
        {
            switch (enm)
            {
                case ApprovalRole.User_GL:
                    return "User_GL";

                case ApprovalRole.SRI_SS:
                    return "SRI_SS審核";

                case ApprovalRole.SRI_SS_Approval:
                    return "SRI_SS_審核";

                case ApprovalRole.SRI_SS_GL:
                    return "SRI_SS_GL";

                case ApprovalRole.ACC_First:
                    return "ACC Data初審";

                case ApprovalRole.ACC_Second:
                    return "ACC Data複核";

                case ApprovalRole.ACC_Last:
                    return "ACC Data覆核";

                case ApprovalRole.QSM:
                    return "QSM";

                case ApprovalRole.QSM_GL:
                    return "QSM_GL";

                case ApprovalRole.Empty:
                default:
                    return null;
            }
        }

        /// <summary> 轉換為 ID
        /// </summary>
        /// <param name="enm"> 審核角色 </param>
        /// <returns></returns>
        public static Guid? ToID(this ApprovalRole enm)
        {
            switch (enm)
            {
                case ApprovalRole.SRI_SS:
                    return new Guid("429d57be-977c-48ed-983a-2746109dabfa");

                case ApprovalRole.SRI_SS_Approval:
                    return new Guid("fd530d27-dc90-48bb-ae9f-7264fc1d7626");

                case ApprovalRole.SRI_SS_GL:
                    return new Guid("D8F3D735-A9E1-47BC-8D49-D841CB751B29");

                case ApprovalRole.ACC_First:
                    return new Guid("583606B6-124A-4425-BAB3-70EF43911D20");

                case ApprovalRole.ACC_Second:
                    return new Guid("C3EDC1DF-7FAE-48C8-8AA3-80A1F0604D8B");

                case ApprovalRole.ACC_Last:
                    return new Guid("1E164E3B-D09C-4A32-B1CD-B5980387AD27");

                case ApprovalRole.QSM:
                    return new Guid("30910EBE-D964-4144-A61B-8CCDFF355FED");

                case ApprovalRole.QSM_GL:
                    return new Guid("AB5B56A0-C6F5-4871-A1C8-470543A9DB82");

                case ApprovalRole.User_GL:
                case ApprovalRole.Empty:
                default:
                    return null;
            }
        }
    }
}
