using BI.AllApproval;
using BI.AllApproval.Models;
using BI.SPA.Enums;
using BI.SPA_CostService.Enums;
using BI.SPA_Violation.Enums;
using BI.SPA_ScoringInfo.Enums;
using BI.STQA.Enums;
using BI.PaymentSuppliers.Enums;
using BI.Suppliers;
using BI.Suppliers.Enums;
using BI.Suppliers.Models;
using BI.Suppliers.Utils;
using BI.Suppliers.Validators;
using Newtonsoft.Json;
using Platform.AbstractionClass;
using Platform.ORM;
using Platform.WebSite.Filters;
using Platform.WebSite.Models;
using Platform.WebSite.Services;
using Platform.WebSite.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using BI.PaymentSuppliers;

namespace Platform.WebSite.Controllers
{
    [WebApiAuthorizeCore]
    public class SupplierApprovalApiController : ApiController
    {
        private TET_PaymentSupplierManager _paymentSupplierMgr = new TET_PaymentSupplierManager();
        private TET_SupplierManager _supplierMgr = new TET_SupplierManager();
        private TET_SupplierApprovalManager _mgr = new TET_SupplierApprovalManager();
        private AllApprovalManager _approvalMgr = new AllApprovalManager();

        [Route("~/api/SupplierApprovalApi/GetDataTableList")]
        [HttpPost]
        public WebApiDataContainer<ApprovalModel> PostToGetList([FromBody] DataTablePager inputParameters)
        {
            DateTime cDate = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var pager = inputParameters.ToPager();
            var list = this._approvalMgr.GetApprovalList(cUser.Account, cDate, pager);

            WebApiDataContainer<ApprovalModel> retList = new WebApiDataContainer<ApprovalModel>();
            retList.recordsFiltered = pager.TotalRow;
            retList.recordsTotal = pager.TotalRow;
            retList.data = list;

            foreach (var item in retList.data)
            {
                switch (item.ParentType)
                {
                    case "Supplier":
                        item.Level_Text = _supplierMgr.GetLevelDisplayName(item.ParentID, item.ID);
                        break;

                    case "Revision":
                        item.Level_Text = _supplierMgr.GetLevelDisplayName(item.ParentID, item.ID);
                        break;

                    case "PaymentSupplier":
                        item.Level_Text = _paymentSupplierMgr.GetLevelDisplayName(item.ParentID, item.ID);
                        break;

                    case "PaymentSupplierRevision":
                        item.Level_Text = BI.PaymentSuppliers.Utils.ApprovalUtils.ParseApprovalLevel(item.Level).ToDisplayText();
                        break;

                    case "SPA":
                        item.Level_Text = BI.SPA.Utils.ApprovalUtils.ParseApprovalLevel(item.Level).ToDisplayText();
                        break;

                    case "STQA":
                        item.Level_Text = BI.STQA.Utils.ApprovalUtils.ParseApprovalLevel(item.Level).ToDisplayText();
                        break;

                    case "COSTSERVICE":
                        item.Level_Text = BI.SPA_CostService.Utils.ApprovalUtils.ParseApprovalLevel(item.Level).ToDisplayText();
                        break;

                    case "VIOLATION":
                        item.Level_Text = BI.SPA_Violation.Utils.ApprovalUtils.ParseApprovalLevel(item.Level).ToDisplayText();
                        break;

                    case "SCORINGINFO":
                        item.Level_Text = BI.SPA_ScoringInfo.Utils.ApprovalUtils.ParseApprovalLevel(item.Level).ToDisplayText();
                        break;
                    default:
                        break;
                }
            }

            return retList;
        }

        [Route("~/api/SupplierApprovalApi/Detail/{id}")]
        [HttpGet]
        // GET api/SupplierApprovalApi/Detail/{id}
        public TET_SupplierApprovalModel GetOne([FromUri] Guid id)
        {
            var result = this._mgr.GetTET_SupplierApproval(id);
            return result;
        }



        [Route("~/api/SupplierApprovalApi/Submit")]
        [HttpPost]
        public IHttpActionResult Submit()
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cDate = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var inp = HttpContext.Current.Request.Form["Main"];
            TET_SupplierModel supplierModel;
            TET_SupplierApprovalModel approvalModel;

            // 嘗試做反序列化，如果錯誤的話丟 Bad Request
            try
            {
                supplierModel = JsonConvert.DeserializeObject<TET_SupplierModel>(inp);
                if (supplierModel == null)
                    return BadRequest("Supplier is required.");

                approvalModel = JsonConvert.DeserializeObject<TET_SupplierApprovalModel>(inp);
                if (approvalModel == null)
                    return BadRequest("Supplier is required.");
            }
            catch (Exception ex)
            {
                return BadRequest("Supplier is required.");
            }

