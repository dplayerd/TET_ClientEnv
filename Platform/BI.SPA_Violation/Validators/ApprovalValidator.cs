using Platform.AbstractionClass;
using BI.SPA_Violation.Models;
using BI.SPA_Violation.Enums;
using BI.SPA_Violation.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_Violation.Validators
{
    public class ApprovalValidator
    {
        /// <summary> 設定資料 </summary>
        private static List<ValidateConfig> _validConfigs = new List<ValidateConfig>()
        {
            new ValidateConfig() { Required = false, CanEdit = false, Name = "Period",     Title = "評鑑期間" },
        };

        /// <summary> 取得全部設定 </summary>
        /// <returns></returns>
        public static List<ValidateConfig> GetValidConfigs()
        {
            return _validConfigs;
        }

        /// <summary> 驗證新增資料 </summary>
        /// <param name="model"> 輸入資料 </param>
        /// <param name="msgList"> 錯誤訊息 </param>
        /// <returns></returns>
        public static bool Valid(SPA_ViolationApprovalModel model, out List<string> msgList)
        {
            msgList = new List<string>();

            if (string.IsNullOrWhiteSpace(model.Result))
                msgList.Add("審核結果 為必填");
            else
            {
                // 將簽核結果轉換為 Enum
                ApprovalResult result = ApprovalUtils.ParseApprovalResult(model.Result);
                if (result == ApprovalResult.Empty)
                    throw new Exception(ApprovalUtils.ParseApprovalResultError);


                if (result == ApprovalResult.RejectToStart)
                {
                    if (string.IsNullOrWhiteSpace(model.Comment))
                        msgList.Add("審核意見 為必填");
                }
            }

            if (msgList.Count > 0)
                return false;
            else
                return true;
        }
    }
}
