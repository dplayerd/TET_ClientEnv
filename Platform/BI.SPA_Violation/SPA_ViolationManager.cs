using BI.Shared;
using BI.Shared.Extensions;
using BI.Shared.Utils;
using BI.SPA_Violation.Enums;
using BI.SPA_Violation.Models;
using BI.SPA_Violation.Models.Exporting;
using BI.SPA_Violation.Validators;
using Platform.AbstractionClass;
using Platform.Auth;
using Platform.FileSystem;
using Platform.Infra;
using Platform.LogService;
using Platform.Messages;
using Platform.ORM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Xml.Linq;

namespace BI.SPA_Violation
{
    public class SPA_ViolationManager
    {
        private Logger _logger = new Logger();
        private UserManager _userMgr = new UserManager();

        #region Read

        /// <summary> 取得 供應商SPA違規紀錄資料維護 清單 </summary>
        /// <param name="periodText">評鑑期間(模糊搜尋)</param>
        /// <param name="approvestatusText">評鑑期間(模糊搜尋)</param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<SPA_ViolationModel> GetList(string period, string[] approveStatus, string userID, DateTime cDate, Pager pager)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var baseQuery =
                        from item in context.TET_SPA_Violation
                        select item;

                    //--- 組合過濾條件 ---

                    if (!string.IsNullOrWhiteSpace(period))
                        baseQuery = baseQuery.Where(obj => obj.Period.Contains(period));

                    if (approveStatus != null && approveStatus.Length > 0)
                    {
                        if (approveStatus.Contains(string.Empty))
                            baseQuery = baseQuery.Where(obj => obj.ApproveStatus == null || approveStatus.Contains(obj.ApproveStatus));
                        else
                            baseQuery = baseQuery.Where(obj => approveStatus.Contains(obj.ApproveStatus));
                    }

                    var query =
                        from item in baseQuery
                        orderby item.Period descending
                        select new SPA_ViolationModel
                        {
                            ID = item.ID,
                            Period = item.Period,
                            ApproveStatus = item.ApproveStatus,
                            CreateUser = item.CreateUser,
                            CreateDate = item.CreateDate,
                            ModifyUser = item.ModifyUser,
                            ModifyDate = item.ModifyDate,
                        };

                    query = query.OrderByDescending(obj => obj.CreateDate);
                    var list = query.ProcessPager(pager).ToList();

