using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using Platform.AbstractionClass;
using Platform.LogService;
using Platform.ORM;

namespace Platform.FileSystem
{
    public class MediaFileManager
    {
        private Logger _logger = new Logger();

        #region 前台用查詢
        /// <summary> 取得多媒體檔案 </summary>
        /// <param name="ID"> 主鍵 </param>
        /// <returns></returns>
        public MediaFileModel GetMediaFile(Guid ID)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in context.MediaFiles
                        where
                            item.ID == ID &&
                            (item.DeleteDate == null && item.DeleteUser == null)
                        select new MediaFileModel()
                        {
                            ID = item.ID,
                            ModuleName = item.ModuleName,
                            ModuleID = item.ModuleID,
                            Purpose = item.Purpose,
                            FilePath = item.FilePath,
                            OrgFileName = item.OrgFileName,
                            OutputFileName = item.OutputFileName,
                            MimeType = item.MimeType,
                            RequireAuth = item.RequireAuth,
                            IsEnable = item.IsEnable,
                        };

                    var result = query.FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> 取得多媒體檔案 </summary>
        /// <param name="moduleName"> 模組名稱 </param>
        /// <param name="moduleID"> 模組資料主鍵 </param>
        /// <param name="purpose"> 用途 </param>
        /// <returns></returns>
        public MediaFileModel GetMediaFile(string moduleName, string moduleID, string purpose = MediaFileModel.DefaultPurpose)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in context.MediaFiles
                        where
                            item.ModuleName == moduleName &&
                            item.ModuleID == moduleID &&
                            item.Purpose == purpose &&
                            (item.DeleteDate == null && item.DeleteUser == null)
                        select new MediaFileModel()
                        {
                            ID = item.ID,
                            ModuleName = item.ModuleName,
                            ModuleID = item.ModuleID,
                            Purpose = item.Purpose,
                            FilePath = item.FilePath,
                            OrgFileName = item.OrgFileName,
                            OutputFileName = item.OutputFileName,
                            MimeType = item.MimeType,
                            RequireAuth = item.RequireAuth,
                            IsEnable = item.IsEnable,
                        };

                    var result = query.FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> 取得多媒體檔案清單 </summary>
        /// <param name="moduleName"> 模組名稱 </param>
        /// <param name="moduleID"> 模組資料主鍵 </param>
        /// <param name="purpose"> 用途 </param>
        /// <returns></returns>
        public List<MediaFileModel> GetMediaFileList(string moduleName, string moduleID, string purpose = MediaFileModel.DefaultPurpose)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in context.MediaFiles
                        where
                            item.ModuleName == moduleName &&
                            item.ModuleID == moduleID &&
                            item.Purpose == purpose &&
                            (item.DeleteDate == null && item.DeleteUser == null)
                        select new MediaFileModel()
                        {
                            ID = item.ID,
                            ModuleName = item.ModuleName,
                            ModuleID = item.ModuleID,
                            Purpose = item.Purpose,
                            FilePath = item.FilePath,
                            OrgFileName = item.OrgFileName,
                            OutputFileName = item.OutputFileName,
                            MimeType = item.MimeType,
                            RequireAuth = item.RequireAuth,
                            IsEnable = item.IsEnable,
                        };

                    var result = query.ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> 取得檔案的完整路徑 </summary>
        /// <param name="model"> 多媒體檔案 </param>
        /// <returns></returns>
        public string GetMediaFilePath(MediaFileModel model)
        {
            if (model == null)
                throw new ArgumentNullException("Model is required.");

            // 計算起始路徑，如果不是上傳資料夾根目錄，要附加在最前面
            string rootFolder = GetRootFolder();

            if (!model.FilePath.StartsWith(rootFolder, StringComparison.OrdinalIgnoreCase))
                model.FilePath = Path.Combine(rootFolder, model.FilePath);


            // 計算上傳資料夾絕對路徑，並上傳
            // 取得新檔名後，再寫回 MediaFile 的路徑中
            string folderPath = HostingEnvironment.MapPath("~/" + model.FilePath);
            return folderPath;
        }
        #endregion

