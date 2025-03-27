using BI.SPA_ScoringInfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.SPA_ScoringInfo.Utils
{
    public class ViewUtils
    {
        private const string _fixText_Factory = "Factory";
        private const string _fixText_Local = "Local";
        private const string _fixText_Startup = "Startup";
        private const string _fixText_Safety = "Safety";
        private const string _fixText_FE = "FE";
        private const string _fixText_Startup_DSS = "Startup(DSS)";
        private const string _fixText_Non_startup_DSS_ = "Non-startup(DSS)";
        private const string _fixText_Modification = "Modification";

        /// <summary> 透過評鑑項目以及 PO Source 來決定頁籤是否隱藏 </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static TabVisiableModel ComputeAcl(SPA_ScoringInfoModel model)
        {
            TabVisiableModel retObj = new TabVisiableModel();

            if (IsMatch(model, _fixText_Startup, _fixText_Factory))
            {
                retObj.Tab1 = true;
                retObj.Tab3 = true;
                retObj.Tab5 = true;
                retObj.Tab6 = true;
                retObj.Tab7 = true;
            }

            if (IsMatch(model, _fixText_Startup, _fixText_Local))
            {
                retObj.Tab1 = true;
                retObj.Tab2 = true;
                retObj.Tab3 = true;
                retObj.Tab5 = true;
                retObj.Tab6 = true;
                retObj.Tab7 = true;
            }

            if (IsMatch(model, _fixText_FE, _fixText_Local))
            {
                retObj.Tab1 = true;
                retObj.Tab3 = true;
                retObj.Tab6 = true;
                retObj.Tab7 = true;
            }

            if (IsMatch(model, _fixText_Safety, _fixText_Local))
            {
                retObj.Tab1 = true;
                retObj.Tab4 = true;
                retObj.Tab5 = true;
                retObj.Tab6 = true;
                retObj.Tab7 = true;
            }

            if (IsMatch(model, _fixText_Startup_DSS, _fixText_Local))
            {
                retObj.Tab2 = true;
                retObj.Tab3 = true;
                retObj.Tab6 = true;
                retObj.Tab7 = true;
            }

            if (IsMatch(model, _fixText_Non_startup_DSS_, _fixText_Local))
            {
                retObj.Tab2 = true;
                retObj.Tab3 = true;
                retObj.Tab6 = true;
                retObj.Tab7 = true;
            }

            if (IsMatch(model, _fixText_Modification, _fixText_Local))
            {
                retObj.Tab2 = true;
                retObj.Tab3 = true;
                retObj.Tab6 = true;
                retObj.Tab7 = true;
            }

            return retObj;
        }


        private static bool IsMatch(SPA_ScoringInfoModel model, string serviceItem, string poSource)
        {
            bool isServiceItem = string.Compare(serviceItem, model.ServiceItem, true) == 0;
            bool isPoSource = string.Compare(poSource, model.POSource, true) == 0;

            return (isServiceItem && isPoSource);
        }
    }
}
