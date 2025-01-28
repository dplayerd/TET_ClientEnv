using BI.PaymentSuppliers.Models;
using BI.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Platform.Infra;
using Platform.WebSite.Filters;
using Platform.WebSite.Models;
using Platform.WebSite.Services;
using System.IO;
using System.Web.Hosting;
using Platform.FileSystem;
using Platform.AbstractionClass;
using Platform.Auth;
using BI.Suppliers.Enums;
using BI.PaymentSuppliers;

namespace Platform.WebSite.Controllers
{
    [AuthorizeCore]
    public class PaymentSupplierController : BaseMVCController
    {
        private TET_PaymentSupplierManager _mgr = new TET_PaymentSupplierManager();
        private TET_PaymentSupplierAttachmentManager _attachmentMgr = new TET_PaymentSupplierAttachmentManager();
        private UserManager _userMgr = new UserManager();
        private UserRoleManager _userRoleMgr = new UserRoleManager();

        #region Read
        // GET: Supplier
        public ActionResult Index(Guid? id)
        {
            // 如果 ID 不存在，用模組名稱找出，並跳頁
            if (!id.HasValue)
            {
                return this.FindModuleAndRedirectToPage(BI.PaymentSuppliers.ModuleConfig.ModuleName_Supplier);
            }

            this.InitAction(id);
            return View();
        }
        #endregion

        public ActionResult Create(Guid id)
        {
            this.ViewBag.ViewReturn = "Index";
            this.ViewBag.ViewReturnID = id;
            this.ViewBag.IsCreateMode = true;

            this.ViewBag.ParamList_Country = TET_ParameterService.GetTET_ParametersList("國家別");
            this.ViewBag.ParamList_PaymentTerm = TET_ParameterService.GetTET_ParametersList("付款條件");
            this.ViewBag.ParamList_Incoterms = TET_ParameterService.GetTET_ParametersList("交易條件");
            this.ViewBag.ParamList_BillingDocument = TET_ParameterService.GetTET_ParametersList("請款憑證");
            this.ViewBag.ParamList_Currency = TET_ParameterService.GetTET_ParametersList("幣別");
            this.ViewBag.ParamList_BankCountry = TET_ParameterService.GetTET_ParametersList("銀行國別");
            this.ViewBag.ParamList_CoSignApprover = this.GetAllEmp();

            // 沒有帶 ID ，新增模式
            ViewBag.Name = "新增一般付款對象";
            this.ViewBag.IsCreateMode = true;
            this.ViewBag.Mode = "Create";
            this.InitAction(id);
            return View("Edit");
        }

