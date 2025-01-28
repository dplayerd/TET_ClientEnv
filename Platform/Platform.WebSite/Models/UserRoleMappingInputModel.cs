using System;
using System.Collections.Generic;

namespace Platform.WebSite.Models
{
    public class UserRoleMappingInputModel
    {
        public IEnumerable<string> UserIDList { get; set; }
        public IEnumerable<Guid> RoleIDList { get; set; }
    }
}
