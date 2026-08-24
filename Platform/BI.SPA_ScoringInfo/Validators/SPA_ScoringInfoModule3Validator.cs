using BI.Shared.Utils;
using BI.SPA_ApproverSetup.Enums;
using BI.SPA_ScoringInfo.Models;
using Platform.AbstractionClass;
using Platform.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ScoringInfo.Validators
{
    public class SPA_ScoringInfoModule3Validator
    {
        private const string _reqText = "為必填欄位";
        private const string _shouldBeNaText = "必須為 NA";
        private const string _prevText = "前期匯入";

        /// <summary> 設定資料 </summary>
        private static List<ValidateConfig> _validConfigs = new List<ValidateConfig>()
        {
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "Date",            Title = "時間" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "Location",        Title = "地點" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "TELLoss",         Title = "TEL財損" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "CustomerLoss",    Title = "客戶財損" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "Accident",        Title = "人身事故" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "Description",     Title = "事件說明" },
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
        public static bool Valid(List<SPA_ScoringInfoModule3Model> modelList, out List<string> msgList)
        {
            return Valid(modelList, null, out msgList);
        }

        public static bool Valid(List<SPA_ScoringInfoModule3Model> modelList, SPA_ScoringInfoSheetModel sheetSetting, out List<string> msgList)
        {
            Dictionary<string, string> dicMsg;
            var configs = GetValidConfigs(sheetSetting);

            msgList = new List<string>();

            foreach (var model in modelList)
            {
                var result = ColumnValidator.ValidProperty<SPA_ScoringInfoModule3Model>(model, configs, out dicMsg);
                msgList.AddRange(dicMsg.Values.ToList());

                // 驗證商業邏輯
                var biValidResult = SPA_ScoringInfoModule3Validator.ValidModel(model, out List<string> tempMsgList);
                if (!biValidResult)
                    msgList.AddRange(tempMsgList);
            }

            msgList = msgList.Distinct().ToList();

            if (msgList.Count > 0)
                return false;

            return true;
        }

        private static List<ValidateConfig> GetValidConfigs(SPA_ScoringInfoSheetModel sheetSetting)
        {
            if (sheetSetting == null)
                return _validConfigs;

            return new List<ValidateConfig>()
            {
                new ValidateConfig() { Required = sheetSetting.IsSheet3DateFill, CanEdit = true, Name = "Date", Title = "時間" },
                new ValidateConfig() { Required = sheetSetting.IsSheet3LocationFill, CanEdit = true, Name = "Location", Title = "地點" },
                new ValidateConfig() { Required = sheetSetting.IsSheet3TELLossFill, CanEdit = true, Name = "TELLoss", Title = "TEL財損" },
                new ValidateConfig() { Required = sheetSetting.IsSheet3CustomerLossFill, CanEdit = true, Name = "CustomerLoss", Title = "客戶財損" },
                new ValidateConfig() { Required = sheetSetting.IsSheet3AccidentFill, CanEdit = true, Name = "Accident", Title = "人身事故" },
                new ValidateConfig() { Required = sheetSetting.IsSheet3DescriptionFill, CanEdit = true, Name = "Description", Title = "事件說明" },
            };
        }

        /// <summary> 驗證商業邏輯 </summary>
        /// <param name="model"></param>
        /// <param name="msgList"></param>
        /// <returns></returns>
        private static bool ValidModel(SPA_ScoringInfoModule3Model model, out List<string> msgList)
        {
            msgList = new List<string>();

            if (msgList.Count > 0)
                return false;
            return true;
        }
    }
}
