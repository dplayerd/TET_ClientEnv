using BI.SPA_ApproverSetup.Models;
using Platform.AbstractionClass;
using Platform.Infra;
using Platform.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ApproverSetup.Validators
{
    public class SPA_ApproverSetupValidator
    {
        /// <summary> 設定資料 </summary>
        private static List<ValidateConfig> _validConfigs = new List<ValidateConfig>()
        {
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "ServiceItemID", Title = "評鑑項目" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "BUID",          Title = "評鑑單位" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "InfoFill",      Title = "計分資料填寫者" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "InfoConfirm",   Title = "計分資料確認者" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "Lv1Apprvoer",   Title = "第一關審核者" },
            new ValidateConfig() { Required = false, CanEdit = true, Name = "Lv2Apprvoer",   Title = "第二關審核者" },
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
        public static bool Valid(TET_SPA_ApproverSetupModel model, out List<string> msgList)
        {
            Dictionary<string, string> dicMsg;
            var configs = _validConfigs;

            var result = ColumnValidator.ValidProperty<TET_SPA_ApproverSetupModel>(model, configs, out dicMsg);
            msgList = dicMsg.Values.ToList();

            return result;
        }
    }
}
