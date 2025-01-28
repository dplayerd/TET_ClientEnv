using BI.AllApproval;
using BI.AllApproval.Models;
using BI.PaymentSuppliers;
using BI.PaymentSuppliers.Enums;
using BI.PaymentSuppliers.Models;
using BI.PaymentSuppliers.Utils;
using BI.PaymentSuppliers.Validators;
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

namespace Platform.WebSite.Controllers
{
    [WebApiAuthorizeCore]
    public class PaymentSupplierApprovalApiController : ApiController
    {
        private TET_PaymentSupplierManager _supplierMgr = new TET_PaymentSupplierManager();
        private TET_PaymentSupplierApprovalManager _mgr = new TET_PaymentSupplierApprovalManager();
        private AllApprovalManager _approvalMgr = new AllApprovalManager();

        [Route("~/api/PaymentSupplierApprovalApi/GetDataTableList")]
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

            foreach(var item in retList.data)
            {
                item.Level_Text = ApprovalUtils.ParseApprovalLevel(item.Level).ToDisplayText();
            }

            return retList;
        }

        [Route("~/api/PaymentSupplierApprovalApi/Detail/{id}")]
        [HttpGet]
        // GET api/SupplierApprovalApi/Detail/{id}
        public TET_PaymentSupplierApprovalModel GetOne([FromUri] Guid id)
        {
            var result = this._mgr.GetTET_PaymentSupplierApproval(id);
            return result;
        }

        [Route("~/api/PaymentSupplierApprovalApi/Submit")]
        [HttpPost]
        public IHttpActionResult Submit()
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cDate = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var inp = HttpContext.Current.Request.Form["Main"];
            TET_PaymentSupplierModel supplierModel;
            TET_PaymentSupplierApprovalModel approvalModel;

            // 嘗試做反序列化，如果錯誤的話丟 Bad Request
            try
            {
                supplierModel = JsonConvert.DeserializeObject<TET_PaymentSupplierModel>(inp);
                if (supplierModel == null)
                    return BadRequest("Payment Supplier is required.");

                approvalModel = JsonConvert.DeserializeObject<TET_PaymentSupplierApprovalModel>(inp);
                if (approvalModel == null)
                    return BadRequest("Payment Supplier is required.");
            }
            catch (Exception ex)
            {
                return BadRequest("Payment Supplier is required.");
            }

            // Map Columns
            var dbApproverModel = this._mgr.GetTET_PaymentSupplierApproval(approvalModel.ID);
            if (approvalModel == null)
                return BadRequest("Payment Supplier is required.");

            dbApproverModel.Result = approvalModel.Result;
            dbApproverModel.Comment = approvalModel.Comment;
            dbApproverModel.ModifyUser = cUser.ID;
            dbApproverModel.ModifyDate = cDate;
            approvalModel = dbApproverModel;

            supplierModel.ID = approvalModel.PSID;
            var dbSupplierModel = this._supplierMgr.GetTET_PaymentSupplier(approvalModel.PSID);
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
            ApprovalResult result = ApprovalUtils.ParseApprovalResult(approvalModel.Result);

            // 如果是退回上一關或退回申請人，都不用檢查供應商的必填
            // 同意時才需要檢查
            if (result == ApprovalResult.Agree)
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
                this._mgr.SubmitTET_PaymentSupplierApproval(approvalModel, supplierModel, cUser.ID, cDate);
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
            return Ok();
        }

        private void MappingSupplier(TET_PaymentSupplierModel source, TET_PaymentSupplierModel dbModel)
        {
            dbModel.ApplyReason = source.ApplyReason;
            dbModel.VenderCode = source.VenderCode;
            dbModel.RegisterDate = source.RegisterDate;
            dbModel.CName = source.CName;
            dbModel.EName = source.EName;
            dbModel.Country = source.Country;
            dbModel.TaxNo = source.TaxNo;
            dbModel.Address = source.Address;
            dbModel.OfficeTel = source.OfficeTel;
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

            dbModel.ContactList = source.ContactList;
            dbModel.AttachmentList = source.AttachmentList;
        }
    }
}
