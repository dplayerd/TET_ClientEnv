using BI.Shared.Utils;
using BI.SPA_ApproverSetup;
using BI.SPA_Evaluation;
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
    public class SPA_EvaluationReportController : BaseMVCController
    {
        private SPA_EvaluationManager _mgr = new SPA_EvaluationManager();
        //private TET_SupplierManager _supplierMgr = new TET_SupplierManager();
        //private SPA_PeriodManager _spaPeriodMgr = new SPA_PeriodManager();


        // GET: SPA_EvaluationReport
        public ActionResult Index(Guid? id)
        {
            // 如果 ID 不存在，用模組名稱找出，並跳頁
            var cUser = UserProfileService.GetCurrentUser();

            if (!id.HasValue)
            {
                return this.FindModuleAndRedirectToPage(BI.SPA_Evaluation.ModuleConfig.ModuleName_Report);
            }

            // 其它值
            this.ViewBag.CurrentUser = cUser.ID;

            this.InitAction(id);
            return View();
        }

        // GET: Edit/
        public ActionResult Edit(Guid id, Guid SPA_EvaluationReportID)
        {
            // 如果 ID 不存在，用模組名稱找出，並跳頁
            var cUser = UserProfileService.GetCurrentUser();
            DateTime cTime = DateTime.Now;


            this.ViewBag.ViewReturn = "Index";
            this.ViewBag.ViewReturnID = id;
            this.ViewBag.IsCreateMode = false;


            // 修改模式
            var model = this._mgr.GetOne(SPA_EvaluationReportID, cUser.ID, cTime);
            if (model == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            this.ViewBag.Name = "修改";
            this.ViewBag.IsCreateMode = false;
            this.ViewBag.Mode = "Edit";
            this.InitAction(id);
            return View("Edit", SPA_EvaluationReportID);
        }

        // GET: Detail/
        public ActionResult Detail(Guid id, Guid SPA_EvaluationReportID)
        {
            // 如果 ID 不存在，用模組名稱找出，並跳頁
            var cUser = UserProfileService.GetCurrentUser();
            DateTime cTime = DateTime.Now;


            this.ViewBag.ViewReturn = "Index";
            this.ViewBag.ViewReturnID = id;
            this.ViewBag.IsCreateMode = false;

            // 修改模式
            var model = this._mgr.GetOne(SPA_EvaluationReportID, cUser.ID, cTime);
            if (model == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            this.ViewBag.Name = "檢視";
            this.ViewBag.IsCreateMode = false;
            this.ViewBag.Mode = "Detail";
            this.InitAction(id);
            return View("Edit", SPA_EvaluationReportID);
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