                    return list;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                return default;
            }
        }

        /// <summary>
        /// 取得 違規紀錄資料維護 清單
        /// </summary>
        /// <param name="id">Key</param>
        /// <returns></returns>
        public SPA_ViolationModel GetOne(Guid id)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in context.TET_SPA_Violation
                        where item.ID == id
                        select new SPA_ViolationModel
                        {
                            ID = item.ID,
                            Period = item.Period,
                            ApproveStatus = item.ApproveStatus,
                            CreateUser = item.CreateUser,
                            CreateDate = item.CreateDate,
                            ModifyUser = item.ModifyUser,
                            ModifyDate = item.ModifyDate,
                        };

                    var result = query.FirstOrDefault();

                    if (result != null)
                    {
                        // 查詢明細
                        var detailQuery =
                            from item in context.TET_SPA_ViolationDetail
                            where item.ViolationID == id
                            orderby item.ModifyDate descending
                            select new SPA_ViolationDetailModel
                            {
                                ID = item.ID,
                                ViolationID = item.ViolationID,
                                Date = item.Date,
                                BelongTo = item.BelongTo,
                                BU = item.BU,
                                AssessmentItem = item.AssessmentItem,
                                MiddleCategory = item.MiddleCategory,
                                SmallCategory = item.SmallCategory,
                                CustomerName = item.CustomerName,
                                CustomerPlant = item.CustomerPlant,
                                CustomerDetail = item.CustomerDetail,
                                Description = item.Description,
                                CreateUser = item.CreateUser,
                                CreateDate = item.CreateDate,
                                ModifyUser = item.ModifyUser,
                                ModifyDate = item.ModifyDate,
                            };

                        var detailList = detailQuery.ToList();
                        result.DetailList = detailList;

                        // 查詢附件
                        var detailIdList = detailList.Select(obj => obj.ID);
                        var attechQuery =
                            from item in context.TET_SPA_ViolationAttachments
                            where detailIdList.Contains(item.VDetailID)
                            select new SPA_ViolationAttachmentModel
                            {
                                ID = item.ID,
                                VDetailID = item.VDetailID,
                                FilePath = item.FilePath,
                                FileName = item.FileName,
                                OrgFileName = item.OrgFileName,
                                FileExtension = item.FileExtension,
                                FileSize = item.FileSize,
                                CreateUser = item.CreateUser,
                                CreateDate = item.CreateDate,
                                ModifyUser = item.ModifyUser,
                                ModifyDate = item.ModifyDate,
                            };

                        var attachList = attechQuery.ToList();
                        foreach (var item in result.DetailList)
                        {
                            var detailAttachList = attachList.Where(obj => obj.VDetailID == item.ID).ToList();
                            item.AttachmentList = detailAttachList;
                        }

                        var approvalQuery =
                              from item in context.TET_SPA_ViolationApproval
                              where
                                  item.ViolationID == id &&
                                  item.Result != null
                              orderby item.CreateDate, item.ModifyDate
                              select
                                  new SPA_ViolationApprovalModel()
                                  {
                                      ID = item.ID,
                                      ViolationID = item.ViolationID,
                                      Type = item.Type,
                                      Description = item.Description,
                                      Level = item.Level,
                                      Approver = item.Approver,
                                      Result = item.Result,
                                      Comment = item.Comment,
                                      CreateUser = item.CreateUser,
                                      CreateDate = item.CreateDate,
                                      ModifyUser = item.ModifyUser,
                                      ModifyDate = item.ModifyDate,
                                  };

                        result.ApprovalList = approvalQuery.ToList();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> 查詢附件 </summary>
        /// <param name="id"> 附件 Id </param>
        /// <returns></returns>
        public SPA_ViolationAttachmentModel GetAttachment(Guid id)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    // 查詢附件
                    var query =
                        from item in context.TET_SPA_ViolationAttachments
                        where item.ID == id
                        select new SPA_ViolationAttachmentModel
                        {
                            ID = item.ID,
                            VDetailID = item.VDetailID,
                            FilePath = item.FilePath,
                            FileName = item.FileName,
                            OrgFileName = item.OrgFileName,
                            FileExtension = item.FileExtension,
                            FileSize = item.FileSize,
                            CreateUser = item.CreateUser,
                            CreateDate = item.CreateDate,
                            ModifyUser = item.ModifyUser,
                            ModifyDate = item.ModifyDate,
                        };

                    var reesult = query.FirstOrDefault();
                    return reesult;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> 新增時檢查，同一評鑑期間只能有一筆資料 </summary>
        /// <param name="period"> 評鑑期間 </param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public bool HasSamePeriod(string period, string userID, DateTime cDate)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query = context.TET_SPA_Violation.Where(obj => obj.Period == period);

                    // 同一評鑑期間只能有一筆資料
                    if (!query.Any())
                        return false;

                    return true;
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

        /// <summary> 新增 供應商SPA違規紀錄資料維護 </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public Guid Create(SPA_ViolationModel model, string userID, DateTime cDate)
        {
            if (model == null)
                throw new ArgumentNullException("Supplier is required.");

            //--- 先檢查是否能通過商業邏輯 ---
            List<string> tempMsgList;
            List<string> msgList = new List<string>();

            var validResult = SPA_ViolationValidator.Valid(model, out tempMsgList);

            if (!validResult)
                msgList.AddRange(tempMsgList);
            var validDetailResult = SPA_ViolationDetailValidator.Valid(model.DetailList, out tempMsgList);
            if (!validResult)
                msgList.AddRange(tempMsgList);

            if (!validResult || !validDetailResult)
                throw new ArgumentException(string.Join(Environment.NewLine, msgList));
            //--- 先檢查是否能通過商業邏輯 ---

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    // 同一填寫者，同一評鑑期間只能有一筆資料
                    if (this.HasSamePeriod(context, model, userID, cDate))
                        throw new ArgumentException("同一評鑑期間只能有一筆資料");

                    var entity = new TET_SPA_Violation()
                    {
                        ID = Guid.NewGuid(),
                        Period = model.Period,
                        ApproveStatus = null,
                        CreateUser = userID,
                        CreateDate = cDate,
                        ModifyUser = userID,
                        ModifyDate = cDate,
                    };
                    context.TET_SPA_Violation.Add(entity);

                    foreach (var item in model.DetailList)
                    {
                        var detailEntity = new TET_SPA_ViolationDetail()
                        {
                            ID = Guid.NewGuid(),

                            ViolationID = entity.ID,
                            Date = item.Date,
                            BelongTo = item.BelongTo,
                            BU = item.BU,
                            AssessmentItem = item.AssessmentItem,
                            MiddleCategory = item.MiddleCategory,
                            SmallCategory = item.SmallCategory,
                            CustomerName = item.CustomerName,
                            CustomerPlant = item.CustomerPlant,
                            CustomerDetail = item.CustomerDetail,
                            Description = item.Description,
                            CreateUser = userID,
                            CreateDate = cDate,
                            ModifyUser = userID,
                            ModifyDate = cDate,
                        };
                        context.TET_SPA_ViolationDetail.Add(detailEntity);

                        if (item.UploadFileList != null && item.UploadFileList.Any())
                        {
                            foreach (var fileItem in item.UploadFileList)
                            {
                                var attachmEnttity = this.WriteFile(detailEntity.ID, fileItem, userID, cDate);
                                context.TET_SPA_ViolationAttachments.Add(attachmEnttity);
                            }
                        }
                    }

                    context.SaveChanges();
                    return entity.ID;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> 修改 供應商SPA違規紀錄資料維護 </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void Modify(SPA_ViolationModel model, string userID, DateTime cDate)
        {
            if (model == null)
                throw new ArgumentNullException("Model is required.");


            //--- 先檢查是否能通過商業邏輯 ---
            List<string> tempMsgList;
            List<string> msgList = new List<string>();

            var validResult = SPA_ViolationValidator.Valid(model, out tempMsgList);

            if (!validResult)
                msgList.AddRange(tempMsgList);
            var validDetailResult = SPA_ViolationDetailValidator.Valid(model.DetailList, out tempMsgList);
            if (!validResult)
                msgList.AddRange(tempMsgList);

            if (!validResult || !validDetailResult)
                throw new ArgumentException(string.Join(Environment.NewLine, msgList));
            //--- 先檢查是否能通過商業邏輯 ---

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                    (from item in context.TET_SPA_Violation
                     where item.ID == model.ID
                     select item).FirstOrDefault();

                    if (dbModel == null)
                        throw new NullReferenceException($"{model.ID} don't exists.");

                    dbModel.Period = model.Period;
                    dbModel.ModifyUser = userID;
                    dbModel.ModifyDate = cDate;

                    // 明細清單
                    var detailList = context.TET_SPA_ViolationDetail.Where(item => item.ViolationID == model.ID).ToList();
                    var detailIdList = detailList.Select(obj => obj.ID);

                    // 附件清單
                    var attachList = context.TET_SPA_ViolationAttachments.Where(item => detailIdList.Contains(item.VDetailID)).ToList();
                    var inpDetailIdList = model.DetailList.Where(obj => obj.ID.HasValue).Select(obj => obj.ID.Value).ToList();

                    //--- 找到已移除的明細及附檔，並刪除 ---
                    var notExistDetailId = detailIdList.Except(inpDetailIdList).ToList();

                    var notExistList = detailList.Where(item => notExistDetailId.Contains(item.ID)).ToList();
                    foreach (var item in notExistList)
                    {
                        context.TET_SPA_ViolationDetail.Remove(item);

                    }
                    //context.TET_SPA_ViolationDetail.RemoveRange(notExistList);

                    var willRemoveAttachment = attachList.Where(item => notExistDetailId.Contains(item.VDetailID));

                    if (willRemoveAttachment.Any())
                    {
                        this.DropFile(willRemoveAttachment);
                        context.TET_SPA_ViolationAttachments.RemoveRange(willRemoveAttachment);
                    }
                    //--- 找到已移除的明細及附檔，並刪除 ---

                    //--- 找到已存在的明細及附檔，並更新 ---
                    foreach (var item in model.DetailList)
                    {
                        if (!item.ID.HasValue)
                            continue;

                        var orgDetail = detailList.Where(obj => obj.ID == item.ID.Value).FirstOrDefault();

                        if (orgDetail != null)
                        {
                            orgDetail.Date = item.Date;
                            orgDetail.BelongTo = item.BelongTo;
                            orgDetail.BU = item.BU;
                            orgDetail.AssessmentItem = item.AssessmentItem; 
                            orgDetail.MiddleCategory = item.MiddleCategory;
                            orgDetail.SmallCategory = item.SmallCategory;
                            orgDetail.CustomerName = item.CustomerName;
                            orgDetail.CustomerPlant = item.CustomerPlant;
                            orgDetail.CustomerDetail = item.CustomerDetail;
                            orgDetail.Description = item.Description;
                            orgDetail.ModifyUser = userID;
                            orgDetail.ModifyDate = cDate;

                            if (item.UploadFileList != null && item.UploadFileList.Any())
                            {
                                foreach (var fileItem in item.UploadFileList)
                                {
                                    var attachmEnttity = this.WriteFile(orgDetail.ID, fileItem, userID, cDate);
                                    context.TET_SPA_ViolationAttachments.Add(attachmEnttity);
                                }
                            }

                            // 移除被刪除的已附加檔案
                            var attachmentListOfThisDetail = attachList.Where(obj => obj.VDetailID == orgDetail.ID).ToList();
                            var attachmentIdList = item.AttachmentList.Select(obj => obj.ID).ToList();
                            var willRemoveAttachmentList = attachmentListOfThisDetail.Where(obj => !attachmentIdList.Contains(obj.ID)).ToList();

                            this.DropFile(willRemoveAttachmentList);
                            context.TET_SPA_ViolationAttachments.RemoveRange(willRemoveAttachmentList);
                        }
                    }
                    //--- 找到已存在的明細及附檔，並更新 ---

                    //--- 找到新增的明細及附檔，並建立 ---
                    var willCreateDetails = model.DetailList.Where(obj => !obj.ID.HasValue || Guid.Empty == obj.ID.Value).ToList();
                    foreach (var item in willCreateDetails)
                    {
                        var detailEntity = new TET_SPA_ViolationDetail()
                        {
                            ID = Guid.NewGuid(),
                            ViolationID = model.ID.Value,
                            Date = item.Date,
                            BelongTo = item.BelongTo,
                            BU = item.BU,
                            AssessmentItem = item.AssessmentItem,
                            MiddleCategory = item.MiddleCategory,
                            SmallCategory = item.SmallCategory,
                            CustomerName = item.CustomerName,
                            CustomerPlant = item.CustomerPlant,
                            CustomerDetail = item.CustomerDetail,
                            Description = item.Description,
                            CreateUser = userID,
                            CreateDate = cDate,
                            ModifyUser = userID,
                            ModifyDate = cDate,
                        };

                        context.TET_SPA_ViolationDetail.Add(detailEntity);

                        if (item.UploadFileList != null && item.UploadFileList.Any())
                        {
                            foreach (var fileItem in item.UploadFileList)
                            {
                                var attachmEnttity = this.WriteFile(detailEntity.ID, fileItem, userID, cDate);
                                context.TET_SPA_ViolationAttachments.Add(attachmEnttity);
                            }
                        }
                    }
                    //--- 找到新增的明細及附檔，並建立 ---

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }

        #endregion

        #region Exporting

        /// <summary>
        /// 取得 違規紀錄資料維護 清單
        /// </summary>
        /// <param name="period">評鑑期間</param>
        /// <param name="approveStatus">審核狀態</param>
        /// <param name="userID"> 目前登入者 </param>
        /// <param name="cDate"> 目前時間 </param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<SPA_ViolationExportModel> GetExportList(string period, string[] approveStatus, string userID, DateTime cDate)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    //----- 主表資料 -----
                    var query =
                        from item in context.TET_SPA_Violation
                        select item;


                    //--- 組合過濾條件 ---
                    if (!string.IsNullOrWhiteSpace(period))
                        query = query.Where(obj => obj.Period.Contains(period));

                    if (approveStatus != null && approveStatus.Length > 0)
                    {
                        if (approveStatus.Contains(string.Empty))
                            query = query.Where(obj => obj.ApproveStatus == null || approveStatus.Contains(obj.ApproveStatus));
                        else
                            query = query.Where(obj => approveStatus.Contains(obj.ApproveStatus));
                    }
                    //--- 組合過濾條件 ---

                    query = query.OrderByDescending(obj => obj.CreateDate);
                    var list = query.ToList();
                    //----- 主表資料 -----


                    //----- 明細資料 -----
                    var ids = list.Select(obj => obj.ID).ToList();

                    var detailQuery =
                        from item in context.TET_SPA_ViolationDetail
                        where ids.Contains(item.ViolationID)
                        select item;

                    var detailList = detailQuery.ToList();
                    //----- 明細資料 -----


                    //--- 處理輸出資料 ---
                    var retList = new List<SPA_ViolationExportModel>();

                    foreach (var mainItem in list)
                    {
                        var thisDetailList = detailList.Where(obj => obj.ViolationID == mainItem.ID).ToList();

                        // 如果沒有明細，仍產生一筆資料
                        if (!thisDetailList.Any())
                        {
                            var item = new SPA_ViolationExportModel()
                            {
                                Period = mainItem.Period,
                            };

                            // 起始結束時間
                            var periodDate = PeriodUtil.ParsePeriod(item.Period);
                            item.PeriodStart = periodDate?.StartDate?.ToString("yyyy-MM-dd");
                            item.PeriodEnd = periodDate?.EndDate?.ToString("yyyy-MM-dd");

                            retList.Add(item);
                        }
                        else
                        {
                            foreach (var detail in thisDetailList)
                            {
                                var item = new SPA_ViolationExportModel()
                                {
                                    Period = mainItem.Period,
                                    Date = detail.Date,
                                    BelongTo = detail.BelongTo,
                                    BU = detail.BU,
                                    AssessmentItem = detail.AssessmentItem,
                                    MiddleCategory = detail.MiddleCategory,
                                    SmallCategory = detail.SmallCategory,
                                    CustomerName = detail.CustomerName,
                                    CustomerPlant = detail.CustomerPlant,
                                    CustomerDetail = detail.CustomerDetail,
                                    Description = detail.Description,
                                };

                                // 起始結束時間
                                var periodDate = PeriodUtil.ParsePeriod(item.Period);
                                item.PeriodStart = periodDate?.StartDate?.ToString("yyyy-MM-dd");
                                item.PeriodEnd = periodDate?.EndDate?.ToString("yyyy-MM-dd");

                                retList.Add(item);
                            }
                        }
                    }
                    //--- 處理輸出資料 ---


                    return retList;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                return default;
            }
        }

        #endregion

        #region Private

        /// <summary> 新增時檢查，同一填寫者，同一評鑑期間只能有一筆資料 </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        private bool HasSamePeriod(PlatformContextModel context, SPA_ViolationModel model, string userID, DateTime cDate)
        {
            var query = context.TET_SPA_Violation.Where(obj => obj.Period == model.Period);

            if (model.ID.HasValue)
                query = query.Where(obj => obj.ID != model.ID.Value);

            // 同一評鑑期間只能有一筆資料
            if (!query.Any())
                return false;

            return true;
        }

        /// <summary> 寫入檔案，並回傳附件 </summary>
        /// <param name="detailId"></param>
        /// <param name="file"></param>
        /// <param name="userID"></param>
        /// <param name="cDate"></param>
        /// <returns></returns>
        private TET_SPA_ViolationAttachments WriteFile(Guid detailId, FileContent file, string userID, DateTime cDate)
        {
            // 將前端傳入的檔案寫入檔案系統及資料表
            // 計算起始路徑，如果不是上傳資料夾根目錄，要附加在最前面
            string rootFolder = MediaFileManager.GetRootFolder();
            string filePath = Path.Combine(rootFolder, ModuleConfig.FolderPath);

            if (!filePath.StartsWith(rootFolder, StringComparison.OrdinalIgnoreCase))
                filePath = Path.Combine(rootFolder, ModuleConfig.FolderPath);

            string folderPath = HostingEnvironment.MapPath("~/" + filePath);

            var newFileName = FileUtility.Upload(file, folderPath);
            var entity = new TET_SPA_ViolationAttachments()
            {
                ID = Guid.NewGuid(),
                VDetailID = detailId,
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

            return entity;
        }

        /// <summary> 刪除檔案 </summary>
        /// <param name="attachments"></param>
        private void DropFile(IEnumerable<TET_SPA_ViolationAttachments> attachments)
        {
            foreach (var item in attachments)
            {
                FileUtility.DeleteFile(item.FilePath);
            }
        }

        #endregion
    }
}
