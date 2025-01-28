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
    public class SPA_CostServiceValidator
    {
        /// <summary> 設定資料 </summary>
        private static List<ValidateConfig> _validConfigs = new List<ValidateConfig>()
        {
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "Period",   Title = "評鑑期間" },
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
        public static bool Valid(SPA_CostServiceModel model, out List<string> msgList)
        {
            Dictionary<string, string> dicMsg;
            var configs = _validConfigs;

            var result = ColumnValidator.ValidProperty<SPA_CostServiceModel>(model, configs, out dicMsg);
            msgList = dicMsg.Values.ToList();

            if (!PeriodUtil.IsPeriodFormat(model.Period, out List<string> tempMsgList))
                msgList.AddRange(tempMsgList);

            if (msgList.Count > 0)
                return false;
            return true;
        }
    }
}
