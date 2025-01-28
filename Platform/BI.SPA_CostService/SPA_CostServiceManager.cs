using BI.SPA_CostService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Platform.AbstractionClass;
using Platform.Infra;
using Platform.LogService;
using Platform.ORM;
using Platform.Auth;
using BI.SPA_CostService.Validators;
using System.Xml.Linq;
using Platform.FileSystem;
using BI.Shared.Extensions;
using System.IO;
using System.Web.Hosting;
using BI.SPA_CostService.Models.Exporting;
using BI.SPA_ApproverSetup.Enums;
using System.Web.UI.WebControls;
using BI.Shared.Utils;

namespace BI.SPA_CostService
{
    public class SPA_CostServiceManager
    {
        private const string _prevText = "前期匯入";
        private const string _isEvaluateText = "評鑑";

        private Logger _logger = new Logger();
        private UserManager _userMgr = new UserManager();

        #region Read
        /// <summary>
        /// 取得 Cost&Service資料維護 清單
        /// </summary>
        /// <param name="period">評鑑期間</param>
        /// <param name="approveStatus">審核狀態</param>
        /// <param name="userID"> 目前登入者 </param>
        /// <param name="cDate"> 目前時間 </param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<SPA_CostServiceModel> GetList(string period, string[] approveStatus, string userID, DateTime cDate, Pager pager)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var orgQuery =
                        from item in context.TET_SPA_CostService
                        select item;


                    //--- 組合過濾條件 ---
                    if (!string.IsNullOrWhiteSpace(period))
                        orgQuery = orgQuery.Where(obj => obj.Period.Contains(period));

                    if (approveStatus != null && approveStatus.Length > 0)
                    {
                        if (approveStatus.Contains(string.Empty))
                            orgQuery = orgQuery.Where(obj => obj.ApproveStatus == null || approveStatus.Contains(obj.ApproveStatus));
                        else
                            orgQuery = orgQuery.Where(obj => approveStatus.Contains(obj.ApproveStatus));
                    }
                    //--- 組合過濾條件 ---

                    var query =
                        from item in orgQuery
                        select
                            new SPA_CostServiceModel()
                            {
                                ID = item.ID,
                                Period = item.Period,
                                Filler = item.Filler,
                                ApproveStatus = item.ApproveStatus,
                                CreateUser = item.CreateUser,
                                CreateDate = item.CreateDate,
                                ModifyUser = item.ModifyUser,
                                ModifyDate = item.ModifyDate,
                            };

                    query = query.OrderByDescending(obj => obj.CreateDate);
                    var list = query.ProcessPager(pager).ToList();

                    //--- 處理輸出資料 ---
                    var empList = this._userMgr.GetUserList(list.Select(obj => obj.Filler));

