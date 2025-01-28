using Platform.AbstractionClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Suppliers.Enums
{
    /// <summary> 審核狀態 </summary>
    public enum ApprovalStatus 
    {
        /// <summary> 空白 </summary>
        Empty,
        
        /// <summary> 審核中 </summary>
        Verify,

        /// <summary> 已完成 </summary>
        Completed,

        /// <summary> 已退回 </summary>
        Rejected
    }

    /// <summary> 審核狀態擴充 </summary>
    public static class ApprovalStatusExtension
    {
        /// <summary> 轉換為文字
        /// </summary>
        /// <param name="enm"> 審核狀態 </param>
        /// <returns></returns>
        public static string ToText(this ApprovalStatus enm)
        {
            switch (enm)
            {
                case ApprovalStatus.Empty:
                    return null;

                case ApprovalStatus.Verify:
                    return "審核中";

                case ApprovalStatus.Completed:
                    return "已完成";

                case ApprovalStatus.Rejected:
                    return "已退回";

                default:
                    return null;
            }
        }
    }
}
