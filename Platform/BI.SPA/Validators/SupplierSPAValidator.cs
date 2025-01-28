using BI.SPA.Models;
using BI.SPA.Utils;
using Platform.AbstractionClass;
using Platform.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BI.SPA.Validators
{
    public class SupplierSPAValidator
    {
        /// <summary> 設定資料 </summary>
        private static List<ValidateConfig> _validConfigs = new List<ValidateConfig>()
        {
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "BelongTo",          Title = "供應商" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "Period",            Title = "評鑑期間" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "BU",                Title = "評鑑單位" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "ServiceFor",        Title = "服務對象" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "AssessmentItem",    Title = "評鑑項目" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "PerformanceLevel",  Title = "Performance Level" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "TotalScore",        Title = "Total Score" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "TScore",            Title = "Technology Score" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "DScore",            Title = "Delivery Score" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "QScore",            Title = "Quality Score" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "CScore",            Title = "Cost Score" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "SScore",            Title = "Service Score" },
            new ValidateConfig() { Required = false, CanEdit = true, Name = "SPAComment",        Title = "備註" },
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
        public static bool Valid(TET_SupplierSPAModel model, out List<string> msgList)
        {
            Dictionary<string, string> dicMsg;
            var configs = _validConfigs;

            var result = ColumnValidator.ValidProperty<TET_SupplierSPAModel>(model, configs, out dicMsg);
            msgList = dicMsg.Values.ToList();

            if (!string.IsNullOrWhiteSpace(model.Period))
            {
                if (!PeriodUtil.IsPeriodFormat(model.Period))
                {
                    result = false;
                    msgList.Add("評鑑期間 格式不正確");
                }
            }

            // 驗證總分為數定，且固定小數兩位
            if (!ValieTotalScoreFormat(model.TotalScore, out string msg))
            { 
                result = false;
                msgList.Add(msg);
            }

            // 驗證總分為數定，且固定小數一位，或是 Na
            if (!ValieOtherScoreFormat("Technology Score", model.TScore, out string msg2))
            {
                result = false;
                msgList.Add(msg2);
            }
            if (!ValieOtherScoreFormat("Delivery Score", model.DScore, out string msg3))
            {
                result = false;
                msgList.Add(msg3);
            }
            if (!ValieOtherScoreFormat("Quality Score", model.QScore, out string msg4))
            {
                result = false;
                msgList.Add(msg4);
            }
            if (!ValieOtherScoreFormat("Cost Score", model.CScore, out string msg5))
            {
                result = false;
                msgList.Add(msg5);
            }
            if (!ValieOtherScoreFormat("Service Score", model.SScore, out string msg6))
            {
                result = false;
                msgList.Add(msg6);
            }

            return result;
        }

        /// <summary> 驗證其它分數為數定，或是 Na </summary>
        /// <param name="columnName"></param>
        /// <param name="input"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static bool ValieOtherScoreFormat(string columnName, string input, out string msg)
        {
            msg = string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                msg = $"{columnName} 必須是數字，且固定小數一位。或是必須是 Na 。";
                return false;
            }

            // 驗證 NA
            string[] arr = { "na", "n/a" };
            if (arr.Contains(input.ToLower()))
                return true;

            // 驗證數字
            if(!double.TryParse(input , out double temp))
            {
                msg = $"{columnName} 必須是數字，且固定小數一位。或是必須是 Na 。";
                return false;
            }

            // 驗證小數位
            // 使用 Split 方法將整數部分和小數部分分開
            string[] parts = input.Split('.');

            // 檢查是否有小數部分
            int integerDigits = parts[0].Length;
            int decimalDigits = 0;

            if (parts.Length == 2 && parts[1].Length > 0)
            {
                decimalDigits = parts[1].Length;
            }

            if (decimalDigits != 1)
            {
                msg = $"{columnName} 必須是數字，且固定小數一位。或是必須是 Na 。";
                return false;
            }

            return true;
        }


        /// <summary> 驗證總分為數定，且固定小數兩位 </summary>
        /// <param name="input"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static bool ValieTotalScoreFormat(string input, out string msg)
        {
            msg = string.Empty;

            if (string.IsNullOrWhiteSpace(input) || !double.TryParse(input, out double temp))
            {
                msg = "Total Score 必須是數字，且固定小數兩位";
                return false;
            }

            // 使用 Split 方法將整數部分和小數部分分開
            string[] parts = input.Split('.');

            // 檢查是否有小數部分
            int integerDigits = parts[0].Length;
            int decimalDigits = 0;

            if (parts.Length == 2 && parts[1].Length > 0)
            {
                decimalDigits = parts[1].Length;
            }

            if (decimalDigits != 2)
            {
                msg = "Total Score 必須是數字，且固定小數兩位";
                return false;
            }

            return true;
        }
    }
}
