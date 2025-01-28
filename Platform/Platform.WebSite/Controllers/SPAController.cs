using BI.SPA;
using BI.Suppliers;
using Platform.WebSite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Platform.WebSite.Controllers
{
    public class SPAController : BaseMVCController
    {
        private TET_SPAManager _mgr = new TET_SPAManager();
        private TET_SupplierManager _supplierMgr = new TET_SupplierManager();


        // GET: SPA
        public ActionResult Index(Guid? id)
        {
            // 如果 ID 不存在，用模組名稱找出，並跳頁
            if (!id.HasValue)
            {
                return this.FindModuleAndRedirectToPage(BI.SPA.ModuleConfig.ModuleName);
            }

            this.InitAction(id);

            this.ViewBag.ParamList_BelongTo = this._supplierMgr.GetBelongToList();
            this.ViewBag.ParamList_Period = this._mgr.GetPeriodList();
            this.ViewBag.ParamList_BU = TET_ParameterService.GetTET_ParametersList("SPA評鑑單位");
            this.ViewBag.ParamList_AssessmentItem = TET_ParameterService.GetTET_ParametersList("SPA評鑑項目");
            this.ViewBag.ParamList_PerformanceLevel = TET_ParameterService.GetTET_ParametersList("SPA Level");

            return View();
        }

        public ActionResult Create(Guid id)
        {
            this.ViewBag.ViewReturn = "Index";
            this.ViewBag.ViewReturnID = id;
            this.ViewBag.IsCreateMode = true;

            this.ViewBag.ParamList_BelongTo = this._supplierMgr.GetBelongToList();
            this.ViewBag.ParamList_BU = TET_ParameterService.GetTET_ParametersList("SPA評鑑單位");
            this.ViewBag.ParamList_AssessmentItem = TET_ParameterService.GetTET_ParametersList("SPA評鑑項目");
            this.ViewBag.ParamList_ServiceFor = TET_ParameterService.GetTET_ParametersList("SPA服務對象");
            this.ViewBag.ParamList_PerformanceLevel = TET_ParameterService.GetTET_ParametersList("SPA Level");


            // 沒有帶 ID ，新增模式
            this.ViewBag.Name = "新增SPA";
            this.ViewBag.IsCreateMode = true;
            this.ViewBag.Mode = "Create";
            this.InitAction(id);
            return View("Edit");
        }

        public ActionResult Edit(Guid id, Guid spaID)
        {
            this.ViewBag.ViewReturn = "Index";
            this.ViewBag.ViewReturnID = id;
            this.ViewBag.IsCreateMode = false;

            this.ViewBag.ParamList_BelongTo = this._supplierMgr.GetBelongToList();
            this.ViewBag.ParamList_BU = TET_ParameterService.GetTET_ParametersList("SPA評鑑單位");
            this.ViewBag.ParamList_AssessmentItem = TET_ParameterService.GetTET_ParametersList("SPA評鑑項目");
            this.ViewBag.ParamList_ServiceFor = TET_ParameterService.GetTET_ParametersList("SPA服務對象");
            this.ViewBag.ParamList_PerformanceLevel = TET_ParameterService.GetTET_ParametersList("SPA Level");

            // 修改模式
            var model = this._mgr.GetSPA(spaID);
            if (model == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            this.ViewBag.Name = model.BelongTo;
            this.ViewBag.IsCreateMode = false;
            this.ViewBag.Mode = "Edit";
            this.InitAction(id);
            return View(spaID);
        }

        public ActionResult Detail(Guid id, Guid spaID)
        {
            this.ViewBag.ViewReturn = "Index";
            this.ViewBag.ViewReturnID = id;
            this.ViewBag.IsCreateMode = true;

            this.ViewBag.ParamList_BelongTo = this._supplierMgr.GetBelongToList();
            this.ViewBag.ParamList_BU = TET_ParameterService.GetTET_ParametersList("SPA評鑑單位");
            this.ViewBag.ParamList_AssessmentItem = TET_ParameterService.GetTET_ParametersList("SPA評鑑項目");
            this.ViewBag.ParamList_ServiceFor = TET_ParameterService.GetTET_ParametersList("SPA服務對象");
            this.ViewBag.ParamList_PerformanceLevel = TET_ParameterService.GetTET_ParametersList("SPA Level");

            // 修改模式
            var model = this._mgr.GetSPA(spaID);
            if (model == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            this.ViewBag.Name = model.BelongTo;
            this.ViewBag.IsCreateMode = false;
            this.ViewBag.Mode = "Detail";
            this.InitAction(id);
            return View("Edit", spaID);
        }


        // GET: SPA/SingleQuery
        public ActionResult SingleQuery(Guid id)
        {
            this.InitAction(id);

            this.ViewBag.ParamList_BelongTo = this._supplierMgr.GetBelongToList();
            this.ViewBag.ParamList_Period = this._mgr.GetPeriodList(true);
            this.ViewBag.ParamList_BU = TET_ParameterService.GetTET_ParametersList("SPA評鑑單位");
            this.ViewBag.ParamList_AssessmentItem = TET_ParameterService.GetTET_ParametersList("SPA評鑑項目");
            this.ViewBag.ParamList_PerformanceLevel = TET_ParameterService.GetTET_ParametersList("SPA Level");

            return View();
        }


        // GET: SPA/MultiQuery
        public ActionResult MultiQuery(Guid id)
        {
            this.InitAction(id);

            this.ViewBag.ParamList_BelongTo = this._supplierMgr.GetBelongToList();
            this.ViewBag.ParamList_Period = this._mgr.GetPeriodList(true);
            this.ViewBag.ParamList_BU = TET_ParameterService.GetTET_ParametersList("SPA評鑑單位");
            this.ViewBag.ParamList_AssessmentItem = TET_ParameterService.GetTET_ParametersList("SPA評鑑項目");

            return View();
        }
    }
}