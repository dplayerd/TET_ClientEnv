using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BI.SampleData;
using BI.SampleData.Models;
using Platform.AbstractionClass;
using Platform.WebSite.Filters;
using Platform.WebSite.Models;
using Platform.WebSite.Services;

namespace Platform.WebSite.Controllers
{
    [WebApiAuthorizeCore]
    public class SampleDataApiController : ApiController
    {
        [Route("~/api/SampleDataApi/")]
        [HttpGet]
        public List<SampleDataModel> Get()
        {
            var list = new SampleDataManager().GetList();
            return list;
        }

        [Route("~/api/SampleDataApi/GetDataTableList")]
        [HttpPost]
        public WebApiDataContainer<SampleDataModel> PostToGetList([FromBody] SampleDataRequestParameter inputParameters)
        {
            var pager = inputParameters.ToPager();
            var filter = inputParameters.ToFilterObject();
            var ienm = new SampleDataManager().GetList(filter, pager);

            WebApiDataContainer<SampleDataModel> retList = new WebApiDataContainer<SampleDataModel>();
            retList.recordsFiltered = pager.TotalRow;
            retList.recordsTotal = pager.TotalRow;
            retList.data = ienm;

            return retList;
        }

        [Route("~/api/SampleDataApi/{id}")]
        [HttpGet]
        // GET api/SampleDataApi/PageID
        public SampleDataModel GetOne([FromUri] int id)
        {
            var result = new SampleDataManager().GetAdminDetail(id);
            return result;
        }

        [Route("~/api/SampleDataApi/Create")]
        [HttpPost]
        public void Create([FromBody] SampleDataModel model)
        {
            string cUser = UserProfileService.GetCurrentUserID();
            DateTime cTime = DateTime.Now;

            new SampleDataManager().Create(model, cUser, cTime);
        }

        [Route("~/api/SampleDataApi/Modify")]
        [HttpPost]
        public void Modify([FromBody] SampleDataModel model)
        {
            string cUser = UserProfileService.GetCurrentUserID();
            DateTime cTime = DateTime.Now;

            new SampleDataManager().Modify(model, cUser, cTime);
        }

        [Route("~/api/SampleDataApi/Delete")]
        [HttpPost]
        public void Delete([FromBody] SampleDataModel model)
        {
            string cUser = UserProfileService.GetCurrentUserID();
            DateTime cTime = DateTime.Now;

            new SampleDataManager().Delete(model.Id, cUser, cTime);
        }
    }
}
