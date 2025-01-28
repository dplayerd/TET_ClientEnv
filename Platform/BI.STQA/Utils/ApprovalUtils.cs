using BI.STQA.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.STQA.Utils
{
    public class ApprovalUtils
    {
        #region ApprovalLevel
        /// <summary> ApprovalLevel 轉換錯誤訊息 </summary>
        public static string ParseApprovalLevelError = $"Level is required, and must be ['{ApprovalLevel.User_GL.ToText()}', '{ApprovalLevel.SRI_SS.ToText()}', {ApprovalLevel.SRI_SS_GL.ToText()}', '{ApprovalLevel.ACC_First.ToText()}', {ApprovalLevel.ACC_Second.ToText()}', '{ApprovalLevel.ACC_Last.ToText()}']";

        /// <summary> 文字轉為 ApprovalLevel </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static ApprovalLevel ParseApprovalLevel(string val)
        {
            if (string.IsNullOrWhiteSpace(val))
                return ApprovalLevel.Empty;

            ApprovalLevel enm;

            if (val == ApprovalLevel.User_GL.ToText())                  // 供應商表單審核
                enm = ApprovalLevel.User_GL;
            else if (val == ApprovalLevel.SRI_SS.ToText())              // 供應商表單初審
                enm = ApprovalLevel.SRI_SS;
            else if (val == ApprovalLevel.SRI_SS_GL.ToText())           // 供應商表單覆審
                enm = ApprovalLevel.SRI_SS_GL;
            else if (val == ApprovalLevel.ACC_First.ToText())           // ACC初審
                enm = ApprovalLevel.ACC_First;
            else if (val == ApprovalLevel.ACC_Second.ToText())          // ACC複審
                enm = ApprovalLevel.ACC_Second;
            else if (val == ApprovalLevel.ACC_Last.ToText())            // ACC覆核
                enm = ApprovalLevel.ACC_Last;
            else
                enm = ApprovalLevel.Empty;

            return enm;
        }
        #endregion

        #region ApprovalType
        /// <summary> ApprovalType 轉換錯誤訊息 </summary>
        public static string ParseApprovalTypeError = $"Type is required, and must be ['{ApprovalType.New.ToText()}', '{ApprovalType.Modify.ToText()}']";

        /// <summary> 文字轉為 ApprovalType </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static ApprovalType ParseApprovalType(string val)
        {
            if (string.IsNullOrWhiteSpace(val))
                return ApprovalType.Empty;

            ApprovalType enm;
            if (val == ApprovalType.New.ToText())                // 新增供應商審核
                enm = ApprovalType.New;
            else if (val == ApprovalType.Modify.ToText())        // 供應商改版審核
                enm = ApprovalType.Modify;
            else
                enm = ApprovalType.Empty;

            return enm;
        }
        #endregion

        #region ApprovalStatus
        /// <summary> ApprovalStatus 轉換錯誤訊息 </summary>
        public static string ParseApprovalStatusError = $"Result is required, and must be ['{ApprovalStatus.Verify.ToText()}', '{ApprovalStatus.Rejected.ToText()}', '{ApprovalStatus.Completed.ToText()}']";

        /// <summary> 文字轉為 ApprovalStatus </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static ApprovalStatus ParseApprovalStatus(string val)
        {
            if (string.IsNullOrWhiteSpace(val))
                return ApprovalStatus.Empty;

            // 將簽核結果轉換為 Enum
            ApprovalStatus enm;
            if (val == ApprovalStatus.Verify.ToText())              // 審核中
                enm = ApprovalStatus.Verify;
            else if (val == ApprovalStatus.Rejected.ToText())       // 已退回
                enm = ApprovalStatus.Rejected;
            else if (val == ApprovalStatus.Completed.ToText())      // 已完成
                enm = ApprovalStatus.Completed;
            else
                enm = ApprovalStatus.Empty;

            return enm;
        }
        #endregion

        #region ApprovalResult
        /// <summary> ApprovalType 轉換錯誤訊息 </summary>
        public static string ParseApprovalResultError = $"Result is required, and must be ['{ApprovalResult.Agree.ToText()}', '{ApprovalResult.RejectToPrev.ToText()}']";

        /// <summary> 文字轉為 ApprovalResult </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static ApprovalResult ParseApprovalResult(string val)
        {
            if (string.IsNullOrWhiteSpace(val))
                return ApprovalResult.Empty;

            // 將簽核結果轉換為 Enum
            ApprovalResult enm;
            if (val == ApprovalResult.Agree.ToText())              // 同意
                enm = ApprovalResult.Agree;
            else if (val == ApprovalResult.RejectToPrev.ToText())  // 退回上一關
                enm = ApprovalResult.RejectToPrev;
            else
                enm = ApprovalResult.Empty;

            return enm;
        }
        #endregion
    }
}
