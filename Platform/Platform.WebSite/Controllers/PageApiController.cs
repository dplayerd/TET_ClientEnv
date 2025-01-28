using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Platform.AbstractionClass;
using Platform.Portal.Models;
using Platform.WebSite.Filters;
using Platform.WebSite.Models;
using Platform.WebSite.Services;

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
            var result = PageService.GetAdminPage(pageID);
            return result;
        }

        [Route("~/api/PageApi/Create")]
        [HttpPost]
        // POST api/PageApi/Create
        public void Create([FromBody] PageModel model)
        {
            string cUser = UserProfileService.GetCurrentUserID();
            DateTime cTime = DateTime.Now;

            PageService.CreatePage(model, cUser, cTime);
        }

        [Route("~/api/PageApi/Modify")]
        [HttpPost]
        public void Modify([FromBody] PageModel model)
        {
            string cUser = UserProfileService.GetCurrentUserID();
            DateTime cTime = DateTime.Now;

            PageService.ModifyPage(model, cUser, cTime);
        }

        [Route("~/api/PageApi/Delete")]
        [HttpPost]
        public void Delete([FromBody] PageModel model)
        {
            string cUser = UserProfileService.GetCurrentUserID();
            DateTime cTime = DateTime.Now;

            PageService.DeletePage(model.ID, cUser, cTime);
        }
    }
}