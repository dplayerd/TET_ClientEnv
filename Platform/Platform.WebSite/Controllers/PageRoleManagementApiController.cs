using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Platform.Portal.Models;
using Platform.WebSite.Models;
using Platform.WebSite.Services;

namespace Platform.WebSite.Controllers
{
    public class PageRoleManagementApiController : ApiController
    {
        [Route("~/api/PageRoleManagementApi/GetDataTableList/{roleID}")]
        [HttpPost]
        public WebApiDataContainer<PageRoleModel> GetDataTableList([FromUri] Guid roleID, [FromBody] DataTablePager inputParameters)
        {
            var pager = inputParameters.ToPager();
            pager.AllowPaging = false;
            var list = PageRoleService.GetRolePageList(pager, roleID);

            WebApiDataContainer<PageRoleModel> retList = new WebApiDataContainer<PageRoleModel>();
            retList.recordsFiltered = pager.TotalRow;
            retList.recordsTotal = pager.TotalRow;
            retList.data = list;

            return retList;
        }

        [Route("~/api/PageRoleManagementApi/{pageID}/{roleID}")]
        [HttpGet]
        // GET api/PageApi/PageID
        public PageRoleModel GetOne([FromUri] Guid pageID, [FromUri] Guid roleID)
        {
            var obj = PageRoleService.GetPageRole(pageID, roleID);
            return obj;
        }


        [Route("~/api/PageRoleManagementApi/MapPageRole")]
        [HttpPost]
        public void MapPageRole([FromBody] PageRoleUpdateModel model)
        {
            string cUser = UserProfileService.GetCurrentUserID();
            DateTime cTime = DateTime.Now;


            List<PageRoleModel> list = model.Items.Select(obj => new PageRoleModel()
            {
                PageID = obj.PageID,
                RoleID = obj.RoleID,
                AllowActs = obj.AllowActs,
            }).ToList();

            PageRoleService.MapPageRole(list, cUser, cTime);
        }
    }
}
