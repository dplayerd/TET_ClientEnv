using BI.Shared.Utils;
using BI.SPA_CostService.Models;
using Platform.AbstractionClass;
using Platform.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_CostService.Validators
{
    public class SPA_CostServiceDetailValidator
    {
        private const string _reqText = "為必填欄位";
        private const string _shouldBeNaText = "必須為 NA";

        /// <summary> 設定資料 </summary>
        private static List<ValidateConfig> _validConfigs = new List<ValidateConfig>()
        {
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "Source",           Title = "資料來源" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "IsEvaluate",       Title = "評鑑與否" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "BU",               Title = "評鑑單位" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "ServiceFor",       Title = "服務對象" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "BelongTo",         Title = "受評供應商" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "POSource",         Title = "PO Source" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "AssessmentItem",   Title = "評鑑項目" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "PriceDeflator",    Title = "價格競爭力" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "PaymentTerm",      Title = "付款條件" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "Cooperation",      Title = "配合度" },
            new ValidateConfig() { Required =  false, CanEdit = true, Name = "Advantage",        Title = "優點、滿意、值得鼓勵之處" },
            new ValidateConfig() { Required =  false, CanEdit = true, Name = "Improved",         Title = "不滿意、期望改善之處" },
            new ValidateConfig() { Required =  false, CanEdit = true, Name = "Comment",          Title = "客戶評論與其他補充說明" },
            new ValidateConfig() { Required =  false, CanEdit = true, Name = "Remark",           Title = "備註" },
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
        public static bool Valid(List<SPA_CostServiceDetailModel> modelList, out List<string> msgList)
        {
            Dictionary<string, string> dicMsg;
            var configs = _validConfigs;

            msgList = new List<string>();

            foreach (var model in modelList)
            {
                var result = ColumnValidator.ValidProperty<SPA_CostServiceDetailModel>(model, configs, out dicMsg);
                msgList.AddRange(dicMsg.Values.ToList());

                // 驗證商業邏輯
                var biValidResult = SPA_CostServiceDetailValidator.ValidModel(model, out List<string> tempMsgList);
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
        private static bool ValidModel(SPA_CostServiceDetailModel model, out List<string> msgList)
        {
            msgList = new List<string>();

            // 若評鑑與否 = 不評鑑，該筆資料備註欄位為必填
            if (model.IsEvaluate == "不評鑑" && string.IsNullOrWhiteSpace(model.Remark))
            {
                msgList.Add("備註" + _reqText);
            }

            // 若配合度 = 很滿意，該筆資料優點、滿意、值得鼓勵之處欄位為必填
            if (model.Cooperation == "很滿意" && string.IsNullOrWhiteSpace(model.Advantage))
            {
                msgList.Add("優點、滿意、值得鼓勵之處" + _reqText);
            }

            // 若配合度 = 不滿意、很不滿意，該筆資料不滿意、期望改善之處欄位為必填
            if ((model.Cooperation == "不滿意" || model.Cooperation == "很不滿意")&& string.IsNullOrWhiteSpace(model.Improved))
            {
                msgList.Add("不滿意、期望改善之處" + _reqText);
            }
              
            // 若PO Source = Factory，該筆資料價格競爭力、付款條件、配合度欄位必須為NA
            if (model.POSource == "Factory")
            {
                if(string.Compare("NA", model.PriceDeflator, true) != 0)
                    msgList.Add("價格競爭力" + _shouldBeNaText);

                if (string.Compare("NA", model.PaymentTerm, true) != 0)
                    msgList.Add("付款條件" + _shouldBeNaText);

                if (string.Compare("NA", model.Cooperation, true) != 0)
                    msgList.Add("配合度" + _shouldBeNaText);
            }

            if (msgList.Count > 0)
                return false;
            return true;
        }
    }
}
