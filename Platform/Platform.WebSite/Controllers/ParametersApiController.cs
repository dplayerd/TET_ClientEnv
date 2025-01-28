using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BI.Shared;
using BI.Shared.Models;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using Platform.AbstractionClass;
using Platform.Portal.Models;
using Platform.WebSite.Filters;
using Platform.WebSite.Models;
using Platform.WebSite.Services;

namespace Platform.WebSite.Controllers
{
    [WebApiAuthorizeCore]
    public class ParametersApiController : ApiController
    {
        private TET_ParametersManager _mgr = new TET_ParametersManager();

        public class TempClass
        {
            public List<TET_ParametersModel> Items { get; set; } = new List<TET_ParametersModel>();
        }


        [Route("~/api/ParametersApi/List/{type}")]
        // GET api/ParametersApi/List/{type}
        public List<TET_ParametersModel> GetList(string type)
        {
            var list = this._mgr.GetTET_ParametersList(type);
            return list;
        }

        [Route("~/api/ParametersApi/Save")]
        [HttpPost]
        // POST api/PageApi/Save
        public IHttpActionResult Save([FromBody] TempClass model)
        {
            string cUser = UserProfileService.GetCurrentUserID();
            DateTime cTime = DateTime.Now;

            // 送出
            try
            {
                this._mgr.WriteParameterList(model.Items, cUser, cTime);
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject(new string[] { ex.Message }));
            }

            return Ok();
        }
    }
}