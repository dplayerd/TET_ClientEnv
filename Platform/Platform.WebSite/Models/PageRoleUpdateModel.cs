using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Platform.WebSite.Models
{
    /// <summary> 更新用的物件 </summary>
    public class PageRoleUpdateModel
    {
        /// <summary> 定義角色、頁面允許的行為 </summary>
        public class Item
        {
            /// <summary> 頁面代碼 </summary>
            public Guid PageID { get; set; }

            /// <summary> 角色代碼 </summary>
            public Guid RoleID { get; set; }

            /// <summary> 允許的標準行為 </summary>
            public byte AllowActs { get; set; }
        }

        /// <summary> 全部清單 </summary>
        public List<Item> Items { get; set; } = new List<Item>();
    }
}