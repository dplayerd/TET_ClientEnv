using BI.Shared.Utils;
using BI.SPA_ApproverSetup.Models;
using Platform.AbstractionClass;
using Platform.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ApproverSetup.Validators
{
    public class SPA_PeriodValidator
    {
        /// <summary> 設定資料 </summary>
        private static List<ValidateConfig> _validConfigs = new List<ValidateConfig>()
        {
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "Period",   Title = "評鑑期間" },
            //new ValidateConfig() { Required =  true, CanEdit = true, Name = "Status",   Title = "評鑑狀態" },
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
        public static bool Valid(TET_SPA_PeriodModel model, out List<string> msgList)
        {
            Dictionary<string, string> dicMsg;
            var configs = _validConfigs;

            var result = ColumnValidator.ValidProperty<TET_SPA_PeriodModel>(model, configs, out dicMsg);
            msgList = dicMsg.Values.ToList();

            if (!PeriodUtil.IsPeriodFormat(model.Period))
                msgList.Add("評鑑期間 格式不正確，必須為 FY23-1H 的格式");

            if (msgList.Count > 0)
                return false;
            return true;
        }
    }
}
