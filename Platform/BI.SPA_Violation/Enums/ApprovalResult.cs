using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_Violation.Enums
{
    /// <summary> 審核結果 </summary>
    public enum ApprovalResult
    {
        /// <summary> 空白 </summary>
        Empty,

        /// <summary> 同意 </summary>
        Agree,

        /// <summary> 退回申請者 </summary>
        RejectToStart,
    }

    /// <summary> 審核結果擴充 </summary>
    public static class ApprovalResultExtension
    {
        /// <summary> 轉換為文字
        /// </summary>
        /// <param name="enm"> 審核結果 </param>
        /// <returns></returns>
        public static string ToText(this ApprovalResult enm)
        {
            switch (enm)
            {
                case ApprovalResult.Agree:
                    return "同意";

                case ApprovalResult.RejectToStart:
                    return "退回申請者";

                case ApprovalResult.Empty:
                default:
                    return null;
            }
        }
    }
}
