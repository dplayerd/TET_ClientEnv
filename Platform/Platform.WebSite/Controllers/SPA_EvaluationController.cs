using BI.Shared.Utils;
using BI.SPA_ApproverSetup;
using BI.SPA_CostService;
using BI.SPA_CostService.Enums;
using BI.Suppliers;
using Platform.AbstractionClass;
using Platform.FileSystem;
using Platform.Portal;
using Platform.WebSite.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Platform.WebSite.Controllers
{
    public class SPA_EvaluationController : BaseMVCController
    {
        private SPA_PeriodManager _mgr = new SPA_PeriodManager();
        private PageManager _pageMgr = new PageManager();


        // GET: SPA_Evaluation
        public ActionResult Index(Guid? id)
        {
            // 如果 ID 不存在，用模組名稱找出，並跳頁
            if (!id.HasValue)
            {
                return this.FindModuleAndRedirectToPage(BI.SPA_Evaluation.ModuleConfig.ModuleName);
            }


            this.InitAction(id);
            return View();
        }

        // GET: Detail/
        public ActionResult Detail(Guid id, Guid SPA_EvaluationID)
        {
            this.ViewBag.ViewReturn = "Index";
            this.ViewBag.ViewReturnID = id;
            this.ViewBag.IsCreateMode = false;

            // 修改模式
            var model = this._mgr.GetDetail(SPA_EvaluationID);
            if (model == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);


            this.ViewBag.Name = "檢視";
            this.ViewBag.IsCreateMode = false;
            this.ViewBag.Mode = "Detail";
            this.InitAction(id);


            //--- 取得 ScoringInfo / Violation 兩個模組的頁面 ---
            var site = SiteService.GetDefaultSite(id);
            var scoringInfoPage = this._pageMgr.GetPageListOfModule(Guid.Parse(site.ID), BI.SPA_ScoringInfo.ModuleConfig.ModuleName);
            var violationPage = this._pageMgr.GetPageListOfModule(Guid.Parse(site.ID), BI.SPA_Violation.ModuleConfig.ModuleName);

            string scoringInfoUrl = string.Empty;
            string violationUrl = string.Empty;

            if (scoringInfoPage.Any())
            {
                scoringInfoUrl = Url.Action("Detail", BI.SPA_ScoringInfo.ModuleConfig.ModuleName, new { id = scoringInfoPage.First().ID, SPA_ScoringInfoID = "__spa_scoringInfoId__", AllowButton = false });
                scoringInfoUrl = scoringInfoUrl.TrimStart('/').Replace("TET_Supplier_Eva/","");
            }

            if (violationPage.Any())
            {
                violationUrl = Url.Action("Detail", BI.SPA_Violation.ModuleConfig.ModuleName, new { id = violationPage.First().ID, spa_violationId = "__spa_violationId__", AllowButton = false });
                violationUrl = violationUrl.TrimStart('/').Replace("TET_Supplier_Eva/", "");
            }

            this.ViewBag.ScoringInfoUrl = scoringInfoUrl;
            this.ViewBag.ViolationUrl = violationUrl;
            //--- 取得 ScoringInfo / Violation 兩個模組的頁面 ---


            return View("Edit", SPA_EvaluationID);
        }
    }
}