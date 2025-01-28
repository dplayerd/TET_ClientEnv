using BI.SPA;
using BI.SPA.Validators;
using BI.SPA.Enums;
using BI.SPA.Models;
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
    public class SPAApprovalApiController : ApiController
    {
        private TET_SPAManager _SPAMgr = new TET_SPAManager();
        private TET_SPAApprovalManager _mgr = new TET_SPAApprovalManager();


        [Route("~/api/SPAApprovalApi/Submit")]
        [HttpPost]
        public IHttpActionResult Submit()
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cDate = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var inp = HttpContext.Current.Request.Form["Main"];
            TET_SupplierSPAApprovalModel approvalModel;

            // 嘗試做反序列化，如果錯誤的話丟 Bad Request
            try
            {
                approvalModel = JsonConvert.DeserializeObject<TET_SupplierSPAApprovalModel>(inp);
                if (approvalModel == null)
                    return BadRequest("SPA is required.");
            }
            catch (Exception ex)
            {
                return BadRequest("SPA is required.");
            }

            // Map Columns
            var dbApproverModel = this._mgr.GetDetail(approvalModel.ID);
            var dbSPAModel = this._SPAMgr.GetSPA(approvalModel.SPAID);
            if (approvalModel == null || dbSPAModel == null)
                return BadRequest("SPA is required.");

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
                this._mgr.Approve(approvalModel, dbSPAModel, cUser.ID, cDate);
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
            return Ok();
        }



        private void MappingApprovalModel(TET_SupplierSPAApprovalModel source, TET_SupplierSPAApprovalModel dbModel)
        {
            dbModel.Result = source.Result;
            dbModel.Comment = source.Comment;
        }
    }
}
