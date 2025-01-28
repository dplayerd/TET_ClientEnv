using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Platform.AbstractionClass;
using Platform.ORM;
using Platform.Portal.Models;
using Platform.Infra;
using Platform.LogService;
using System.Data.Entity.Core.Metadata.Edm;
using System.Security.Policy;

namespace Platform.Portal
{
    public class PageManager
    {
        private Logger _logger = new Logger();

        #region Read
        /// <summary> 查出所有的頁面 </summary>
        /// <param name="siteID"></param>
        /// <param name="menuType"></param>
        /// <returns></returns>
        public List<PageModel> GetPageList(Guid siteID, MenuTypeEnum? menuType = null)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        (from obj in context.Pages
                         where
                            obj.SiteID == siteID &&
                            obj.IsEnable == true 
                         select obj);

                    if (menuType != null)
                    {
                        byte mType = (byte)menuType;
                        query = query.Where(obj => obj.MenuType == mType);
                    }

                    var list =
                       (from obj in query
                        orderby obj.SortNo
                        select
                            new PageModel()
                            {
                                ID = obj.ID,
                                SiteID = obj.SiteID,
                                ParentID = obj.ParentID,
                                Name = obj.Name,
                                PageTitle = obj.Description,
                                MenuType = obj.MenuType,
                                OuterLink = obj.Linkurl,
                                ModuleID = obj.ModuleID,
                                PageIcon = obj.PageIcon,
                                SortNo = obj.SortNo,
                                IsEnable = obj.IsEnable,
                                CreateUser = obj.CreateUser,
                                CreateDate = obj.CreateDate,
                                ModifyUser = obj.ModifyUser,
                                ModifyDate = obj.ModifyDate,
                            }
                       ).ToList();

