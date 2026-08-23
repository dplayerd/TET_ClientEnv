using BI.SPA_ScoringInfo;
using BI.SPA_ScoringInfo.Models;
using Newtonsoft.Json;
using Platform.AbstractionClass;
using Platform.WebSite.Filters;
using Platform.WebSite.Models;
using Platform.WebSite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Platform.WebSite.Controllers
{
    [WebApiAuthorizeCore]
    public class SPA_ScoringInfoSheetSetupApiController : ApiController
    {
        private SPA_ScoringInfoSheetManager _mgr = new SPA_ScoringInfoSheetManager();

        public class TempPager : DataTablePager
        {
            public string[] ServiceItemID { get; set; } = new string[0];
            public string[] POSource { get; set; } = new string[0];
        }

        [Route("~/api/SPA_ScoringInfoSheetSetupApi/GetDataTableList/{siteID?}")]
        [HttpPost]
        public WebApiDataContainer<SPA_ScoringInfoSheetModel> GetDataTableList([FromBody] TempPager dataTablePager)
        {
            DateTime cTime = DateTime.Now;
            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            if (dataTablePager == null)
                dataTablePager = new TempPager();

            var serviceItemIDs = new List<Guid>();
            foreach (var item in dataTablePager.ServiceItemID)
            {
                if (Guid.TryParse(item, out Guid temp))
                    serviceItemIDs.Add(temp);
            }

            var poSource = dataTablePager.POSource?.Where(obj => !string.IsNullOrWhiteSpace(obj)).ToArray();
            Pager pager = dataTablePager.ToPager();
            var list = this._mgr.GetList(serviceItemIDs, poSource, cUser.ID, cTime, pager);

            return new WebApiDataContainer<SPA_ScoringInfoSheetModel>()
            {
                recordsFiltered = pager.TotalRow,
                recordsTotal = pager.TotalRow,
                data = list
            };
        }

        [Route("~/api/SPA_ScoringInfoSheetSetupApi/Detail")]
        [HttpGet]
        public SPA_ScoringInfoSheetModel GetOne([FromUri] Guid serviceItemID, [FromUri] string poSource)
        {
            return this._mgr.GetDetail(serviceItemID, poSource);
        }

        [Route("~/api/SPA_ScoringInfoSheetSetupApi/Modify")]
        [HttpPost]
        public IHttpActionResult Modify([FromBody] SPA_ScoringInfoSheetModel model)
        {
            string cUser = UserProfileService.GetCurrentUserID();
            DateTime cTime = DateTime.Now;

            try
            {
                this._mgr.Modify(model, cUser, cTime);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
        }

        [Route("~/api/SPA_ScoringInfoSheetSetupApi/Create")]
        [HttpPost]
        public IHttpActionResult Create([FromBody] SPA_ScoringInfoSheetModel model)
        {
            string cUser = UserProfileService.GetCurrentUserID();
            DateTime cTime = DateTime.Now;

            try
            {
                this._mgr.Create(model, cUser, cTime);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
        }
    }
}
