﻿using Platform.AbstractionClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.STQA.Enums
{
    /// <summary> 審核類型 </summary>
    public enum ApprovalType
    {
        /// <summary> 空白 </summary>
        Empty,

        /// <summary> 新增STQA審核 </summary>
        New,

        /// <summary> STQA改版審核 </summary>
        Modify,
    }

    /// <summary> 審核類型擴充 </summary>
    public static class ApprovalTypeExtension
    {
        /// <summary> 轉換為文字
        /// </summary>
        /// <param name="enm"> 審核類型 </param>
        /// <returns></returns>
        public static string ToText(this ApprovalType enm)
        {
            switch (enm)
            {
                case ApprovalType.New:
                    return "新增STQA資料審核";

                case ApprovalType.Modify:
                    return "STQA改版審核";

                case ApprovalType.Empty:
                default:
                    return null;
            }
        }
    }
}
