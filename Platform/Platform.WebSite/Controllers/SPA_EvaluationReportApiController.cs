using BI.Shared.Utils;
using BI.SPA_Evaluation;
using BI.SPA_Evaluation.Models;
using BI.SPA_Evaluation.Models.Exporting;
using BI.SPA_Evaluation.Validators;
using Newtonsoft.Json;
using Platform.AbstractionClass;
using Platform.WebSite.Models;
using Platform.WebSite.Services;
using Platform.WebSite.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Platform.WebSite.Controllers
{
    public class SPA_EvaluationReportApiController : ApiController
    {
        private const string _fileUploadPrefix = "Attachment_";

        private SPA_EvaluationManager _mgr = new SPA_EvaluationManager();


        public class TempPager : DataTablePager
        {
            public string period { get; set; }
            public string[] approveStatus { get; set; }
        }

        #region Query
        [Route("~/api/SPA_EvaluationReportApi/GetDataTableList")]
        [HttpPost]
        public WebApiDataContainer<SPA_EvaluationReportModel> PostToGetList([FromBody] TempPager filter)
        {
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();


            var pager = filter.ToPager();
            var list = this._mgr.GetList(filter.period, cUser.ID, cTime, pager);

            WebApiDataContainer<SPA_EvaluationReportModel> retList = new WebApiDataContainer<SPA_EvaluationReportModel>();
            retList.recordsFiltered = pager.TotalRow;
            retList.recordsTotal = pager.TotalRow;
            retList.data = list;

            return retList;
        }


        [Route("~/api/SPA_EvaluationReportApi/Detail/{id}")]
        [HttpGet]
        // GET api/SPA_EvaluationReportApi/Detail/{id}
        public SPA_EvaluationReportModel GetOne([FromUri] Guid id)
        {
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var result = this._mgr.GetOne(id, cUser.ID, cTime);
            return result;
        }
        #endregion


        #region Attachment
        public IHttpActionResult UploadAll()
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            return Ok();
        }
        #endregion


        #region CUD
        [Route("~/api/SPA_EvaluationReportApi/SendMessage/{id}")]
        [HttpPost]
        public IHttpActionResult SendMessage(Guid id)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var dbEntity = this._mgr.GetOne(id, cUser.ID, cTime); 


            try
            {
                // 發送
                this._mgr.SendMessage(dbEntity, cUser.ID, cTime);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
        }


        [Route("~/api/SPA_EvaluationReportApi/Modify/{id}")]
        [HttpPost]
        public IHttpActionResult Modify(Guid id)
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var inp = HttpContext.Current.Request.Form["Main"];
            SPA_EvaluationReportModel model;

            // 嘗試做反序列化，如果錯誤的話丟 Bad Request
            try
            {
                model = JsonConvert.DeserializeObject<SPA_EvaluationReportModel>(inp);
                if (model == null)
                    return BadRequest("SPA EvaluationReport is required.");
            }
            catch (Exception ex)
            {
                return BadRequest("SPA EvaluationReport is required.");
            }

            // 取得本次上傳的附件
            List<FileContent> fileUploads_QSM = new List<FileContent>();
            List<FileContent> fileUploads_All = new List<FileContent>();
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                foreach (var key in HttpContext.Current.Request.Files.AllKeys)
                {
                    var httpPostedFile = HttpContext.Current.Request.Files[key];
                    var fileContent = UploadUtil.ConvertToFileContent(httpPostedFile);

                    if (key.Contains("QSM"))
                        fileUploads_QSM.Add(fileContent);
                    else if (key.Contains("All"))
                        fileUploads_All.Add(fileContent);
                }
            }


            // 驗證正確性
            List<string> msgList = new List<string>();
            var validResult = SPA_EvaluationReportValidator.Valid(model, out msgList);
            if (!validResult)
                return BadRequest(JsonConvert.SerializeObject(msgList));
   

            try
            {
                // 修改
                this._mgr.Modify(model, fileUploads_QSM, fileUploads_All, cUser.ID, cTime);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
        }
        #endregion
    }
}