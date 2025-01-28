using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Platform.WebSite.Controllers
{
    public class ErrorController : BaseMVCController
    {
        // GET: Error/Error404
        public ActionResult Error404()
        {
            this.InitAction();
            return View();
        }

        // GET: Error/Error401
        public ActionResult Error401()
        {
            this.InitAction();
            return View();
        }
    }
}
