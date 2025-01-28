using BI.Shared.Utils;
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
    public class SPA_ScoringInfoValidator
    {
        private const string _reqText = "為必填欄位";
        private const string _fixText_Startup = "Startup";
        private const string _fixText_FE = "FE";
        private const string _fixText_WorkerCount = "出工人數";
        private const string _fixText_Positive = "必須為正整數";
        private const string _fixText_MoreThen0 = "必須大於 0";

        /// <summary> 設定資料 </summary>
        private static List<ValidateConfig> _validConfigs = new List<ValidateConfig>()
        {
            //new ValidateConfig() { Required =  true, CanEdit = true, Name = "Period",   Title = "評鑑期間" },
        };

        /// <summary> 設定資料 </summary>
        private static List<ValidateConfig> _validConfigs_tab3 = new List<ValidateConfig>()
        {
        };

        /// <summary> 設定資料 </summary>
        private static List<ValidateConfig> _validConfigs_tab4 = new List<ValidateConfig>()
        {
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "Correctness",  Title = "作業正確性" },
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "Contribution", Title = "人員備齊貢獻度" },
        };

        /// <summary> 設定資料 </summary>
        private static List<ValidateConfig> _validConfigs_tab5 = new List<ValidateConfig>()
        {
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "SelfTraining",  Title = "供應商自訓程度" },
        };

        /// <summary> 設定資料 </summary>
        private static List<ValidateConfig> _validConfigs_tab6 = new List<ValidateConfig>()
        {
            new ValidateConfig() { Required =  true, CanEdit = true, Name = "Cooperation",  Title = "配合度" },
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
        public static bool Valid(SPA_ScoringInfoModel model, out List<string> msgList)
        {
            Dictionary<string, string> dicMsg;
            var configs = _validConfigs;

            var result = ColumnValidator.ValidProperty<SPA_ScoringInfoModel>(model, configs, out dicMsg);
            msgList = dicMsg.Values.ToList();

            if (msgList.Count > 0)
                return false;
            return true;
        }

        /// <summary> 驗證必填 </summary>
        /// <param name="model"> 原資料 </param>
        /// <param name="msgList"> 錯誤訊息 </param>
        /// <returns></returns>
        public static bool Valid_Tab3(SPA_ScoringInfoModel model, out List<string> msgList)
        {
            Dictionary<string, string> dicMsg;
            var configs = _validConfigs_tab3;

            var result = ColumnValidator.ValidProperty<SPA_ScoringInfoModel>(model, configs, out dicMsg);
            msgList = dicMsg.Values.ToList();


            if (string.Compare(_fixText_Startup, model.ServiceItem, true) == 0 || string.Compare(_fixText_FE, model.ServiceItem, true) == 0)
            {
                if (!model.WorkerCount.HasValue)
                    msgList.Add(_fixText_WorkerCount + _reqText);
                else if(model.WorkerCount.Value <= 0)
                    msgList.Add(_fixText_WorkerCount + _fixText_MoreThen0);
            }

            if (model.WorkerCount.HasValue && model.WorkerCount.Value < 0)
                msgList.Add(_fixText_WorkerCount + _fixText_Positive);



            if (msgList.Count > 0)
                return false;
            return true;
        }

        /// <summary> 驗證必填 </summary>
        /// <param name="model"> 原資料 </param>
        /// <param name="msgList"> 錯誤訊息 </param>
        /// <returns></returns>
        public static bool Valid_Tab4(SPA_ScoringInfoModel model, out List<string> msgList)
        {
            Dictionary<string, string> dicMsg;
            var configs = _validConfigs_tab4;

            var result = ColumnValidator.ValidProperty<SPA_ScoringInfoModel>(model, configs, out dicMsg);
            msgList = dicMsg.Values.ToList();

            if (msgList.Count > 0)
                return false;
            return true;
        }

        /// <summary> 驗證必填 </summary>
        /// <param name="model"> 原資料 </param>
        /// <param name="msgList"> 錯誤訊息 </param>
        /// <returns></returns>
        public static bool Valid_Tab5(SPA_ScoringInfoModel model, out List<string> msgList)
        {
            Dictionary<string, string> dicMsg;
            var configs = _validConfigs_tab5;

            var result = ColumnValidator.ValidProperty<SPA_ScoringInfoModel>(model, configs, out dicMsg);
            msgList = dicMsg.Values.ToList();

            if (msgList.Count > 0)
                return false;
            return true;
        }

        /// <summary> 驗證必填 </summary>
        /// <param name="model"> 原資料 </param>
        /// <param name="msgList"> 錯誤訊息 </param>
        /// <returns></returns>
        public static bool Valid_Tab6(SPA_ScoringInfoModel model, out List<string> msgList)
        {
            Dictionary<string, string> dicMsg;
            var configs = _validConfigs_tab6;

            var result = ColumnValidator.ValidProperty<SPA_ScoringInfoModel>(model, configs, out dicMsg);
            msgList = dicMsg.Values.ToList();


            if (model.Cooperation == "很滿意" && string.IsNullOrWhiteSpace(model.Advantage))
                msgList.Add("優點、滿意、值得鼓勵之處 " + _reqText);

            if ((model.Cooperation == "不滿意" || model.Cooperation == "很不滿意") && string.IsNullOrWhiteSpace(model.Improved))
                msgList.Add("不滿意、期望改善之處 " + _reqText);


            if (msgList.Count > 0)
                return false;
            return true;
        }
    }
}
