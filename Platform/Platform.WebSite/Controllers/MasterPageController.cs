using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Platform.WebSite.Services;

namespace Platform.WebSite.Controllers
{
    public class MasterPageController : Controller
    {
        [ChildActionOnly]
        public ActionResult UserInfo()
        {
            var cUser = UserProfileService.GetCurrentUser();
            return View("~/Views/Shared/_pvUser.cshtml", cUser);
        }

        [ChildActionOnly]
        public ActionResult UserProfile()
        {
            var cUser = UserProfileService.GetCurrentUser();
            return View("~/Views/Shared/_pvUserProfile.cshtml", cUser);
        }
    }
}