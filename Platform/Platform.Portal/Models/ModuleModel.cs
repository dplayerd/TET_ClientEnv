using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Portal.Models
{
    public class ModuleModel
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string AdminController { get; set; }
        public string AdminAction { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string ModifyUser { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string DeleteUser { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
