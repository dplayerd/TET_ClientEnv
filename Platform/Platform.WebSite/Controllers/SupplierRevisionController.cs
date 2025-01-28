﻿using BI.Suppliers;
using BI.Suppliers.Enums;
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
    public class SupplierRevisionController : BaseMVCController
    {
        private TET_SupplierManager _supplierMgr = new TET_SupplierManager();
        private TET_SupplierApprovalManager _mgr = new TET_SupplierApprovalManager();
        private UserManager _userMgr = new UserManager();
        private UserRoleManager _userRoleMgr = new UserRoleManager();


        // GET: SupplierRevision
        public ActionResult Index(Guid? id)
        {
            // 如果 ID 不存在，用模組名稱找出，並跳頁
            if (!id.HasValue)
            {
                return this.FindModuleAndRedirectToPage(BI.Suppliers.ModuleConfig.ModuleName_SupplierRevision);
            }

            DateTime cDate = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();


            this.ViewBag.CurrentUser = cUser.ID;

            this.InitAction(id);
            return View();
        }



        // GET: SupplierRevision/Detail
        public ActionResult Detail(Guid id, Guid supplierID)
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

            // 修改模式
            var supplierModel = this._supplierMgr.GetTET_Supplier(supplierID);
            if (supplierModel == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            this.ViewBag.Name = supplierModel.CName;
            //this.ViewBag.CurrentLevelName = model.Level;
            this.ViewBag.IsCreateMode = false;
            this.ViewBag.Mode = "Detail";
            this.ViewBag.SupplierID = supplierModel.ID;
            this.InitAction(id);
            return View("Edit", supplierID);
        }


        // GET: SupplierRevision/Edit
        public ActionResult Edit(Guid id, Guid supplierID)
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

            // 修改模式
            var supplierModel = this._supplierMgr.GetTET_Supplier(supplierID);
            if (supplierModel == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            this.ViewBag.Name = supplierModel.CName;
            //this.ViewBag.CurrentLevelName = model.Level;
            this.ViewBag.IsCreateMode = false;
            this.ViewBag.Mode = "Edit";
            this.ViewBag.SupplierID = supplierModel.ID;
            this.InitAction(id);
            return View(supplierID);
        }


        // GET: SupplierRevision/SupplierRevision
        public ActionResult SupplierRevision(Guid id, Guid supplierID)
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

            // 修改模式
            var model = this._mgr.GetTET_SupplierApproval(supplierID);
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
            return View(supplierID);
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