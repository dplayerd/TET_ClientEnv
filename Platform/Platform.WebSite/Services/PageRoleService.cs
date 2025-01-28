using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Platform.AbstractionClass;
using Platform.Portal;
using Platform.Portal.Models;

namespace Platform.WebSite.Services
{
    public class PageRoleService
    {
        private const string _permissionKey = "PermissionKey";

        /// <summary> 取得頁面角色清單 </summary>
        /// <param name="pager"></param>
        /// <param name="pageID"></param>
        /// <returns></returns>
        public static List<PageRoleModel> GetPageRoleList(Pager pager, Guid pageID)
        {
            PageRoleManager mgr = new PageRoleManager();
            return mgr.GetPageRoleList(pager, pageID);
        }

        /// <summary> 取得頁面角色清單 </summary>
        /// <param name="pager"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public static List<PageRoleModel> GetRolePageList(Pager pager, Guid roleID)
        {
            PageRoleManager mgr = new PageRoleManager();
            return mgr.GetRolePageList(pager, roleID);
        }

        /// <summary> 取得頁面角色清單 </summary>
        /// <param name="pageID"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public static PageRoleModel GetPageRole(Guid pageID, Guid roleID)
        {
            PageRoleManager mgr = new PageRoleManager();
            var obj = mgr.GetPageRole(pageID, roleID);
            return obj;
        }

        /// <summary> 設定頁面角色 </summary>
        /// <param name="model"></param>
        /// <param name="cUserID"></param>
        /// <param name="time"></param>
        public static void MapPageRole(List<PageRoleModel> model, string cUserID, DateTime time)
        {
            PageRoleManager mgr = new PageRoleManager();
            mgr.MapPageAndRole(model, cUserID, time);
        }

        ///// <summary> 是否有權限執行標準行為 </summary>
        ///// <param name="enm"></param>
        ///// <returns></returns>
        //public static bool IsAuthorized(Guid pageID, AllowActionEnum enm)
        //{
        //    var cPage = SiteService.GetDefaultSite(pageID).CurrentPage;

        //    if (cPage == null)
        //        return false;

        //    if (!UserProfileService.HasLogin())
        //        return false;

        //    var cUserID = UserProfileService.GetCurrentUserID();
        //    var cPageID = (cPage == null) ? Guid.Empty : new Guid(cPage.ID);
        //    return IsAuthorized(cPageID, cUserID, enm);
        //}

        /// <summary> 是否有權限執行標準行為 </summary>
        /// <param name="pageID"></param>
        /// <param name="userID"></param>
        /// <param name="enm"></param>
        /// <returns></returns>
        public static bool IsAuthorized(Guid pageID, string userID, AllowActionEnum enm)
        {
            PermissionModel model = GetPermission(pageID, userID);
            return model.IsAllowed(enm);
        }

        ///// <summary> 是否有權限執行特殊行為 </summary>
        ///// <param name="functionCode"></param>
        ///// <returns></returns>
        //public static bool IsAuthorized(string functionCode)
        //{
        //    var cPage = SiteService.GetDefaultSite().CurrentPage;

        //    if (cPage == null)
        //        return false;

        //    if (!UserProfileService.HasLogin())
        //        return false;

        //    var cUser = UserProfileService.GetCurrentUserID();
        //    var cPageID = (cPage == null) ? Guid.Empty : new Guid(cPage.ID);
        //    return IsAuthorized(cPageID, cUser, functionCode);
        //}

        /// <summary> 是否有權限執行特殊行為 </summary>
        /// <param name="pageID"></param>
        /// <param name="userID"></param>
        /// <param name="functionCode"></param>
        /// <returns></returns>
        public static bool IsAuthorized(Guid pageID, string userID, string functionCode)
        {
            PermissionModel model = GetPermission(pageID, userID);
            return model.IsAllowed(functionCode);
        }

        #region "Private"
        /// <summary> 讀取權限，並先放到快取中，避免重覆讀取 </summary>
        /// <param name="pageID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        private static PermissionModel GetPermission(Guid pageID, string userID)
        {
            PermissionModel model;

            if (HttpContext.Current.Items[_permissionKey] == null)
            {
                PageRoleManager mgr = new PageRoleManager();
                model = mgr.GetPermission(pageID, userID);

                HttpContext.Current.Items[_permissionKey] = model;
            }
            else
                model = HttpContext.Current.Items[_permissionKey] as PermissionModel;
            return model;
        }
        #endregion
    }
}