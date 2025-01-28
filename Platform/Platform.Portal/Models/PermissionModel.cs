using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Platform.Portal.Helpers;

namespace Platform.Portal.Models
{
    /// <summary> 授權資訊 </summary>
    public class PermissionModel
    {
        /// <summary> 標準行為 (Flag Enum) </summary>
        public AllowActionEnum AllowAction { get; set; }

        /// <summary> 特殊行為 </summary>
        public List<string> Functions { get; set; }

        /// <summary> 標準行為 </summary>
        public List<AllowActionEnum> GetAllowActionList()
        {
            return AllowActionHelper.EnumFlagToList(this.AllowAction);
        }

        /// <summary> 是否允許標準行為 </summary>
        /// <param name="enm"></param>
        /// <returns></returns>
        public bool IsAllowed(AllowActionEnum enm)
        {
            var result = (this.AllowAction & enm) == enm;
            return result;
        }

        /// <summary> 是否允許非標準行為 </summary>
        /// <param name="enm"></param>
        /// <returns></returns>
        public bool IsAllowed(string functionCode)
        {
            var result = this.Functions.Contains(functionCode);
            return result;
        }
    }
}
