using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Auth.Models
{
    /// <summary> 角色 </summary>
    public class RoleModel
    {
        /// <summary> 主代碼 </summary>
        public Guid ID { get; set; }

        /// <summary> 角色名稱 </summary>
        public string Name { get; set; }

        /// <summary> 是否啟用 </summary>
        public bool IsEnable { get; set; }

        /// <summary> 建立者 </summary>
        public string CreateUser { get; set; }

        /// <summary> 建立時間 </summary>
        public DateTime CreateDate { get; set; }

        /// <summary> 修改者 </summary>
        public string ModifyUser { get; set; }

        /// <summary> 修改時間 </summary>
        public DateTime ModifyDate { get; set; }
    }
}
