using BI.SPA;
using BI.SPA.Enums;
using BI.SPA_CostService;
using BI.SPA_ScoringInfo;
using BI.SPA_Violation;
using BI.STQA;
using BI.Suppliers;
using BI.PaymentSuppliers;
using Platform.AbstractionClass;
using Platform.Auth;
using Platform.WebSite.Filters;
using Platform.WebSite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Platform.WebSite.Controllers
{
    [AuthorizeCore]
    public class SupplierApprovalController : BaseMVCController
    {
        private TET_SupplierApprovalManager _suuplierApprovalMgr = new TET_SupplierApprovalManager();
        private TET_SupplierManager _supplierMgr = new TET_SupplierManager();
        private TET_STQAApprovalManager _stqaApprovalMgr = new TET_STQAApprovalManager();
        private TET_STQAManager _stqaMgr = new TET_STQAManager();
        private TET_SPAApprovalManager _spaApprovalMgr = new TET_SPAApprovalManager();
        private TET_SPAManager _spaMgr = new TET_SPAManager();
        private UserManager _userMgr = new UserManager();
        private UserRoleManager _userRoleMgr = new UserRoleManager();
        private TET_PaymentSupplierApprovalManager _paymentsupplierApprovalMgr = new TET_PaymentSupplierApprovalManager();
        private TET_PaymentSupplierManager _paymentsupplierMgr = new TET_PaymentSupplierManager();
		private SPA_CostServiceApprovalManager _costServiceApprovalManager = new SPA_CostServiceApprovalManager();
        private SPA_ViolationApprovalManager _violationApprovalManager = new SPA_ViolationApprovalManager();
        private SPA_ScoringInfoApprovalManager _scoringInfoApprovalManager = new SPA_ScoringInfoApprovalManager();
        private SPA_ScoringInfoManager _scoringInfoManager = new SPA_ScoringInfoManager();


        // GET: SupplierApproval
        public ActionResult Index(Guid? id)
        {
            // 如果 ID 不存在，用模組名稱找出，並跳頁
            if (!id.HasValue)
            {
                return this.FindModuleAndRedirectToPage(BI.Suppliers.ModuleConfig.ModuleName_SupplierApproval);
            }

            this.InitAction(id);
            return View();
        }


        // GET: SupplierApproval/SupplierApproval
        public ActionResult SupplierApproval(Guid id, Guid approvalID)
        {
            this.ViewBag.ViewReturn = "Index";
            this.ViewBag.ViewReturnID = id;
            this.ViewBag.IsCreateMode = false;

            this.ViewBag.ParamList_SupplierCategory = TET_ParameterService.GetTET_ParametersList("廠商類別");
            this.ViewBag.ParamList_BusinessCategory = TET_ParameterService.GetTET_ParametersList("交易主類別");
            this.ViewBag.ParamList_BusinessAttribute = TET_ParameterService.GetTET_ParametersList("交易子類別");
            this.ViewBag.ParamList_Country = TET_ParameterService.GetTET_ParametersList("國家別");
            this.ViewBag.ParamList_PaymentTerm = TET_ParameterService.GetTET_ParametersList("付款條件");
            this.ViewBag.ParamList_Incoterms = TET_ParameterService.GetTET_ParametersList("交易條件");
            this.ViewBag.ParamList_BillingDocument = TET_ParameterService.GetTET_ParametersList("請款憑證");
            this.ViewBag.ParamList_Currency = TET_ParameterService.GetTET_ParametersList("幣別");
            this.ViewBag.ParamList_BankCountry = TET_ParameterService.GetTET_ParametersList("銀行國別");
            this.ViewBag.ParamList_RelatedDept = TET_ParameterService.GetTET_ParametersList("BU");
            this.ViewBag.ParamList_Buyer = this.GetAllEmp();
            this.ViewBag.ParamList_SupplierStatus = TET_ParameterService.GetTET_ParametersList("供應商狀態");

            // 修改模式
            var model = this._suuplierApprovalMgr.GetTET_SupplierApproval(approvalID);
            if (model == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            var supplierModel = this._supplierMgr.GetTET_Supplier(model.SupplierID);
            if (supplierModel == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            this.ViewBag.Name = supplierModel.CName;
            this.ViewBag.CurrentLevelName = model.Level;
            this.ViewBag.IsCreateMode = false;
            this.ViewBag.Mode = "Edit";
            this.ViewBag.SupplierID = model.SupplierID;
            this.InitAction(id);
            return View(approvalID);
        }

        // GET: SupplierApproval/RevisionApproval
        public ActionResult RevisionApproval(Guid id, Guid approvalID)
        {
            this.ViewBag.ViewReturn = "Index";
            this.ViewBag.ViewReturnID = id;
            this.ViewBag.IsCreateMode = false;

            this.ViewBag.ParamList_SupplierCategory = TET_ParameterService.GetTET_ParametersList("廠商類別");
            this.ViewBag.ParamList_BusinessCategory = TET_ParameterService.GetTET_ParametersList("交易主類別");
            this.ViewBag.ParamList_BusinessAttribute = TET_ParameterService.GetTET_ParametersList("交易子類別");
            this.ViewBag.ParamList_Country = TET_ParameterService.GetTET_ParametersList("國家別");
            this.ViewBag.ParamList_PaymentTerm = TET_ParameterService.GetTET_ParametersList("付款條件");
            this.ViewBag.ParamList_Incoterms = TET_ParameterService.GetTET_ParametersList("交易條件");
            this.ViewBag.ParamList_BillingDocument = TET_ParameterService.GetTET_ParametersList("請款憑證");
            this.ViewBag.ParamList_Currency = TET_ParameterService.GetTET_ParametersList("幣別");
            this.ViewBag.ParamList_BankCountry = TET_ParameterService.GetTET_ParametersList("銀行國別");
            this.ViewBag.ParamList_RelatedDept = TET_ParameterService.GetTET_ParametersList("BU");
            this.ViewBag.ParamList_Buyer = this.GetAllEmp();
            this.ViewBag.ParamList_SupplierStatus = TET_ParameterService.GetTET_ParametersList("供應商狀態");

            // 修改模式
            var model = this._suuplierApprovalMgr.GetTET_SupplierApproval(approvalID);
            if (model == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            var supplierModel = this._supplierMgr.GetTET_Supplier(model.SupplierID);
            if (supplierModel == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            this.ViewBag.Name = supplierModel.CName;
            this.ViewBag.CurrentLevelName = model.Level;
            this.ViewBag.IsCreateMode = false;
            this.ViewBag.Mode = "Edit";
            this.ViewBag.SupplierID = model.SupplierID;
            this.InitAction(id);
            return View(approvalID);
        }


        // GET: SupplierApproval/STQAApproval
        public ActionResult STQAApproval(Guid id, Guid approvalID)
        {
            var cUser = UserProfileService.GetCurrentUser();

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
            var model = this._stqaApprovalMgr.GetDetail(approvalID);
            if (model == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            var parentModel = this._stqaMgr.GetSTQA(model.STQAID);
            if (parentModel == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            // 檢查是否可以閱讀，如果不行就回傳 401
            if (!this._stqaMgr.CanRead(model.STQAID, cUser.ID))
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);


            this.ViewBag.Name = parentModel.BelongTo;
            this.ViewBag.CurrentLevelName = model.Level;
            this.ViewBag.IsCreateMode = false;
            this.ViewBag.Mode = "Edit";
            this.ViewBag.ParentID = model.STQAID;
            this.InitAction(id);
            return View(approvalID);
        }


        // GET: SupplierApproval/SPAApproval
        public ActionResult SPAApproval(Guid id, Guid approvalID)
        {
            var cUser = UserProfileService.GetCurrentUser();

            this.ViewBag.ViewReturn = "Index";
            this.ViewBag.ViewReturnID = id;
            this.ViewBag.IsCreateMode = false;

            this.ViewBag.ParamList_BelongTo = this._supplierMgr.GetBelongToList();
            this.ViewBag.ParamList_BU = TET_ParameterService.GetTET_ParametersList("SPA評鑑單位");
            this.ViewBag.ParamList_AssessmentItem = TET_ParameterService.GetTET_ParametersList("SPA評鑑項目");
            this.ViewBag.ParamList_ServiceFor = TET_ParameterService.GetTET_ParametersList("SPA服務對象");
            this.ViewBag.ParamList_PerformanceLevel = TET_ParameterService.GetTET_ParametersList("SPA Level");

            // 修改模式
            var model = this._spaApprovalMgr.GetDetail(approvalID);
            if (model == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            var parentModel = this._spaMgr.GetSPA(model.SPAID);
            if (parentModel == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            // 檢查是否可以閱讀，如果不行就回傳 401
            if (!this._spaMgr.CanRead(model.SPAID, cUser.ID))
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            this.ViewBag.Name = parentModel.BelongTo;
            this.ViewBag.CurrentLevelName = model.Level;
            this.ViewBag.IsCreateMode = false;
            this.ViewBag.Mode = "Edit";
            this.ViewBag.ParentID = model.SPAID;
            this.InitAction(id);
            return View(approvalID);
        }

        // GET: SupplierApproval/SPA_CostServiceApproval
        public ActionResult SPA_CostServiceApproval(Guid id, Guid approvalID)
        {
            var cUser = UserProfileService.GetCurrentUser();

            this.ViewBag.ViewReturn = "Index";
            this.ViewBag.ViewReturnID = id;
            this.ViewBag.IsCreateMode = false;

            // 查詢下拉選單用內容
            this.ViewBag.ParamList_ServiceFor = TET_ParameterService.GetTET_ParametersList("SPA服務對象");
            this.ViewBag.ParamList_BU = TET_ParameterService.GetTET_ParametersList("SPA評鑑單位");
            this.ViewBag.ParamList_BelongTo = this._supplierMgr.GetBelongToList();
            this.ViewBag.ParamList_AssessmentItem = TET_ParameterService.GetTET_ParametersList("SPA評鑑項目");
            this.ViewBag.ParamList_PriceDeflator = TET_ParameterService.GetTET_ParametersList("SPA價格競爭力");
            this.ViewBag.ParamList_PaymentTerm = TET_ParameterService.GetTET_ParametersList("SPA付款條件");
            this.ViewBag.ParamList_Cooperation = TET_ParameterService.GetTET_ParametersList("SPA配合度");

            // 修改模式
            var model = this._costServiceApprovalManager.GetDetail(approvalID);
            if (model == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            this.ViewBag.Name = "審核Cost&Service資料維護";
            this.ViewBag.CurrentLevelName = model.Level;
            this.ViewBag.IsCreateMode = false;
            this.ViewBag.Mode = "Edit";
            this.ViewBag.ParentID = model.CSID;
            this.InitAction(id);
            return View(approvalID);
        }

        // GET: SupplierApproval/SPA_ViolationApproval
        public ActionResult SPA_ViolationApproval(Guid id, Guid approvalID)
        {
            var cUser = UserProfileService.GetCurrentUser();

            this.ViewBag.ViewReturn = "Index";
            this.ViewBag.ViewReturnID = id;
            this.ViewBag.IsCreateMode = false;

            // 查詢下拉選單用內容
            this.ViewBag.ParamList_BU = TET_ParameterService.GetTET_ParametersList("SPA評鑑單位");
            this.ViewBag.ParamList_BelongTo = this._supplierMgr.GetBelongToList();
            this.ViewBag.ParamList_AssessmentItem = TET_ParameterService.GetTET_ParametersList("SPA評鑑項目");
            this.ViewBag.ParamList_ViolationMiddleCategory = TET_ParameterService.GetTET_ParametersList("SPA違規紀錄中分類");
            this.ViewBag.ParamList_ViolationSmallCategory = TET_ParameterService.GetTET_ParametersList("SPA違規紀錄小分類");

            // 修改模式
            var model = this._violationApprovalManager.GetDetail(approvalID);
            if (model == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            this.ViewBag.Name = "審核違規紀錄資料維護";
            this.ViewBag.CurrentLevelName = model.Level;
            this.ViewBag.IsCreateMode = false;
            this.ViewBag.Mode = "Edit";
            this.ViewBag.ParentID = model.ViolationID;
            this.InitAction(id);
            return View(approvalID);
        }

        // GET: SupplierApproval/SPA_ScoringInfoApproval
        public ActionResult SPA_ScoringInfoApproval(Guid id, Guid approvalID)
        {
            var cUser = UserProfileService.GetCurrentUser();

            this.ViewBag.ViewReturn = "Index";
            this.ViewBag.ViewReturnID = id;
            this.ViewBag.IsCreateMode = false;

            // 查詢下拉選單用內容
            this.ViewBag.ParamList_MajorJob = TET_ParameterService.GetTET_ParametersList("SPA主要負責作業");
            this.ViewBag.ParamList_ServiceFor = TET_ParameterService.GetTET_ParametersList("SPA服務對象");
            this.ViewBag.ParamList_WorkItem = TET_ParameterService.GetTET_ParametersList("SPA作業項目");


            // 修改模式
            var model = this._scoringInfoApprovalManager.GetOne(approvalID);
            if (model == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            this.ViewBag.Name = "審核 供應商SPA評鑑計分資料維護 ";
            this.ViewBag.CurrentLevelName = model.Level;
            this.ViewBag.IsCreateMode = false;
            this.ViewBag.Mode = "Edit";
            this.ViewBag.ParentID = model.SIID;

            var mainModel = this._scoringInfoManager.GetOne(model.SIID);
            this.ViewBag.TabVisiable = BI.SPA_ScoringInfo.Utils.ViewUtils.ComputeAcl(mainModel);

            this.InitAction(id);
            return View(approvalID);
        }

        /// <summary> 由於分散的 js 有讀取順序的問題，故整合為一個 </summary>
        /// <returns></returns>
        // GET: SupplierApproval/SPA_ScoringInfoApproval_TabsJS
        public ActionResult SPA_ScoringInfoApproval_TabsJS()
        {
            var pathes =
                new string[] {
                    "~/ModuleResources/JavaScripts/SupplierApproval/SPA_ScoringInfoApproval/SPA_ScoringInfoApproval.js",
                    "~/ModuleResources/JavaScripts/SupplierApproval/SPA_ScoringInfoApproval/editTab1.js",
                    "~/ModuleResources/JavaScripts/SupplierApproval/SPA_ScoringInfoApproval/editTab2.js",
                    "~/ModuleResources/JavaScripts/SupplierApproval/SPA_ScoringInfoApproval/editTab3.js",
                    "~/ModuleResources/JavaScripts/SupplierApproval/SPA_ScoringInfoApproval/editTab4.js",
                    "~/ModuleResources/JavaScripts/SupplierApproval/SPA_ScoringInfoApproval/editTab5.js",
                    "~/ModuleResources/JavaScripts/SupplierApproval/SPA_ScoringInfoApproval/editTab6.js",
                    "~/ModuleResources/JavaScripts/SupplierApproval/SPA_ScoringInfoApproval/editTab7.js",
                };

            var result = string.Empty;

            foreach (var path in pathes)
            {
                try
                {
                    var jsContent = System.IO.File.ReadAllText(Server.MapPath(path));
                    result += Environment.NewLine + Environment.NewLine + Environment.NewLine + jsContent;
                }
                catch (Exception ex)
                {
                    return HttpNotFound(ex.ToString());
                }
            }

            return Content(result, "text/javascript");
        }

        // GET: SupplierApproval/PaymentSupplierApproval
        public ActionResult PaymentSupplierApproval(Guid id, Guid approvalID)
        {
            this.ViewBag.ViewReturn = "Index";
            this.ViewBag.ViewReturnID = id;
            this.ViewBag.IsCreateMode = false;

            this.ViewBag.ParamList_Country = TET_ParameterService.GetTET_ParametersList("國家別");
            this.ViewBag.ParamList_PaymentTerm = TET_ParameterService.GetTET_ParametersList("付款條件");
            this.ViewBag.ParamList_Incoterms = TET_ParameterService.GetTET_ParametersList("交易條件");
            this.ViewBag.ParamList_BillingDocument = TET_ParameterService.GetTET_ParametersList("請款憑證");
            this.ViewBag.ParamList_Currency = TET_ParameterService.GetTET_ParametersList("幣別");
            this.ViewBag.ParamList_BankCountry = TET_ParameterService.GetTET_ParametersList("銀行國別");

            // 修改模式
            var model = this._paymentsupplierApprovalMgr.GetTET_PaymentSupplierApproval(approvalID);
            if (model == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            var supplierModel = this._paymentsupplierMgr.GetTET_PaymentSupplier(model.PSID);
            if (supplierModel == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            this.ViewBag.Name = supplierModel.CName;
            this.ViewBag.CurrentLevelName = model.Level;
            this.ViewBag.IsCreateMode = false;
            this.ViewBag.Mode = "Edit";
            this.ViewBag.SupplierID = model.PSID;
            this.InitAction(id);
            return View(approvalID);
        }

        // GET: SupplierApproval/PaymentSupplierRevisionApproval
        public ActionResult PaymentSupplierRevisionApproval(Guid id, Guid approvalID)
        {
            this.ViewBag.ViewReturn = "Index";
            this.ViewBag.ViewReturnID = id;
            this.ViewBag.IsCreateMode = false;
            this.ViewBag.ParamList_Country = TET_ParameterService.GetTET_ParametersList("國家別");
            this.ViewBag.ParamList_PaymentTerm = TET_ParameterService.GetTET_ParametersList("付款條件");
            this.ViewBag.ParamList_Incoterms = TET_ParameterService.GetTET_ParametersList("交易條件");
            this.ViewBag.ParamList_BillingDocument = TET_ParameterService.GetTET_ParametersList("請款憑證");
            this.ViewBag.ParamList_Currency = TET_ParameterService.GetTET_ParametersList("幣別");
            this.ViewBag.ParamList_BankCountry = TET_ParameterService.GetTET_ParametersList("銀行國別");

            // 修改模式
            var model = this._paymentsupplierApprovalMgr.GetTET_PaymentSupplierApproval(approvalID);
            if (model == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            var supplierModel = this._paymentsupplierMgr.GetTET_PaymentSupplier(model.PSID);
            if (supplierModel == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            this.ViewBag.Name = supplierModel.CName;
            this.ViewBag.CurrentLevelName = model.Level;
            this.ViewBag.IsCreateMode = false;
            this.ViewBag.Mode = "Edit";
            this.ViewBag.SupplierID = model.PSID;
            this.InitAction(id);
            return View(approvalID);
        }

        private List<KeyTextModel> GetAllEmp()
        {
            var list = this._userRoleMgr.GetUserListInRole(ApprovalRole.SRI_SS.ToID().Value);

            var result =
                (from item in list
                 select new KeyTextModel()
                 {
                     Key = item.EmpID,
                     Text = $"{item.FirstNameEN} {item.LastNameEN}({item.EmpID})"
                 }).ToList();

            return result;
        }
    }
}