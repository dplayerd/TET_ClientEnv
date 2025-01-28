using BI.Suppliers;
using BI.Suppliers.Models;
using BI.Suppliers.Validators;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using Platform.AbstractionClass;
using Platform.WebSite.Filters;
using Platform.WebSite.Models;
using Platform.WebSite.Services;
using Platform.WebSite.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Platform.WebSite.Controllers
{
    [WebApiAuthorizeCore]
    public class SupplierRevisionApiController : ApiController
    {
        private TET_SupplierManager _supplierMgr = new TET_SupplierManager();
        private TET_SupplierRevisionManager _mgr = new TET_SupplierRevisionManager();

        public class TempPager : DataTablePager
        {
            public string caption { get; set; }
            public string taxNo { get; set; }
        }


        [Route("~/api/SupplierRevisionApi/GetDataTableList")]
        [HttpPost]
        public WebApiDataContainer<SupplierListModel> PostToGetList([FromBody] TempPager inputParameters)
        {
            DateTime cDate = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var pager = inputParameters.ToPager();
            var list = this._mgr.GetTET_SupplierReversionList(inputParameters.caption, inputParameters.taxNo, cUser.ID, pager);

            WebApiDataContainer<SupplierListModel> retList = new WebApiDataContainer<SupplierListModel>();
            retList.recordsFiltered = pager.TotalRow;
            retList.recordsTotal = pager.TotalRow;
            retList.data = list;

            return retList;
        }


        [Route("~/api/SupplierRevisionApi/Modify/{id}")]
        [HttpPost]
        public IHttpActionResult Modify(Guid id)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var inp = HttpContext.Current.Request.Form["Main"];
            TET_SupplierModel model;

            // 嘗試做反序列化，如果錯誤的話丟 Bad Request
            try
            {
                model = JsonConvert.DeserializeObject<TET_SupplierModel>(inp);
                if (model == null)
                    return BadRequest("Supplier is required.");
            }
            catch (Exception ex)
            {
                return BadRequest("Supplier is required.");
            }

            // 取得本次上傳的附件
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                foreach (var key in HttpContext.Current.Request.Files.AllKeys)
                {
                    var httpPostedFile = HttpContext.Current.Request.Files[key];
                    var fileContent = UploadUtil.ConvertToFileContent(httpPostedFile);

                    model.UploadFiles.Add(fileContent);
                }
            }

            // 驗證正確性
            List<string> msgList;
            var validResult = RevisionValidator.Valid(model, out msgList);
            if (!validResult)
                return BadRequest(JsonConvert.SerializeObject(msgList));

            // 修改
            try
            {
                this._mgr.ModifyTET_Supplier(model, cUser.ID, cTime);
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
            return Ok();
        }


        [Route("~/api/SupplierRevisionApi/Revision/{id}")]
        [HttpPost]
        public IHttpActionResult Revision(Guid id)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cDate = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            // Map Columns
            var dbApproverModel = this._supplierMgr.GetTET_Supplier(id);
            if (dbApproverModel == null)
                return BadRequest("Supplier is required.");

            // 送出
            var newid = this._mgr.CopyCurrentReversion(id, cUser.ID, cDate);
            return Ok(newid);
        }


        [Route("~/api/SupplierRevisionApi/Submit")]
        [HttpPost]
        public IHttpActionResult Submit()
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var inp = HttpContext.Current.Request.Form["Main"];
            TET_SupplierModel model;

            // 嘗試做反序列化，如果錯誤的話丟 Bad Request
            try
            {
                model = JsonConvert.DeserializeObject<TET_SupplierModel>(inp);
                if (model == null)
                    return BadRequest("Supplier is required.");
            }
            catch (Exception ex)
            {
                return BadRequest("Supplier is required.");
            }

            // 取得本次上傳的附件
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                foreach (var key in HttpContext.Current.Request.Files.AllKeys)
                {
                    var httpPostedFile = HttpContext.Current.Request.Files[key];
                    var fileContent = UploadUtil.ConvertToFileContent(httpPostedFile);

                    model.UploadFiles.Add(fileContent);
                }
            }

            //// 驗證正確性
            //List<string> msgList = new List<string>();
            //List<string> tempMsgList;
            //var validResult = SupplierValidator.ValidCreate(model, out tempMsgList);
            //if (!validResult)
            //    msgList.AddRange(tempMsgList);
            //var validFileResult = SupplierAttachmentValidator.Valid(model.AttachmentList, model.UploadFiles, out tempMsgList);
            //if (!validResult)
            //    msgList.AddRange(tempMsgList);

            //if (!validResult || !validFileResult)
            //    return BadRequest(JsonConvert.SerializeObject(msgList));


            // 如果還沒新增過，就跳警告
            if (!model.ID.HasValue)
                return BadRequest("Supplier is required.");

            var dbModel = this._supplierMgr.GetTET_Supplier(model.ID.Value);
            this.MappingSupplier(model, dbModel);

            // 修改
            try
            {
                // 先儲存
                this._supplierMgr.ModifyTET_Supplier(dbModel, cUser.ID, cTime);

                // 送出
                this._mgr.SubmitTET_SupplierRevision(dbModel, cUser.ID, cTime);
                return Ok(dbModel.ID);
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
        }


        private void MappingSupplier(TET_SupplierModel source, TET_SupplierModel dbModel)
        {
            dbModel.ApplyReason = source.ApplyReason;
            dbModel.SupplierCategory = source.SupplierCategory;
            dbModel.BusinessCategory = source.BusinessCategory;
            dbModel.BusinessAttribute = source.BusinessAttribute;
            dbModel.CName = source.CName;
            dbModel.EName = source.EName;
            dbModel.TaxNo = source.TaxNo;
            dbModel.Charge = source.Charge;
            dbModel.PaymentTerm = source.PaymentTerm;
            dbModel.BankName = source.BankName;
            dbModel.BankCode = source.BankCode;
            dbModel.BankBranchName = source.BankBranchName;
            dbModel.BankBranchCode = source.BankBranchCode;
            dbModel.BankAccountNo = source.BankAccountNo;
            dbModel.BankAccountName = source.BankAccountName;
            dbModel.Currency = source.Currency;
            dbModel.BankCountry = source.BankCountry;
            dbModel.BankAddress = source.BankAddress;
            dbModel.SwiftCode = source.SwiftCode;
            dbModel.CompanyCity = source.CompanyCity;
            dbModel.NDANo = source.NDANo;
            dbModel.Contract = source.Contract;

            dbModel.ContactList = source.ContactList;
            dbModel.AttachmentList = source.AttachmentList;
            dbModel.UploadFiles = source.UploadFiles;
        }
    }
}
