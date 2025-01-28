using BI.SPA_ApproverSetup;
using BI.SPA_ApproverSetup.Models;
using BI.SPA_ApproverSetup.Utils;
using Newtonsoft.Json;
using Platform.AbstractionClass;
using Platform.WebSite.Filters;
using Platform.WebSite.Models;
using Platform.WebSite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Platform.WebSite.Controllers
{
    [WebApiAuthorizeCore]
    public class SPA_PeriodApiController : ApiController
    {
        private SPA_PeriodManager _mgr = new SPA_PeriodManager();

        #region Input / Output Classes
        public class TempPager : DataTablePager
        {
            public string period { get; set; } = string.Empty;
        }
        #endregion


        [Route("~/api/SPA_PeriodApi/GetDataTableList/{siteID?}")]
        [HttpPost]
        public WebApiDataContainer<TET_SPA_PeriodModel> GetDataTableList([FromBody] TempPager dataTablePager)
        {
            DateTime cTime = DateTime.Now;
            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();


            Pager pager = dataTablePager.ToPager();
            var list = this._mgr.GetList(dataTablePager.period, cUser.ID, cTime, pager);

            var retObj = new WebApiDataContainer<TET_SPA_PeriodModel>()
            {
                recordsFiltered = pager.TotalRow,
                recordsTotal = pager.TotalRow,
                data = list
            };

            return retObj;
        }

        [Route("~/api/SPA_PeriodApi/Detail/{id}")]
        [HttpGet]
        // GET api/SPA_PeriodApi/serviceItemID/buid
        public TET_SPA_PeriodModel GetOne([FromUri] Guid id)
        {
            var result = this._mgr.GetDetail(id);
            return result;
        }

        [Route("~/api/SPA_PeriodApi/Create")]
        [HttpPost]
        // POST api/SPA_PeriodApi/Create
        public IHttpActionResult Create([FromBody] TET_SPA_PeriodModel model)
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

        [Route("~/api/SPA_PeriodApi/Modify/{id}")]
        [HttpPost]
        public IHttpActionResult Modify([FromBody] TET_SPA_PeriodModel model)
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


        public class TempChangeStatus
        {
            public Guid id { get; set; }
            public string status { get; set; }
        }


        [Route("~/api/SPA_PeriodApi/ChangeStatus/{spa_period_id}")]
        [HttpPost]
        public IHttpActionResult ChangeStatus([FromBody] TempChangeStatus model)
        {
            string cUser = UserProfileService.GetCurrentUserID();
            DateTime cTime = DateTime.Now;

            var enm = SPA_Period_Util.ParseToStatus(model.status);

            try
            {
                if (this._mgr.IsAnyPeriodStarted(model.id))
                    throw new Exception("已有其它評鑑期間的狀態為「進行中」");

                this._mgr.ChangeStatus(model.id, enm, cUser, cTime);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
        }
    }
}