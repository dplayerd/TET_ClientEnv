using BI.SPA_ApproverSetup;
using BI.Suppliers;
using Platform.WebSite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BI.SPA_ApproverSetup;
using Platform.Auth;
using Platform.AbstractionClass;

namespace Platform.WebSite.Controllers
{
    public class SPA_ApproverSetupController : BaseMVCController
    {
        private SPA_ApproverSetupManager _mgr = new SPA_ApproverSetupManager();
        private UserManager _userMgr = new UserManager();


        // GET: SPA_ApproverSetup
        public ActionResult Index(Guid? id)
        {
            // 如果 ID 不存在，用模組名稱找出，並跳頁
            if (!id.HasValue)
            {
                return this.FindModuleAndRedirectToPage(BI.SPA_ApproverSetup.ModuleConfig.ModuleName_SPA_ApproverSetup);
            }


            this.InitAction(id);

            this.ViewBag.ParamList_AssessmentItem = TET_ParameterService.GetTET_ParametersList("SPA評鑑項目", TET_ParameterService.KeyType.Id);
            this.ViewBag.ParamList_BU = TET_ParameterService.GetTET_ParametersList("SPA評鑑單位", TET_ParameterService.KeyType.Id);

            return View();
        }

        public ActionResult Create(Guid id)
        {
            this.ViewBag.ViewReturn = "Index";
            this.ViewBag.ViewReturnID = id;
            this.ViewBag.IsCreateMode = true;

            // 查詢下拉選單用內容
            this.ViewBag.ParamList_UserList = this._userMgr.GetUserKeyTextList(new Pager() { AllowPaging = false });
            this.ViewBag.ParamList_AssessmentItem = TET_ParameterService.GetTET_ParametersList("SPA評鑑項目", TET_ParameterService.KeyType.Id);
            this.ViewBag.ParamList_BU = TET_ParameterService.GetTET_ParametersList("SPA評鑑單位", TET_ParameterService.KeyType.Id);

            // 沒有帶 ID ，新增模式
            this.ViewBag.Name = "新增供應商SPA評鑑審核者";
            this.ViewBag.IsCreateMode = true;
            this.ViewBag.Mode = "Create";
            this.InitAction(id);
            return View("Edit");
        }

        public ActionResult Edit(Guid id, Guid spa_ApproverSetupID)
        {
            this.ViewBag.ViewReturn = "Index";
            this.ViewBag.ViewReturnID = id;
            this.ViewBag.IsCreateMode = false;

            // 查詢下拉選單用內容
            this.ViewBag.ParamList_UserList = this._userMgr.GetUserKeyTextList(new Pager() { AllowPaging = false });
            this.ViewBag.ParamList_AssessmentItem = TET_ParameterService.GetTET_ParametersList("SPA評鑑項目", TET_ParameterService.KeyType.Id);
            this.ViewBag.ParamList_BU = TET_ParameterService.GetTET_ParametersList("SPA評鑑單位", TET_ParameterService.KeyType.Id);

            // 修改模式
            var model = this._mgr.GetDetail(spa_ApproverSetupID);
            if (model == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            this.ViewBag.Name = "修改供應商SPA評鑑審核者";
            this.ViewBag.IsCreateMode = false;
            this.ViewBag.Mode = "Edit";
            this.InitAction(id);
            return View("Edit", spa_ApproverSetupID);
        }
    }
}