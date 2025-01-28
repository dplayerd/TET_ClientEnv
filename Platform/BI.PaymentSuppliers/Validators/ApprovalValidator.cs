using BI.PaymentSuppliers.Enums;
using BI.PaymentSuppliers.Models;
using BI.PaymentSuppliers.Utils;
using Platform.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace BI.PaymentSuppliers.Validators
{
    /// <summary> 審核驗證 </summary>
    public class ApprovalValidator
    {
        private static string _ACC_Second = ApprovalLevel.ACC_Second.ToText();

        /// <summary> 設定資料 </summary>
        private static List<ApprovalValidConfig> _validConfigs = new List<ApprovalValidConfig>()
        {
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = "",          ColumnName = "ApplyReason",       ColumnTitle = "申請原因" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = "",          ColumnName = "RegisterDate",      ColumnTitle = "登錄日期" },
            new ApprovalValidConfig() { Disabled =  true, Required = true , EditAtLevel = _ACC_Second, ColumnName = "VenderCode",        ColumnTitle = "供應商代碼 " },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = "",          ColumnName = "CName",             ColumnTitle = "中文名稱" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = "",          ColumnName = "EName",             ColumnTitle = "英文名稱" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = "",          ColumnName = "Country",           ColumnTitle = "國家別" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = "",          ColumnName = "Address",           ColumnTitle = "公司地址" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = "",          ColumnName = "OfficeTel",         ColumnTitle = "公司電話" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = "",          ColumnName = "TaxNo",             ColumnTitle = "統一編號" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = "",          ColumnName = "IdNo",              ColumnTitle = "身分證字號" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = "",          ColumnName = "Charge",            ColumnTitle = "公司負責人" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = "",          ColumnName = "PaymentTerm",       ColumnTitle = "付款條件" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = "",          ColumnName = "Incoterms",         ColumnTitle = "交易條件" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = "",          ColumnName = "Remark",            ColumnTitle = "一般付款對象相關備註" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = "",          ColumnName = "BillingDocument",   ColumnTitle = "請款憑證" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = "",          ColumnName = "BankName",          ColumnTitle = "銀行名稱 " },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = "",          ColumnName = "BankCode",          ColumnTitle = "銀行代碼 " },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = "",          ColumnName = "BankBranchName",    ColumnTitle = "分行名稱 " },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = "",          ColumnName = "BankBranchCode",    ColumnTitle = "分行代碼" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = "",          ColumnName = "BankAccountNo",     ColumnTitle = "帳號 " },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = "",          ColumnName = "BankAccountName",   ColumnTitle = "帳戶名稱 " },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = "",          ColumnName = "Currency",          ColumnTitle = "幣別" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = "",          ColumnName = "BankCountry",       ColumnTitle = "銀行國別" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = "",          ColumnName = "BankAddress",       ColumnTitle = "銀行地址" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = "",          ColumnName = "SwiftCode",         ColumnTitle = "SWIFT CODE " },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = "",          ColumnName = "CompanyCity",       ColumnTitle = "公司註冊地城市" },
        };

        /// <summary> 取得全部設定 </summary>
        /// <returns></returns>
        public static List<ApprovalValidConfig> GetValidConfigs()
        {
            return _validConfigs;
        }

        /// <summary> 驗證必填 </summary>
        /// <param name="model"> 原資料 </param>
        /// <param name="levelName"> 關卡名稱 </param>
        /// <param name="msgList"> 錯誤訊息 </param>
        /// <returns></returns>
        public static bool ValidSupplier(TET_PaymentSupplierModel model, string levelName, out List<string> msgList)
        {
            Dictionary<string, string> dicMsg;
            var configs =
                (from config in _validConfigs
                 where
                     (config.Required && string.IsNullOrWhiteSpace(config.EditAtLevel)) ||
                     (string.Compare(levelName, config.EditAtLevel, StringComparison.OrdinalIgnoreCase) == 0)
                 select config).ToList();


            //--- 必填檢查先寫死，之後再整合 ---

            if ((string.Compare(levelName, ApprovalLevel.ACC_Second.ToText(), StringComparison.OrdinalIgnoreCase) == 0))
            {
                var sri_ss_RequiredColumn = new string[] { "VenderCode" };
                var items = configs.Where(obj => sri_ss_RequiredColumn.Contains(obj.Name)).ToList();
                foreach (var item in items)
                    item.Required = true;
            }
            //--- 必填檢查先寫死，之後再整合 ---


            var result = ColumnValidator.ValidProperty<TET_PaymentSupplierModel>(model, configs, out dicMsg);
            msgList = dicMsg.Values.ToList();
            return result;
        }

        /// <summary> 驗證新增資料 </summary>
        /// <param name="model"> 輸入資料 </param>
        /// <param name="msgList"> 錯誤訊息 </param>
        /// <returns></returns>
        public static bool Valid(TET_PaymentSupplierApprovalModel model, out List<string> msgList)
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

                if (result == ApprovalResult.RejectToPrev || result == ApprovalResult.RejectToStart)
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

