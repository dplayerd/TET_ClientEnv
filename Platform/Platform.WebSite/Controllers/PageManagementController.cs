using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Platform.WebSite.Filters;
using Platform.WebSite.Services;

namespace Platform.WebSite.Controllers
{
    [AuthorizeCore]
    public class PageManagementController : BaseMVCController
    {
        public ActionResult Index(Guid? id)
        {
            this.InitAction(id);
            return View();
        }
    }
}