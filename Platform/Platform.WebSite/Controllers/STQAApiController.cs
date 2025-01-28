using System;
using System.Web.Http;
using System.Collections.Generic;
using System.Linq;
using Platform.Infra;
using Platform.WebSite.Filters;
using Platform.WebSite.Models;
using Platform.WebSite.Services;
using Newtonsoft.Json;
using System.Web.Http.Results;
using Platform.Auth.Models;
using System.IO;
using System.Web;
using Platform.WebSite.Util;
using BI.STQA;
using BI.STQA.Models;
using BI.STQA.Validators;


namespace Platform.WebSite.Controllers
{
    [WebApiAuthorizeCore]
    public class STQAApiController : ApiController
    {
        private TET_STQAManager _mgr = new TET_STQAManager();

        public class TempPager : DataTablePager
        {
            public string[] belongTo { get; set; } = new string[0];
            public string[] businessTerm { get; set; } = new string[0];
            public string[] type { get; set; } = new string[0];
            public DateTime? dateStart { get; set; }
            public DateTime? dateEnd { get; set; }
        }

        [Route("~/api/STQAApi/GetDataTableList")]
        [HttpPost]
        public WebApiDataContainer<TET_SupplierSTQAModel> PostToGetList([FromBody] TempPager filter)
        {
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();


            var pager = filter.ToPager();
            var list = this._mgr.GetSTQAList(filter.belongTo, filter.businessTerm, filter.type, filter.dateStart, filter.dateEnd, cUser.ID, cTime, pager);

            WebApiDataContainer<TET_SupplierSTQAModel> retList = new WebApiDataContainer<TET_SupplierSTQAModel>();
            retList.recordsFiltered = pager.TotalRow;
            retList.recordsTotal = pager.TotalRow;
            retList.data = list;

            return retList;
        }


        [Route("~/api/STQAApi/Detail/{id}")]
        [HttpGet]
        // GET api/STQAApi/Detail/{id}
        public TET_SupplierSTQAModel GetOne([FromUri] Guid id, bool includeApprovalList = false)
        {
            var result = this._mgr.GetSTQA(id, includeApprovalList);
            return result;
        }


        [Route("~/api/STQAApi/Create")]
        [HttpPost]
        public IHttpActionResult Create()
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var inp = HttpContext.Current.Request.Form["Main"];
            TET_SupplierSTQAModel model;

            // 嘗試做反序列化，如果錯誤的話丟 Bad Request
            try
            {
                model = JsonConvert.DeserializeObject<TET_SupplierSTQAModel>(inp);
                if (model == null)
                    return BadRequest("Supplier is required.");
            }
            catch (Exception ex)
            {
                return BadRequest("Supplier is required.");
            }

            // 驗證正確性
            List<string> msgList = new List<string>();
            List<string> tempMsgList;
            var validResult = SupplierSTQAValidator.Valid(model, out tempMsgList);
            if (!validResult)
                msgList.AddRange(tempMsgList);

            if (!validResult)
                return BadRequest(JsonConvert.SerializeObject(msgList));

            // 新增
            var newID = _mgr.CreateSTQA(model, cUser.ID, cTime);
            return Ok(newID);
        }

        [Route("~/api/STQAApi/Submit")]
        [HttpPost]
        public IHttpActionResult Submit()
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var inp = HttpContext.Current.Request.Form["Main"];
            TET_SupplierSTQAModel model;

            // 嘗試做反序列化，如果錯誤的話丟 Bad Request
            try
            {
                model = JsonConvert.DeserializeObject<TET_SupplierSTQAModel>(inp);
                if (model == null)
                    return BadRequest("Supplier is required.");
            }
            catch (Exception ex)
            {
                return BadRequest("Supplier is required.");
            }

            // 驗證正確性
            List<string> msgList = new List<string>();
            List<string> tempMsgList;
            var validResult = SupplierSTQAValidator.Valid(model, out tempMsgList);
            if (!validResult)
                msgList.AddRange(tempMsgList);

            if (!validResult)
                return BadRequest(JsonConvert.SerializeObject(msgList));

            // 如果還沒新增過，就先新增
            Guid supplierID;
            if (model.ID.HasValue)
            {
                // 查不到資料也視為要新增
                if (this._mgr.GetSTQA(model.ID.Value) == null)
                {
                    supplierID = this._mgr.CreateSTQA(model, cUser.ID, cTime);
                    model.ID = supplierID;
                }
                else
                {
                    // 如果查得到，就先存檔，避免漏資料
                    this._mgr.ModifySTQA(model, cUser.ID, cTime);
                }
            }
            else
            {
                supplierID = this._mgr.CreateSTQA(model, cUser.ID, cTime);
                model.ID = supplierID;
            }


            // 送出
            this._mgr.SubmitSTQA(model, cUser.ID, cTime);
            return Ok(model.ID);
        }

        [Route("~/api/STQAApi/Modify/{id}")]
        [HttpPost]
        public IHttpActionResult Modify(Guid id)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var inp = HttpContext.Current.Request.Form["Main"];
            TET_SupplierSTQAModel model;

            // 嘗試做反序列化，如果錯誤的話丟 Bad Request
            try
            {
                model = JsonConvert.DeserializeObject<TET_SupplierSTQAModel>(inp);
                if (model == null)
                    return BadRequest("Supplier is required.");
            }
            catch (Exception ex)
            {
                return BadRequest("Supplier is required.");
            }

            // 驗證正確性
            var validResult = SupplierSTQAValidator.Valid(model, out List<string> tempMsgList);

            if (!validResult)
                return BadRequest(JsonConvert.SerializeObject(tempMsgList));

            // 修改
            this._mgr.ModifySTQA(model, cUser.ID, cTime);
            return Ok();
        }

        [Route("~/api/STQAApi/Revision/{id}")]
        [HttpPost]
        public IHttpActionResult Revision(Guid id)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            // 刪除
            this._mgr.Revision(id, cUser.ID, cTime);
            return Ok();
        }

        [Route("~/api/STQAApi/Delete/{id}")]
        [HttpPost]
        public IHttpActionResult Delete(Guid id)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            // 刪除
            this._mgr.DeleteTET_STQA(id, cUser.ID, cTime);
            return Ok();
        }

        [Route("~/api/STQAApi/Abord/{id}")]
        [HttpPost]
        public IHttpActionResult Abord(Guid id)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            // 中止
            this._mgr.AbordTET_STQA(id, cUser.ID, cTime);
            return Ok();
        }
    }
}
