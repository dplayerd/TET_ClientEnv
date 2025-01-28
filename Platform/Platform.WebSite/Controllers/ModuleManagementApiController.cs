using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Platform.AbstractionClass;
using Platform.Portal;
using Platform.Portal.Models;
using Platform.WebSite.Models;
using Platform.WebSite.Services;

namespace Platform.WebSite.Controllers
{
    public class ModuleManagementApiController : ApiController
    {
        private ModuleManager _mgr = new ModuleManager();

        [Route("~/api/ModuleManagementApi/GetDataTableList/")]
        [HttpPost]
        // GET api/ModuleManagementApi/SiteID
        public WebApiDataContainer<ModuleModel> GetDataTableList([FromBody] DataTablePager dataTablePager)
        {
            Pager pager = dataTablePager.ToPager();

            var list = this._mgr.GetAdminList(pager);
            var retObj = new WebApiDataContainer<ModuleModel>()
            {
                recordsFiltered = pager.TotalRow,
                recordsTotal = pager.TotalRow,
                data = list
            };

            return retObj;
        }

        [Route("~/api/ModuleManagementApi/{moduleID}")]
        [HttpGet]
        // GET api/ModuleManagementApi/PageID
        public ModuleModel GetOne([FromUri] Guid moduleID)
        {
            var result = this._mgr.GetAdminDetail(moduleID);
            return result;
        }

        [Route("~/api/ModuleManagementApi/Create")]
        [HttpPost]
        // POST api/ModuleManagementApi/Create
        public void Create([FromBody] ModuleModel model)
        {
            string cUser = UserProfileService.GetCurrentUserID();
            DateTime cTime = DateTime.Now;

            this._mgr.CreateModule(model, cUser, cTime);
        }

        [Route("~/api/ModuleManagementApi/Modify")]
        [HttpPost]
        public void Modify([FromBody] ModuleModel model)
        {
            string cUser = UserProfileService.GetCurrentUserID();
            DateTime cTime = DateTime.Now;

            this._mgr.ModifyModule(model, cUser, cTime);
        }

        [Route("~/api/ModuleManagementApi/Delete")]
        [HttpPost]
        public void Delete([FromBody] ModuleModel model)
        {
            string cUser = UserProfileService.GetCurrentUserID();
            DateTime cTime = DateTime.Now;

            this._mgr.DeleteModule(model.ID, cUser, cTime);
        }

        [Route("~/api/ModuleManagementApi/GetModuleList")]
        [HttpGet]
        public List<ModuleModel> GetModuleList()
        {
            var list = this._mgr.GetModuleList();
            return list;
        }
    }
}
