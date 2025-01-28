using BI.STQA;
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
    public class STQAController : BaseMVCController
    {
        private TET_STQAManager _mgr = new TET_STQAManager();
        private TET_SupplierManager _supplierMgr = new TET_SupplierManager();


        // GET: STQA
        public ActionResult Index(Guid? id)
        {
            // 如果 ID 不存在，用模組名稱找出，並跳頁
            if (!id.HasValue)
            {
                return this.FindModuleAndRedirectToPage(BI.STQA.ModuleConfig.ModuleName);
            }


            this.InitAction(id);   
            
            this.ViewBag.ParamList_BelongTo = this._supplierMgr.GetBelongToList();
            this.ViewBag.ParamList_BusinessTerm = TET_ParameterService.GetTET_ParametersList("STQA業務類別");
            this.ViewBag.ParamList_Type = TET_ParameterService.GetTET_ParametersList("STQA方式");

            return View();
        }

        public ActionResult Create(Guid id)
        {
            this.ViewBag.ViewReturn = "Index";
            this.ViewBag.ViewReturnID = id;
            this.ViewBag.IsCreateMode = true;

            this.ViewBag.ParamList_BelongTo = this._supplierMgr.GetBelongToList();
            this.ViewBag.ParamList_Purpose = TET_ParameterService.GetTET_ParametersList("STQA理由");
            this.ViewBag.ParamList_BusinessTerm = TET_ParameterService.GetTET_ParametersList("STQA業務類別");
            this.ViewBag.ParamList_Type = TET_ParameterService.GetTET_ParametersList("STQA方式");
            this.ViewBag.ParamList_UnitALevel = TET_ParameterService.GetTET_ParametersList("STQA Level");
            this.ViewBag.ParamList_UnitCLevel = this.ViewBag.ParamList_UnitALevel;
            this.ViewBag.ParamList_UnitDLevel = this.ViewBag.ParamList_UnitALevel;

            // 沒有帶 ID ，新增模式
            this.ViewBag.Name = "新增STQA";
            this.ViewBag.IsCreateMode = true;
            this.ViewBag.Mode = "Create";
            this.InitAction(id);
            return View("Edit");
        }


        public ActionResult Edit(Guid id, Guid stqaID)
        {
            this.ViewBag.ViewReturn = "Index";
            this.ViewBag.ViewReturnID = id;
            this.ViewBag.IsCreateMode = false;

            this.ViewBag.ParamList_BelongTo = this._supplierMgr.GetBelongToList();
            this.ViewBag.ParamList_Purpose = TET_ParameterService.GetTET_ParametersList("STQA理由");
            this.ViewBag.ParamList_BusinessTerm = TET_ParameterService.GetTET_ParametersList("STQA業務類別");
            this.ViewBag.ParamList_Type = TET_ParameterService.GetTET_ParametersList("STQA方式");
            this.ViewBag.ParamList_UnitALevel = TET_ParameterService.GetTET_ParametersList("STQA Level");
            this.ViewBag.ParamList_UnitCLevel = this.ViewBag.ParamList_UnitALevel;
            this.ViewBag.ParamList_UnitDLevel = this.ViewBag.ParamList_UnitALevel;

            // 修改模式
            var model = this._mgr.GetSTQA(stqaID);
            if (model == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            this.ViewBag.Name = model.BelongTo;
            this.ViewBag.IsCreateMode = false;
            this.ViewBag.Mode = "Edit";
            this.InitAction(id);
            return View(stqaID);
        }

        public ActionResult Detail(Guid id, Guid stqaID)
        {
            this.ViewBag.ViewReturn = "Index";
            this.ViewBag.ViewReturnID = id;
            this.ViewBag.IsCreateMode = true;

            this.ViewBag.ParamList_BelongTo = this._supplierMgr.GetBelongToList();
            this.ViewBag.ParamList_Purpose = TET_ParameterService.GetTET_ParametersList("STQA理由");
            this.ViewBag.ParamList_BusinessTerm = TET_ParameterService.GetTET_ParametersList("STQA業務類別");
            this.ViewBag.ParamList_Type = TET_ParameterService.GetTET_ParametersList("STQA方式");
            this.ViewBag.ParamList_UnitALevel = TET_ParameterService.GetTET_ParametersList("STQA Level");
            this.ViewBag.ParamList_UnitCLevel = this.ViewBag.ParamList_UnitALevel;
            this.ViewBag.ParamList_UnitDLevel = this.ViewBag.ParamList_UnitALevel;

            // 修改模式
            var model = this._mgr.GetSTQA(stqaID);
            if (model == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            this.ViewBag.Name = model.BelongTo;
            this.ViewBag.IsCreateMode = false;
            this.ViewBag.Mode = "Detail";
            this.InitAction(id);
            return View("Edit", stqaID);
        }
    }
}