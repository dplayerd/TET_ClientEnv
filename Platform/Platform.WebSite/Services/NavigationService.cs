using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPOI.SS.UserModel;
using Platform.Portal;
using Platform.Portal.Models;
using Platform.WebSite.Models;

namespace Platform.WebSite.Services
{
    public class NavigationService
    {
        /// <summary> 取得選單結構 (樹狀結構) </summary>
        /// <param name="siteID"></param>
        /// <param name="menuType"></param>
        /// <returns></returns>
        public static List<NavigateItemViewModel> GetTreeList(Guid siteID, MenuTypeEnum? menuType = null)
        {
            var sourceList = PageService.GetPageList(siteID, menuType);
            var retList = sourceList.Select(obj => ConvertToNavigateViewModel(obj)).ToList();
            retList = ProcessToNodeTree(retList).ToList();
            ProcessAuth(retList);
            return retList;
        }

        /// <summary> 取得選單結構 (清單結構) </summary>
        /// <param name="siteID"></param>
        /// <param name="menuType"></param>
        /// <returns></returns>
        public static List<NavigateItemViewModel> GetList(Guid siteID, MenuTypeEnum? menuType = null)
        {
            var sourceList = PageService.GetPageList(siteID, menuType);
            var retList = sourceList.Select(obj => ConvertToNavigateViewModel(obj)).OrderBy(obj => obj.Name).ToList();
            ProcessToNodeTree(retList);
            return retList;
        }

        /// <summary> 取得頁尾的連結 </summary>
        /// <param name="siteID"></param>
        /// <returns></returns>
        public static List<NavigateItemViewModel> GetFooterTreeList(Guid siteID)
        {
            List<NavigateItemViewModel> list = new List<NavigateItemViewModel>();
            return list;
        }

        #region Private Methods
        /// <summary> 處理頁面授權 </summary>
        /// <param name="treeList"></param>
        /// <returns></returns>
        private static void ProcessAuth(List<NavigateItemViewModel> treeList)
        {
            var currentUserID = UserProfileService.GetCurrentUserID();
            if (currentUserID == null)
            {
                treeList = new List<NavigateItemViewModel>();
                return;
            }

            var authedPageIDList = new PageRoleManager().GetPageIDListByUserID(currentUserID).Select(obj => obj.ToString()).ToList();
            ProcessAuth(treeList, authedPageIDList);
        }

        /// <summary> 處理頁面授權 </summary>
        /// <param name="treeList"></param>
        /// <param name="authedPageIDList"> 目前有被授權的頁面 ID </param>
        /// <returns></returns>
        private static void ProcessAuth(List<NavigateItemViewModel> treeList, IEnumerable<string> authedPageIDList)
        {
            if (treeList == null || treeList.Count == 0)
                return;

            foreach(var model in treeList)
            {
                ProcessAuth(model, authedPageIDList);
            }
        }

        /// <summary> 處理頁面授權 </summary>
        /// <param name="item"></param>
        /// <param name="authedPageIDList"> 目前有被授權的頁面 ID </param>
        /// <returns></returns>
        private static void ProcessAuth(NavigateItemViewModel item, IEnumerable<string> authedPageIDList)
        {
            if (item == null)
                return;

            // 自己的是否要保留
            item.IsKeep = authedPageIDList.Contains(item.ID);

            foreach (var subItem in item.Children)
            {
                subItem.IsKeep = authedPageIDList.Contains(subItem.ID);

                if(subItem.HasChildren())
                    ProcessAuth(subItem, authedPageIDList);
            }

            // 如果子項目需要保留，自己就需要保留
            if (item.Children.Where(obj => obj.IsKeep).Any())
                item.IsKeep = true;
        }

        /// <summary> 從 PageModel 轉換為選單 </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static NavigateItemViewModel ConvertToNavigateViewModel(PageModel model)
        {
            NavigateItemViewModel vModel = new NavigateItemViewModel()
            {
                ID = model.ID.ToString(),
                ParentID = (model.ParentID == null) ? null : model.ParentID.ToString(),
                Name = model.Name,
                IconName = model.PageIcon,
                Url = NavigationService.BuildModuleUrl(model),
                MenuType = model.MenuType,
                SortIndex = 0,
                IsOuterLink = false,
                TipText = model.PageTitle,
                TipType = NavigateItemTipType.None
            };

            return vModel;
        }

        /// <summary> 組出模組連結 </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static string BuildModuleUrl(PageModel model)
        {
            //UrlHelper.GenerateUrl(
            //  null, "MyAction", "MyController", "http", "www.mydomain.com", String.Empty, null, RouteTable.Routes,
            //  this.ControllerContext.RequestContext, false
            //  );

            //TODO: 頁面連結(未完成)
            string url = "/Navigate/Index/" + model.ID;
            string path = VirtualPathUtility.ToAbsolute("~/" + url);
            return path;
        }

        /// <summary> 將清單轉換為樹狀選單 </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static IEnumerable<NavigateItemViewModel> ProcessToNodeTree(IEnumerable<NavigateItemViewModel> list)
        {
            Dictionary<string, NavigateItemViewModel> dic = new Dictionary<string, NavigateItemViewModel>();

            foreach (var item in list)
            {
                if (dic.ContainsKey(item.ID))
                    continue;


                // 如果父節點 id 為 null ，代表即為 root node
                if (string.IsNullOrEmpty(item.ParentID))
                {
                    dic.Add(item.ID, item);
                    continue;
                }

                // 處理子節點
                if (dic.ContainsKey(item.ParentID))
                {
                    var parentNode = dic[item.ParentID];

                    item.ParentNode = parentNode;
                    parentNode.Children.Add(item);
                }
                else
                {
                    var parentNode = list.Where(obj => obj.ID == item.ParentID).FirstOrDefault();

                    if (parentNode == null)
                        continue;

                    item.ParentNode = parentNode;
                    parentNode.Children.Add(item);
                }
            }

            var retlist = dic.Values as IEnumerable<NavigateItemViewModel>;
            return retlist;
        }
        #endregion
    }
}