        public ActionResult Edit(Guid id, Guid supplierID)
        {
            var cUser = UserProfileService.GetCurrentUser();

            // 檢查是否可以閱讀，如果不行就回傳 401
            if (!this._mgr.CanRead(supplierID, cUser.ID))
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            this.ViewBag.ViewReturn = "Index";
            this.ViewBag.ViewReturnID = id;
            this.ViewBag.IsCreateMode = false;

            this.ViewBag.ParamList_Country = TET_ParameterService.GetTET_ParametersList("國家別");
            this.ViewBag.ParamList_PaymentTerm = TET_ParameterService.GetTET_ParametersList("付款條件");
            this.ViewBag.ParamList_Incoterms = TET_ParameterService.GetTET_ParametersList("交易條件");
            this.ViewBag.ParamList_BillingDocument = TET_ParameterService.GetTET_ParametersList("請款憑證");
            this.ViewBag.ParamList_Currency = TET_ParameterService.GetTET_ParametersList("幣別");
            this.ViewBag.ParamList_BankCountry = TET_ParameterService.GetTET_ParametersList("銀行國別");
            this.ViewBag.ParamList_CoSignApprover = this.GetAllEmp();

            // 修改模式
            var model = this._mgr.GetTET_PaymentSupplier(supplierID);
            if (model == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            this.ViewBag.Name = model.CName;
            this.ViewBag.IsCreateMode = false;
            this.ViewBag.SupplierID = model.ID;
            this.ViewBag.Mode = "Edit";
            this.InitAction(id);
            return View(supplierID);
        }

        public ActionResult Detail(Guid id, Guid supplierID)
        {
            this.ViewBag.ViewReturn = "Index";
            this.ViewBag.ViewReturnID = id;
            this.ViewBag.IsCreateMode = true;

            this.ViewBag.ParamList_Country = TET_ParameterService.GetTET_ParametersList("國家別");
            this.ViewBag.ParamList_PaymentTerm = TET_ParameterService.GetTET_ParametersList("付款條件");
            this.ViewBag.ParamList_Incoterms = TET_ParameterService.GetTET_ParametersList("交易條件");
            this.ViewBag.ParamList_BillingDocument = TET_ParameterService.GetTET_ParametersList("請款憑證");
            this.ViewBag.ParamList_Currency = TET_ParameterService.GetTET_ParametersList("幣別");
            this.ViewBag.ParamList_BankCountry = TET_ParameterService.GetTET_ParametersList("銀行國別");

            // 修改模式
            var model = this._mgr.GetTET_PaymentSupplier(supplierID);
            if (model == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            this.ViewBag.Name = model.CName;
            this.ViewBag.IsCreateMode = false;
            this.ViewBag.SupplierID = model.ID;
            this.ViewBag.Mode = "Detail";
            this.InitAction(id);
            return View("Edit", supplierID);
        }

        #region Query

        // GET: PaymentSupplier/Query
        public ActionResult Query(Guid id)
        {
            var cUser = UserProfileService.GetCurrentUser();
            this.ViewBag.canExport = this._mgr.CanExport(cUser.ID);

            this.InitAction(id);

            return View();
        }

        public ActionResult QueryView(Guid id, Guid supplierID)
        {
            this.ViewBag.ViewReturn = "Query";
            this.ViewBag.ViewReturnID = id;
            this.ViewBag.IsCreateMode = false;

            this.ViewBag.ParamList_Country = TET_ParameterService.GetTET_ParametersList("國家別");
            this.ViewBag.ParamList_PaymentTerm = TET_ParameterService.GetTET_ParametersList("付款條件");
            this.ViewBag.ParamList_Incoterms = TET_ParameterService.GetTET_ParametersList("交易條件");
            this.ViewBag.ParamList_BillingDocument = TET_ParameterService.GetTET_ParametersList("請款憑證");
            this.ViewBag.ParamList_Currency = TET_ParameterService.GetTET_ParametersList("幣別");
            this.ViewBag.ParamList_BankCountry = TET_ParameterService.GetTET_ParametersList("銀行國別");

            // 修改模式
            var model = this._mgr.GetTET_PaymentSupplier(supplierID);
            if (model == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            this.ViewBag.Name = model.CName;
            this.ViewBag.ID = model.ID;
            this.ViewBag.IsCreateMode = false;
            this.ViewBag.Mode = "Detail";
            this.InitAction(id);
            return View(supplierID);
        }

        public ActionResult QueryEdit(Guid id, Guid supplierID)
        {
            this.ViewBag.ViewReturn = "Query";
            this.ViewBag.ViewReturnID = id;
            this.ViewBag.IsCreateMode = false;

            this.ViewBag.ParamList_Country = TET_ParameterService.GetTET_ParametersList("國家別");
            this.ViewBag.ParamList_PaymentTerm = TET_ParameterService.GetTET_ParametersList("付款條件");
            this.ViewBag.ParamList_Incoterms = TET_ParameterService.GetTET_ParametersList("交易條件");
            this.ViewBag.ParamList_BillingDocument = TET_ParameterService.GetTET_ParametersList("請款憑證");
            this.ViewBag.ParamList_Currency = TET_ParameterService.GetTET_ParametersList("幣別");
            this.ViewBag.ParamList_BankCountry = TET_ParameterService.GetTET_ParametersList("銀行國別");

            // 修改模式
            var model = this._mgr.GetTET_PaymentSupplier(supplierID);
            if (model == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            this.ViewBag.Name = model.CName;
            this.ViewBag.ID = model.ID;
            this.ViewBag.IsCreateMode = false;
            this.ViewBag.Mode = "Edit";
            this.InitAction(id);
            return View(supplierID);
        }

        #endregion


        [System.Web.Mvc.AllowAnonymous]
        public ActionResult Attachment(Guid id)
        {
            var model = this._attachmentMgr.GetTET_PaymentSupplierAttachment(id);
            if (model == null)
                return HttpNotFound();

            string orgFileName = model.OrgFileName;
            string fileName = model.FileName;
            string mime = MimeMapping.GetMimeMapping(model.OrgFileName);

            // 計算起始路徑，如果不是上傳資料夾根目錄，要附加在最前面
            string rootFolder = MediaFileManager.GetRootFolder();

            if (!model.FilePath.StartsWith(rootFolder, StringComparison.OrdinalIgnoreCase))
                model.FilePath = Path.Combine(rootFolder, model.FilePath);

            // 檢查檔案是否存在
            string path = HostingEnvironment.MapPath("~/" + model.FilePath);
            path = Path.Combine(path, fileName);
            if (!System.IO.File.Exists(path))
                return HttpNotFound();

            return File(path, mime, orgFileName);
        }

        private List<KeyTextModel> GetAllEmp()
        {
            var list = this._userMgr.GetUserList(new Pager() { AllowPaging = true });

            var result =
                (from item in list
                 select new KeyTextModel()
                 {
                     Key = item.EmpID,
                     Text = $"{item.FirstNameEN} {item.LastNameEN}({item.EmpID}) - {item.UnitName}"
                 }).ToList();

            return result;
        }
    }
}
