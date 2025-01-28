using BI.SPA.Enums;
using BI.SPA.Models;
using BI.SPA.Utils;
using Platform.AbstractionClass;
using Platform.Infra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BI.SPA.Validators
{
    /// <summary> 審核驗證 </summary>
    public class ApprovalValidator
    {
        /// <summary> 設定資料 </summary>
        private static List<ValidateConfig> _validConfigs = new List<ValidateConfig>()
        {
            new ValidateConfig() { Required = false, CanEdit = false, Name = "BelongTo",          Title = "供應商" },
            new ValidateConfig() { Required = false, CanEdit = false, Name = "Period",            Title = "評鑑期間" },
            new ValidateConfig() { Required = false, CanEdit = false, Name = "BU",                Title = "評鑑單位" },
            new ValidateConfig() { Required = false, CanEdit = false, Name = "ServiceFor",        Title = "服務對象" },
            new ValidateConfig() { Required = false, CanEdit = false, Name = "AssessmentItem",    Title = "評鑑項目" },
            new ValidateConfig() { Required = false, CanEdit = false, Name = "PerformanceLevel",  Title = "Performance Level" },
            new ValidateConfig() { Required = false, CanEdit = false, Name = "TotalScore",        Title = "Total Score" },
            new ValidateConfig() { Required = false, CanEdit = false, Name = "TScore",            Title = "Technology Score" },
            new ValidateConfig() { Required = false, CanEdit = false, Name = "DScore",            Title = "Delivery Score" },
            new ValidateConfig() { Required = false, CanEdit = false, Name = "QScore",            Title = "Quality Score" },
            new ValidateConfig() { Required = false, CanEdit = false, Name = "CScore",            Title = "Cost Score" },
            new ValidateConfig() { Required = false, CanEdit = false, Name = "SScore",            Title = "Service Score" },
            new ValidateConfig() { Required = false, CanEdit = false, Name = "SPAComment",        Title = "備註" },
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
        public static bool Valid(TET_SupplierSPAApprovalModel model, out List<string> msgList)
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


                if (result == ApprovalResult.RejectToPrev)
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
