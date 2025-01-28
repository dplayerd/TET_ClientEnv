using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Platform.AbstractionClass;
using Platform.Portal;
using Platform.Portal.Models;
using Platform.WebSite.Models;

namespace Platform.WebSite.Services
{
    public class SiteService
    {
        private const string _siteInfoKey = "Shared.SiteInfo";
        public static Guid DefaultSiteID { get; set; } = new Guid("15E34669-CC25-48C5-85C6-6AF49252CBFE");

        public static SiteViewModel GetDefaultSite(Guid? currentPageID)
        {
            SiteViewModel siteInfo;

            if (HttpContext.Current.Items[_siteInfoKey] == null)
            {
                var model = SiteManager.GetSite(DefaultSiteID);
                siteInfo = new SiteViewModel()
                {
                    ID = model.ID.ToString(),
                    Name = model.Name,
                    FullName = model.Title,
                    HeaderText = model.HeaderText,
                    FooterText = model.FooterText,
                    ImageUrl = model.ImageUrl,
                    // TODO: 未完成
                    MaintainerUrl = model.ImageUrl,
                };

                siteInfo.MainMenus = NavigationService.GetTreeList(model.ID);
                siteInfo.FooterMenus = NavigationService.GetFooterTreeList(model.ID);

                HttpContext.Current.Items[_siteInfoKey] = siteInfo;
            }
            else
            {
                siteInfo = HttpContext.Current.Items[_siteInfoKey] as SiteViewModel;
            }

            SiteService.SetCurrentPage(siteInfo, currentPageID);
            return siteInfo;
        }

        /// <summary> 設定目標前頁面 </summary>
        /// <param name="list"></param>
        private static void SetCurrentPage(SiteViewModel model, Guid? currentPageID)
        {
            var treeList = model.MainMenus;
            if (treeList == null)
                return;

            NavigateItemViewModel node = FindCurrentPage(treeList, currentPageID);

            if (node == null)
            {
                //node = treeList.SelectMany(obj => obj.FlattenNodeList()).LastOrDefault();
                node = new NavigateItemViewModel()
                {
                    ID = Guid.Empty.ToString(),
                    Name = "首頁",
                    Children = new List<NavigateItemViewModel>(),
                    ParentID = null,
                    MenuType = (byte)MenuTypeEnum.HTML,
                };
            }

            model.CurrentPage = node;
            node.IsCurrentPage = true;
        }

        private static NavigateItemViewModel FindCurrentPage(IEnumerable<NavigateItemViewModel> treeList, Guid? currentPageID)
        {
            //return treeList.LastOrDefault();


            //var pageID = HttpContext.Current.Request.QueryString["PageID"];
            string pageID = (currentPageID.HasValue) ? currentPageID.Value.ToString() : null;
            var query = treeList.Select(obj => obj.FindNode(pageID)).Where(obj => obj != null);

            var retObj = query.FirstOrDefault(); 
            return retObj;
        }

        /// <summary> 修改站台資訊 </summary>
        /// <param name="vModel"></param>
        /// <param name="fileContent"> 站台 LOGO </param>
        /// <param name="userID"> 目前登入者 </param>
        public static void ModifySiteInfo(SiteViewModel vModel, FileContent fileContent, string userID)
        {
            if (!UserProfileService.HasLogin())
            {
                throw new Exception("Auth Exception");
            }

            SiteManager sitemgr = new SiteManager();
            Guid.TryParse(vModel.ID, out Guid siteID);
            var dbModel = SiteManager.GetSite(siteID);

            dbModel.HeaderText = vModel.HeaderText;
            dbModel.FooterText = vModel.FooterText;
            dbModel.Name = vModel.Name;
            dbModel.Title = vModel.FullName;
            dbModel.HeaderText = vModel.HeaderText;

            sitemgr.ModifySiteInfo(dbModel, fileContent, userID);
        }
    }
}