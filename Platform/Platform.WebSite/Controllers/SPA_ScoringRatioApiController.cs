using BI.SPA_ApproverSetup.Models;
using BI.SPA_ApproverSetup.Utils;
using BI.SPA_ApproverSetup;
using Platform.AbstractionClass;
using Platform.WebSite.Models;
using Platform.WebSite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Platform.WebSite.Filters;
using BI.Shared.Models;
using BI.Shared;
using Newtonsoft.Json;

namespace Platform.WebSite.Controllers
{
    [WebApiAuthorizeCore]
    public class SPA_ScoringRatioApiController : ApiController
    {
        private SPA_ScoringRatioManager _mgr = new SPA_ScoringRatioManager();


        [Route("~/api/SPA_ScoringRatioApi/List")]
        public List<SPA_ScoringRatioModel> GetList()
        {
            DateTime cTime = DateTime.Now;
            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            var pager = new Pager() { AllowPaging = false };

            var list = this._mgr.GetList(cUser.ID, cTime, pager);
            return list;
        }


        public class EditTempClass
        {
            public List<SPA_ScoringRatioModel> Items { get; set; } = new List<SPA_ScoringRatioModel>();
        }

        [Route("~/api/SPA_ScoringRatioApi/Save")]
        [HttpPost]
        public IHttpActionResult Save([FromBody] EditTempClass inputModel)
        {
            string cUser = UserProfileService.GetCurrentUserID();
            DateTime cTime = DateTime.Now;

            if(inputModel?.Items == null)
                return BadRequest("'Input Model' and 'Items' is required.");

            try
            {
                this._mgr.Modify(inputModel.Items, cUser, cTime);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }
        }
    }
}