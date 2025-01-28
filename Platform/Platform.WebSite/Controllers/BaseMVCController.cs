using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Platform.AbstractionClass;
using Platform.Portal;
using Platform.WebSite.Services;

namespace Platform.WebSite.Controllers
{
    public class BaseMVCController : Controller
    {
        private PageManager _pageManager = new PageManager();


        /// <summary> 加入訊息 </summary>
        /// <param name="msg"></param>
        public void AddTipMessage(string msg)
        {
            this.TempData["TipMsg"] = msg;
        }

        /// <summary> 初始化 View 需要的共通資料 </summary>
        /// <returns></returns>
        public ActionResult InitAction()
        {
            return this.InitAction(null);
        }

        /// <summary> 初始化 View 需要的共通資料 </summary>
        /// <param name="currentPageID"></param>
        /// <returns></returns>
        public ActionResult InitAction(Guid? currentPageID)
        {
            // 取得登入者
            var cUser = UserProfileService.GetCurrentUser();

            // 取得目前站台資訊
            var site = SiteService.GetDefaultSite(currentPageID);

            this.ViewBag.MasterInfo = site;
            return View();
        }

        /// <summary> 寫入分頁控制項需要的資訊 </summary>
        /// <param name="pager"></param>
        public void SetPagingInfo(Pager pager)
        {
            this.ViewData["PagingInfo"] = pager;
        }

        /// <summary> 導頁到指定模組名稱的頁面 </summary>
        /// <param name="moduleName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ActionResult FindModuleAndRedirectToPage(string moduleName)
        {
            var site = SiteService.GetDefaultSite(null);
            var siteID = new Guid(site.ID);

            var pageList = PageService.GetPageListOfModule(siteID, moduleName);
            if (pageList == null || pageList.Count == 0)
                return null;

            var page = pageList.First();
            var module = ModuleService.GetModule(page.ModuleID.Value);
            return RedirectToAction(module.Action, module.Controller, new { id = page.ID });
        }
    }
}