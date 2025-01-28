using Platform.AbstractionClass;
using Platform.ORM;
using Platform.Portal.Models;
using Platform.WebSite.Filters;
using Platform.WebSite.Models;
using Platform.WebSite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using BI.SPA_ApproverSetup;
using BI.SPA_ApproverSetup.Models;
using Platform.Auth;
using Platform.Auth.Models;
using Newtonsoft.Json;

namespace Platform.WebSite.Controllers
{
    [WebApiAuthorizeCore]
    public class SPA_ApproverSetupApiController : ApiController
    {
        private SPA_ApproverSetupManager _mgr = new SPA_ApproverSetupManager();
        private UserManager _userManager = new UserManager();

        #region Input / Output Classes
        public class TempPager : DataTablePager
        {
            public string[] ServiceItemID { get; set; } = new string[0];
            public string[] BUID { get; set; } = new string[0];
        }
        #endregion

        [Route("~/api/SPA_ApproverSetupApi/GetDataTableList/{siteID?}")]
        [HttpPost]
        public WebApiDataContainer<TET_SPA_ApproverSetupModel> GetDataTableList([FromBody] TempPager dataTablePager)
        {

            DateTime cTime = DateTime.Now;
            var cUser = UserProfileService.GetCurrentUser();
            if (string.IsNullOrWhiteSpace(cUser.ID))
                throw new UnauthorizedAccessException();

            // 轉換查詢條件的型別
            List<Guid> ServiceItemIDs = new List<Guid>();
            List<Guid> BUIDs = new List<Guid>();
            foreach (var item in dataTablePager.ServiceItemID)
            {
                if (Guid.TryParse(item, out Guid temp))
                    ServiceItemIDs.Add(temp);
            }

            foreach (var item in dataTablePager.BUID)
            {
                if (Guid.TryParse(item, out Guid temp))
                    BUIDs.Add(temp);
            }

            Pager pager = dataTablePager.ToPager();
            var list = this._mgr.GetList(ServiceItemIDs, BUIDs, cUser.ID, cTime, pager);

            var retObj = new WebApiDataContainer<TET_SPA_ApproverSetupModel>()
            {
                recordsFiltered = pager.TotalRow,
                recordsTotal = pager.TotalRow,
                data = list
            };

            // 調整 UI 顯示用欄位
            //var serviceItemList = TET_ParameterService.GetTET_ParametersList("SPA評鑑項目", TET_ParameterService.KeyType.Id);
            //var buList = TET_ParameterService.GetTET_ParametersList("SPA評鑑單位", TET_ParameterService.KeyType.Id);
            foreach (var item in list)
            {
                //item.ServiceItemText = serviceItemList.Where(obj => obj.Key == item.ServiceItemID.ToString()).FirstOrDefault()?.Text;
                //item.BUText = buList.Where(obj => obj.Key == item.BUID.ToString()).FirstOrDefault()?.Text;

                var users = this._userManager.GetUserList(item.InfoFills);
                item.InfoConfirm = ToUserInfoText(this._userManager.GetUser(item.InfoConfirm));
                item.Lv1Apprvoer = ToUserInfoText(this._userManager.GetUser(item.Lv1Apprvoer));
                item.Lv2Apprvoer = ToUserInfoText(this._userManager.GetUser(item.Lv2Apprvoer));

                item.InfoFillUserInfos.AddRange(users.Select(obj => ToUserInfoText(obj)));
            }

            return retObj;
        }

        /// <summary> 將個人資訊轉為指定格式的文字 </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string ToUserInfoText(UserModel obj)
        {
            if(obj == null)
                return string.Empty;

            return obj.FirstNameEN + " " + obj.LastNameEN + " (" + obj.EmpID + ")";
        }

        [Route("~/api/SPA_ApproverSetupApi/Detail/{ID}")]
        [HttpGet]
        // GET api/SPA_ApproverSetupApi/serviceItemID/buid
        public TET_SPA_ApproverSetupModel GetOne([FromUri] Guid ID)
        {
            var result = this._mgr.GetDetail(ID);
            return result;
        }

        [Route("~/api/SPA_ApproverSetupApi/Create")]
        [HttpPost]
        // POST api/SPA_ApproverSetupApi/Create
        public IHttpActionResult Create([FromBody] TET_SPA_ApproverSetupModel model)
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

        [Route("~/api/SPA_ApproverSetupApi/Modify/{ID}")]
        [HttpPost]
        public IHttpActionResult Modify([FromBody] TET_SPA_ApproverSetupModel model)
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
    }
}