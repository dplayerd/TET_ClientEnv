using BI.SPA_ScoringInfo;
using BI.SPA_ScoringInfo.Enums;
using BI.SPA_ScoringInfo.Models;
using BI.SPA_ScoringInfo.Validators;
using BI.SPA_ScoringInfo;
using BI.SPA_ScoringInfo.Models;
using Newtonsoft.Json;
using Platform.WebSite.Filters;
using Platform.WebSite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Platform.WebSite.Controllers
{
    [WebApiAuthorizeCore]
    public class SPA_ScoringInfoApprovalApiController : ApiController
    {
        private SPA_ScoringInfoManager _mainMgr = new SPA_ScoringInfoManager();
        private SPA_ScoringInfoApprovalManager _mgr = new SPA_ScoringInfoApprovalManager();


        [Route("~/api/SPA_ScoringInfoApprovalApi/Detail/{id}")]
        [HttpGet]
        // GET api/SPA_ScoringInfoApprovalApi/Detail/{id}
        public SPA_ScoringInfoModel GetOne([FromUri] Guid id)
        {
            DateTime cDate = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();


            var dbApproverModel = this._mgr.GetOne(id);
            var result = this._mainMgr.GetOne(dbApproverModel.SIID);

            return result;
        }


        [Route("~/api/SPA_ScoringInfoApprovalApi/Submit")]
        [HttpPost]
        public IHttpActionResult Submit(SPA_ScoringInfoApprovalModel approvalModel)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cDate = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();


            // Map Columns
            var dbApproverModel = this._mgr.GetOne(approvalModel.ID);
            var dbMainModel = this._mainMgr.GetOne(dbApproverModel.SIID);
            if (approvalModel == null || dbMainModel == null)
                return BadRequest("SPA_ScoringInfo is required.");

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


        private void MappingApprovalModel(SPA_ScoringInfoApprovalModel source, SPA_ScoringInfoApprovalModel dbModel)
        {
            dbModel.Result = source.Result;
            dbModel.Comment = source.Comment;
        }
    }
}