                    foreach (var item in list)
                    {
                        // 填寫者
                        if (item.Filler != null)
                            item.FillerFullName = empList.Where(obj => item.Filler == obj.UserID).Select(obj => $"{obj.FirstNameEN} {obj.LastNameEN}({obj.EmpID})").FirstOrDefault();
                    }
                    //--- 處理輸出資料 ---

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
        /// 取得 Cost&Service資料維護 清單
        /// </summary>
        /// <param name="id">Key</param>
        /// <returns></returns>
        public SPA_CostServiceModel GetOne(Guid id)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in context.TET_SPA_CostService
                        where item.ID == id
                        select new SPA_CostServiceModel
                        {
                            ID = item.ID,
                            Period = item.Period,
                            Filler = item.Filler,
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
                            from item in context.TET_SPA_CostServiceDetail
                            where item.CSID == id
                            orderby item.IsEvaluate descending, item.ServiceFor, item.BU, item.AssessmentItem, item.POSource
                            select new SPA_CostServiceDetailModel
                            {
                                ID = item.ID,
                                CSID = item.CSID,
                                Source = item.Source,
                                IsEvaluate = item.IsEvaluate,
                                BU = item.BU,
                                ServiceFor = item.ServiceFor,
                                BelongTo = item.BelongTo,
                                POSource = item.POSource,
                                AssessmentItem = item.AssessmentItem,
                                PriceDeflator = item.PriceDeflator,
                                PaymentTerm = item.PaymentTerm,
                                Cooperation = item.Cooperation,
                                Advantage = item.Advantage,
                                Improved = item.Improved,
                                Comment = item.Comment,
                                Remark = item.Remark,
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
                            from item in context.TET_SPA_CostServiceAttachments
                            where detailIdList.Contains(item.CSDetailID)
                            select new SPA_CostServiceAttachmentModel
                            {
                                ID = item.ID,
                                CSDetailID = item.CSDetailID,
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
                            var detailAttachList = attachList.Where(obj => obj.CSDetailID == item.ID).ToList();
                            item.AttachmentList = detailAttachList;
                        }

                        var approvalQuery =
                              from item in context.TET_SPA_CostServiceApproval
                              where
                                  item.CSID == id &&
                                  item.Result != null
                              orderby item.CreateDate, item.ModifyDate
                              select
                                  new SPA_CostServiceApprovalModel()
                                  {
                                      ID = item.ID,
                                      CSID = item.CSID,
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

        /// <summary> 取得前一期的明細 </summary>
        /// <param name="period"> 評鑑期間 </param>
        /// <param name="serviceForArr"> 服務對象 </param>
        /// <param name="userID"> 目前登入者 </param>
        /// <param name="cDate"> 目前時間 </param>
        /// <returns></returns>
        public List<SPA_CostServiceDetailModel> GetPrevPeriodDetails(DatePeriod period, string[] serviceForArr, string userID, DateTime cDate)
        {
            try
            {
                // 取得前期的評鑑期間
                var prevPeriod = period.GetPrevPeriod().PeriodText;

                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var subQuery =
                        (from item in context.TET_SPA_CostService
                         where item.Period == prevPeriod
                         select item.ID);

                    var query =
                        from item in context.TET_SPA_CostServiceDetail
                        where
                            item.IsEvaluate == _isEvaluateText &&
                            serviceForArr.Contains(item.ServiceFor) &&
                            subQuery.Contains(item.CSID)
                        select
                            new SPA_CostServiceDetailModel
                            {
                                ID = null,
                                CSID = null,
                                Source = _prevText,
                                IsEvaluate = item.IsEvaluate,
                                BU = item.BU,
                                ServiceFor = item.ServiceFor,
                                BelongTo = item.BelongTo,
                                POSource = item.POSource,
                                AssessmentItem = item.AssessmentItem,
                                PriceDeflator = item.PriceDeflator,
                                PaymentTerm = item.PaymentTerm,
                                Cooperation = item.Cooperation,
                                Advantage = item.Advantage,
                                Improved = item.Improved,
                                Comment = item.Comment,
                                Remark = string.Empty,
                                CreateUser = item.CreateUser,
                                CreateDate = item.CreateDate,
                                ModifyUser = item.ModifyUser,
                                ModifyDate = item.ModifyDate,
                            };

                    var list = query.ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                return default;
            }
        }

        /// <summary> 查詢附件 </summary>
        /// <param name="id"> 附件 Id </param>
        /// <returns></returns>
        public SPA_CostServiceAttachmentModel GetAttachment(Guid id)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    // 查詢附件
                    var query =
                        from item in context.TET_SPA_CostServiceAttachments
                        where item.ID == id
                        select new SPA_CostServiceAttachmentModel
                        {
                            ID = item.ID,
                            CSDetailID = item.CSDetailID,
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

        /// <summary> 新增時檢查，同一填寫者，同一評鑑期間只能有一筆資料 </summary>
        /// <param name="period"> 評鑑期間 </param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public bool HasSamePeriod(string period, string userID, DateTime cDate)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query = context.TET_SPA_CostService.Where(obj => obj.Filler == userID && obj.Period == period);

                    // 同一填寫者，同一評鑑期間只能有一筆資料
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
        /// <summary> 新增 Cost&Service資料維護 </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public Guid Create(SPA_CostServiceModel model, string userID, DateTime cDate)
        {
            if (model == null)
                throw new ArgumentNullException("Supplier is required.");

            //--- 先檢查是否能通過商業邏輯 ---
            List<string> tempMsgList;
            List<string> msgList = new List<string>();
            var validResult = SPA_CostServiceValidator.Valid(model, out tempMsgList);
            if (!validResult)
                msgList.AddRange(tempMsgList);
            var validDetailResult = SPA_CostServiceDetailValidator.Valid(model.DetailList, out tempMsgList);
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
                        throw new ArgumentException("同一填寫者，同一評鑑期間只能有一筆資料");


                    var entity = new TET_SPA_CostService()
                    {
                        ID = Guid.NewGuid(),

                        Period = model.Period,
                        Filler = userID,
                        ApproveStatus = null,

                        CreateUser = userID,
                        CreateDate = cDate,
                        ModifyUser = userID,
                        ModifyDate = cDate,
                    };
                    context.TET_SPA_CostService.Add(entity);

                    foreach (var item in model.DetailList)
                    {
                        var detailEntity = new TET_SPA_CostServiceDetail()
                        {
                            ID = Guid.NewGuid(),

                            CSID = entity.ID,
                            Source = item.Source,
                            IsEvaluate = item.IsEvaluate,
                            BU = item.BU,
                            ServiceFor = item.ServiceFor,
                            BelongTo = item.BelongTo,
                            POSource = item.POSource,
                            AssessmentItem = item.AssessmentItem,
                            PriceDeflator = item.PriceDeflator,
                            PaymentTerm = item.PaymentTerm,
                            Cooperation = item.Cooperation,
                            Advantage = item.Advantage,
                            Improved = item.Improved,
                            Comment = item.Comment,
                            Remark = item.Remark,

                            CreateUser = userID,
                            CreateDate = cDate,
                            ModifyUser = userID,
                            ModifyDate = cDate,
                        };
                        context.TET_SPA_CostServiceDetail.Add(detailEntity);

                        if (item.UploadFileList != null && item.UploadFileList.Any())
                        {
                            foreach (var fileItem in item.UploadFileList)
                            {
                                var attachmEnttity = this.WriteFile(detailEntity.ID, fileItem, userID, cDate);
                                context.TET_SPA_CostServiceAttachments.Add(attachmEnttity);
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

        /// <summary> 修改 Cost&Service資料維護 </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void Modify(SPA_CostServiceModel model, string userID, DateTime cDate)
        {
            if (model == null)
                throw new ArgumentNullException("Model is required.");


            //--- 先檢查是否能通過商業邏輯 ---
            List<string> tempMsgList;
            List<string> msgList = new List<string>();
            var validResult = SPA_CostServiceValidator.Valid(model, out tempMsgList);
            if (!validResult)
                msgList.AddRange(tempMsgList);
            var validDetailResult = SPA_CostServiceDetailValidator.Valid(model.DetailList, out tempMsgList);
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
                    (from item in context.TET_SPA_CostService
                     where item.ID == model.ID
                     select item).FirstOrDefault();

                    if (dbModel == null)
                        throw new NullReferenceException($"{model.ID} don't exists.");

                    dbModel.Period = model.Period;
                    dbModel.ModifyUser = userID;
                    dbModel.ModifyDate = cDate;

                    // 明細清單
                    var detailList = context.TET_SPA_CostServiceDetail.Where(item => item.CSID == model.ID).ToList();
                    var detailIdList = detailList.Select(obj => obj.ID);
                    // 附件清單
                    var attachList = context.TET_SPA_CostServiceAttachments.Where(item => detailIdList.Contains(item.CSDetailID)).ToList();

                    var inpDetailIdList = model.DetailList.Where(obj => obj.ID.HasValue).Select(obj => obj.ID.Value).ToList();


                    //--- 找到已移除的明細及附檔，並刪除 ---
                    var notExistDetailId = detailIdList.Except(inpDetailIdList).ToList();

                    context.TET_SPA_CostServiceDetail.RemoveRange(detailList.Where(item => notExistDetailId.Contains(item.ID)));

                    var willRemoveAttachment = attachList.Where(item => notExistDetailId.Contains(item.CSDetailID));
                    if (willRemoveAttachment.Any())
                    {
                        this.DropFile(willRemoveAttachment);
                        context.TET_SPA_CostServiceAttachments.RemoveRange(willRemoveAttachment);
                    }
                    //--- 找到已移除的明細及附檔，並刪除 ---


                    //--- 找到已存在的明細及附檔，並更新 ---
                    foreach (var item in model.DetailList)
                    {
                        if (!item.ID.HasValue)
                            continue;

                        // 以下欄位在前期匯入時，是不允許編輯的
                        // 評鑑單位、服務對象、受評供應商、PO Source、評鑑項目
                        var isPrevImport = (string.Compare(_prevText, item.Source, true) == 0);
                        var orgDetail = detailList.Where(obj => obj.ID == item.ID.Value).FirstOrDefault();

                        if (orgDetail != null)
                        {
                            orgDetail.Source = item.Source;
                            orgDetail.IsEvaluate = item.IsEvaluate;
                            if (!isPrevImport)
                            {
                                orgDetail.BU = item.BU;
                                orgDetail.ServiceFor = item.ServiceFor;
                                orgDetail.BelongTo = item.BelongTo;
                                orgDetail.POSource = item.POSource;
                                orgDetail.AssessmentItem = item.AssessmentItem;
                            }
                            orgDetail.PriceDeflator = item.PriceDeflator;
                            orgDetail.PaymentTerm = item.PaymentTerm;
                            orgDetail.Cooperation = item.Cooperation;
                            orgDetail.Advantage = item.Advantage;
                            orgDetail.Improved = item.Improved;
                            orgDetail.Comment = item.Comment;
                            orgDetail.Remark = item.Remark;

                            orgDetail.ModifyUser = userID;
                            orgDetail.ModifyDate = cDate;

                            // 寫入新的附加檔案
                            if (item.UploadFileList != null && item.UploadFileList.Any())
                            {
                                foreach (var fileItem in item.UploadFileList)
                                {
                                    var attachmEnttity = this.WriteFile(orgDetail.ID, fileItem, userID, cDate);
                                    context.TET_SPA_CostServiceAttachments.Add(attachmEnttity);
                                }
                            }

                            // 移除被刪除的已附加檔案
                            var attachmentListOfThisDetail = attachList.Where(obj=>obj.CSDetailID == orgDetail.ID).ToList();    
                            var attachmentIdList = item.AttachmentList.Select(obj=>obj.ID).ToList();
                            var willRemoveAttachmentList = attachmentListOfThisDetail.Where(obj=> !attachmentIdList.Contains(obj.ID)).ToList();

                            this.DropFile(willRemoveAttachmentList);
                            context.TET_SPA_CostServiceAttachments.RemoveRange(willRemoveAttachmentList);
                        }
                    }
                    //--- 找到已存在的明細及附檔，並更新 ---



                    //--- 找到新增的明細及附檔，並建立 ---
                    var willCreateDetails = model.DetailList.Where(obj => !obj.ID.HasValue || Guid.Empty == obj.ID.Value).ToList();
                    foreach (var item in willCreateDetails)
                    {
                        var detailEntity = new TET_SPA_CostServiceDetail()
                        {
                            ID = Guid.NewGuid(),
                            CSID = model.ID.Value,

                            Source = item.Source,
                            IsEvaluate = item.IsEvaluate,
                            BU = item.BU,
                            ServiceFor = item.ServiceFor,
                            BelongTo = item.BelongTo,
                            POSource = item.POSource,
                            AssessmentItem = item.AssessmentItem,
                            PriceDeflator = item.PriceDeflator,
                            PaymentTerm = item.PaymentTerm,
                            Cooperation = item.Cooperation,
                            Advantage = item.Advantage,
                            Improved = item.Improved,
                            Comment = item.Comment,
                            Remark = item.Remark,

                            CreateUser = userID,
                            CreateDate = cDate,
                            ModifyUser = userID,
                            ModifyDate = cDate,
                        };

                        context.TET_SPA_CostServiceDetail.Add(detailEntity);

                        if (item.UploadFileList != null && item.UploadFileList.Any())
                        {
                            foreach (var fileItem in item.UploadFileList)
                            {
                                var attachmEnttity = this.WriteFile(detailEntity.ID, fileItem, userID, cDate);
                                context.TET_SPA_CostServiceAttachments.Add(attachmEnttity);
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
        /// 取得 Cost&Service資料維護 清單
        /// </summary>
        /// <param name="period">評鑑期間</param>
        /// <param name="approveStatus">審核狀態</param>
        /// <param name="userID"> 目前登入者 </param>
        /// <param name="cDate"> 目前時間 </param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<SPA_CostServiceExportModel> GetExportList(string period, string[] approveStatus, string userID, DateTime cDate)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    //----- 主表資料 -----
                    var query =
                        from item in context.TET_SPA_CostService
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
                        from item in context.TET_SPA_CostServiceDetail
                        where ids.Contains(item.CSID)
                        select item;

                    var detailList = detailQuery.ToList();
                    //----- 明細資料 -----


                    //--- 處理輸出資料 ---
                    var retList = new List<SPA_CostServiceExportModel>();
                    var empList = this._userMgr.GetUserList(list.Select(obj => obj.Filler));


                    foreach (var mainItem in list)
                    {
                        var thisDetailList = detailList.Where(obj => obj.CSID == mainItem.ID).ToList();

                        // 如果沒有明細，仍產生一筆資料
                        if (!thisDetailList.Any())
                        {
                            var item = new SPA_CostServiceExportModel()
                            {
                                Period = mainItem.Period,
                            };

                            // 填寫者
                            if (item.Filler != null)
                                item.FillerFullName = empList.Where(obj => item.Filler == obj.UserID).Select(obj => $"{obj.FirstNameEN} {obj.LastNameEN}({obj.EmpID})").FirstOrDefault();

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
                                var item = new SPA_CostServiceExportModel()
                                {
                                    Period = mainItem.Period,
                                    Filler = mainItem.Filler,
                                    Source = detail.Source,
                                    IsEvaluate = detail.IsEvaluate,
                                    BU = detail.BU,
                                    ServiceFor = detail.ServiceFor,
                                    BelongTo = detail.BelongTo,
                                    POSource = detail.POSource,
                                    AssessmentItem = detail.AssessmentItem,
                                    PriceDeflator = detail.PriceDeflator,
                                    PaymentTerm = detail.PaymentTerm,
                                    Cooperation = detail.Cooperation,
                                    Advantage = detail.Advantage,
                                    Improved = detail.Improved,
                                    Comment = detail.Comment,
                                    Remark = detail.Remark,
                                };

                                // 填寫者
                                if (item.Filler != null)
                                    item.FillerFullName = empList.Where(obj => item.Filler == obj.UserID).Select(obj => $"{obj.FirstNameEN} {obj.LastNameEN}({obj.EmpID})").FirstOrDefault();

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
        private bool HasSamePeriod(PlatformContextModel context, SPA_CostServiceModel model, string userID, DateTime cDate)
        {
            var query = context.TET_SPA_CostService.Where(obj => obj.Filler == userID && obj.Period == model.Period);

            if (model.ID.HasValue)
                query = query.Where(obj => obj.ID != model.ID.Value);

            // 同一填寫者，同一評鑑期間只能有一筆資料
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
        private TET_SPA_CostServiceAttachments WriteFile(Guid detailId, FileContent file, string userID, DateTime cDate)
        {
            // 將前端傳入的檔案寫入檔案系統及資料表
            // 計算起始路徑，如果不是上傳資料夾根目錄，要附加在最前面
            string rootFolder = MediaFileManager.GetRootFolder();
            string filePath = Path.Combine(rootFolder, ModuleConfig.FolderPath);

            if (!filePath.StartsWith(rootFolder, StringComparison.OrdinalIgnoreCase))
                filePath = Path.Combine(rootFolder, ModuleConfig.FolderPath);

            string folderPath = HostingEnvironment.MapPath("~/" + filePath);

            var newFileName = FileUtility.Upload(file, folderPath);
            var entity = new TET_SPA_CostServiceAttachments()
            {
                ID = Guid.NewGuid(),
                CSDetailID = detailId,
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
        private void DropFile(IEnumerable<TET_SPA_CostServiceAttachments> attachments)
        {
            foreach (var item in attachments)
            {
                FileUtility.DeleteFile(item.FilePath);
            }
        }
        #endregion
    }
}
