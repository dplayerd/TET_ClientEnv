using BI.Shared;
using BI.SPA_CostService.Models.ToolTips;
using Platform.AbstractionClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_CostService.Utils
{
    /// <summary> 負責查 ToolTip 並轉為 Class </summary>
    public class ToolTipUtil
    {
        private static List<KeyTextModel> ReadModuleToolTips()
        {
            ToolTipManager mgr = new ToolTipManager();
            return mgr.GetList(ModuleConfig.ModuleName);
        }

        /// <summary> 讀取 ToolTips </summary>
        /// <returns></returns>
        public static CostServiceToolTip ReadTab()
        {
            var list = ReadModuleToolTips();
            var toolTips = new CostServiceToolTip(list);
            return toolTips;
        }
    }
}
