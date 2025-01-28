using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Platform.WebSite.Services;
using Platform.WebSite.Filters;

namespace Platform.WebSite.Controllers
{
    [AuthorizeCore]
    public class RoleManagementController : BaseMVCController
    {
        /// <summary> 角色列表頁 </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Index(Guid id)
        {
            this.ViewBag.SiteID = id;

            this.InitAction(id);
            return View();
        }

        /// <summary> 角色使用者管理 </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult RoleUser(Guid id, Guid roleID)
        {
            this.ViewBag.RoleID = roleID;

            this.InitAction(id);
            return View();
        }

        /// <summary> 角色選單管理 </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult RoleMenu(Guid id, Guid roleID)
        {
            this.ViewBag.RoleID = roleID;

            this.InitAction(id);
            return View();
        }
    }
}
