using BI.Shared.Models;
using Platform.AbstractionClass;
using Platform.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Shared.Validators
{
    public class ParameterValidator
    {
        /// <summary> 設定資料 </summary>
        private static List<ValidateConfig> _validConfigs = new List<ValidateConfig>()
        {
            new ValidateConfig() { Required = true, CanEdit = true, Name = nameof(TET_ParametersModel.Type),      Title = "參數類型" },
            new ValidateConfig() { Required = true, CanEdit = true, Name = nameof(TET_ParametersModel.Item),      Title = "項目名稱" },
            new ValidateConfig() { Required = true, CanEdit = true, Name = nameof(TET_ParametersModel.Seq),       Title = "排序" },
            new ValidateConfig() { Required = true, CanEdit = true, Name = nameof(TET_ParametersModel.IsEnable),  Title = "是否啟用" },
        };

        /// <summary> 取得全部設定 </summary>
        /// <returns></returns>
        public static List<ValidateConfig> GetValidConfigs()
        {
            return _validConfigs;
        }

        /// <summary> 驗證必填 </summary>
        /// <param name="list"> 原資料 </param>
        /// <param name="msgList"> 錯誤訊息 </param>
        /// <returns></returns>
        public static bool Valid(List<TET_ParametersModel> list, out List<string> msgList)
        {
            msgList = new List<string>();
            var configs = _validConfigs;

            foreach (var model in list)
            {
                ColumnValidator.ValidProperty<TET_ParametersModel>(model, configs, out Dictionary<string, string> dicMsg);
                msgList.AddRange(dicMsg.Values.ToList());
            }


            var allCnt = list.Select(obj => obj.Item.ToLower()).Count();
            var distinctCnt = list.Select(obj => obj.Item.ToLower()).Distinct().Count();

            if (allCnt != distinctCnt)
                msgList.Add("項目名稱 不允許重覆。");


            msgList = msgList.Distinct().ToList();

            var result = (msgList.Count == 0);
            return result;
        }
    }
}
