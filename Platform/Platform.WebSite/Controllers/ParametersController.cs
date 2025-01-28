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
    public class ParametersController : BaseMVCController
    {
        private TET_SPAManager _mgr = new TET_SPAManager();
        private TET_SupplierManager _supplierMgr = new TET_SupplierManager();


        // GET: SPA
        public ActionResult Index(Guid id)
        {
            this.InitAction(id);

            this.ViewBag.ParamList_Type = TET_ParameterService.GetTET_ParametersTypeList();

            return View();
        }
    }
}