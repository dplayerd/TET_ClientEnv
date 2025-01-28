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
    public class SPA_ScoringInfoModule2Validator
    {
        private const string _reqText = "為必填欄位";
        private const string _shouldBeNaText = "必須為 NA";
        private const string _prevText = "前期匯入";
        private const string _repeatText = "為重複資料";
        private const string _columnName_MachineNo = "機台Serial No.";
        private const string _columnName_MachineName = "承攬機台名稱";
        private const string _startupText = "Startup";

        /// <summary> 設定資料 </summary>
        private static List<ValidateConfig> _validConfigs = new List<ValidateConfig>()
        {
            new ValidateConfig() { Required = false,  CanEdit = true, Name = "ServiceFor",      Title = "服務對象" },
            new ValidateConfig() { Required = false,  CanEdit = true, Name = "WorkItem",        Title = "作業項目" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "MachineName",     Title = "承攬機台名稱" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "MachineNo",       Title = "機台Serial No." },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "OnTime",          Title = "是否準時交付" },
            new ValidateConfig() { Required = false,  CanEdit = true, Name = "Remark",          Title = "備註" },
        };

        /// <summary> 取得全部設定 </summary>
        /// <returns></returns>
        public static List<ValidateConfig> GetValidConfigs()
        {
            return _validConfigs;
        }

        /// <summary> 驗證必填 </summary>
        /// <param name="mainModel"> 主要資料 </param>
        /// <param name="modelList"> 原資料 </param>
        /// <param name="msgList"> 錯誤訊息 </param>
        /// <returns></returns>
        public static bool Valid(SPA_ScoringInfoModel mainModel, List<SPA_ScoringInfoModule2Model> modelList, out List<string> msgList)
        {
            Dictionary<string, string> dicMsg;
            var configs = _validConfigs;

            msgList = new List<string>();


            var hasRepeat =
                from item in modelList
                group item by new { item.MachineNo, item.MachineName } into tmpGroup
                where tmpGroup.Count() > 1
                select tmpGroup;

            if (hasRepeat.Any())
            {
                var repeatTxtEnm = hasRepeat.Select(obj => $"{_columnName_MachineNo}: {obj.Key.MachineNo}, {_columnName_MachineName}: {obj.Key.MachineName} {_repeatText}");
                msgList.AddRange(repeatTxtEnm);
            }


            foreach (var model in modelList)
            {
                var result = ColumnValidator.ValidProperty<SPA_ScoringInfoModule2Model>(model, configs, out dicMsg);
                msgList.AddRange(dicMsg.Values.ToList());

                // 驗證商業邏輯
                var biValidResult = SPA_ScoringInfoModule2Validator.ValidModel(mainModel, model, out List<string> tempMsgList);
                if (!biValidResult)
                    msgList.AddRange(tempMsgList);
            }

            msgList = msgList.Distinct().ToList();

            if (msgList.Count > 0)
                return false;

            return true;
        }

        /// <summary> 驗證商業邏輯 </summary>
        /// <param name="mainModel"> 主要資料 </param>
        /// <param name="model"></param>
        /// <param name="msgList"></param>
        /// <returns></returns>
        private static bool ValidModel(SPA_ScoringInfoModel mainModel, SPA_ScoringInfoModule2Model model, out List<string> msgList)
        {
            msgList = new List<string>();

            if(string.Compare(_startupText, mainModel.ServiceItem, true) != 0 )
            {
                if (string.IsNullOrWhiteSpace(model.ServiceFor))
                    msgList.Add("服務對象 " + _reqText);

                if (string.IsNullOrWhiteSpace(model.WorkItem))
                    msgList.Add("作業項目 " + _reqText);
            }

            if (msgList.Count > 0)
                return false;
            return true;
        }
    }
}
