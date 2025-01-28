using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Platform.WebSite.Services;
using Platform.Portal.Models;

namespace Platform.WebSite.Controllers
{
    public class NavigateController : BaseMVCController
    {
        // TODO: 檢查權限
        [Route("~/Navigate/Index/{id}")]
        public ActionResult Index(Guid id)
        {
            var page = PageService.GetPage(id);
            if (page == null)
                return new HttpNotFoundResult($"Page doesn't exist: [Page: {id}]");

            if (page.GetMenuTypeEnum() == MenuTypeEnum.Module)
            {
                if(!page.ModuleID.HasValue)
                    return new HttpNotFoundResult($"Page doesn't have moduleID.");

                var module = ModuleService.GetModule(page.ModuleID.Value);
                if(module == null)
                    return new HttpNotFoundResult($"Module doesn't exist: [Module: {page.ModuleID}]");

                // 跳至指定模組，並帶入頁面 id / 站台 id 作為參數
                return RedirectToAction(module.Action, module.Controller, new { Id = id });
            }
            else
                return View();
        }
    }
}
