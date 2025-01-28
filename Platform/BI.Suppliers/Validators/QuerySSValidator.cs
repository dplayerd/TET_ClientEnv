using BI.Suppliers.Models;
using Platform.AbstractionClass;
using Platform.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Suppliers.Validators
{
    public class QuerySSValidator
    {
        /// <summary> 設定資料 </summary>
        private static List<ValidateConfig> _validConfigs = new List<ValidateConfig>()
        {
            new ValidateConfig() { Required =  false, CanEdit = true, Name = "ApplyReason",    Title = "申請原因" },
            new ValidateConfig() { Required =   true, CanEdit = true, Name = "BelongTo",        Title = "歸屬公司名稱" },
            new ValidateConfig() { Required =   true, CanEdit = true, Name = "SupplierCategory",Title = "廠商類別" },
            new ValidateConfig() { Required =   true, CanEdit = true, Name = "BusinessCategory",Title = "交易主類別" },
            new ValidateConfig() { Required =   true, CanEdit = true, Name = "BusinessAttribute",Title = "交易子類別" },
            new ValidateConfig() { Required =   true, CanEdit = true, Name = "RelatedDept",     Title = "相關BU" },
            new ValidateConfig() { Required =  false, CanEdit = true, Name = "Buyer",           Title = "採購擔當" },
            new ValidateConfig() { Required =  false, CanEdit = true, Name = "SearchKey",       Title = "關鍵字" },
            new ValidateConfig() { Required =   true, CanEdit = true, Name = "EName",           Title = "英文名稱" },
            new ValidateConfig() { Required =   true, CanEdit = true, Name = "Country",         Title = "國家別" },
            new ValidateConfig() { Required =   true, CanEdit = true, Name = "Address",         Title = "公司地址" },
            new ValidateConfig() { Required =   true, CanEdit = true, Name = "OfficeTel",       Title = "公司電話" },
            new ValidateConfig() { Required =  false, CanEdit = true, Name = "Email",           Title = "公司E-mail" },
            new ValidateConfig() { Required =  false, CanEdit = true, Name = "Website",         Title = "公司網站" },
            new ValidateConfig() { Required =  false, CanEdit = true, Name = "ISO",             Title = "ISO認證" },
            new ValidateConfig() { Required =   true, CanEdit = true, Name = "CapitalAmount",   Title = "資本額" },
            new ValidateConfig() { Required =   true, CanEdit = true, Name = "Charge",          Title = "公司負責人" },
            new ValidateConfig() { Required =  false, CanEdit = true, Name = "Employees",       Title = "員工人數" },
            new ValidateConfig() { Required =   true, CanEdit = true, Name = "PaymentTerm",     Title = "付款條件" },
            new ValidateConfig() { Required =   true, CanEdit = true, Name = "Incoterms",       Title = "交易條件" },
            new ValidateConfig() { Required =   true, CanEdit = true, Name = "BillingDocument", Title = "請款憑證" },
            new ValidateConfig() { Required =   true, CanEdit = true, Name = "MainProduct",     Title = "主要產品/服務項目" },
            new ValidateConfig() { Required =  false, CanEdit = true, Name = "Remark",          Title = "供應商相關備註" },
            new ValidateConfig() { Required =  false, CanEdit = true, Name = "NDANo",           Title = "NDA審查書號碼" },
            new ValidateConfig() { Required =  false, CanEdit = true, Name = "Contract",        Title = "合約(Y/N)" },
            new ValidateConfig() { Required =  false, CanEdit = true, Name = "SignDate1",       Title = "行為準則承諾書簽屬日期" },
            new ValidateConfig() { Required =  false, CanEdit = true, Name = "SignDate2",       Title = "承攬商安全衛生環保承諾書簽屬日期" },
            new ValidateConfig() { Required =  false, CanEdit = true, Name = "KeySupplier",     Title = "供應商狀態" },
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

            var contactMsgList = new List<string>();
            var hasEmpty = model.ContactList.Where(obj => string.IsNullOrWhiteSpace(obj.ContactName) || string.IsNullOrWhiteSpace(obj.ContactTel)).Any();
            if (hasEmpty)
            {
                result = false;
                msgList.Add("姓名、電話 為必填");
            }                

            return result;
        }
    }
}
