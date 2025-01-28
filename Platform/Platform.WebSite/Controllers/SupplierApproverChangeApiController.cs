using BI.AllApproval;
using BI.AllApproval.Models;
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

namespace Platform.WebSite.Controllers
{
    [WebApiAuthorizeCore]
    public class SupplierApproverChangeApiController : ApiController
    {
        private TET_SupplierManager _supplierMgr = new TET_SupplierManager();
        private TET_SupplierApprovalManager _mgr = new TET_SupplierApprovalManager();
        private ApproverChangeManager _mgr1 = new ApproverChangeManager();
        private AllApprovalManager _approvalMgr = new AllApprovalManager();

        public class TempPager : DataTablePager
        {
            public string approver { get; set; }
        }

        [Route("~/api/SupplierApproverChangeApi/GetDataTableList")]
        [HttpPost]
        public WebApiDataContainer<ApprovalModel> PostToGetList([FromBody] TempPager filter)
        {
            DateTime cDate = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var pager = filter.ToPager();
            var list = this._approvalMgr.GetApproverChangeList(filter.approver, cDate, pager);

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

        [Route("~/api/SupplierApproverChangeApi/Detail/{id}")]
        [HttpGet]
        // GET api/SupplierApproverChangeApi/Detail/{id}
        public TET_SupplierApprovalModel GetOne([FromUri] Guid id)
        {
            var result = this._mgr1.GetTET_SupplierApproval(id);
            return result;
        }

        [Route("~/api/SupplierApproverChangeApi/Modify/{id}")]
        [HttpPost]
        public IHttpActionResult Modify(Guid id)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var inp = HttpContext.Current.Request.Form["Main"];
            TET_SupplierApprovalModel model;

            // 嘗試做反序列化，如果錯誤的話丟 Bad Request
            try
            {
                model = JsonConvert.DeserializeObject<TET_SupplierApprovalModel>(inp);
                if (model == null)
                    return BadRequest("Supplier is required.");
            }
            catch (Exception ex)
            {
                return BadRequest("Supplier is required.");
            }

            // 驗證正確性
            //var validResult = SupplierSTQAValidator.Valid(model, out List<string> tempMsgList);

            //if (!validResult)
            //    return BadRequest(JsonConvert.SerializeObject(tempMsgList));

            // 修改
            try
            {
                this._mgr1.ModifyApprover(model, cUser.ID, cTime);
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
            return Ok();
        }
    }
}
