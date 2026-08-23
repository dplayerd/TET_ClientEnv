using BI.SPA_ScoringInfo;
using BI.SPA_ScoringInfo.Models;
using Platform.WebSite.Services;
using System;
using System.Net;
using System.Web.Mvc;

namespace Platform.WebSite.Controllers
{
    public class SPA_ScoringInfoSheetSetupController : BaseMVCController
    {
        private SPA_ScoringInfoSheetManager _mgr = new SPA_ScoringInfoSheetManager();

        public ActionResult Index(Guid? id)
        {
            if (!id.HasValue)
            {
                return this.FindModuleAndRedirectToPage(BI.SPA_ScoringInfo.ModuleConfig.ModuleName_SPA_ScoringInfoSheetSetup);
            }

            this.InitAction(id);
            this.ViewBag.ParamList_AssessmentItem = TET_ParameterService.GetTET_ParametersList("SPA評鑑項目", TET_ParameterService.KeyType.Id);

            return View();
        }

        public ActionResult Edit(Guid id, Guid serviceItemID, string poSource)
        {
            if (string.IsNullOrWhiteSpace(poSource))
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            var model = this._mgr.GetDetail(serviceItemID, poSource);
            if (model == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            this.ViewBag.ViewReturn = "Index";
            this.ViewBag.ViewReturnID = id;
            this.ViewBag.Name = "修改SPA評鑑計分資料頁籤顯示設定";
            this.ViewBag.IsCreateMode = false;
            this.ViewBag.ParamList_AssessmentItem = TET_ParameterService.GetTET_ParametersList("SPA評鑑項目", TET_ParameterService.KeyType.Id);
            this.InitAction(id);

            return View("Edit", model);
        }

        public ActionResult Create(Guid id)
        {
            this.ViewBag.ViewReturn = "Index";
            this.ViewBag.ViewReturnID = id;
            this.ViewBag.Name = "新增SPA評鑑計分資料頁籤顯示設定";
            this.ViewBag.IsCreateMode = true;
            this.ViewBag.ParamList_AssessmentItem = TET_ParameterService.GetTET_ParametersList("SPA評鑑項目", TET_ParameterService.KeyType.Id);
            this.InitAction(id);

            return View("Edit", new SPA_ScoringInfoSheetModel());
        }
    }
}
