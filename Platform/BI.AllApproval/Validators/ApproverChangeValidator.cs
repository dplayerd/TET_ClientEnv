using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Platform.AbstractionClass;


namespace BI.AllApproval.Validators
{
    class ApproverChangeValidator
    {
        /// <summary> 設定資料 </summary>
        private static List<ValidateConfig> _validConfigs = new List<ValidateConfig>()
        {
            new ValidateConfig() { Required = false, CanEdit = false, Name = "Type",     Title = "類別" },
            new ValidateConfig() { Required = false, CanEdit = false, Name = "Description",      Title = "審核說明" },
            new ValidateConfig() { Required = false, CanEdit = false, Name = "Level_Text", Title = "審核關卡" },
            new ValidateConfig() { Required = true, CanEdit = true, Name = "Approver",         Title = "審核人" },
        };

        /// <summary> 取得全部設定 </summary>
        /// <returns></returns>
        public static List<ValidateConfig> GetValidConfigs()
        {
            return _validConfigs;
        }
    }
}
