using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Platform.ORM;
using Platform.Portal.Models;
using Platform.LogService;
using Platform.AbstractionClass;
using Platform.FileSystem;

namespace Platform.Portal
{
    public class SiteManager
    {
        private static Logger _logger = new Logger();

        /// <summary> 讀取站台清單 </summary>
        /// <returns></returns>
        public static List<SiteModel> GetSiteList()
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var list =
                        context.Sites.Select(
                            obj =>
                            new SiteModel()
                            {
                                ID = obj.ID,
                                Name = obj.Name,
                                Title = obj.Title,
                                HeaderText = obj.HeaderText,
                                FooterText = obj.FooterText,
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

        /// <summary> 是否存在同樣 ID 的站台 </summary>
        /// <param name="siteID"></param>
        /// <returns></returns>
        public static bool IsExist(Guid siteID)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var result =
                        (from obj in context.Sites
                         where obj.ID == siteID
                         select obj).Any();

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                return default;
            }

        }

        /// <summary> 是否存在同樣名稱的站台 </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsExist(string siteName)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var result =
                        (from obj in context.Sites
                         where obj.Name == siteName
                         select obj).Any();

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                return default;
            }
        }

        /// <summary> 取得站台資訊 </summary>
        /// <param name="siteID"></param>
        /// <returns></returns>
        public static SiteModel GetSite(Guid siteID)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var mediaFileQuery =
                        from obj in context.MediaFiles
                        where
                            obj.ModuleID == siteID.ToString() &&
                            obj.ModuleName == ModuleConfig.ModuleName_Site &&
                            obj.Purpose == MediaFileModel.DefaultPurpose &&
                            (obj.DeleteDate == null && obj.DeleteUser == null)
                        select obj;

                    var query =
                        from siteItem in context.Sites
                        join mediaFileItem in mediaFileQuery
                            on siteItem.ID.ToString() equals mediaFileItem.ModuleID into temp
                        where siteItem.ID == siteID
                        from tempItem in temp.DefaultIfEmpty()
                        select
                            new SiteModel()
                            {
                                ID = siteItem.ID,
                                Name = siteItem.Name,
                                Title = siteItem.Title,
                                HeaderText = siteItem.HeaderText,
                                FooterText = siteItem.FooterText,
                                MediaFileID = siteItem.MediaFileID,
                                ImageUrl = tempItem.FilePath
                            };

                    var retObj = query.FirstOrDefault();
                    return retObj;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                return default;
            }
        }

        /// <summary> 修改站台資訊 </summary>
        /// <param name="siteModel"></param>
        /// <param name="fileContent"></param>
        /// <param name="userID"></param>
        public void ModifySiteInfo(SiteModel siteModel, FileContent fileContent, string userID)
        {
            var cDate = DateTime.Now;

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    //----- 查詢站台 -----
                    var query =
                        (from obj in context.Sites
                         where obj.ID == siteModel.ID
                         select obj);

                    var siteEntity = query.FirstOrDefault();
                    if (siteEntity == null)
                        throw new NullReferenceException($"Site doesn't exists[{siteModel.ID}]");
                    //----- 查詢站台 -----


                    //----- 儲存圖片以及資料庫 -----
                    Guid? mediaFileID = siteEntity.MediaFileID;

                    if (fileContent != null)
                    {
                        var fileManager = new MediaFileManager();

                        // 取得站台舊 logo ，如果存在，刪除之
                        var oldFile = fileManager.GetAdminMediaFile(context, ModuleConfig.ModuleName_Site, siteModel.ID.ToString());
                        if (oldFile != null)
                            fileManager.DeleteDataAndFile(context, oldFile.ID, userID, cDate);

                        // 資料庫
                        MediaFileModel mediaFileModel = new MediaFileModel()
                        {
                            ModuleName = ModuleConfig.ModuleName_Site,
                            ModuleID = siteModel.ID.ToString(),
                            MimeType = fileContent.MimeType,
                            FilePath = ModuleConfig.SiteLogoFolderPath,
                            RequireAuth = false,
                            OutputFileName = fileContent.FileName,
                            OrgFileName = fileContent.FileName,
                        };

                        // 儲存圖片以及資料庫
                        mediaFileID = fileManager.UploadAndCreate(context, mediaFileModel, fileContent, userID, cDate);
                    }
                    //----- 儲存圖片以及資料庫 -----

                    //----- 修改 Site -----
                    siteEntity.Name = siteModel.Name;
                    siteEntity.Title = siteModel.Title;
                    siteEntity.MediaFileID = mediaFileID;
                    siteEntity.HeaderText = siteModel.HeaderText;
                    siteEntity.FooterText = siteModel.FooterText;
                    //----- 修改 Site -----

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                throw;
            }
        }
    }
}
