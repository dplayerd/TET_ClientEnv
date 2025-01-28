using BI.Shared;
using BI.SPA_ScoringInfo.Models.ToolTips;
using Platform.AbstractionClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ScoringInfo.Utils
{
    /// <summary> 負責查 ToolTip 並轉為 Class </summary>
    public class ToolTipUtil
    {
        private static List<KeyTextModel> ReadModuleToolTips()
        {
            ToolTipManager mgr = new ToolTipManager();
            return mgr.GetList(ModuleConfig.ModuleName);
        }

        /// <summary> 讀取頁籤的 ToolTips </summary>
        /// <returns></returns>
        public static Tab1ToolTip ReadTab1()
        {
            var list = ReadModuleToolTips();
            var toolTips = new Tab1ToolTip(list);
            return toolTips;
        }

        /// <summary> 讀取頁籤的 ToolTips </summary>
        /// <returns></returns>
        public static Tab2ToolTip ReadTab2()
        {
            var list = ReadModuleToolTips();
            var toolTips = new Tab2ToolTip(list);
            return toolTips;
        }

        /// <summary> 讀取頁籤的 ToolTips </summary>
        /// <returns></returns>
        public static Tab3ToolTip ReadTab3()
        {
            var list = ReadModuleToolTips();
            var toolTips = new Tab3ToolTip(list);
            return toolTips;
        }

        /// <summary> 讀取頁籤的 ToolTips </summary>
        /// <returns></returns>
        public static Tab4ToolTip ReadTab4()
        {
            var list = ReadModuleToolTips();
            var toolTips = new Tab4ToolTip(list);
            return toolTips;
        }

        /// <summary> 讀取頁籤的 ToolTips </summary>
        /// <returns></returns>
        public static Tab5ToolTip ReadTab5()
        {
            var list = ReadModuleToolTips();
            var toolTips = new Tab5ToolTip(list);
            return toolTips;
        }

        /// <summary> 讀取頁籤的 ToolTips </summary>
        /// <returns></returns>
        public static Tab6ToolTip ReadTab6()
        {
            var list = ReadModuleToolTips();
            var toolTips = new Tab6ToolTip(list);
            return toolTips;
        }
    }
}
