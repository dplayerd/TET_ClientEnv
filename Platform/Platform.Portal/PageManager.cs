using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Platform.AbstractionClass;
using Platform.ORM;
using Platform.Portal.Models;
using Platform.Infra;
using Platform.LogService;
using Platform.FileSystem;

namespace Platform.Portal
{
    public class PageManager
    {
        private Logger _logger = new Logger();
        MediaFileManager fileManager = new MediaFileManager();

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

                    var mediaFileQuery =
                        from obj in context.MediaFiles
                        where
                            obj.ModuleName == ModuleConfig.ModuleName_Page &&
                            obj.Purpose == MediaFileModel.DefaultPurpose &&
                            (obj.DeleteDate == null && obj.DeleteUser == null)
                        select new MediaFileModel()
                        {
                            ID = obj.ID,
                            ModuleName = obj.ModuleName,
                            ModuleID = obj.ModuleID,
                            Purpose = obj.Purpose,
                            FilePath = obj.FilePath,
                            OrgFileName = obj.OrgFileName,
                            OutputFileName = obj.OutputFileName,
                            MimeType = obj.MimeType,
                            RequireAuth = obj.RequireAuth,
                            IsEnable = obj.IsEnable,
                        };


                    var list =
                       (from item in query
                        orderby item.SortNo
                        join mediaFileItem in mediaFileQuery
                            on item.ID.ToString() equals mediaFileItem.ModuleID
                            into temp
                        from tempItem in temp.DefaultIfEmpty()
                        select
                            new PageModel()
                            {
                                ID = item.ID,
                                SiteID = item.SiteID,
                                ParentID = item.ParentID,
                                Name = item.Name,
                                PageTitle = item.Description,
                                MenuType = item.MenuType,
                                OuterLink = item.Linkurl,
                                ModuleID = item.ModuleID,
                                PageIcon = item.PageIcon,
                                SortNo = item.SortNo,
                                IsEnable = item.IsEnable,
                                CreateUser = item.CreateUser,
                                CreateDate = item.CreateDate,
                                ModifyUser = item.ModifyUser,
                                ModifyDate = item.ModifyDate,

                                Attachment = temp.FirstOrDefault()
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
                    var mediaFileQuery =
                        from obj in context.MediaFiles
                        where
                            obj.ModuleID == id.ToString() &&
                            obj.ModuleName == ModuleConfig.ModuleName_Page &&
                            obj.Purpose == MediaFileModel.DefaultPurpose &&
                            (obj.DeleteDate == null && obj.DeleteUser == null)
                        select new MediaFileModel()
                        {
                            ID = obj.ID,
                            ModuleName = obj.ModuleName,
                            ModuleID = obj.ModuleID,
                            Purpose = obj.Purpose,
                            FilePath = obj.FilePath,
                            OrgFileName = obj.OrgFileName,
                            OutputFileName = obj.OutputFileName,
                            MimeType = obj.MimeType,
                            RequireAuth = obj.RequireAuth,
                            IsEnable = obj.IsEnable,
                        };


                    var query =
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
                        );

                    var mediaFile = mediaFileQuery.FirstOrDefault();
                    var result = query.FirstOrDefault();
                    if (result != null)
                        result.Attachment = mediaFile;
                    
                    return result;
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
        /// <param name="cUser"></param>
        /// <param name="cDate"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void CreatePage(PageModel model, string cUser, DateTime cDate)
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
                        CreateUser = cUser,
                        CreateDate = cDate,
                        ModifyUser = cUser,
                        ModifyDate = cDate,
                    };

                    dbModel.ID = Guid.NewGuid();

                    // 取得使用者舊頭像，如果舊頭像存在，刪除之
                    var oldFile = fileManager.GetAdminMediaFile(context, ModuleConfig.ModuleName_Page, dbModel.ID.ToString());
                    if (oldFile != null)
                        fileManager.DeleteDataAndFile(context, oldFile.ID, cUser, cDate);

                    // 儲存資料庫
                    MediaFileModel mediaFileModel = new MediaFileModel()
                    {
                        ModuleName = ModuleConfig.ModuleName_Page,
                        ModuleID = dbModel.ID.ToString(),
                        MimeType = model.UploadFile.MimeType,
                        FilePath = ModuleConfig.PageFileFolderPath,
                        RequireAuth = false,
                        OutputFileName = model.UploadFile.FileName,
                        OrgFileName = model.UploadFile.FileName,
                    };

                    // 存檔
                    fileManager.UploadAndCreate(context, mediaFileModel, model.UploadFile, cUser, cDate);




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
        /// <param name="clearImage"></param>
        /// <param name="cUser"></param>
        /// <param name="cDate"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void ModifyPage(PageModel model, bool clearImage, string cUser, DateTime cDate)
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
                    dbModel.ModifyUser = cUser;
                    dbModel.ModifyDate = cDate;

                    if (model.ModuleID != null)
                        dbModel.ModuleID = model.ModuleID;
                    else
                        dbModel.ModuleID = dbModel.ModuleID;

                    if (model.PageIcon != null)
                        dbModel.PageIcon = model.PageIcon;
                    else
                        dbModel.PageIcon = dbModel.PageIcon;


                    // 取得使用者舊頭像，如果舊頭像存在，刪除之
                    var oldFile = fileManager.GetAdminMediaFile(context, ModuleConfig.ModuleName_Page, dbModel.ID.ToString());
                    if (oldFile != null && (clearImage || model.UploadFile != null))
                        fileManager.DeleteDataAndFile(context, oldFile.ID, cUser, cDate);

                    if (model.UploadFile != null)
                    {
                        // 儲存資料庫
                        MediaFileModel mediaFileModel = new MediaFileModel()
                        {
                            ModuleName = ModuleConfig.ModuleName_Page,
                            ModuleID = dbModel.ID.ToString(),
                            MimeType = model.UploadFile.MimeType,
                            FilePath = ModuleConfig.PageFileFolderPath,
                            RequireAuth = false,
                            OutputFileName = model.UploadFile.FileName,
                            OrgFileName = model.UploadFile.FileName,
                        };

                        // 存檔
                        fileManager.UploadAndCreate(context, mediaFileModel, model.UploadFile, cUser, cDate);
                    }

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
        /// <summary> 依站台及頁面名稱查詢頁面 </summary>
        /// <param name="siteID"></param>
        /// <param name="pageName"></param>
        /// <param name="excludePageID"> 要排除的頁面 ID </param>
        /// <returns></returns>
        private PageModel GetPage(Guid siteID, string pageName, Guid? excludePageID)
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
