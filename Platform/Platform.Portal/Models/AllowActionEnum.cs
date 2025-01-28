using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Portal.Models
{
    /// <summary> 標準行為 </summary>
    [Flags]
    public enum AllowActionEnum
    {
        /// <summary> 讀取列表 </summary>
        ReadList = 1,

        /// <summary> 讀取內頁 </summary>
        ReadDetail = 2,

        /// <summary> 新增 </summary>
        Create = 4,

        /// <summary> 修改 </summary>
        Modify = 8,

        /// <summary> 刪除 </summary>
        Delete = 16,

        /// <summary> 匯出 </summary>
        Export = 32,

        /// <summary> 管理者功能 </summary>
        Admin = 64,
    }
}
