using BI.SPA_ApproverSetup.Enums;
using Platform.AbstractionClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BI.SPA_ApproverSetup.Utils
{
    public class SPA_Period_Util
    {
        #region SPA_Period_Status
        /// <summary> 文字轉為 SPA_Period_Status </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static SPA_Period_Status ParseToStatus(string val)
        {
            if (string.IsNullOrWhiteSpace(val))
                return SPA_Period_Status.Empty;

            SPA_Period_Status enm;

            if (val == SPA_Period_Status.Ready.ToText())                  // 未開始
                enm = SPA_Period_Status.Ready;
            else if (val == SPA_Period_Status.Executing.ToText())         // 進行中
                enm = SPA_Period_Status.Executing;
            else if (val == SPA_Period_Status.Completed.ToText())         // 已完成
                enm = SPA_Period_Status.Completed;
            else
                enm = SPA_Period_Status.Empty;

            return enm;
        }
        #endregion
    }
}
