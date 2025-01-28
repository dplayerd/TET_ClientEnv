using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BI.SampleData;
using BI.SampleData.Models;
using Platform.AbstractionClass;
using Platform.WebSite.Filters;
using Platform.WebSite.Services;

namespace Platform.WebSite.Controllers
{
    [AuthorizeCore]
    public class SampleDataController : BaseMVCController
    {
        #region Standard Http
        // GET: Home
        public ActionResult Index([System.Web.Http.FromUri] int? pageIndex, [System.Web.Http.FromUri] int? pageSize)
        {
            this.InitAction();

            Pager pager = Pager.GetDefaultPager();
            if(pageIndex.HasValue)
                pager.PageIndex = pageIndex.Value;
            if (pageSize.HasValue)
                pager.PageSize = pageSize.Value;

            var list = new SampleDataManager().GetList(new SampleDataFilterConditions(), pager);

            this.SetPagingInfo(pager);
            return View(list);
        }

        public ActionResult Details(int id)
        {
            this.InitAction();

            var item = new SampleDataManager().GetDetail(id);
            return View(item);
        }

        public ActionResult Create()
        {
            this.InitAction();

            // 給予欄位預設值
            var item = SampleDataModel.GetDefault();
            return View("Edit", item);
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            string cUser = UserProfileService.GetCurrentUserID();
            DateTime cTime = DateTime.Now;

            var model = new SampleDataModel()
            {
                Title = collection["Title"],
                Name = collection["Name"]
            };

            new SampleDataManager().Create(model, cUser, cTime);
            this.AddTipMessage("已儲存");
            return RedirectToAction("Edit", new { Id = model.Id });
        }

        public ActionResult Edit(int id)
        {
            this.InitAction();

            var item = new SampleDataManager().GetDetail(id);
            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            string cUser = UserProfileService.GetCurrentUserID();
            DateTime cTime = DateTime.Now;

            var manager = new SampleDataManager(); 
            var model = manager.GetDetail(id);

            if (model != null)
            {
                model.Title = collection["Title"];
                model.Name = collection["Name"];

                manager.Modify(model, cUser, cTime);
                this.AddTipMessage("已更新");
                return RedirectToAction("Edit", new { Id = model.Id });
            }
            else
            {
                this.AddTipMessage("資料不存在");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            string cUser = UserProfileService.GetCurrentUserID();
            DateTime cTime = DateTime.Now;

            var manager = new SampleDataManager(); 
            var model = manager.GetDetail(id);

            if (model != null)
            {
                manager.Delete(id, cUser, cTime);
                this.AddTipMessage("已刪除");
            }

            return RedirectToAction("Index");
        }
        #endregion
    }
}
