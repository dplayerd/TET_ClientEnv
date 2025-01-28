using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Platform.Auth.Models;
using Platform.WebSite.Filters;
using Platform.WebSite.Models;
using Platform.WebSite.Services;

namespace Platform.WebSite.Controllers
{
    [WebApiAuthorizeCore]
    public partial class UserRoleManagementApiController : ApiController
    {
        #region Query Classes
        public class InputQuery : DataTablePager
        {
            public string caption { get; set; }
        }
        #endregion


        [Route("~/api/UserRoleManagement/GetDataTableList")]
        [HttpPost]
        public WebApiDataContainer<UserRoleModel> GetDataTableList([FromBody] DataTablePager inputParameters)
        {
            var pager = inputParameters.ToPager();
            var ienm = UserRoleService.GetUserRoleList(pager);

            WebApiDataContainer<UserRoleModel> retList = new WebApiDataContainer<UserRoleModel>();
            retList.recordsFiltered = pager.TotalRow;
            retList.recordsTotal = pager.TotalRow;
            retList.data = ienm;

            return retList;
        }

        [Route("~/api/UserRoleManagement/MapUserRole")]
        [HttpPost]
        public void MapUserRole([FromBody] UserRoleMappingInputModel model)
        {
            string cUser = UserProfileService.GetCurrentUserID();
            DateTime cTime = DateTime.Now;

            UserRoleService.MapUserRole(model.UserIDList, model.RoleIDList, cUser, cTime);
        }

        [Route("~/api/UserRoleManagement/UnmapUserRole")]
        [HttpPost]
        public void UnmapUserRole([FromBody] UserRoleMappingInputModel model)
        {
            string cUser = UserProfileService.GetCurrentUserID();
            DateTime cTime = DateTime.Now;

            UserRoleService.UnmapUserRole(model.UserIDList, model.RoleIDList, cUser, cTime);
        }

        [Route("~/api/UserRoleManagement/GetAssignedRoleUserList/{roleID}")]
        [HttpPost]
        public WebApiDataContainer<UserAccountModel> GetAssignedRoleUserList([FromUri] Guid roleID, [FromBody] InputQuery inputParameters)
        {
            var pager = inputParameters.ToPager();
            var ienm = UserRoleService.GetAssignedRoleUserList(roleID, pager, inputParameters.caption);

            WebApiDataContainer<UserAccountModel> retList = new WebApiDataContainer<UserAccountModel>();
            retList.recordsFiltered = pager.TotalRow;
            retList.recordsTotal = pager.TotalRow;
            retList.data = ienm;

            return retList;
        }

        [Route("~/api/UserRoleManagement/GetUnassignedRoleUserList/{roleID}")]
        [HttpPost]
        public WebApiDataContainer<UserAccountModel> GetUnassignedRoleUserList([FromUri] Guid roleID, [FromBody] InputQuery inputParameters)
        {
            var pager = inputParameters.ToPager();
            var ienm = UserRoleService.GetUnassignedRoleUserList(roleID, pager, inputParameters.caption);

            WebApiDataContainer<UserAccountModel> retList = new WebApiDataContainer<UserAccountModel>();
            retList.recordsFiltered = pager.TotalRow;
            retList.recordsTotal = pager.TotalRow;
            retList.data = ienm;

            return retList;
        }
    }
}
