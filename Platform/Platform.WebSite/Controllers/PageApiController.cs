using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using BI.Suppliers.Models;
using Newtonsoft.Json;
using Platform.AbstractionClass;
using Platform.Portal.Models;
using Platform.WebSite.Filters;
using Platform.WebSite.Models;
using Platform.WebSite.Services;
using Platform.WebSite.Util;

namespace Platform.WebSite.Controllers
{
    [WebApiAuthorizeCore]
    public class PageApiController : ApiController
    {
        [Route("~/api/PageApi/GetDataTableList/{siteID?}")]
        [HttpPost]
        // GET api/PageApi/SiteID
        public WebApiDataContainer<PageModel> GetDataTableList(Guid? siteID, [FromBody] DataTablePager dataTablePager)
        {
            Pager pager = dataTablePager.ToPager();

            var id = siteID ?? SiteService.DefaultSiteID;
            var list = PageService.GetPageAdminList(id, pager);
            var retObj = new WebApiDataContainer<PageModel>()
            {
                recordsFiltered = pager.TotalRow,
                recordsTotal = pager.TotalRow,
                data = list
            };

            return retObj;
        }

        [Route("~/api/PageApi/List/{siteID}")]
        [HttpGet]
        // GET api/PageApi/PageID
        public List<PageModel> GetList([FromUri] Guid siteID)
        {
            var result = PageService.GetPageList(siteID);
            return result;
        }

        [Route("~/api/PageApi/{pageID}")]
        [HttpGet]
        // GET api/PageApi/PageID
        public PageModel GetOne([FromUri] Guid pageID)
        {
            var result = PageService.GetPage(pageID);
            return result;
        }

        [Route("~/api/PageApi/Create")]
        [HttpPost]
        // POST api/PageApi/Create
        public IHttpActionResult Create()
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var inp = HttpContext.Current.Request.Form["Main"];
            PageModel model;


            // 嘗試做反序列化，如果錯誤的話丟 Bad Request
            try
            {
                model = JsonConvert.DeserializeObject<PageModel>(inp);
                if (model == null)
                    return BadRequest("Page Content is required.");
            }
            catch (Exception ex)
            {
                return BadRequest("Page Content is required.");
            }

            // 取得本次上傳的附件
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                foreach (var key in HttpContext.Current.Request.Files.AllKeys)
                {
                    var httpPostedFile = HttpContext.Current.Request.Files[key];
                    var fileContent = UploadUtil.ConvertToFileContent(httpPostedFile);

                    model.UploadFile = fileContent;
                }
            }

            PageService.CreatePage(model, cUser.ID, cTime);

            return Ok();
        }

        [Route("~/api/PageApi/Modify")]
        [HttpPost]
        public IHttpActionResult Modify()
        {
            // 因為需要上傳檔案，此處原始 Http 來處理
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var inp = HttpContext.Current.Request.Form["Main"];
            PageModel model;
            TempClearImage tempClearImage;

            // 嘗試做反序列化，如果錯誤的話丟 Bad Request
            try
            {
                model = JsonConvert.DeserializeObject<PageModel>(inp);
                if (model == null)
                    return BadRequest("Page Content is required.");

                tempClearImage = JsonConvert.DeserializeObject<TempClearImage>(inp);
            }
            catch (Exception ex)
            {
                return BadRequest("Page Content is required.");
            }

            // 取得本次上傳的附件
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                foreach (var key in HttpContext.Current.Request.Files.AllKeys)
                {
                    var httpPostedFile = HttpContext.Current.Request.Files[key];
                    var fileContent = UploadUtil.ConvertToFileContent(httpPostedFile);

                    model.UploadFile = fileContent;
                }
            }

            var willClearImage = string.Compare("true", tempClearImage.ClearImage, true) == 0;
            PageService.ModifyPage(model, willClearImage, cUser.ID, cTime);

            return Ok();
        }

        [Route("~/api/PageApi/Delete")]
        [HttpPost]
        public IHttpActionResult Delete([FromBody] PageModel model)
        {
            string cUser = UserProfileService.GetCurrentUserID();
            DateTime cTime = DateTime.Now;

            PageService.DeletePage(model.ID, cUser, cTime);
            return Ok();
        }


        #region Private Class
        public class TempClearImage
        {
            public string ClearImage { get; set; }
        }
        #endregion
    }
}