        #region 管理用查詢
        /// <summary> 取得多媒體資訊 </summary>
        /// <param name="context"></param>
        /// <param name="ID"> 代碼 </param>
        /// <returns></returns>
        public MediaFile GetAdminMediaFile(PlatformContextModel context, Guid ID)
        {
            var query =
                from item in context.MediaFiles
                where
                    item.ID == ID &&
                    (item.DeleteDate == null && item.DeleteUser == null)
                select item;

            var result = query.FirstOrDefault();
            return result;
        }

        /// <summary> 取得多媒體資訊 </summary>
        /// <param name="context"></param>
        /// <param name="moduleName"> 模組名稱 </param>
        /// <param name="moduleID"> 模組資料主鍵 </param>
        /// <param name="purpose"> 用途 </param>
        /// <returns></returns>
        public MediaFile GetAdminMediaFile(PlatformContextModel context, string moduleName, string moduleID, string purpose = MediaFileModel.DefaultPurpose)
        {
            var query =
                from item in context.MediaFiles
                where
                    item.ModuleName == moduleName &&
                    item.ModuleID == moduleID &&
                    item.Purpose == purpose &&
                    (item.DeleteDate == null && item.DeleteUser == null)
                select item;

            var result = query.FirstOrDefault();
            return result;
        }
        #endregion

        /// <summary> 刪除資料及檔案 </summary>
        /// <param name="context"></param>
        /// <param name="mediaFileID"></param>
        /// <param name="userID"></param>
        /// <param name="time"></param>
        public void DeleteDataAndFile(PlatformContextModel context, Guid mediaFileID, string userID, DateTime time)
        {
            var mediaFile = this.GetAdminMediaFile(context, mediaFileID);

            if (mediaFile != null)
            {
                string filePath = HostingEnvironment.MapPath("~/" + mediaFile.FilePath);
                FileUtility.DeleteFile(filePath);

                mediaFile.DeleteUser = userID;
                mediaFile.DeleteDate = time;
            }
        }

        /// <summary> 上傳並存檔 </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="fileContent"></param>
        /// <param name="userID"></param>
        /// <param name="time"></param>
        public Guid UploadAndCreate(PlatformContextModel context, MediaFileModel model, FileContent fileContent, string userID, DateTime time)
        {
            // 計算起始路徑，如果不是上傳資料夾根目錄，要附加在最前面
            string rootFolder = GetRootFolder();

            if (!model.FilePath.StartsWith(rootFolder, StringComparison.OrdinalIgnoreCase))
                model.FilePath = Path.Combine(rootFolder, model.FilePath);

            // 計算上傳資料夾絕對路徑，並上傳
            // 取得新檔名後，再寫回 MediaFile 的路徑中
            string folderPath = HostingEnvironment.MapPath("~/" + model.FilePath);
            string newFileName = FileUtility.Upload(fileContent, folderPath);
            model.FilePath = Path.Combine(model.FilePath, newFileName);
            model.FilePath = model.FilePath.Replace("\\", "/").Replace("//", "/");

            // 如果目的為空，改為指定字串
            if (string.IsNullOrWhiteSpace(model.Purpose))
                model.Purpose = MediaFileModel.DefaultPurpose;

            // 放到待存區
            var entity = new MediaFile()
            {
                ID = Guid.NewGuid(),
                ModuleName = model.ModuleName,
                ModuleID = model.ModuleID,
                Purpose = model.Purpose,
                FilePath = model.FilePath,
                OrgFileName = model.OrgFileName,
                OutputFileName = model.OutputFileName,
                MimeType = model.MimeType,
                RequireAuth = model.RequireAuth,

                IsEnable = true,
                CreateUser = userID,
                CreateDate = time,
            };

            context.MediaFiles.Add(entity);
            return entity.ID;
        }

        /// <summary> 取得檔案上傳根目錄 </summary>
        /// <returns></returns>
        public static string GetRootFolder()
        {
            return ConfigurationManager.AppSettings["FileUploadFolder"];
        }
    }
}
