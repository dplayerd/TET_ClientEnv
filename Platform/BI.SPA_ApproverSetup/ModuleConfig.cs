using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ApproverSetup
{
    public class ModuleConfig
    {
        /// <summary> 寄信的超連結 </summary>
        public static string EmailRootUrl = "https://FakeDomain/Supplier/";

        /// <summary> 模組名稱 </summary>
        public const string ModuleName_SPA_Period = "SPA_Period";
        public const string ModuleName_SPA_ScoringRatio = "SPA_ScoringRatio"; 
        public const string ModuleName_SPA_ApproverSetup = "SPA_ApproverSetup";
    }
}
