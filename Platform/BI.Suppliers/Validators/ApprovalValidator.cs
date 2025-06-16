using BI.Suppliers.Enums;
using BI.Suppliers.Models;
using BI.Suppliers.Utils;
using Platform.AbstractionClass;
using Platform.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace BI.Suppliers.Validators
{
    /// <summary> 審核驗證 </summary>
    public class ApprovalValidator
    {
        private static string _SRI_SS = ApprovalLevel.SRI_SS.ToText();
        private static string _SRI_SS_GL = ApprovalLevel.SRI_SS_GL.ToText();
        private static string _ACC_Second = ApprovalLevel.ACC_Second.ToText();

        /// <summary> 設定資料 </summary>
        private static List<ApprovalValidConfig> _validConfigs = new List<ApprovalValidConfig>()
        {
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = "",          ColumnName = "IsSecret",          ColumnTitle = "是否機密揭露" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = "",          ColumnName = "IsNDA",             ColumnTitle = "是否簽屬相關NDA" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = "",          ColumnName = "ApplyReason",       ColumnTitle = "申請原因" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = "",          ColumnName = "RegisterDate",      ColumnTitle = "登錄日期" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = _ACC_Second, ColumnName = "VenderCode",        ColumnTitle = "供應商代碼 " },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = _SRI_SS,     ColumnName = "BelongTo",          ColumnTitle = "歸屬公司名稱" },
            new ApprovalValidConfig() { Disabled =  true, Required =  true, EditAtLevel = _SRI_SS,     ColumnName = "SupplierCategory",  ColumnTitle = "廠商類別" },
            new ApprovalValidConfig() { Disabled =  true, Required =  true, EditAtLevel = _SRI_SS,     ColumnName = "BusinessCategory",  ColumnTitle = "交易主類別" },
            new ApprovalValidConfig() { Disabled =  true, Required =  true, EditAtLevel = _SRI_SS,     ColumnName = "BusinessAttribute", ColumnTitle = "交易子類別" },
            new ApprovalValidConfig() { Disabled =  true, Required =  true, EditAtLevel = _SRI_SS,     ColumnName = "RelatedDept",       ColumnTitle = "相關BU" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = _SRI_SS,     ColumnName = "Buyer",             ColumnTitle = "採購擔當" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = _SRI_SS,     ColumnName = "SearchKey",         ColumnTitle = "關鍵字 " },
            new ApprovalValidConfig() { Disabled =  true, Required =  true, EditAtLevel = _SRI_SS,     ColumnName = "CName",             ColumnTitle = "中文名稱" },
            new ApprovalValidConfig() { Disabled =  true, Required =  true, EditAtLevel = _SRI_SS,     ColumnName = "EName",             ColumnTitle = "英文名稱" },
            new ApprovalValidConfig() { Disabled =  true, Required =  true, EditAtLevel = _SRI_SS,     ColumnName = "Country",           ColumnTitle = "國家別" },
            new ApprovalValidConfig() { Disabled =  true, Required =  true, EditAtLevel = _SRI_SS,     ColumnName = "Address",           ColumnTitle = "公司地址" },
            new ApprovalValidConfig() { Disabled =  true, Required =  true, EditAtLevel = _SRI_SS,     ColumnName = "Address",           ColumnTitle = "公司地址" },
            new ApprovalValidConfig() { Disabled =  true, Required =  true, EditAtLevel = _SRI_SS,     ColumnName = "OfficeTel",         ColumnTitle = "公司電話" },
            new ApprovalValidConfig() { Disabled =  true, Required =  true, EditAtLevel = _SRI_SS,     ColumnName = "TaxNo",             ColumnTitle = "統一編號" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = _SRI_SS,     ColumnName = "Email",             ColumnTitle = "公司E-mail" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = _SRI_SS,     ColumnName = "Website",           ColumnTitle = "公司網站" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = _SRI_SS,     ColumnName = "ISO",               ColumnTitle = "ISO認證" },
            new ApprovalValidConfig() { Disabled =  true, Required =  true, EditAtLevel = _SRI_SS,     ColumnName = "CapitalAmount",     ColumnTitle = "資本額" },
            new ApprovalValidConfig() { Disabled =  true, Required =  true, EditAtLevel = _SRI_SS,     ColumnName = "Charge",            ColumnTitle = "公司負責人" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = _SRI_SS,     ColumnName = "Employees",         ColumnTitle = "員工人數" },
            new ApprovalValidConfig() { Disabled =  true, Required =  true, EditAtLevel = _SRI_SS,     ColumnName = "PaymentTerm",       ColumnTitle = "付款條件" },
            new ApprovalValidConfig() { Disabled =  true, Required =  true, EditAtLevel = _SRI_SS,     ColumnName = "Incoterms",         ColumnTitle = "交易條件" },
            new ApprovalValidConfig() { Disabled =  true, Required =  true, EditAtLevel = "",          ColumnName = "BillingDocument",   ColumnTitle = "請款憑證" },
            new ApprovalValidConfig() { Disabled =  true, Required =  true, EditAtLevel = "",          ColumnName = "MainProduct",       ColumnTitle = "主要產品/服務項目 " },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = "",          ColumnName = "Remark",            ColumnTitle = "供應商相關備註" },
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
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = _SRI_SS,     ColumnName = "NDANo",             ColumnTitle = "保密條款相關合約審查書號碼" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = _SRI_SS,     ColumnName = "Contract",          ColumnTitle = "合約(Y/N)" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = _SRI_SS,     ColumnName = "KeySupplier",       ColumnTitle = "主要供應商" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = _SRI_SS,     ColumnName = "IsSign1",           ColumnTitle = "行為準則承諾書" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = _SRI_SS,     ColumnName = "SignDate1",         ColumnTitle = "行為準則承諾書簽屬日期" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = _SRI_SS,     ColumnName = "IsSign2",           ColumnTitle = "承攬商安全衛生環保承諾書" },
            new ApprovalValidConfig() { Disabled =  true, Required = false, EditAtLevel = _SRI_SS,     ColumnName = "SignDate2",         ColumnTitle = "承攬商安全衛生環保承諾書簽屬日期" },
            new ApprovalValidConfig() { Disabled =  true, Required =  true, EditAtLevel = _SRI_SS_GL,  ColumnName = "STQAApplication",   ColumnTitle = "STQA Application" },
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
        public static bool ValidSupplier(TET_SupplierModel model, string levelName, out List<string> msgList)
        {
            Dictionary<string, string> dicMsg;
            var configs =
                (from config in _validConfigs
                 where
                     (config.Required && string.IsNullOrWhiteSpace(config.EditAtLevel)) ||
                     (string.Compare(levelName, config.EditAtLevel, StringComparison.OrdinalIgnoreCase) == 0)
                 select config).ToList();


            //--- 必填檢查先寫死，之後再整合 ---
            if ((string.Compare(levelName, ApprovalLevel.SRI_SS.ToText(), StringComparison.OrdinalIgnoreCase) == 0))
            {
                var sri_ss_RequiredColumn = new string[] { "BelongTo", "RelatedDept" };
                var items = configs.Where(obj => sri_ss_RequiredColumn.Contains(obj.Name)).ToList();
                foreach (var item in items)
                    item.Required = true;
            }

            if ((string.Compare(levelName, ApprovalLevel.ACC_Second.ToText(), StringComparison.OrdinalIgnoreCase) == 0))
            {
                var sri_ss_RequiredColumn = new string[] { "VenderCode" };
                var items = configs.Where(obj => sri_ss_RequiredColumn.Contains(obj.Name)).ToList();
                foreach (var item in items)
                    item.Required = true;
            }
            //--- 必填檢查先寫死，之後再整合 ---


            var result = ColumnValidator.ValidProperty<TET_SupplierModel>(model, configs, out dicMsg);
            msgList = dicMsg.Values.ToList();
            return result;
        }

        /// <summary> 驗證新增資料 </summary>
        /// <param name="model"> 輸入資料 </param>
        /// <param name="msgList"> 錯誤訊息 </param>
        /// <returns></returns>
        public static bool Valid(TET_SupplierApprovalModel model, out List<string> msgList)
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

