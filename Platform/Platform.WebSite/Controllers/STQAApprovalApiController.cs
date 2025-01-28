using BI.STQA;
using BI.STQA.Validators;
using BI.STQA.Enums;
using BI.STQA.Models;
using Newtonsoft.Json;
using Platform.AbstractionClass;
using Platform.WebSite.Filters;
using Platform.WebSite.Models;
using Platform.WebSite.Services;
using Platform.WebSite.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Platform.WebSite.Controllers
{
    [WebApiAuthorizeCore]
    public class STQAApprovalApiController : ApiController
    {
        private TET_STQAManager _stqaMgr = new TET_STQAManager();
        private TET_STQAApprovalManager _mgr = new TET_STQAApprovalManager();


        [Route("~/api/STQAApprovalApi/Submit")]
        [HttpPost]
        public IHttpActionResult Submit()
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cDate = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var inp = HttpContext.Current.Request.Form["Main"];
            TET_SupplierSTQAApprovalModel approvalModel;

            // 嘗試做反序列化，如果錯誤的話丟 Bad Request
            try
            {
                approvalModel = JsonConvert.DeserializeObject<TET_SupplierSTQAApprovalModel>(inp);
                if (approvalModel == null)
                    return BadRequest("STQA is required.");
            }
            catch (Exception ex)
            {
                return BadRequest("STQA is required.");
            }

            // Map Columns
            var dbApproverModel = this._mgr.GetDetail(approvalModel.ID);
            var dbSTQAModel = this._stqaMgr.GetSTQA(approvalModel.STQAID);
            if (approvalModel == null || dbSTQAModel == null)
                return BadRequest("STQA is required.");

            this.MappingApprovalModel(approvalModel, dbApproverModel);
            dbApproverModel.ModifyUser = cUser.ID;
            dbApproverModel.ModifyDate = cDate;
            approvalModel = dbApproverModel;



            // 驗證正確性
            var validResult = ApprovalValidator.Valid(approvalModel, out List<string> tempList1);

            if (!validResult)
            {
                List<string> msgList = tempList1;
                return BadRequest(JsonConvert.SerializeObject(msgList));
            }

            // 送出
            try
            {
                this._mgr.Approve(approvalModel, dbSTQAModel, cUser.ID, cDate);
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
            return Ok();
        }



        private void MappingApprovalModel(TET_SupplierSTQAApprovalModel source, TET_SupplierSTQAApprovalModel dbModel)
        {
            dbModel.Result = source.Result;
            dbModel.Comment = source.Comment;
        }
    }
}