            // Map Columns
            var dbApproverModel = this._mgr.GetTET_SupplierApproval(approvalModel.ID);
            if (approvalModel == null)
                return BadRequest("Supplier is required.");

            dbApproverModel.Result = approvalModel.Result;
            dbApproverModel.Comment = approvalModel.Comment;
            dbApproverModel.ModifyUser = cUser.ID;
            dbApproverModel.ModifyDate = cDate;
            approvalModel = dbApproverModel;


            supplierModel.ID = approvalModel.SupplierID;
            var dbSupplierModel = this._supplierMgr.GetTET_Supplier(approvalModel.SupplierID);
            this.MappingSupplier(supplierModel, dbSupplierModel);
            supplierModel = dbSupplierModel;


            // 取得本次上傳的附件
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                foreach (var key in HttpContext.Current.Request.Files.AllKeys)
                {
                    var httpPostedFile = HttpContext.Current.Request.Files[key];
                    var fileContent = UploadUtil.ConvertToFileContent(httpPostedFile);

                    supplierModel.UploadFiles.Add(fileContent);
                }
            }


            // 驗證正確性
            var validResult = ApprovalValidator.Valid(approvalModel, out List<string> tempList1);
            var validSupplierResult = ApprovalValidator.ValidSupplier(supplierModel, approvalModel.Level, out List<string> tempList2);

            if (!validResult)
                return BadRequest(JsonConvert.SerializeObject(tempList1));


            // 將簽核結果轉換為 Enum
            BI.Suppliers.Enums.ApprovalResult result = BI.Suppliers.Utils.ApprovalUtils.ParseApprovalResult(approvalModel.Result);

            // 如果是退回上一關或退回申請人，都不用檢查供應商的必填
            // 同意時才需要檢查
            if (result == BI.Suppliers.Enums.ApprovalResult.Agree)
            {
                if (!validSupplierResult)
                {
                    List<string> msgList = tempList2.Union(tempList1).ToList();
                    return BadRequest(JsonConvert.SerializeObject(msgList));
                }
            }

            // 送出
            try
            {
                this._mgr.SubmitTET_SupplierApproval(approvalModel, supplierModel, cUser.ID, cDate);
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
            return Ok();
        }



        private void MappingSupplier(TET_SupplierModel source, TET_SupplierModel dbModel)
        {
            dbModel.IsSecret = source.IsSecret;
            dbModel.IsNDA = source.IsNDA;
            dbModel.ApplyReason = source.ApplyReason;
            dbModel.BelongTo = source.BelongTo;
            dbModel.VenderCode = source.VenderCode;
            dbModel.SupplierCategory = source.SupplierCategory;
            dbModel.BusinessCategory = source.BusinessCategory;
            dbModel.BusinessAttribute = source.BusinessAttribute;
            dbModel.SearchKey = source.SearchKey;
            dbModel.RelatedDept = source.RelatedDept;
            dbModel.Buyer = source.Buyer;
            dbModel.RegisterDate = source.RegisterDate;
            dbModel.CName = source.CName;
            dbModel.EName = source.EName;
            dbModel.Country = source.Country;
            dbModel.TaxNo = source.TaxNo;
            dbModel.Address = source.Address;
            dbModel.OfficeTel = source.OfficeTel;
            dbModel.ISO = source.ISO;
            dbModel.Email = source.Email;
            dbModel.Website = source.Website;
            dbModel.CapitalAmount = source.CapitalAmount;
            dbModel.MainProduct = source.MainProduct;
            dbModel.Employees = source.Employees;
            dbModel.Charge = source.Charge;
            dbModel.PaymentTerm = source.PaymentTerm;
            dbModel.BillingDocument = source.BillingDocument;
            dbModel.Incoterms = source.Incoterms;
            dbModel.Remark = source.Remark;
            dbModel.BankCountry = source.BankCountry;
            dbModel.BankName = source.BankName;
            dbModel.BankCode = source.BankCode;
            dbModel.BankBranchName = source.BankBranchName;
            dbModel.BankBranchCode = source.BankBranchCode;
            dbModel.Currency = source.Currency;
            dbModel.BankAccountName = source.BankAccountName;
            dbModel.BankAccountNo = source.BankAccountNo;
            dbModel.CompanyCity = source.CompanyCity;
            dbModel.BankAddress = source.BankAddress;
            dbModel.SwiftCode = source.SwiftCode;
            dbModel.NDANo = source.NDANo;
            dbModel.Contract = source.Contract;
            dbModel.IsSign1 = source.IsSign1;
            dbModel.SignDate1 = source.SignDate1;
            dbModel.IsSign2 = source.IsSign2;
            dbModel.SignDate2 = source.SignDate2;
            dbModel.STQAApplication = source.STQAApplication;
            dbModel.KeySupplier = source.KeySupplier;

            dbModel.ContactList = source.ContactList;
            dbModel.AttachmentList = source.AttachmentList;
        }
    }
}
