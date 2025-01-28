using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Platform.Portal;
using Platform.Portal.Models;

namespace Platform.WebSite.Services
{
    public class ModuleService
    {
        public static ModuleModel GetModule(Guid id)
        {
            return new ModuleManager().GetAdminDetail(id);
        }
    }
}