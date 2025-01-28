using BI.Suppliers.Models;
using Platform.AbstractionClass;
using Platform.LogService;
using Platform.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using Platform.FileSystem;
using System.IO;
using System.Web.Hosting;
using BI.Suppliers.Validators;

namespace BI.Suppliers
{
    public class TET_SupplierAttachmentManager
    {
        private Logger _logger = new Logger();

        #region Read
        /// <summary>
        /// 取得 供應商附件 清單
        /// </summary>
        /// <param name="context"></param>
        /// <param name="supplierID"> 供應商 ID </param>
        /// <returns></returns>
        public List<TET_SupplierAttachmentModel> GetTET_SupplierAttachmentsList(PlatformContextModel context, Guid supplierID)
        {
            var query =
            from item in context.TET_SupplierAttachments
            where item.SupplierID == supplierID
            orderby item.CreateDate
            select
            new TET_SupplierAttachmentModel()
            {
                ID = item.ID,
                SupplierID = item.SupplierID,
                FilePath = item.FilePath,
                FileName = item.FileName,
                FileExtension = item.FileExtension,
                FileSize = item.FileSize,
                OrgFileName = item.OrgFileName,
                CreateUser = item.CreateUser,
                CreateDate = item.CreateDate,
                ModifyUser = item.ModifyUser,
                ModifyDate = item.ModifyDate,
            };

            var list = query.ToList();
            return list;
        }


        /// <summary>
        /// 取得 供應商附件 
        /// </summary>
        /// <param name="id"> 附檔 id </param>
        /// <returns></returns>
        public TET_SupplierAttachmentModel GetTET_SupplierAttachment(Guid id)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                    from item in context.TET_SupplierAttachments
                    where item.ID == id
                    orderby item.CreateDate
                    select
                    new TET_SupplierAttachmentModel()
                    {
                        ID = item.ID,
                        SupplierID = item.SupplierID,
                        FilePath = item.FilePath,
                        FileName = item.FileName,
                        OrgFileName = item.OrgFileName,
                        CreateUser = item.CreateUser,
                        CreateDate = item.CreateDate,
                        ModifyUser = item.ModifyUser,
                        ModifyDate = item.ModifyDate,
                    };

                    var result = query.FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }
        #endregion

        #region CUD
        /// <summary> 新增 供應商附件 </summary>
        /// <param name="context"> ORM 核心 </param>
        /// <param name="supplierID"> 供應商 ID </param>
        /// <param name="currentModelList"> 目前供應商附檔 List </param>
        /// <param name="fileList"> 上傳的 File List </param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        internal void WriteTET_SupplierAttachment(PlatformContextModel context, Guid supplierID, List<TET_SupplierAttachmentModel> currentModelList, List<FileContent> fileList, string userID, DateTime cDate)
        {
            if (fileList == null)
                throw new ArgumentNullException("Model is required.");

            // 新增前，先檢查是否能通過商業邏輯
            if (!SupplierAttachmentValidator.Valid(currentModelList, fileList, out List<string> msgList))
                throw new ArgumentException(string.Join(Environment.NewLine, msgList));


            // 先移除已上傳，但被前端標示為刪除的檔案
            var currentIDList = currentModelList.Select(obj => obj.ID).ToList();
            var willRemoveList =
                (from item in context.TET_SupplierAttachments
                 where
                     item.SupplierID == supplierID &&
                     !currentIDList.Contains(item.ID)
                 select item).ToList();

            foreach (var item in willRemoveList)
            {
                FileUtility.DeleteFile(item.FilePath);          // 刪除檔案
                context.TET_SupplierAttachments.Remove(item);   // 刪除資料庫內容
            }



            // 將前端傳入的檔案寫入檔案系統及資料表
            // 計算起始路徑，如果不是上傳資料夾根目錄，要附加在最前面
            string rootFolder = MediaFileManager.GetRootFolder();
            string filePath = Path.Combine(rootFolder, ModuleConfig.FolderPath);

            if (!filePath.StartsWith(rootFolder, StringComparison.OrdinalIgnoreCase))
                filePath = Path.Combine(rootFolder, ModuleConfig.FolderPath);

            string folderPath = HostingEnvironment.MapPath("~/" + filePath);

            foreach (var file in fileList)
            {
                var newFileName = FileUtility.Upload(file, folderPath);
                var entity = new TET_SupplierAttachments()
                {
                    ID = Guid.NewGuid(),
                    SupplierID = supplierID,
                    FileName = newFileName,
                    OrgFileName = file.FileName,
                    FilePath = filePath,
                    FileExtension = Path.GetExtension(file.FileName),
                    FileSize = file.ContentLength,
                    CreateUser = userID,
                    CreateDate = cDate,
                    ModifyUser = userID,
                    ModifyDate = cDate,
                };

                context.TET_SupplierAttachments.Add(entity);
            }
        }

        /// <summary> 複製 供應商附件 </summary>
        /// <param name="context"> ORM 核心 </param>
        /// <param name="supplierID"> 供應商 ID </param>
        /// <param name="currentModelList"> 目前供應商附檔 List </param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        internal void CopyTET_SupplierAttachment(PlatformContextModel context, Guid supplierID, List<TET_SupplierAttachmentModel> currentModelList, string userID, DateTime cDate)
        {
            //// 將前端傳入的檔案寫入檔案系統及資料表
            //// 計算起始路徑，如果不是上傳資料夾根目錄，要附加在最前面
            //string rootFolder = MediaFileManager.GetRootFolder();
            //string filePath = Path.Combine(rootFolder, ModuleConfig.FolderPath);

            //if (!filePath.StartsWith(rootFolder, StringComparison.OrdinalIgnoreCase))
            //    filePath = Path.Combine(rootFolder, ModuleConfig.FolderPath);

            //string folderPath = HostingEnvironment.MapPath("~/" + filePath);

            //foreach (var model in currentModelList)
            //{
            //    string orgFilePath = Path.Combine(folderPath, model.FileName);
            //    string newFileName = FileUtility.CopyAndRenameFile(orgFilePath, folderPath);
            //    var entity = new TET_SupplierAttachments()
            //    {
            //        ID = Guid.NewGuid(),
            //        SupplierID = supplierID,
            //        FileName = newFileName,
            //        OrgFileName = model.OrgFileName,
            //        FilePath = model.FilePath,
            //        FileExtension = model.FileExtension,
            //        FileSize = model.FileSize,
            //        CreateUser = userID,
            //        CreateDate = cDate,
            //        ModifyUser = userID,
            //        ModifyDate = cDate,
            //    };

            //    context.TET_SupplierAttachments.Add(entity);
            //}
        }

        /// <summary> 新增 供應商附件 </summary>
        /// <param name="context"> ORM 核心 </param>
        /// <param name="supplierID"> 供應商 ID </param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        internal void DeleteTET_SupplierAttachment(PlatformContextModel context, Guid supplierID, string userID, DateTime cDate)
        {
            var willRemoveList =
                (from item in context.TET_SupplierAttachments
                 where
                     item.SupplierID == supplierID 
                 select item).ToList();

            foreach (var item in willRemoveList)
            {
                //FileUtility.DeleteFile(item.FilePath);          // 刪除檔案
                context.TET_SupplierAttachments.Remove(item);   // 刪除資料庫內容
            }
        }
        #endregion
    }
}