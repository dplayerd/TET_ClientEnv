using Newtonsoft.Json;
using Platform.WebSite.Filters;
using Platform.WebSite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using BI.SPA_CostService;
using BI.SPA_CostService.Models;
using BI.SPA_CostService.Validators;
using BI.SPA_CostService.Enums;

namespace Platform.WebSite.Controllers
{
    [WebApiAuthorizeCore]
    public class SPA_CostServiceApprovalApiController : ApiController
    {
        private SPA_CostServiceManager _mainMgr = new SPA_CostServiceManager();
        private SPA_CostServiceApprovalManager _mgr = new SPA_CostServiceApprovalManager();


        [Route("~/api/SPA_CostServiceApprovalApi/Detail/{id}")]
        [HttpGet]
        // GET api/SPA_CostServiceApprovalApi/Detail/{id}
        public SPA_CostServiceModel GetOne([FromUri] Guid id)
        {
            DateTime cDate = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();


            var dbApproverModel = this._mgr.GetDetail(id);
            var result = this._mainMgr.GetOne(dbApproverModel.CSID);
            
            // 如果是 BU 人員，只允許看到自己的資料
            if (dbApproverModel.Level == ApprovalLevel.BU.ToText())
            {
                var dicConfig = this._mgr.GetSetupDic(result, cUser.ID, cDate);
                var detailList = new List<SPA_CostServiceDetailModel>();

                foreach(var item in dicConfig)
                {
                    if (item.Key.InfoFills == null || item.Key.InfoFills.Length == 0)
                        continue;

                    if (!item.Key.InfoFills.Contains(cUser.ID))
                        continue;

                    var selectedItem = result.DetailList.Where(obj => obj.BU == item.Key.BUText && obj.AssessmentItem == item.Key.ServiceItemText).ToList();

                    if (selectedItem == null)
                        continue;

                    foreach (var detailitem in selectedItem)
                    {
                        detailList.Add(detailitem);
                    }                  
                }

                result.DetailList = detailList;
            }

            return result;
        }


        [Route("~/api/SPA_CostServiceApprovalApi/Submit")]
        [HttpPost]
        public IHttpActionResult Submit()
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cDate = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var inp = HttpContext.Current.Request.Form["Main"];
            SPA_CostServiceApprovalModel approvalModel;

            // 嘗試做反序列化，如果錯誤的話丟 Bad Request
            try
            {
                approvalModel = JsonConvert.DeserializeObject<SPA_CostServiceApprovalModel>(inp);
                if (approvalModel == null)
                    return BadRequest("Approval is required.");
            }
            catch (Exception ex)
            {
                return BadRequest("Approval is required.");
            }

            // Map Columns
            var dbApproverModel = this._mgr.GetDetail(approvalModel.ID);
            var dbMainModel = this._mainMgr.GetOne(dbApproverModel.CSID);
            if (approvalModel == null || dbMainModel == null)
                return BadRequest("SPA_CostService is required.");

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



        private void MappingApprovalModel(SPA_CostServiceApprovalModel source, SPA_CostServiceApprovalModel dbModel)
        {
            dbModel.Result = source.Result;
            dbModel.Comment = source.Comment;
        }
    }
}