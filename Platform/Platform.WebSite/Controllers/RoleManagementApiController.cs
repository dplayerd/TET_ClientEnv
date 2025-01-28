using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Platform.AbstractionClass;
using Platform.Auth.Models;
using Platform.WebSite.Models;
using Platform.WebSite.Services;

namespace Platform.WebSite.Controllers
{
    public class RoleManagementApiController : ApiController
    {
        [Route("~/api/RoleManagement/")]
        [HttpGet]
        public List<RoleModel> Get()
        {
            var list = RoleService.GetList();
            return list;
        }

        public class TempPager : DataTablePager
        {
            public string caption { get; set; }
        }


        [Route("~/api/RoleManagement/GetDataTableList")]
        [HttpPost]
        public WebApiDataContainer<RoleModel> PostToGetList([FromBody] TempPager inputParameters)
        {
            var pager = inputParameters.ToPager();
            var ienm = RoleService.GetAdminList(inputParameters.caption, pager);

            WebApiDataContainer<RoleModel> retList = new WebApiDataContainer<RoleModel>();
            retList.recordsFiltered = pager.TotalRow;
            retList.recordsTotal = pager.TotalRow;
            retList.data = ienm;

            return retList;
        }

        [Route("~/api/RoleManagement/{id}")]
        [HttpGet]
        // GET api/SampleDataApi/PageID
        public RoleModel GetOne([FromUri] Guid id)
        {
            var result = RoleService.GetDetail(id);
            return result;
        }

        [Route("~/api/RoleManagement/Create")]
        [HttpPost]
        public void Create([FromBody] RoleModel model)
        {
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            RoleService.Create(model, cUser.Account, cTime);
        }

        [Route("~/api/RoleManagement/Modify")]
        [HttpPost]
        public void Modify([FromBody] RoleModel model)
        {
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            RoleService.Modify(model, cUser.Account, cTime);
        }

        [Route("~/api/RoleManagement/Delete")]
        [HttpPost]
        public void Delete([FromBody] RoleModel model)
        {
            DateTime cTime = DateTime.Now;

            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            RoleService.Delete(model.ID, cUser.Account, cTime);
        }
    }
}
