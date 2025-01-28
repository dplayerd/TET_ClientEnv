using BI.Suppliers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace BI.Suppliers.Validators
{
    /// <summary> 供應商欄位驗證設定 </summary>
    public class SupplierValidator
    {
        private const string _bankCodeText = "必須為 3 碼數字";
        private const string _bankBranchCodeText = "必須為 4 碼數字";
        private const string _bankAccountNoText = "必須為數字";
        private const string _reqText = "為必填欄位";
        //private const string _fixText_TW = "台灣";
        private const string _fixText_TW = "6E2CB503-4B76-4A54-9207-4CF602CDE54E";
        private const string _fixText_YES = "YES";


        /// <summary> 設定資料 </summary>
        private static List<ValidConfig> _validConfigs = new List<ValidConfig>()
        {
            new ValidConfig() { RequiredOnCreate = false, RequiredOnModify = false, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "CoSignApprover",  ColumnTitle = "加簽人員" },
            new ValidConfig() { RequiredOnCreate =  true, RequiredOnModify =  true, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "IsSecret",        ColumnTitle = "是否機密揭露" },
            new ValidConfig() { RequiredOnCreate =  true, RequiredOnModify =  true, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "IsNDA",           ColumnTitle = "是否簽屬相關NDA" },
            new ValidConfig() { RequiredOnCreate =  true, RequiredOnModify =  true, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "ApplyReason",     ColumnTitle = "申請原因" },
            new ValidConfig() { RequiredOnCreate = false, RequiredOnModify = false, ShowOnCreate = false, ShowOnModify =  true, ColumnName = "BelongTo",        ColumnTitle = "歸屬公司名稱" },
            new ValidConfig() { RequiredOnCreate = false, RequiredOnModify = false, ShowOnCreate = false, ShowOnModify =  true, ColumnName = "VenderCode",      ColumnTitle = "供應商代碼" },
            new ValidConfig() { RequiredOnCreate =  true, RequiredOnModify =  true, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "SupplierCategory", ColumnTitle = "廠商類別" },
            new ValidConfig() { RequiredOnCreate =  true, RequiredOnModify =  true, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "BusinessCategory", ColumnTitle = "交易主類別" },
            new ValidConfig() { RequiredOnCreate =  true, RequiredOnModify =  true, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "BusinessAttribute", ColumnTitle = "交易子類別" },
            new ValidConfig() { RequiredOnCreate = false, RequiredOnModify = false, ShowOnCreate = false, ShowOnModify =  true, ColumnName = "SearchKey",       ColumnTitle = "關鍵字" },
            new ValidConfig() { RequiredOnCreate = false, RequiredOnModify = false, ShowOnCreate = false, ShowOnModify =  true, ColumnName = "RelatedDept",     ColumnTitle = "相關BU" },
            new ValidConfig() { RequiredOnCreate = false, RequiredOnModify = false, ShowOnCreate = false, ShowOnModify =  true, ColumnName = "Buyer",           ColumnTitle = "採購擔當" },
            new ValidConfig() { RequiredOnCreate = false, RequiredOnModify = false, ShowOnCreate = false, ShowOnModify =  true, ColumnName = "RegisterDate",    ColumnTitle = "登錄日期" },
            new ValidConfig() { RequiredOnCreate =  true, RequiredOnModify =  true, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "CName",           ColumnTitle = "供應商中文名稱" },
            new ValidConfig() { RequiredOnCreate =  true, RequiredOnModify =  true, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "EName",           ColumnTitle = "供應商英文名稱" },
            new ValidConfig() { RequiredOnCreate =  true, RequiredOnModify =  true, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "Country",         ColumnTitle = "國家別" },
            new ValidConfig() { RequiredOnCreate =  true, RequiredOnModify =  true, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "TaxNo",           ColumnTitle = "統一編號" },
            new ValidConfig() { RequiredOnCreate =  true, RequiredOnModify =  true, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "Address",         ColumnTitle = "公司地址" },
            new ValidConfig() { RequiredOnCreate =  true, RequiredOnModify =  true, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "OfficeTel",       ColumnTitle = "公司電話" },
            new ValidConfig() { RequiredOnCreate = false, RequiredOnModify = false, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "ISO",             ColumnTitle = "ISO認證" },
            new ValidConfig() { RequiredOnCreate = false, RequiredOnModify = false, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "Email",           ColumnTitle = "公司Email" },
            new ValidConfig() { RequiredOnCreate = false, RequiredOnModify = false, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "Website",         ColumnTitle = "公司網站" },
            new ValidConfig() { RequiredOnCreate =  true, RequiredOnModify =  true, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "CapitalAmount",   ColumnTitle = "資本額" },
            new ValidConfig() { RequiredOnCreate =  true, RequiredOnModify =  true, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "MainProduct",     ColumnTitle = "主要產品/服務項目" },
            new ValidConfig() { RequiredOnCreate = false, RequiredOnModify = false, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "Employees",       ColumnTitle = "員工人數" },
            new ValidConfig() { RequiredOnCreate =  true, RequiredOnModify =  true, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "Charge",          ColumnTitle = "公司負責人" },
            new ValidConfig() { RequiredOnCreate =  true, RequiredOnModify =  true, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "PaymentTerm",     ColumnTitle = "付款條件" },
            new ValidConfig() { RequiredOnCreate =  true, RequiredOnModify =  true, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "BillingDocument", ColumnTitle = "請款憑證" },
            new ValidConfig() { RequiredOnCreate =  true, RequiredOnModify =  true, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "Incoterms",       ColumnTitle = "交易條件" },
            new ValidConfig() { RequiredOnCreate = false, RequiredOnModify = false, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "Remark",          ColumnTitle = "供應商相關備註" },
            new ValidConfig() { RequiredOnCreate =  true, RequiredOnModify =  true, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "BankCountry",     ColumnTitle = "銀行國別" },
            new ValidConfig() { RequiredOnCreate =  true, RequiredOnModify =  true, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "BankName",        ColumnTitle = "銀行名稱" },
            new ValidConfig() { RequiredOnCreate =  true, RequiredOnModify =  true, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "BankCode",        ColumnTitle = "銀行代碼" },
            new ValidConfig() { RequiredOnCreate =  true, RequiredOnModify =  true, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "BankBranchName",  ColumnTitle = "分行名稱" },
            new ValidConfig() { RequiredOnCreate =  true, RequiredOnModify =  true, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "BankBranchCode",  ColumnTitle = "分行代碼" },
            new ValidConfig() { RequiredOnCreate =  true, RequiredOnModify =  true, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "Currency",        ColumnTitle = "匯款幣別" },
            new ValidConfig() { RequiredOnCreate =  true, RequiredOnModify =  true, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "BankAccountName", ColumnTitle = "帳戶名稱" },
            new ValidConfig() { RequiredOnCreate =  true, RequiredOnModify =  true, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "BankAccountNo",   ColumnTitle = "帳號" },
            new ValidConfig() { RequiredOnCreate = false, RequiredOnModify = false, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "CompanyCity",     ColumnTitle = "公司註冊地城市" },
            new ValidConfig() { RequiredOnCreate = false, RequiredOnModify = false, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "BankAddress",     ColumnTitle = "銀行地址" },
            new ValidConfig() { RequiredOnCreate = false, RequiredOnModify = false, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "SwiftCode",       ColumnTitle = "SWIFT CODE" },
            new ValidConfig() { RequiredOnCreate = false, RequiredOnModify = false, ShowOnCreate =  true, ShowOnModify =  true, ColumnName = "NDANo",           ColumnTitle = "NDA號碼" },
        };

        /// <summary> 取得全部設定 </summary>
        /// <returns></returns>
        public static List<ValidConfig> GetValidConfigs()
        {
            return _validConfigs;
        }

        /// <summary> 驗證新增資料 </summary>
        /// <param name="model"> 輸入資料 </param>
        /// <param name="msgList"> 錯誤訊息 </param>
        /// <returns></returns>
        public static bool ValidCreate(TET_SupplierModel model, out List<string> msgList)
        {
            var result = CheckRequired(model, true, out msgList);
            ValidSharedCondition(model, msgList);

            if (msgList.Count > 0)
                return false;
            else
                return true;
        }


        /// <summary> 驗證編輯資料 </summary>
        /// <param name="model"> 輸入資料 </param>
        /// <param name="msgList"> 錯誤訊息 </param>
        /// <returns></returns>
        public static bool ValidModify(TET_SupplierModel model, out List<string> msgList)
        {
            var result = CheckRequired(model, false, out msgList);
            ValidSharedCondition(model, msgList);

            if (msgList.Count > 0)
                return false;
            else
                return true;
        }


        /// <summary> 共用檢查條件 </summary>
        /// <param name="model"></param>
        /// <param name="msgList"></param>
        private static void ValidSharedCondition(TET_SupplierModel model, List<string> msgList)
        {
            // 當是否已簽屬相關NDA(保密協議)或於相關往來合約中註明保密條款?欄位選擇Yes，此欄位必填
            if (!string.IsNullOrWhiteSpace(model.IsNDA) && model.IsNDA == _fixText_YES && string.IsNullOrWhiteSpace(model.NDANo))
            {
                var NdaNo = _validConfigs.Where(obj => obj.ColumnName == "NDANo").FirstOrDefault();
                if (NdaNo != null)
                    msgList.Add(NdaNo.ColumnTitle + _reqText);
            }

            // 當銀行國別選擇非台灣，儲存時需檢查必填。
            // 銀行地址
            // SWIFT CODE
            // 公司註冊地城市
            if (!string.IsNullOrWhiteSpace(model.BankCountry) && model.BankCountry.ToUpper() != _fixText_TW.ToUpper())
            {
                if (string.IsNullOrWhiteSpace(model.BankAddress))
                {
                    var BankAddress = _validConfigs.Where(obj => obj.ColumnName == "BankAddress").FirstOrDefault();
                    if (BankAddress != null)
                        msgList.Add(BankAddress.ColumnTitle + _reqText);
                }

                if (string.IsNullOrWhiteSpace(model.SwiftCode))
                {
                    var SwiftCode = _validConfigs.Where(obj => obj.ColumnName == "SwiftCode").FirstOrDefault();
                    if (SwiftCode != null)
                        msgList.Add(SwiftCode.ColumnTitle + _reqText);
                }

                if (string.IsNullOrWhiteSpace(model.CompanyCity))
                {
                    var CompanyCity = _validConfigs.Where(obj => obj.ColumnName == "CompanyCity").FirstOrDefault();
                    if (CompanyCity != null)
                        msgList.Add(CompanyCity.ColumnTitle + _reqText);
                }
            }

            // 若銀行國別為台灣，需檢查銀行代碼為3碼數字、分行代碼為四碼數字
            if(!string.IsNullOrWhiteSpace(model.BankCountry) && model.BankCountry.ToUpper() == _fixText_TW.ToUpper())
            {
                var BankCode = _validConfigs.Where(obj => obj.ColumnName == "BankCode").FirstOrDefault();
                if (model?.BankCode.Trim().Length != 3 || !int.TryParse(model?.BankCode.Trim(), out int tempInt))
                {
                    msgList.Add(BankCode.ColumnTitle + _bankCodeText);
                }
                
                var BankBranchCode = _validConfigs.Where(obj => obj.ColumnName == "BankBranchCode").FirstOrDefault();
                if (model?.BankBranchCode.Trim().Length != 4 || !int.TryParse(model?.BankBranchCode.Trim(), out tempInt))
                {
                    msgList.Add(BankBranchCode.ColumnTitle + _bankBranchCodeText);
                }

                string pattern = "^[0-9]*$";
                var BankAccountNo = _validConfigs.Where(obj => obj.ColumnName == "BankAccountNo").FirstOrDefault();
                if (!Regex.IsMatch(model?.BankAccountNo, pattern, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500)))
                {
                    msgList.Add(BankAccountNo.ColumnTitle + _bankAccountNoText);
                }
            }
        }


        /// <summary> 根據設定，驗證每個欄位是否都有填值 </summary>
        /// <param name="model"> 原始資料 </param>
        /// <param name="isCreateMode"> 是否為新增模式 </param>
        /// <param name="msgList"> 錯誤訊息 </param>
        /// <returns></returns>
        private static bool CheckRequired(TET_SupplierModel model, bool isCreateMode, out List<string> msgList)
        {
            msgList = new List<string>();
            PropertyInfo[] properties = typeof(TET_SupplierModel).GetProperties();

            // 依設定驗證每個欄位
            foreach (var config in _validConfigs)
            {
                var prop = properties.Where(obj => obj.Name == config.ColumnName).FirstOrDefault();

                // 如果是新增模式，且必填
                // 如果是編輯模式，且必填
                bool isCheckInThisMode =
                    (isCreateMode && config.RequiredOnCreate) ||
                    (!isCreateMode && config.RequiredOnModify);

                // 檢查屬性值
                if (isCheckInThisMode)
                {
                    if (prop.PropertyType == typeof(string))
                    {
                        string value = (string)prop.GetValue(model);
                        if (string.IsNullOrEmpty(value))
                        {
                            msgList.Add(config.ColumnTitle + _reqText);
                        }
                    }
                    else if (prop.PropertyType == typeof(string[]))
                    {
                        string[] value = (string[])prop.GetValue(model);
                        if(value == null || value.Length == 0)
                        {
                            msgList.Add(config.ColumnTitle + _reqText);
                        }
                    }
                    else if (prop.PropertyType == typeof(DateTime?))
                    {
                        DateTime? value = (DateTime?)prop.GetValue(model);
                        if (!value.HasValue)
                        {
                            msgList.Add(config.ColumnTitle + _reqText);
                        }
                    }
                    else if (prop.PropertyType == typeof(DateTime))
                    {
                        DateTime value = (DateTime)prop.GetValue(model);
                        if (value == DateTime.MinValue)
                        {
                            msgList.Add(config.ColumnTitle + _reqText);
                        }
                    }
                }
            }

            // 回傳結果
            if (msgList.Count > 0)
                return false;
            else
                return true;
        }
    }
}
