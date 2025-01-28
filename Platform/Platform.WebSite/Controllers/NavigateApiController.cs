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
    public class NavigateApiController : ApiController
    {
        // api/NavigateApi/GetList/{siteID}/?
        [Route("~/api/NavigateApi/GetList/{siteID}")]
        [HttpGet]
        public List<NavigateItemViewModel> GetTreeList(Guid siteID, [FromUri]MenuTypeEnum? MenuType)
        {
            var list = NavigationService.GetList(siteID);
            return list;
        }
    }
}
