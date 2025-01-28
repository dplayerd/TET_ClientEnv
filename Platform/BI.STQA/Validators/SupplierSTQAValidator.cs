using BI.STQA.Models;
using Platform.AbstractionClass;
using Platform.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BI.STQA.Validators
{
    public class SupplierSTQAValidator
    {
        /// <summary> 設定資料 </summary>
        private static List<ValidateConfig> _validConfigs = new List<ValidateConfig>()
        {
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "BelongTo",     Title = "歸屬公司名稱" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "Purpose",      Title = "STQA理由" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "BusinessTerm", Title = "業務類別" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "Date",         Title = "完成日期" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "Type",         Title = "STQA方式" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "UnitALevel",   Title = "Unit-A Level" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "UnitCLevel",   Title = "Unit-C Level" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "UnitDLevel",   Title = "Unit-D Level" },
            new ValidateConfig() { Required = false, CanEdit = true, Name = "STQAComment",  Title = "備註" },
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
        public static bool Valid(TET_SupplierSTQAModel model, out List<string> msgList)
        {
            Dictionary<string, string> dicMsg;
            var configs = _validConfigs;

            var result = ColumnValidator.ValidProperty<TET_SupplierSTQAModel>(model, configs, out dicMsg);
            msgList = dicMsg.Values.ToList();

            return result;
        }
    }
}
