using BI.SPA_CostService;
using BI.Suppliers;
using Platform.AbstractionClass;
using Platform.WebSite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BI.SPA_CostService.Enums;
using System.Net;
using System.Web.Services.Description;
using BI.SPA_ApproverSetup;
using Platform.FileSystem;
using System.IO;
using System.Web.Hosting;
using BI.Shared.Utils;


namespace Platform.WebSite.Controllers
{
    public class SPA_CostServiceController : BaseMVCController
    {
        private SPA_CostServiceManager _mgr = new SPA_CostServiceManager();
        private TET_SupplierManager _supplierMgr = new TET_SupplierManager();
        private SPA_PeriodManager _spaPeriodMgr = new SPA_PeriodManager();


        // GET: SPA_CostService
        public ActionResult Index(Guid? id)
        {
            // 如果 ID 不存在，用模組名稱找出，並跳頁
            var cUser = UserProfileService.GetCurrentUser();

            if (!id.HasValue)
            {
                return this.FindModuleAndRedirectToPage(BI.SPA_CostService.ModuleConfig.ModuleName);
            }

            this.ViewBag.ParamList_ApproveStatus = new List<KeyTextModel>()
            {
                new KeyTextModel() { Key = string.Empty, Text = "未送出" },
                new KeyTextModel() { Key = ApprovalStatus.Verify.ToText(), Text = ApprovalStatus.Verify.ToText() },
                new KeyTextModel() { Key = ApprovalStatus.Completed.ToText(), Text = ApprovalStatus.Completed.ToText() },
                new KeyTextModel() { Key = ApprovalStatus.Rejected.ToText(), Text = ApprovalStatus.Rejected.ToText() }
            };


            // 其它值
            var startingPeriod = this._spaPeriodMgr.GetStartingDetail();
            this.ViewBag.Param_Period = startingPeriod.Period;
            this.ViewBag.CurrentUser = cUser.ID;

            this.InitAction(id);
            return View();
        }

        public ActionResult Create(Guid id)
        {
            this.ViewBag.ViewReturn = "Index";
            this.ViewBag.ViewReturnID = id;
            this.ViewBag.IsCreateMode = true;

            // 如果新增時，沒有執行中的評鑑期間，跳回列表頁
            var startingPeriod = this._spaPeriodMgr.GetStartingDetail();
            if (startingPeriod == null)
                return RedirectToAction(nameof(Index), new { id = id });


            // 查詢下拉選單用內容
            this.ViewBag.ParamList_ServiceFor = TET_ParameterService.GetTET_ParametersList("SPA服務對象");
            this.ViewBag.ParamList_BU = TET_ParameterService.GetTET_ParametersList("SPA評鑑單位");
            this.ViewBag.ParamList_BelongTo = this._supplierMgr.GetBelongToList();
            this.ViewBag.ParamList_AssessmentItem = TET_ParameterService.GetTET_ParametersList("SPA評鑑項目");
            this.ViewBag.ParamList_PriceDeflator = TET_ParameterService.GetTET_ParametersList("SPA價格競爭力");
            this.ViewBag.ParamList_PaymentTerm = TET_ParameterService.GetTET_ParametersList("SPA付款條件");
            this.ViewBag.ParamList_Cooperation = TET_ParameterService.GetTET_ParametersList("SPA配合度");

            // 其它值
            var period = PeriodUtil.ParsePeriod(startingPeriod.Period);
            this.ViewBag.Param_Period = startingPeriod.Period;
            this.ViewBag.Param_PeriodStart = period.StartDate?.ToString("yyyy-MM-dd");
            this.ViewBag.Param_PeriodEnd = period.EndDate?.ToString("yyyy-MM-dd");


            // 沒有帶 ID ，新增模式
            this.ViewBag.Name = "新增Cost&Service資料維護";
            this.ViewBag.IsCreateMode = true;
            this.ViewBag.Mode = "Create";
            this.InitAction(id);
            return View("Edit", null);
        }

        public ActionResult Edit(Guid id, Guid spa_CostServiceID)
        {
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
            var model = this._mgr.GetOne(spa_CostServiceID);
            if (model == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            this.ViewBag.Name = "修改Cost&Service資料維護";
            this.ViewBag.IsCreateMode = false;
            this.ViewBag.Mode = "Edit";
            this.InitAction(id);
            return View("Edit", spa_CostServiceID);
        }

        public ActionResult Detail(Guid id, Guid spa_CostServiceID)
        {
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
            var model = this._mgr.GetOne(spa_CostServiceID);
            if (model == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            this.ViewBag.Name = "檢視Cost&Service資料維護";
            this.ViewBag.IsCreateMode = false;
            this.ViewBag.Mode = "Detail";
            this.InitAction(id);
            return View("Edit", spa_CostServiceID);
        }

        // 附件
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult Attachment(Guid id)
        {
            var model = this._mgr.GetAttachment(id);
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
    }
}