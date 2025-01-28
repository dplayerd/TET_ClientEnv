using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Platform.Portal.Models;

namespace Platform.WebSite.Models
{
    /// <summary> 頁面角色 </summary>
    public class PageRoleMappingInputModel
    {
        /// <summary> 頁面代碼 </summary>
        public Guid PageID { get; set; }

        /// <summary> 角色代碼 </summary>
        public Guid RoleID { get; set; }

        #region Permission Property
        /// <summary> 權限 - 讀取列表 </summary>
        public bool ReadList { get; set; }

        /// <summary> 權限 - 讀取內頁 </summary>
        public bool ReadDetail { get; set; }

        /// <summary> 權限 - 新增 </summary>
        public bool Create { get; set; }

        /// <summary> 權限 - 修改 </summary>
        public bool Modify { get; set; }

        /// <summary> 權限 - 刪除 </summary>
        public bool Delete { get; set; }

        /// <summary> 權限 - 匯出 </summary>
        public bool Export { get; set; }

        /// <summary> 權限 - 管理者功能 </summary>
        public bool Admin { get; set; }
        #endregion

        #region Spesfic Action
        /// <summary> 特定權限 </summary>
        List<SpesficAction> SpesficActionList = new List<SpesficAction>();
        #endregion
    }
}