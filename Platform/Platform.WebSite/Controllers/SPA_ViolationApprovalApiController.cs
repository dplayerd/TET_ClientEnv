using Newtonsoft.Json;
using Platform.WebSite.Filters;
using Platform.WebSite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using BI.SPA_Violation;
using BI.SPA_Violation.Models;
using BI.SPA_Violation.Validators;
using BI.SPA_Violation.Enums;

namespace Platform.WebSite.Controllers
{
    [WebApiAuthorizeCore]
    public class SPA_ViolationApprovalApiController : ApiController
    {
        private SPA_ViolationManager _mainMgr = new SPA_ViolationManager();
        private SPA_ViolationApprovalManager _mgr = new SPA_ViolationApprovalManager();


        [Route("~/api/SPA_ViolationApprovalApi/Detail/{id}")]
        [HttpGet]
        // GET api/SPA_ViolationApprovalApi/Detail/{id}
        public SPA_ViolationModel GetOne([FromUri] Guid id)
        {
            DateTime cDate = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();


            var dbApproverModel = this._mgr.GetDetail(id);
            var result = this._mainMgr.GetOne(dbApproverModel.ViolationID);

            return result;
        }


        [Route("~/api/SPA_ViolationApprovalApi/Submit")]
        [HttpPost]
        public IHttpActionResult Submit()
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cDate = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var inp = HttpContext.Current.Request.Form["Main"];
            SPA_ViolationApprovalModel approvalModel;

            // 嘗試做反序列化，如果錯誤的話丟 Bad Request
            try
            {
                approvalModel = JsonConvert.DeserializeObject<SPA_ViolationApprovalModel>(inp);
                if (approvalModel == null)
                    return BadRequest("Approval is required.");
            }
            catch (Exception ex)
            {
                return BadRequest("Approval is required.");
            }

            // Map Columns
            var dbApproverModel = this._mgr.GetDetail(approvalModel.ID);
            var dbMainModel = this._mainMgr.GetOne(dbApproverModel.ViolationID);
            if (approvalModel == null || dbMainModel == null)
                return BadRequest("SPA Violation is required.");

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
            this._mgr.Approve(approvalModel, dbMainModel, cUser.ID, cDate);
            return Ok();
        }



        private void MappingApprovalModel(SPA_ViolationApprovalModel source, SPA_ViolationApprovalModel dbModel)
        {
            dbModel.Result = source.Result;
            dbModel.Comment = source.Comment;
        }
    }
}