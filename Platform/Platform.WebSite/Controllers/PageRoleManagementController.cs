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
    public class PageRoleManagementController : BaseMVCController
    {
        // GET: PageRoleManagement
        public ActionResult Index(Guid? id)
        {
            this.InitAction(id);
            return View();
        }

        // GET: PageRoleManagement/Edit/e75d18e0-c0c3-47d5-ac98-1abfab7b363c
        public ActionResult Edit(Guid id)
        {
            var pageInfo = PageService.GetAdminPage(id);

            this.InitAction(id);
            return View(pageInfo);
        }
    }
}
