using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Auth.Models
{
    /// <summary> 帳號角色 </summary>
    public class UserRoleModel
    {
        /// <summary> 主代碼 </summary>
        public Guid ID { get; set; }

        /// <summary> 帳號代碼 </summary>
        public string UserID { get; set; }

        /// <summary> 角色代碼 </summary>
        public Guid RoleID { get; set; }

        /// <summary> 帳號 </summary>
        public string Account { get; set; }

        /// <summary> 角色名稱 </summary>
        public string RoleName { get; set; }
    }
}
