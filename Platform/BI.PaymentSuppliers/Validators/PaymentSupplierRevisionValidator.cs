using BI.PaymentSuppliers.Enums;
using BI.PaymentSuppliers.Models;
using Platform.AbstractionClass;
using Platform.Infra;
using Platform.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BI.PaymentSuppliers.Validators
{
    public class PaymentSupplierRevisionValidator
    {
        private const string _bankCodeText = "必須為 3 碼數字";
        private const string _bankBranchCodeText = "必須為 4 碼數字";
        private const string _bankBranchCodeText1 = "必須為 6 碼數字";
        private const string _bankAccountNoText = "必須為數字";
        private const string _reqText = "為必填欄位";
        private const string _reqText1 = "必須填寫英文";
        //private const string _fixText_TW = "台灣";
        private const string _fixText_TW = "6E2CB503-4B76-4A54-9207-4CF602CDE54E";
        private const string _fixText_NTD = "EC937FAF-2CFF-410B-BEAE-3D5F2A798729";

        /// <summary> 設定資料 </summary>
        private static List<ValidateConfig> _validConfigs = new List<ValidateConfig>()
        {
                new ValidateConfig() { Required =  true, CanEdit =  true, Name = "ApplyReason",          Title = "申請原因" },
                new ValidateConfig() { Required = false, CanEdit = false, Name = "RegisterDate",         Title = "登錄日期" },
                new ValidateConfig() { Required = false, CanEdit = false, Name = "VenderCode",           Title = "供應商代碼" },
                new ValidateConfig() { Required =  true, CanEdit =  true, Name = "CName",                Title = "中文名稱" },
                new ValidateConfig() { Required =  true, CanEdit =  true, Name = "EName",                Title = "英文名稱" },
                new ValidateConfig() { Required = false, CanEdit = false, Name = "Country",              Title = "國家別" },
                new ValidateConfig() { Required = false, CanEdit = false, Name = "Address",              Title = "公司地址" },
                new ValidateConfig() { Required = false, CanEdit = false, Name = "OfficeTel",            Title = "公司電話" },
                new ValidateConfig() { Required =  true, CanEdit =  true, Name = "TaxNo",                Title = "統一編號" },
                new ValidateConfig() { Required = false, CanEdit = false, Name = "IdNo",                 Title = "身分證字號" },
                new ValidateConfig() { Required = false, CanEdit = false, Name = "Charge",               Title = "公司負責人" },
                new ValidateConfig() { Required = false, CanEdit = false, Name = "PaymentTerm",          Title = "付款條件" },
                new ValidateConfig() { Required = false, CanEdit = false, Name = "Incoterms",            Title = "交易條件" },
                new ValidateConfig() { Required = false, CanEdit = false, Name = "BillingDocument",      Title = "請款憑證" },
                new ValidateConfig() { Required = false, CanEdit = false, Name = "Remark",               Title = "一般付款對象相關備註" },
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
        public static bool Valid(TET_PaymentSupplierModel model, out List<string> msgList)
        {
            Dictionary<string, string> dicMsg;
            var configs = _validConfigs;

            var result = ColumnValidator.ValidProperty<TET_PaymentSupplierModel>(model, configs, out dicMsg);
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
        private static bool ValidSharedCondition(TET_PaymentSupplierModel model, out List<string> msgList)
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
            if (!string.IsNullOrWhiteSpace(model.BankCountry) && model.BankCountry == _fixText_TW)
            {
                var BankCode = _validConfigs.Where(obj => obj.Name == "BankCode").FirstOrDefault();
                if (model?.BankCode.Trim().Length != 3 || !int.TryParse(model?.BankCode.Trim(), out int tempInt))
                {
                    msgList.Add(BankCode.Title + _bankCodeText);
                }

                var BankBranchCode = _validConfigs.Where(obj => obj.Name == "BankBranchCode").FirstOrDefault();
                if(model?.BankCode.Trim() != "700")
                {
                    if (model?.BankBranchCode.Trim().Length != 4 || !int.TryParse(model?.BankBranchCode.Trim(), out tempInt))
                    {
                        msgList.Add(BankBranchCode.Title + _bankBranchCodeText);
                    }
                }
                else
                {
                    if (model?.BankBranchCode.Trim().Length != 6 || !int.TryParse(model?.BankBranchCode.Trim(), out tempInt))
                    {
                        msgList.Add(BankBranchCode.Title + _bankBranchCodeText1);
                    }
                }

                string pattern = "^[0-9]*$";
                var BankAccountNo = _validConfigs.Where(obj => obj.Name == "BankAccountNo").FirstOrDefault();
                if (!Regex.IsMatch(model?.BankAccountNo, pattern, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500)))
                {
                    msgList.Add(BankAccountNo.Title + _bankAccountNoText);
                }
            }

            //當匯款幣別選擇非NTD，需檢查公司地址、銀行地址、公司註冊地城市為英文
            if (!string.IsNullOrEmpty(model.Currency) && model.Currency != _fixText_NTD)
            {
                string pattern1 = "^[#.0-9a-zA-Z\\s,-]+$";

                var Address = _validConfigs.Where(obj => obj.Name == "Address").FirstOrDefault();
                if (!Regex.IsMatch(model?.Address, pattern1, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500)))
                {
                    msgList.Add(Address.Title + _reqText1);
                }

                var BankAddress = _validConfigs.Where(obj => obj.Name == "BankAddress").FirstOrDefault();
                if (!Regex.IsMatch(model?.BankAddress, pattern1, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500)))
                {
                    msgList.Add(BankAddress.Title + _reqText1);
                }

                var CompanyCity = _validConfigs.Where(obj => obj.Name == "CompanyCity").FirstOrDefault();
                if (!Regex.IsMatch(model?.CompanyCity, pattern1, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500)))
                {
                    msgList.Add(CompanyCity.Title + _reqText1);
                }
            }

            if (msgList.Count > 0)
                return false;

            return true;
        }
    }
}
