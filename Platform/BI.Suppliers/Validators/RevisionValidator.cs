using BI.Suppliers.Enums;
using BI.Suppliers.Models;
using Platform.AbstractionClass;
using Platform.Infra;
using Platform.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BI.Suppliers.Validators
{
    public class RevisionValidator
    {
        private const string _bankCodeText = "必須為 3 碼數字";
        private const string _bankBranchCodeText = "必須為 4 碼數字";
        private const string _bankAccountNoText = "必須為數字";
        //private const string _fixText_TW = "台灣";
        private const string _fixText_TW = "6E2CB503-4B76-4A54-9207-4CF602CDE54E";
        private const string _reqText = "為必填欄位";

        /// <summary> 設定資料 </summary>
        private static List<ValidateConfig> _validConfigs = new List<ValidateConfig>()
        {
                new ValidateConfig() { Required =  true, CanEdit =  true, Name = "ApplyReason",          Title = "申請原因" },
                new ValidateConfig() { Required = false, CanEdit = false, Name = "RegisterDate",         Title = "登錄日期" },
                new ValidateConfig() { Required = false, CanEdit = false, Name = "VenderCode",           Title = "供應商代碼" },
                new ValidateConfig() { Required = false, CanEdit =  true, Name = "BelongTo",             Title = "歸屬公司名稱" },
                new ValidateConfig() { Required = false, CanEdit =  true, Name = "SupplierCategory",     Title = "廠商類別" },
                new ValidateConfig() { Required = false, CanEdit =  true, Name = "BusinessCategory",     Title = "交易主類別" },
                new ValidateConfig() { Required = false, CanEdit =  true, Name = "BusinessAttribute",    Title = "交易子類別" },
                new ValidateConfig() { Required = false, CanEdit =  true, Name = "RelatedDept",          Title = "相關BU" },
                new ValidateConfig() { Required = false, CanEdit =  true, Name = "Buyer",                Title = "採購擔當" },
                new ValidateConfig() { Required =  true, CanEdit =  true, Name = "CName",                Title = "中文名稱" },
                new ValidateConfig() { Required = false, CanEdit = false, Name = "EName",                Title = "英文名稱" },
                new ValidateConfig() { Required = false, CanEdit = false, Name = "Country",              Title = "國家別" },
                new ValidateConfig() { Required = false, CanEdit = false, Name = "Address",              Title = "公司地址" },
                new ValidateConfig() { Required = false, CanEdit = false, Name = "OfficeTel",            Title = "公司電話" },
                new ValidateConfig() { Required =  true, CanEdit =  true, Name = "TaxNo",                Title = "統一編號" },
                new ValidateConfig() { Required = false, CanEdit = false, Name = "Email",                Title = "公司E-mail" },
                new ValidateConfig() { Required = false, CanEdit = false, Name = "Website",              Title = "公司網站" },
                new ValidateConfig() { Required = false, CanEdit = false, Name = "ISO",                  Title = "ISO認證" },
                new ValidateConfig() { Required = false, CanEdit = false, Name = "CapitalAmount",        Title = "資本額" },
                new ValidateConfig() { Required = false, CanEdit =  true, Name = "Charge",               Title = "公司負責人" },
                new ValidateConfig() { Required = false, CanEdit = false, Name = "Employees",            Title = "員工人數" },
                new ValidateConfig() { Required = false, CanEdit =  true, Name = "PaymentTerm",          Title = "付款條件" },
                new ValidateConfig() { Required = false, CanEdit = false, Name = "Incoterms",            Title = "交易條件" },
                new ValidateConfig() { Required = false, CanEdit = false, Name = "BillingDocument",      Title = "請款憑證" },
                new ValidateConfig() { Required = false, CanEdit = false, Name = "MainProduct",          Title = "主要產品/服務項目" },
                new ValidateConfig() { Required = false, CanEdit = false, Name = "Remark",               Title = "供應商相關備註" },
                new ValidateConfig() { Required =  true, CanEdit =  true, Name = "BankName",             Title = "銀行名稱" },
                new ValidateConfig() { Required =  true, CanEdit =  true, Name = "BankCode",             Title = "銀行代碼" },
                new ValidateConfig() { Required =  true, CanEdit =  true, Name = "BankBranchName",       Title = "分行名稱" },
                new ValidateConfig() { Required =  true, CanEdit =  true, Name = "BankBranchCode",       Title = "分行代碼" },
                new ValidateConfig() { Required =  true, CanEdit =  true, Name = "BankAccountNo",        Title = "帳號" },
                new ValidateConfig() { Required =  true, CanEdit =  true, Name = "BankAccountName",      Title = "帳戶名稱" },
                new ValidateConfig() { Required =  true, CanEdit =  true, Name = "Currency",             Title = "幣別" },
                new ValidateConfig() { Required =  true, CanEdit =  true, Name = "BankCountry",          Title = "銀行國別" },
                new ValidateConfig() { Required = false, CanEdit =  true, Name = "BankAddress",          Title = "銀行地址" },
                new ValidateConfig() { Required = false, CanEdit =  true, Name = "SwiftCode",            Title = "SWIFT CODE" },
                new ValidateConfig() { Required = false, CanEdit =  true, Name = "CompanyCity",          Title = "公司註冊地城市" },
                new ValidateConfig() { Required = false, CanEdit =  true, Name = "NDANo",                Title = "NDA審查書號碼" },
                new ValidateConfig() { Required = false, CanEdit =  true, Name = "Contract",             Title = "合約(Y/N)" },
                new ValidateConfig() { Required = false, CanEdit = false, Name = "KeySupplier",          Title = "主要供應商" },
                new ValidateConfig() { Required = false, CanEdit = false, Name = "SignDate1",            Title = "行為準則承諾書簽屬日期" },
                new ValidateConfig() { Required = false, CanEdit = false, Name = "SignDate2",            Title = "承攬商安全衛生環保承諾書簽屬日期" },
        };

        /// <summary> 取得全部設定 </summary>
        /// <returns></returns>
        public static List<ValidateConfig> GetValidConfigs()
        {
            return _validConfigs;
        }


        /// <summary> 驗證必填 </summary>
        /// <param name="model"> 原資料 </param>
        /// <param name="msgList"> 錯誤訊息 </param>
        /// <returns></returns>
        public static bool Valid(TET_SupplierModel model, out List<string> msgList)
        {
            Dictionary<string, string> dicMsg;
            var configs = _validConfigs;

            var result = ColumnValidator.ValidProperty<TET_SupplierModel>(model, configs, out dicMsg);
            msgList = dicMsg.Values.ToList();


            var BankResult = ValidSharedCondition(model, out List<string> temp);
            if (!BankResult)
            {
                msgList.AddRange(temp);
                result = false;
            }

            return result;
        }


        /// <summary> 共用檢查條件 </summary>
        /// <param name="model"></param>
        /// <param name="msgList"></param>
        private static bool ValidSharedCondition(TET_SupplierModel model, out List<string> msgList)
        {
            msgList = new List<string>();
       
            // 當銀行國別選擇非台灣，儲存時需檢查必填。
            // 銀行地址
            // SWIFT CODE
            // 公司註冊地城市
            if (!string.IsNullOrWhiteSpace(model.BankCountry) && model.BankCountry.ToUpper() != _fixText_TW.ToUpper())
            {
                if (string.IsNullOrWhiteSpace(model.BankAddress))
                {
                    var BankAddress = _validConfigs.Where(obj => obj.Name == "BankAddress").FirstOrDefault();
                    if (BankAddress != null)
                        msgList.Add(BankAddress.Title + _reqText);
                }

                if (string.IsNullOrWhiteSpace(model.SwiftCode))
                {
                    var SwiftCode = _validConfigs.Where(obj => obj.Name == "SwiftCode").FirstOrDefault();
                    if (SwiftCode != null)
                        msgList.Add(SwiftCode.Title + _reqText);
                }

                if (string.IsNullOrWhiteSpace(model.CompanyCity))
                {
                    var CompanyCity = _validConfigs.Where(obj => obj.Name == "CompanyCity").FirstOrDefault();
                    if (CompanyCity != null)
                        msgList.Add(CompanyCity.Title + _reqText);
                }
            }

            // 若銀行國別為台灣，需檢查銀行代碼為3碼數字、分行代碼為四碼數字
            if (!string.IsNullOrWhiteSpace(model.BankCountry) && model.BankCountry.ToUpper() == _fixText_TW.ToUpper())
            {
                var BankCode = _validConfigs.Where(obj => obj.Name == "BankCode").FirstOrDefault();
                if (model?.BankCode.Trim().Length != 3 || !int.TryParse(model?.BankCode.Trim(), out int tempInt))
                {
                    msgList.Add(BankCode.Title + _bankCodeText);
                }

                var BankBranchCode = _validConfigs.Where(obj => obj.Name == "BankBranchCode").FirstOrDefault();
                if (model?.BankBranchCode.Trim().Length != 4 || !int.TryParse(model?.BankBranchCode.Trim(), out tempInt))
                {
                    msgList.Add(BankBranchCode.Title + _bankBranchCodeText);
                }

                string pattern = "^[0-9]*$";
                var BankAccountNo = _validConfigs.Where(obj => obj.Name == "BankAccountNo").FirstOrDefault();
                if (!Regex.IsMatch(model?.BankAccountNo, pattern, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500)))
                {
                    msgList.Add(BankAccountNo.Title + _bankAccountNoText);
                }
            }

            if (msgList.Count > 0)
                return false;

            return true;
        }
    }
}
