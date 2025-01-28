using BI.PaymentSuppliers.Models;
using Platform.AbstractionClass;
using Platform.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.PaymentSuppliers.Validators
{
    public class QueryValidator
    {
        /// <summary> 設定資料 </summary>
        private static List<ValidateConfig> _validConfigs = new List<ValidateConfig>()
        {
            new ValidateConfig() { Required =  false, CanEdit = true, Name = "ApplyReason",     Title = "申請原因" },
            new ValidateConfig() { Required =   true, CanEdit = true, Name = "Country",         Title = "國家別" },
            new ValidateConfig() { Required =   true, CanEdit = true, Name = "IdNo",            Title = "身分證字號" },
            new ValidateConfig() { Required =   true, CanEdit = true, Name = "Address",         Title = "公司地址" },
            new ValidateConfig() { Required =   true, CanEdit = true, Name = "OfficeTel",       Title = "公司電話" },
            new ValidateConfig() { Required =   true, CanEdit = true, Name = "Charge",          Title = "公司負責人" },
            new ValidateConfig() { Required =   true, CanEdit = true, Name = "PaymentTerm",     Title = "付款條件" },
            new ValidateConfig() { Required =   true, CanEdit = true, Name = "Incoterms",       Title = "交易條件" },
            new ValidateConfig() { Required =  false, CanEdit = true, Name = "Remark",          Title = "付款單位相關備註" },
            new ValidateConfig() { Required =   true, CanEdit = true, Name = "BillingDocument", Title = "請款憑證" },
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
