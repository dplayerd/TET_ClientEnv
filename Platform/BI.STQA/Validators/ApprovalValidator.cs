using BI.STQA.Enums;
using BI.STQA.Models;
using BI.STQA.Utils;
using Platform.AbstractionClass;
using Platform.Infra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BI.STQA.Validators
{
    /// <summary> 審核驗證 </summary>
    public class ApprovalValidator
    {
        /// <summary> 設定資料 </summary>
        private static List<ValidateConfig> _validConfigs = new List<ValidateConfig>()
        {
            new ValidateConfig() { Required = false, CanEdit = false, Name = "BelongTo",     Title = "歸屬公司名稱" },
            new ValidateConfig() { Required = false, CanEdit = false, Name = "Purpose",      Title = "STQA理由" },
            new ValidateConfig() { Required = false, CanEdit = false, Name = "BusinessTerm", Title = "業務類別" },
            new ValidateConfig() { Required = false, CanEdit = false, Name = "Date",         Title = "完成日期" },
            new ValidateConfig() { Required = false, CanEdit = false, Name = "Type",         Title = "STQA方式" },
            new ValidateConfig() { Required = false, CanEdit = false, Name = "UnitALevel",   Title = "Unit-A Level" },
            new ValidateConfig() { Required = false, CanEdit = false, Name = "UnitCLevel",   Title = "Unit-C Level" },
            new ValidateConfig() { Required = false, CanEdit = false, Name = "UnitDLevel",   Title = "Unit-D Level" },
            new ValidateConfig() { Required = false, CanEdit = false, Name = "STQAComment",  Title = "備註" },
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
        public static bool Valid(TET_SupplierSTQAApprovalModel model, out List<string> msgList)
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
