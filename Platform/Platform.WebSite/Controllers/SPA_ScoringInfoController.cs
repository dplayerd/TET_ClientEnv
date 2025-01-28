using BI.Shared.Utils;
using BI.SPA_ApproverSetup;
using BI.SPA_CostService;
using BI.SPA_CostService.Enums;
using BI.SPA_ScoringInfo;
using BI.SPA_ScoringInfo.Utils;
using BI.Suppliers;
using Platform.AbstractionClass;
using Platform.FileSystem;
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
    public class SPA_ScoringInfoController : BaseMVCController
    {
        private SPA_ScoringInfoManager _mgr = new SPA_ScoringInfoManager();
        private SPA_PeriodManager _spaPeriodMgr = new SPA_PeriodManager();
        private TET_SupplierManager _supplierMgr = new TET_SupplierManager();

        // GET: SPA_ScoringInfo
        public ActionResult Index(Guid? id)
        {
            // 如果 ID 不存在，用模組名稱找出，並跳頁
            var cUser = UserProfileService.GetCurrentUser();

            if (!id.HasValue)
            {
                return this.FindModuleAndRedirectToPage(BI.SPA_ScoringInfo.ModuleConfig.ModuleName);
            }

            this.ViewBag.ParamList_BU = TET_ParameterService.GetTET_ParametersList("SPA評鑑單位");
            this.ViewBag.ParamList_ServiceFor = TET_ParameterService.GetTET_ParametersList("SPA服務對象");
            this.ViewBag.paramList_ServiceItem = TET_ParameterService.GetTET_ParametersList("SPA評鑑項目");
            this.ViewBag.ParamList_BelongTo = this._supplierMgr.GetBelongToList();
            this.ViewBag.ParamList_ApproveStatus = new List<KeyTextModel>()
            {
                new KeyTextModel() { Key = string.Empty, Text = "未送出" },
                new KeyTextModel() { Key = ApprovalStatus.Verify.ToText(), Text = ApprovalStatus.Verify.ToText() },
                new KeyTextModel() { Key = ApprovalStatus.Completed.ToText(), Text = ApprovalStatus.Completed.ToText() },
                new KeyTextModel() { Key = ApprovalStatus.Rejected.ToText(), Text = ApprovalStatus.Rejected.ToText() }
            };


            // 其它值
            this.ViewBag.CurrentUser = cUser.ID;

            this.InitAction(id);
            return View();
        }

        // GET: Edit/
        public ActionResult Edit(Guid id, Guid SPA_ScoringInfoID)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            this.ViewBag.ViewReturn = "Index";
            this.ViewBag.ViewReturnID = id;
            this.ViewBag.IsCreateMode = false;

            // 查詢下拉選單用內容
            this.ViewBag.ParamList_MajorJob = TET_ParameterService.GetTET_ParametersList("SPA主要負責作業");
            this.ViewBag.ParamList_ServiceFor = TET_ParameterService.GetTET_ParametersList("SPA服務對象");
            this.ViewBag.ParamList_WorkItem = TET_ParameterService.GetTET_ParametersList("SPA作業項目");

            // 修改模式
            var model = this._mgr.GetOne(SPA_ScoringInfoID);
            if (model == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            this.ViewBag.Name = "修改";
            this.ViewBag.IsCreateMode = false;
            this.ViewBag.Mode = "Edit";
            this.ViewBag.TabVisiable = ViewUtils.ComputeAcl(model);
            this.ViewBag.CanSubmit = this._mgr.GetSetupInfoConfirm(model.BU, model.ServiceItem, cUser.ID, cTime).Contains(cUser.ID);
            this.ViewBag.AllowButton = true;
            this.InitAction(id);
            return View("Edit", SPA_ScoringInfoID);
        }

        // GET: Detail/
        public ActionResult Detail(Guid id, Guid SPA_ScoringInfoID, bool allowButton = true)
        {
            this.ViewBag.ViewReturn = "Index";
            this.ViewBag.ViewReturnID = id;
            this.ViewBag.IsCreateMode = false;

            // 查詢下拉選單用內容
            this.ViewBag.ParamList_MajorJob = TET_ParameterService.GetTET_ParametersList("SPA主要負責作業");
            this.ViewBag.ParamList_ServiceFor = TET_ParameterService.GetTET_ParametersList("SPA服務對象");
            this.ViewBag.ParamList_WorkItem = TET_ParameterService.GetTET_ParametersList("SPA作業項目");

            // 修改模式
            var model = this._mgr.GetOne(SPA_ScoringInfoID);
            if (model == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            this.ViewBag.Name = "檢視";
            this.ViewBag.IsCreateMode = false;
            this.ViewBag.Mode = "Detail";
            this.ViewBag.TabVisiable = ViewUtils.ComputeAcl(model);
            this.ViewBag.CanSubmit = false;
            this.ViewBag.AllowButton = allowButton;
            this.InitAction(id);
            return View("Edit", SPA_ScoringInfoID);
        }

        /// <summary> 由於分散的 js 有讀取順序的問題，故整合為一個 </summary>
        /// <returns></returns>
        // GET: SPA_ScoringInfo/SPA_ScoringInfo_TabsJS
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult SPA_ScoringInfo_TabsJS()
        {
            var pathes =
                new string[] {
                    "~/ModuleResources/JavaScripts/SPA_ScoringInfo/edit.js",
                    "~/ModuleResources/JavaScripts/SPA_ScoringInfo/editTab1.js",
                    "~/ModuleResources/JavaScripts/SPA_ScoringInfo/editTab2.js",
                    "~/ModuleResources/JavaScripts/SPA_ScoringInfo/editTab3.js",
                    "~/ModuleResources/JavaScripts/SPA_ScoringInfo/editTab4.js",
                    "~/ModuleResources/JavaScripts/SPA_ScoringInfo/editTab5.js",
                    "~/ModuleResources/JavaScripts/SPA_ScoringInfo/editTab6.js",
                    "~/ModuleResources/JavaScripts/SPA_ScoringInfo/editTab7.js",
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

        // 附件
        // GET: Detail/
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