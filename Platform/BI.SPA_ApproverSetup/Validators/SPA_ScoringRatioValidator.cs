using BI.SPA_ApproverSetup.Models;
using Platform.AbstractionClass;
using Platform.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ApproverSetup.Validators
{
    internal class SPA_ScoringRatioValidator
    {
        /// <summary> 設定資料 </summary>
        private static List<ValidateConfig> _validConfigs = new List<ValidateConfig>()
        {
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "ServiceItemID", Title = "評鑑項目系統辨識碼" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "POSource",      Title = "PO Source" },

            new ValidateConfig() { Required =  true, CanEdit = true, Name = "TRatio1",   Title = "TRatio1" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "TRatio2",   Title = "TRatio2" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "DRatio1",   Title = "DRatio1" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "DRatio2",   Title = "DRatio2" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "QRatio1",   Title = "QRatio1" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "QRatio2",   Title = "QRatio2" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "CRatio1",   Title = "CRatio1" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "CRatio2",   Title = "CRatio2" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "SRatio1",   Title = "SRatio1" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "SRatio2",   Title = "SRatio2" },
        };

        /// <summary> 取得全部設定 </summary>
        /// <returns></returns>
        public static List<ValidateConfig> GetValidConfigs()
        {
            return _validConfigs;
        }


        /// <summary> 驗證資料 </summary>
        /// <param name="list"> 原資料 </param>
        /// <param name="msgList"> 錯誤訊息 </param>
        /// <returns></returns>
        public static bool Valid(List<SPA_ScoringRatioModel> list, out List<string> msgList)
        {
            msgList = new List<string>();

            Dictionary<string, string> dicMsg;
            var configs = _validConfigs;

            foreach (var model in list)
            {
                var modelResult = Valid(model, out List<string> msgList2);

                if (!modelResult)
                    msgList.AddRange(msgList2);
            };

            if (msgList.Count > 0)
                return false;

            return true;
        }

        /// <summary> 驗證資料 </summary>
        /// <param name="model"></param>
        /// <param name="msgList"></param>
        /// <returns></returns>
        private static bool Valid(SPA_ScoringRatioModel model, out List<string> msgList)
        {
            msgList = new List<string>();

            Dictionary<string, string> dicMsg;
            var configs = _validConfigs;

            var result = ColumnValidator.ValidProperty<SPA_ScoringRatioModel>(model, configs, out dicMsg);
            msgList.AddRange(dicMsg.Values.ToList());


            // 驗證輸入值的總合必須為 0 或是 100
            var total =
                model.TRatio1 + model.TRatio2 +
                model.DRatio1 + model.DRatio2 +
                model.QRatio1 + model.QRatio2 +
                model.CRatio1 + model.CRatio2 +
                model.SRatio1 + model.SRatio2;

            if (total != 0 && total != 100)
                msgList.Add($"[{model.ServiceItem}, {model.POSource}] 總合必須是 0 或 100 ");


            if (msgList.Count > 0)
                return false;

            return true;
        }
    }
}