                    return list;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                return default;
            }
        }

        /// <summary> 查出後管理用的所有的頁面 </summary>
        /// <param name="siteID"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<PageModel> GetPageAdminList(Guid siteID, Pager pager)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        (from obj in context.Pages
                         join obj2 in context.Modules
                         on obj.ModuleID equals obj2.ID into combine
                         from temp in combine.DefaultIfEmpty()
                         where
                            obj.SiteID == siteID
                         orderby obj.SortNo descending
                         select new PageModel()
                         {
                             ID = obj.ID,
                             SiteID = obj.SiteID,
                             ParentID = obj.ParentID,
                             Name = obj.Name,
                             PageTitle = obj.Description,
                             MenuType = obj.MenuType,
                             OuterLink = obj.Linkurl,
                             ModuleID = obj.ModuleID,
                             ModuleName = temp.Name,

                             PageIcon = obj.PageIcon,
                             SortNo = obj.SortNo,
                             IsEnable = obj.IsEnable,
                             CreateUser = obj.CreateUser,
                             CreateDate = obj.CreateDate,
                             ModifyUser = obj.ModifyUser,
                             ModifyDate = obj.ModifyDate,
                         });

                    var list = query.ProcessPager<PageModel>(pager).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                return default;
            }
        }

        /// <summary> 查出指定頁面 </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PageModel GetPage(Guid id)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var retObj =
                        (from obj in context.Pages
                         where
                            obj.ID == id &&
                            obj.IsEnable == true
                         select
                             new PageModel()
                             {
                                 ID = obj.ID,
                                 SiteID = obj.SiteID,
                                 ParentID = obj.ParentID,
                                 Name = obj.Name,
                                 PageTitle = obj.Description,
                                 MenuType = obj.MenuType,
                                 OuterLink = obj.Linkurl,
                                 ModuleID = obj.ModuleID,
                                 PageIcon = obj.PageIcon,
                                 SortNo = obj.SortNo,
                                 IsEnable = obj.IsEnable,
                                 CreateUser = obj.CreateUser,
                                 CreateDate = obj.CreateDate,
                                 ModifyUser = obj.ModifyUser,
                                 ModifyDate = obj.ModifyDate,
                             }
                        ).FirstOrDefault();

                    return retObj;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                return default;
            }
        }

        /// <summary> 依站台及頁面名稱查詢頁面 </summary>
        /// <param name="siteID"></param>
        /// <param name="pageName"></param>
        /// <param name="excludePageID"> 要排除的頁面 ID </param>
        /// <returns></returns>
        public PageModel GetPage(Guid siteID, string pageName, Guid? excludePageID)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        (from obj in context.Pages
                         where
                            obj.SiteID == siteID &&
                            obj.Name == pageName &&
                            obj.IsEnable == true
                         select obj);

                    if (excludePageID.HasValue)
                        query = query.Where(obj => obj.ID != excludePageID.Value);

                    var retObj =
                        (from obj in query
                         select
                             new PageModel()
                             {
                                 ID = obj.ID,
                                 SiteID = obj.SiteID,
                                 ParentID = obj.ParentID,
                                 Name = obj.Name,
                                 PageTitle = obj.Description,
                                 MenuType = obj.MenuType,
                                 OuterLink = obj.Linkurl,
                                 ModuleID = obj.ModuleID,
                                 PageIcon = obj.PageIcon,
                                 SortNo = obj.SortNo,
                                 IsEnable = obj.IsEnable,
                                 CreateUser = obj.CreateUser,
                                 CreateDate = obj.CreateDate,
                                 ModifyUser = obj.ModifyUser,
                                 ModifyDate = obj.ModifyDate,
                             }
                        ).FirstOrDefault();

                    return retObj;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                return default;
            }
        }

        /// <summary> 依站台及頁面名稱查詢頁面 </summary>
        /// <param name="pageID"></param>
        /// <returns></returns>
        public PageModel GetAdminPage(Guid pageID)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        (from obj in context.Pages
                         where
                            obj.ID == pageID 
                         select obj);

                    var retObj =
                        (from obj in query
                         select
                             new PageModel()
                             {
                                 ID = obj.ID,
                                 SiteID = obj.SiteID,
                                 ParentID = obj.ParentID,
                                 Name = obj.Name,
                                 PageTitle = obj.Description,
                                 MenuType = obj.MenuType,
                                 OuterLink = obj.Linkurl,
                                 ModuleID = obj.ModuleID,
                                 PageIcon = obj.PageIcon,
                                 SortNo = obj.SortNo,
                                 IsEnable = obj.IsEnable,
                                 CreateUser = obj.CreateUser,
                                 CreateDate = obj.CreateDate,
                                 ModifyUser = obj.ModifyUser,
                                 ModifyDate = obj.ModifyDate,
                             }
                        ).FirstOrDefault();

                    return retObj;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                return default;
            }
        }


        /// <summary> 使用模組名稱，查出指定的模組在哪些頁面 </summary>
        /// <param name="siteID"> 站台代碼 </param>
        /// <param name="moduleName"> 模組名稱 </param>
        /// <returns></returns>
        public List<PageModel> GetPageListOfModule(Guid siteID, string moduleName)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        (from obj in context.Pages
                         join moduleItem in context.Modules on obj.ModuleID equals moduleItem.ID
                         where
                            moduleItem.Name == moduleName &&
                            obj.SiteID == siteID &&
                            obj.IsEnable == true
                         select obj);

                    var list =
                       (from obj in query
                        orderby obj.SortNo
                        select
                            new PageModel()
                            {
                                ID = obj.ID,
                                SiteID = obj.SiteID,
                                ParentID = obj.ParentID,
                                Name = obj.Name,
                                PageTitle = obj.Description,
                                MenuType = obj.MenuType,
                                OuterLink = obj.Linkurl,
                                ModuleID = obj.ModuleID,
                                PageIcon = obj.PageIcon,
                                SortNo = obj.SortNo,
                                IsEnable = obj.IsEnable,
                                CreateUser = obj.CreateUser,
                                CreateDate = obj.CreateDate,
                                ModifyUser = obj.ModifyUser,
                                ModifyDate = obj.ModifyDate,
                            }
                       ).ToList();

                    return list;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                return default;
            }
        }
        #endregion

        #region CUD
        /// <summary> 建立頁面 </summary>
        /// <param name="model"></param>
        /// <param name="userID"></param>
        /// <param name="time"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void CreatePage(PageModel model, string userID, DateTime time)
        {
            if (model == null || !SiteManager.IsExist(model.SiteID))
                throw new ArgumentNullException($"Site doesn't exists: [Site: {model?.SiteID}]");

            // 若選了系統模組卻沒有選哪個模組
            if (model.MenuType == 2 && model.ModuleID == null)
                throw new ArgumentNullException($"Module doesn't exists.");

            if (IsExist(model))
                throw new ArgumentException($"已存在相同名稱的頁面: [Name: {model.Name}]");

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel = new Page()
                    {
                        SiteID = model.SiteID,
                        ParentID = model.ParentID,
                        Name = model.Name,
                        Description = model.PageTitle,
                        MenuType = model.MenuType,
                        Linkurl = model.OuterLink ?? string.Empty,
                        ModuleID = model.ModuleID,
                        PageIcon = model.PageIcon,
                        SortNo = model.SortNo,
                        IsEnable = true,
                        CreateUser = userID,
                        CreateDate = time,
                        ModifyUser = userID,
                        ModifyDate = time,
                    };

                    dbModel.ID = Guid.NewGuid();

                    context.Pages.Add(dbModel);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> 修改頁面資料
        /// <para> 站台不因此變更 </para>
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userID"></param>
        /// <param name="time"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void ModifyPage(PageModel model, string userID, DateTime time)
        {
            if (model == null || !SiteManager.IsExist(model.SiteID))
                throw new ArgumentNullException($"Site doesn't exists: [Site: {model?.SiteID}]");

            if (IsExist(model.SiteID, model.Name, model.ID))
                throw new ArgumentException($"已存在相同名稱的頁面: [Name: {model.Name}]");

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel = context.Pages.Where(obj => obj.ID == model.ID).FirstOrDefault();

                    if (dbModel == null)
                        throw new ArgumentNullException($"Page exists: [Page: {model.ID}]");

                    if (dbModel.ID == model.ParentID)
                        throw new ArgumentNullException("Can't select own folder");

                    dbModel.ParentID = model.ParentID;
                    dbModel.Name = model.Name;
                    dbModel.Description = model.PageTitle;
                    dbModel.MenuType = model.MenuType;
                    dbModel.Linkurl = model.OuterLink ?? string.Empty;
                    dbModel.SortNo = model.SortNo;
                    dbModel.IsEnable = true;
                    dbModel.ModifyUser = userID;
                    dbModel.ModifyDate = time;

                    if (model.ModuleID != null)
                        dbModel.ModuleID = model.ModuleID;
                    else
                        dbModel.ModuleID = dbModel.ModuleID;

                    if (model.PageIcon != null)
                        dbModel.PageIcon = model.PageIcon;
                    else
                        dbModel.PageIcon = dbModel.PageIcon;

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> 刪除頁面 </summary>
        /// <param name="pageID"></param>
        /// <param name="userID"></param>
        /// <param name="time"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void DeletePage(Guid pageID, string userID, DateTime time)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var hasChild = this.HasChild(context, pageID);
                    if (hasChild)
                        throw new ArgumentNullException("Can't delete when page has child pages.");


                    // 移除頁面角色授權
                    var pageRoles = context.PageRoles.Where(obj => obj.MenuID == pageID).ToList();

                    // 移除頁面本身
                    var dbModel = context.Pages.Where(obj => obj.ID == pageID).FirstOrDefault();
                    if (dbModel == null)
                        return;

                    context.PageRoles.RemoveRange(pageRoles);
                    context.Pages.Remove(dbModel);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                throw;
            }
        }
        #endregion

        #region Private Methods
        private bool IsExist(PageModel model)
        {
            return IsExist(model.SiteID, model.Name);
        }

        private bool IsExist(Guid siteID, string pageName)
        {
            if (this.GetPage(siteID, pageName, null) != null)
                return true;
            else
                return false;
        }

        private bool IsExist(Guid siteID, string pageName, Guid myID)
        {
            if (this.GetPage(siteID, pageName, myID) != null)
                return true;
            else
                return false;
        }

        /// <summary> 是否有子項目 </summary>
        /// <param name="context"></param>
        /// <param name="pageID"></param>
        /// <returns></returns>
        private bool HasChild(PlatformContextModel context, Guid pageID)
        {
            var query =
                from obj in context.Pages
                where
                    obj.ParentID == pageID
                select obj;

            var result = query.Any();
            return result;
        }
        #endregion
    }
}
