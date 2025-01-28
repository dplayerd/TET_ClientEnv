using BI.AllApproval;
using BI.SPA;
using BI.SPA.Enums;
using BI.STQA;
using BI.Suppliers;
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

namespace Platform.WebSite.Controllers
{
    [AuthorizeCore]
    public class SupplierApproverChangeController : BaseMVCController
    {
        private TET_SupplierApprovalManager _suuplierApprovalMgr = new TET_SupplierApprovalManager();
        private TET_SupplierManager _supplierMgr = new TET_SupplierManager();
        private UserManager _userMgr = new UserManager();
        private UserRoleManager _userRoleMgr = new UserRoleManager();
        private ApproverChangeManager _mgr1 = new ApproverChangeManager();


        // GET: SupplierApproval
        public ActionResult Index(Guid? id)
        {
            // 如果 ID 不存在，用模組名稱找出，並跳頁
            if (!id.HasValue)
            {
                return this.FindModuleAndRedirectToPage(BI.AllApproval.ModuleConfig.ModuleName_SupplierApproverChange);
            }

            this.InitAction(id);

            // 查詢下拉選單用內容
            this.ViewBag.ParamList_UserList = this._userMgr.GetUserKeyTextList(new Pager() { AllowPaging = false });

            return View();
        }

        public ActionResult Edit(Guid id, Guid? ApprovalID)
        {
            // 查詢下拉選單用內容
            this.ViewBag.ParamList_UserList = this._userMgr.GetUserKeyTextList(new Pager() { AllowPaging = false });

            this.ViewBag.ViewReturn = "Index";
            this.ViewBag.ViewReturnID = id;
            this.ViewBag.IsCreateMode = false;

            if (!ApprovalID.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            // 修改模式
            var model = this._mgr1.GetTET_SupplierApproval(ApprovalID.Value);
            if (model == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            this.ViewBag.Name = "修改供應商審核者";
            this.ViewBag.IsCreateMode = false;
            this.ViewBag.Mode = "Edit";
            this.InitAction(id);
            return View("Edit", ApprovalID.Value);
        }
    }
}