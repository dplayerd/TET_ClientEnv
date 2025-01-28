using Platform.AbstractionClass;
using BI.SPA_Violation.Models;
using Platform.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_Violation.Validators
{
    public class SPA_ViolationDetailValidator
    {
        private const string _reqText = "為必填欄位";

        /// <summary> 設定資料 </summary>
        private static List<ValidateConfig> _validConfigs = new List<ValidateConfig>()
        {
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "Date",             Title = "日期" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "BelongTo",         Title = "受評供應商" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "BU",               Title = "評鑑單位" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "AssessmentItem",   Title = "評鑑項目" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "MiddleCategory",   Title = "中分類" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "SmallCategory",    Title = "小分類" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "CustomerName",     Title = "客戶名稱" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "CustomerPlant",    Title = "客戶廠別" },
            new ValidateConfig() { Required =  false, CanEdit = true, Name = "CustomerDetail",   Title = "客戶細分" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "Descrition",       Title = "違規事件說明" },
        };

        /// <summary> 取得全部設定 </summary>
        /// <returns></returns>
        public static List<ValidateConfig> GetValidConfigs()
        {
            return _validConfigs;
        }

        /// <summary> 驗證必填 </summary>
        /// <param name="modelList"> 原資料 </param>
        /// <param name="msgList"> 錯誤訊息 </param>
        /// <returns></returns>
        public static bool Valid(List<SPA_ViolationDetailModel> modelList, out List<string> msgList)
        {
            Dictionary<string, string> dicMsg;
            var configs = _validConfigs;

            msgList = new List<string>();

            foreach (var model in modelList)
            {
                var result = ColumnValidator.ValidProperty<SPA_ViolationDetailModel>(model, configs, out dicMsg);
                msgList.AddRange(dicMsg.Values.ToList());

                // 驗證商業邏輯
                var biValidResult = SPA_ViolationDetailValidator.ValidModel(model, out List<string> tempMsgList);
                if (!biValidResult)
                    msgList.AddRange(tempMsgList);
            }

            msgList = msgList.Distinct().ToList();

            if (msgList.Count > 0)
                return false;

            return true;
        }

        /// <summary> 驗證商業邏輯 </summary>
        /// <param name="model"></param>
        /// <param name="msgList"></param>
        /// <returns></returns>
        private static bool ValidModel(SPA_ViolationDetailModel model, out List<string> msgList)
        {
            msgList = new List<string>();

            if (msgList.Count > 0)
                return false;
            return true;
        }
    }
}
