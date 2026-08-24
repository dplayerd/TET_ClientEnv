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
    public class SPA_ScoringInfoModule1Validator
    {
        private const string _reqText = "為必填欄位";
        private const string _shouldBeNaText = "必須為 NA";
        private const string _prevText = "前期匯入";

        /// <summary> 設定資料 </summary>
        private static List<ValidateConfig> _validConfigs = new List<ValidateConfig>()
        {
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "Source",          Title = "資料來源" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "Type",            Title = "本社/協力廠商" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "Supplier",        Title = "供應商名稱" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "EmpName",         Title = "員工姓名" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "MajorJob",        Title = "主要負責作業" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "IsIndependent",   Title = "能否獨立作業" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "SkillLevel",      Title = "Skill Level" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "EmpStatus",       Title = "員工狀態" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "TELSeniorityY",   Title = "派工至TEL的年資(年)" },
            new ValidateConfig() { Required =  true,  CanEdit = true, Name = "TELSeniorityM",   Title = "派工至TEL的年資(月)" },
            new ValidateConfig() { Required = false,  CanEdit = true, Name = "Remark",          Title = "備註" },
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
        public static bool Valid(List<SPA_ScoringInfoModule1Model> modelList, out List<string> msgList)
        {
            return Valid(modelList, null, out msgList);
        }

        public static bool Valid(List<SPA_ScoringInfoModule1Model> modelList, SPA_ScoringInfoSheetModel sheetSetting, out List<string> msgList)
        {
            Dictionary<string, string> dicMsg;
            var configs = GetValidConfigs(sheetSetting);

            msgList = new List<string>();

            foreach (var model in modelList)
            {
                var result = ColumnValidator.ValidProperty<SPA_ScoringInfoModule1Model>(model, configs, out dicMsg);
                msgList.AddRange(dicMsg.Values.ToList());

                // 驗證商業邏輯
                var biValidResult = SPA_ScoringInfoModule1Validator.ValidModel(model, out List<string> tempMsgList);
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
                new ValidateConfig() { Required = true, CanEdit = true, Name = "Source", Title = "資料來源" },
                new ValidateConfig() { Required = sheetSetting.IsSheet1TypeFill, CanEdit = true, Name = "Type", Title = "本社/協力廠商" },
                new ValidateConfig() { Required = sheetSetting.IsSheet1SupplierFill, CanEdit = true, Name = "Supplier", Title = "供應商名稱" },
                new ValidateConfig() { Required = sheetSetting.IsSheet1EmpNameFill, CanEdit = true, Name = "EmpName", Title = "員工姓名" },
                new ValidateConfig() { Required = sheetSetting.IsSheet1MajorJobFill, CanEdit = true, Name = "MajorJob", Title = "主要負責作業" },
                new ValidateConfig() { Required = sheetSetting.IsSheet1IsIndependentFill, CanEdit = true, Name = "IsIndependent", Title = "能否獨立作業" },
                new ValidateConfig() { Required = sheetSetting.IsSheet1SkillLevelFill, CanEdit = true, Name = "SkillLevel", Title = "Skill Level" },
                new ValidateConfig() { Required = sheetSetting.IsSheet1EmpStatusFill, CanEdit = true, Name = "EmpStatus", Title = "員工狀態" },
                new ValidateConfig() { Required = sheetSetting.IsSheet1TELSeniorityYFill, CanEdit = true, Name = "TELSeniorityY", Title = "派工至TEL的年資(年)" },
                new ValidateConfig() { Required = sheetSetting.IsSheet1TELSeniorityMFill, CanEdit = true, Name = "TELSeniorityM", Title = "派工至TEL的年資(月)" },
                new ValidateConfig() { Required = sheetSetting.IsSheet1RemarkFill, CanEdit = true, Name = "Remark", Title = "備註" },
            };
        }

        /// <summary> 驗證商業邏輯 </summary>
        /// <param name="model"></param>
        /// <param name="msgList"></param>
        /// <returns></returns>
        private static bool ValidModel(SPA_ScoringInfoModule1Model model, out List<string> msgList)
        {
            msgList = new List<string>();

            if (msgList.Count > 0)
                return false;
            return true;
        }
    }
}
