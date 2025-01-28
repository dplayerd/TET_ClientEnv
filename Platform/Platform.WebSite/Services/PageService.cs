using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Platform.AbstractionClass;
using Platform.Portal;
using Platform.Portal.Models;

namespace Platform.WebSite.Services
{
    public class PageService
    {
        #region Private
        private static PageManager GetPageManager()
        {
            return new PageManager();
        }
        #endregion

        #region Public
        /// <summary> 查出所有的頁面 </summary>
        /// <param name="siteID"></param>
        /// <param name="menuType"></param>
        /// <returns></returns>
        public static List<PageModel> GetPageList(Guid siteID, MenuTypeEnum? menuType = null)
        {
            return PageService.GetPageManager().GetPageList(siteID, menuType);
        }

        /// <summary> 查出所有的頁面 </summary>
        /// <param name="siteID"></param>
        /// <returns></returns>
        public static List<PageModel> GetPageAdminList(Guid siteID, Pager pager)
        {
            return PageService.GetPageManager().GetPageAdminList(siteID, pager);
        }

        /// <summary> 查出指定頁面 </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static PageModel GetPage(Guid id)
        {
            return PageService.GetPageManager().GetPage(id);
        }

        /// <summary> 查出指定頁面 </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static PageModel GetAdminPage(Guid id)
        {
            return PageService.GetPageManager().GetAdminPage(id);
        }

        /// <summary> 建立頁面 </summary>
        /// <param name="model"></param>
        /// <param name="userID"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static void CreatePage(PageModel model, string userID, DateTime time)
        {
            PageService.GetPageManager().CreatePage(model, userID, time);
        }

        /// <summary> 修改頁面 </summary>
        /// <param name="model"></param>
        /// <param name="userID"></param>
        /// <param name="time"></param>
        public static void ModifyPage(PageModel model, string userID, DateTime time)
        {
            PageService.GetPageManager().ModifyPage(model, userID, time);
        }

        /// <summary> 修改頁面 </summary>
        /// <param name="pageID"></param>
        /// <param name="userID"></param>
        /// <param name="time"></param>
        public static void DeletePage(Guid pageID, string userID, DateTime time)
        {
            PageService.GetPageManager().DeletePage(pageID, userID, time);
        }


        /// <summary> 使用模組名稱，查出指定的模組在哪些頁面 </summary>
        /// <param name="siteID"> 站台代碼 </param>
        /// <param name="moduleName"> 模組名稱 </param>
        /// <returns></returns>
        public static List<PageModel> GetPageListOfModule(Guid siteID, string moduleName)
        { 
            return PageService.GetPageManager().GetPageListOfModule(siteID, moduleName);
        }
        #endregion
    }
}