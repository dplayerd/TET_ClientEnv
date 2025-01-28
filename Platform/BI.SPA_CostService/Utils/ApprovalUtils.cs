using BI.SPA_CostService.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_CostService.Utils
{
    public class ApprovalUtils
    {
        #region ApprovalLevel
        /// <summary> ApprovalLevel 轉換錯誤訊息 </summary>
        public static string ParseApprovalLevelError = $"Level is required, and must be ['{ApprovalLevel.BU.ToText()}', '{ApprovalLevel.SRI_SS_GL.ToText()}']";

        /// <summary> 文字轉為 ApprovalLevel </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static ApprovalLevel ParseApprovalLevel(string val)
        {
            if (string.IsNullOrWhiteSpace(val))
                return ApprovalLevel.Empty;

            ApprovalLevel enm;

            if (val == ApprovalLevel.BU.ToText())               // BU 人員
                enm = ApprovalLevel.BU;
            else if (val == ApprovalLevel.QSM.ToText())         // QSM
                enm = ApprovalLevel.QSM;
            else if (val == ApprovalLevel.SRI_SS_GL.ToText())   // 供應商表單覆審
                enm = ApprovalLevel.SRI_SS_GL;
            else
                enm = ApprovalLevel.Empty;

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
        public static string ParseApprovalResultError = $"Result is required, and must be ['{ApprovalResult.Checked.ToText()}','{ApprovalResult.Agree.ToText()}', '{ApprovalResult.RejectToStart.ToText()}']";

        /// <summary> 文字轉為 ApprovalResult </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static ApprovalResult ParseApprovalResult(string val)
        {
            if (string.IsNullOrWhiteSpace(val))
                return ApprovalResult.Empty;

            // 將簽核結果轉換為 Enum
            ApprovalResult enm;
            if (val == ApprovalResult.Agree.ToText())               // 同意
                enm = ApprovalResult.Agree;
            //else if (val == ApprovalResult.RejectToPrev.ToText()) // 退回上一關
            //    enm = ApprovalResult.RejectToPrev;
            else if (val == ApprovalResult.RejectToStart.ToText())  // 退回申請人
                enm = ApprovalResult.RejectToStart;
            else if (val == ApprovalResult.Checked.ToText())        // 確認
                enm = ApprovalResult.Checked;
            else
                enm = ApprovalResult.Empty;

            return enm;
        }
        #endregion
    }
}
