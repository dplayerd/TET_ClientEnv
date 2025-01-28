using BI.SPA_ApproverSetup;
using Platform.AbstractionClass;
using Platform.Auth;
using Platform.WebSite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using static Platform.WebSite.Controllers.SPA_ApproverSetupController;

namespace Platform.WebSite.Controllers
{
    public class SPA_PeriodController : BaseMVCController
    {
        private SPA_PeriodManager _mgr = new SPA_PeriodManager();
        private UserManager _userMgr = new UserManager();


        // GET: SPA_Period
        public ActionResult Index(Guid? id)
        {
            // 如果 ID 不存在，用模組名稱找出，並跳頁
            if (!id.HasValue)
            {
                return this.FindModuleAndRedirectToPage(BI.SPA_ApproverSetup.ModuleConfig.ModuleName_SPA_Period);
            }


            this.InitAction(id);
            return View();
        }

        public ActionResult Create(Guid id)
        {
            this.ViewBag.ViewReturn = "Index";
            this.ViewBag.ViewReturnID = id;
            this.ViewBag.IsCreateMode = true;

            // 沒有帶 ID ，新增模式
            this.ViewBag.Name = "新增供應商SPA評鑑期間設定";
            this.ViewBag.IsCreateMode = true;
            this.ViewBag.Mode = "Create";
            this.InitAction(id);
            return View("Edit");
        }

        public ActionResult Edit(Guid id, Guid? spa_periodId)
        {
            this.ViewBag.ViewReturn = "Index";
            this.ViewBag.ViewReturnID = id;
            this.ViewBag.IsCreateMode = false;

            if (!spa_periodId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            // 修改模式
            var model = this._mgr.GetDetail(spa_periodId.Value);
            if (model == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            this.ViewBag.Name = "修改供應商SPA評鑑期間設定";
            this.ViewBag.IsCreateMode = false;
            this.ViewBag.Mode = "Edit";
            this.InitAction(id);
            return View("Edit", spa_periodId.Value);
        }
    }
}