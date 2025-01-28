using BI.Shared.Utils;
using BI.SPA_ApproverSetup;
using BI.SPA_ApproverSetup.Models;
using BI.SPA_ScoringInfo.Enums;
using BI.SPA_ScoringInfo.Models;
using BI.SPA_ScoringInfo.Models.Exporting;
using BI.SPA_ScoringInfo.Validators;
using Platform.AbstractionClass;
using Platform.Auth;
using Platform.Auth.Models;
using Platform.FileSystem;
using Platform.Infra;
using Platform.LogService;
using Platform.ORM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace BI.SPA_ScoringInfo
{
    public class SPA_ScoringInfoManager
    {
        private const string _prevText = "前期匯入";
        private const string _isEvaluateText = "評鑑";

        private Logger _logger = new Logger();
        private UserManager _userMgr = new UserManager();
        private UserRoleManager _userRoleMgr = new UserRoleManager();
        private SPA_ApproverSetupManager _spa_ApproverSetupManager = new SPA_ApproverSetupManager();
        private SPA_ScoringInfoModulesManager _detailMgr = new SPA_ScoringInfoModulesManager();

        #region Read
        /// <summary> 取得 供應商SPA評鑑計分資料維護 清單 </summary>
        /// <param name="period">評鑑期間</param>
        /// <param name="bu">評鑑單位</param>
        /// <param name="serviceFor">服務對象</param>
        /// <param name="serviceItem">評鑑項目</param>
        /// <param name="approveStatus">審核狀態</param>
        /// <param name="userID"> 目前登入者 </param>
        /// <param name="cDate"> 目前時間 </param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<SPA_ScoringInfoModel> GetList(string period, string[] bu, string[] serviceFor, string[] serviceItem, string[] approveStatus, string[] belongTo, string userID, DateTime cDate, Pager pager)
        {
            // 目前登入者是否具 QSM 身份
            bool isQSM = this._userRoleMgr.IsRole(userID, ApprovalRole.QSM.ToID().Value);


            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var orgQuery =
                        from item in context.TET_SPA_ScoringInfo
                        select item;

                    //--- 組合過濾條件 ---
                    if (!string.IsNullOrWhiteSpace(period))
                        orgQuery = orgQuery.Where(obj => obj.Period.Contains(period));

                    if (bu != null && bu.Length > 0)
                        orgQuery = orgQuery.Where(obj => bu.Contains(obj.BU));

                    if (serviceFor != null && serviceFor.Length > 0)
                        orgQuery = orgQuery.Where(obj => serviceFor.Contains(obj.ServiceFor));

                    if (serviceItem != null && serviceItem.Length > 0)
                        orgQuery = orgQuery.Where(obj => serviceItem.Contains(obj.ServiceItem));

                    if (approveStatus != null && approveStatus.Length > 0)
                    {
                        if (approveStatus.Contains(string.Empty))
                            orgQuery = orgQuery.Where(obj => obj.ApproveStatus == null || approveStatus.Contains(obj.ApproveStatus));
                        else
                            orgQuery = orgQuery.Where(obj => approveStatus.Contains(obj.ApproveStatus));
                    }

                    if (belongTo != null && belongTo.Length > 0)
                        orgQuery = orgQuery.Where(obj => belongTo.Contains(obj.BelongTo));
                    //--- 組合過濾條件 ---


                    if (!isQSM)
                    {
                        // 取得設定，依帳號，查出允許的評鑑項目
                        var setupList = this._spa_ApproverSetupManager.GetList(new List<Guid>(), new List<Guid>(), userID, cDate, new Pager() { AllowPaging = false });
                        var userSetupList = setupList.Where(obj => obj.InfoConfirm == userID || obj.InfoFills.Contains(userID)).Select(obj => obj.ServiceItemText + "___" + obj.BUText).ToList();

                        orgQuery =
                            from item in orgQuery
                            let tempCol = item.ServiceItem + "___" + item.BU
                            where userSetupList.Contains(tempCol)
                            select item;
                    }


                    var query =
                        from item in orgQuery
                        select
                            new SPA_ScoringInfoModel()
                            {
                                ID = item.ID,
                                Period = item.Period,
                                BU = item.BU,
                                ServiceItem = item.ServiceItem,
                                ServiceFor = item.ServiceFor,
                                BelongTo = item.BelongTo,
                                POSource = item.POSource,
                                ApproveStatus = item.ApproveStatus,
                                MOCount = item.MOCount,
                                TELLoss = item.TELLoss,
                                CustomerLoss = item.CustomerLoss,
                                Accident = item.Accident,
                                WorkerCount = item.WorkerCount,
                                Correctness = item.Correctness,
                                Contribution = item.Contribution,
                                SelfTraining = item.SelfTraining,
                                SelfTrainingRemark = item.SelfTrainingRemark,
                                Cooperation = item.Cooperation,
                                Complain = item.Complain,
                                Advantage = item.Advantage,
                                Improved = item.Improved,
                                Comment = item.Comment,
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

        /// <summary> 取得 供應商SPA評鑑計分資料維護 清單 </summary>
        /// <param name="id">Key</param>
        /// <returns></returns>
        public SPA_ScoringInfoModel GetOne(Guid id)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in context.TET_SPA_ScoringInfo
                        where item.ID == id
                        select new SPA_ScoringInfoModel
                        {
                            ID = item.ID,
                            Period = item.Period,
                            BU = item.BU,
                            ServiceItem = item.ServiceItem,
                            ServiceFor = item.ServiceFor,
                            BelongTo = item.BelongTo,
                            POSource = item.POSource,
                            ApproveStatus = item.ApproveStatus,
                            MOCount = item.MOCount,
                            TELLoss = item.TELLoss,
                            CustomerLoss = item.CustomerLoss,
                            Accident = item.Accident,
                            WorkerCount = item.WorkerCount,
                            Correctness = item.Correctness,
                            Contribution = item.Contribution,
                            SelfTraining = item.SelfTraining,
                            SelfTrainingRemark = item.SelfTrainingRemark,
                            Cooperation = item.Cooperation,
                            Complain = item.Complain,
                            Advantage = item.Advantage,
                            Improved = item.Improved,
                            Comment = item.Comment,
                            CreateUser = item.CreateUser,
                            CreateDate = item.CreateDate,
                            ModifyUser = item.ModifyUser,
                            ModifyDate = item.ModifyDate,
                        };

                    var result = query.FirstOrDefault();


                    result.Module1List = _detailMgr.GetList_Module1(id);
                    result.Module2List = _detailMgr.GetList_Module2(id);
                    result.Module3List = _detailMgr.GetList_Module3(id);
                    result.Module4List = _detailMgr.GetList_Module4(id);


                    //--- 附件 ---
                    var attachmentQuery =
                        from item in context.TET_SPA_ScoringInfoAttachments
                        where item.SIID == id
                        select new SPA_ScoringInfoAttachmentModel
                        {
                            ID = item.ID,
                            SIID = item.ID,
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

                    result.AttachmentList = attachmentQuery.ToList();
                    //--- 附件 ---


                    //--- 簽核 ---
                    var approvalQuery =
                      from item in context.TET_SPA_ScoringInfoApproval
                      where
                          item.SIID == id &&
                          item.Result != null
                      orderby item.CreateDate,item.ModifyDate
                      select
                          new SPA_ScoringInfoApprovalModel()
                          {
                              ID = item.ID,
                              SIID = item.SIID,
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
                    //--- 簽核 ---


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
        public SPA_ScoringInfoAttachmentModel GetAttachment(Guid id)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    // 查詢附件
                    var query =
                        from item in context.TET_SPA_ScoringInfoAttachments
                        where item.ID == id
                        select new SPA_ScoringInfoAttachmentModel
                        {
                            ID = item.ID,
                            SIID = item.ID,
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
        #endregion


        #region CUD
        /// <summary> 修改 供應商SPA評鑑計分資料維護 </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void Modify_Tab4(SPA_ScoringInfoModel model, string userID, DateTime cDate)
        {
            if (model == null)
                throw new ArgumentNullException("Model is required.");


            //--- 先檢查是否能通過商業邏輯 ---
            List<string> tempMsgList;
            List<string> msgList = new List<string>();
            var validResult = SPA_ScoringInfoValidator.Valid_Tab4(model, out tempMsgList);
            if (!validResult)
                msgList.AddRange(tempMsgList);

            if (!validResult)
                throw new ArgumentException(string.Join(Environment.NewLine, msgList));
            //--- 先檢查是否能通過商業邏輯 ---


            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                    (from item in context.TET_SPA_ScoringInfo
                     where item.ID == model.ID
                     select item).FirstOrDefault();

                    if (dbModel == null)
                        throw new NullReferenceException($"{model.ID} don't exists.");

                    dbModel.Correctness = model.Correctness;
                    dbModel.Contribution = model.Contribution;
                    dbModel.ModifyUser = userID;
                    dbModel.ModifyDate = cDate;


                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> 修改 供應商SPA評鑑計分資料維護 </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void Modify_Tab5(SPA_ScoringInfoModel model, string userID, DateTime cDate)
        {
            if (model == null)
                throw new ArgumentNullException("Model is required.");


            //--- 先檢查是否能通過商業邏輯 ---
            List<string> tempMsgList;
            List<string> msgList = new List<string>();
            var validResult = SPA_ScoringInfoValidator.Valid_Tab5(model, out tempMsgList);
            if (!validResult)
                msgList.AddRange(tempMsgList);

            if (!validResult)
                throw new ArgumentException(string.Join(Environment.NewLine, msgList));
            //--- 先檢查是否能通過商業邏輯 ---


            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                    (from item in context.TET_SPA_ScoringInfo
                     where item.ID == model.ID
                     select item).FirstOrDefault();

                    if (dbModel == null)
                        throw new NullReferenceException($"{model.ID} don't exists.");

                    dbModel.SelfTraining = model.SelfTraining;
                    dbModel.SelfTrainingRemark = model.SelfTrainingRemark;
                    dbModel.ModifyUser = userID;
                    dbModel.ModifyDate = cDate;


                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }



        /// <summary> 修改 供應商SPA評鑑計分資料維護 </summary>
        /// <param name="model"></param>
        /// <param name="attachmentList"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void Modify_Tab7(SPA_ScoringInfoModel model, List<FileContent> attachmentList, string userID, DateTime cDate)
        {
            if (model == null)
                throw new ArgumentNullException("Model is required.");


            //--- 先檢查是否能通過商業邏輯 ---
            List<string> tempMsgList;
            List<string> msgList = new List<string>();
            var validResult = SPA_ScoringInfoValidator.Valid(model, out tempMsgList);
            if (!validResult)
                msgList.AddRange(tempMsgList);

            if (!validResult)
                throw new ArgumentException(string.Join(Environment.NewLine, msgList));
            //--- 先檢查是否能通過商業邏輯 ---


            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                        (from item in context.TET_SPA_ScoringInfo
                         where item.ID == model.ID
                         select item).FirstOrDefault();

                    if (dbModel == null)
                        throw new NullReferenceException($"{model.ID} don't exists.");

                    //--- 附件清單、及現況附件 ID 清單 ---
                    var attachList = context.TET_SPA_ScoringInfoAttachments.Where(item => item.SIID == model.ID).ToList();

                    var dbAttachIdList = attachList.Select(obj => obj.ID).ToList();
                    var inpAttachIdList = model.AttachmentList.Where(obj => obj.ID.HasValue).Select(item => item.ID.Value).ToList();
                    //--- 附件清單、及現況附件 ID 清單 ---

                    //--- 找到已移除的附檔，並刪除資料和實體檔 ---
                    var notExistAttachId = dbAttachIdList.Except(inpAttachIdList).ToList();

                    var willRemoveAttachment = attachList.Where(item => notExistAttachId.Contains(item.ID));
                    if (willRemoveAttachment.Any())
                    {
                        this.DropFile(willRemoveAttachment);
                        context.TET_SPA_ScoringInfoAttachments.RemoveRange(willRemoveAttachment);
                    }
                    //--- 找到已移除的附檔，並刪除資料和實體檔 ---

                    //--- 新增檔案 ---
                    foreach (var fileItem in attachmentList)
                    {
                        var attachmEnttity = this.WriteFile(dbModel.ID, fileItem, userID, cDate);
                        context.TET_SPA_ScoringInfoAttachments.Add(attachmEnttity);
                    }
                    //--- 新增檔案 ---

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
        /// <summary> 取得 供應商SPA評鑑計分資料維護 清單  </summary>
        /// <param name="period">評鑑期間</param>
        /// <param name="approveStatus">審核狀態</param>
        /// <param name="userID"> 目前登入者 </param>
        /// <param name="cDate"> 目前時間 </param>
        /// <returns></returns>
        public SPA_ScoringInfoExportModel GetExportingReport(string period, string[] bu, string[] serviceFor, string[] serviceItem, string[] belongTo, string[] approveStatus, string userID, DateTime cDate)
        {
            // 目前登入者是否具 QSM 身份
            bool isQSM = this._userRoleMgr.IsRole(userID, ApprovalRole.QSM.ToID().Value);


            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    //----- 主表資料 -----
                    var orgQuery =
                         from item in context.TET_SPA_ScoringInfo
                         select item;

                    //--- 組合過濾條件 ---
                    if (!string.IsNullOrWhiteSpace(period))
                        orgQuery = orgQuery.Where(obj => obj.Period.Contains(period));

                    if (bu != null && bu.Length > 0)
                        orgQuery = orgQuery.Where(obj => bu.Contains(obj.BU));

                    if (serviceFor != null && serviceFor.Length > 0)
                        orgQuery = orgQuery.Where(obj => serviceFor.Contains(obj.ServiceFor));

                    if (serviceItem != null && serviceItem.Length > 0)
                        orgQuery = orgQuery.Where(obj => serviceItem.Contains(obj.ServiceItem));

                    if (belongTo != null && belongTo.Length > 0)
                        orgQuery = orgQuery.Where(obj => belongTo.Contains(obj.BelongTo));

                    if (approveStatus != null && approveStatus.Length > 0)
                    {
                        if (approveStatus.Contains(string.Empty))
                            orgQuery = orgQuery.Where(obj => obj.ApproveStatus == null || approveStatus.Contains(obj.ApproveStatus));
                        else
                            orgQuery = orgQuery.Where(obj => approveStatus.Contains(obj.ApproveStatus));
                    }
                    //--- 組合過濾條件 ---


                    if (!isQSM)
                    {
                        // 取得設定，依帳號，查出允許的評鑑項目
                        var setupList = this._spa_ApproverSetupManager.GetList(new List<Guid>(), new List<Guid>(), userID, cDate, new Pager() { AllowPaging = false });
                        var userSetupList = setupList.Where(obj => obj.InfoConfirm == userID || obj.InfoFills.Contains(userID)).Select(obj => obj.ServiceItemText + "___" + obj.BUText).ToList();

                        orgQuery =
                            from item in orgQuery
                            let tempCol = item.ServiceItem + "___" + item.BU
                            where userSetupList.Contains(tempCol)
                            select item;
                    }


                    var query =
                        from item in orgQuery
                        select
                            new SPA_ScoringInfoModel()
                            {
                                ID = item.ID,
                                Period = item.Period,
                                BU = item.BU,
                                ServiceItem = item.ServiceItem,
                                ServiceFor = item.ServiceFor,
                                BelongTo = item.BelongTo,
                                POSource = item.POSource,
                                ApproveStatus = item.ApproveStatus,
                                MOCount = item.MOCount,
                                TELLoss = item.TELLoss,
                                CustomerLoss = item.CustomerLoss,
                                Accident = item.Accident,
                                WorkerCount = item.WorkerCount,
                                Correctness = item.Correctness,
                                Contribution = item.Contribution,
                                SelfTraining = item.SelfTraining,
                                SelfTrainingRemark = item.SelfTrainingRemark,
                                Cooperation = item.Cooperation,
                                Complain = item.Complain,
                                Advantage = item.Advantage,
                                Improved = item.Improved,
                                Comment = item.Comment,
                            };

                    var list = query.ToList();
                    //----- 主表資料 -----


                    var retModel = new SPA_ScoringInfoExportModel();

                    //----- 明細資料 -----
                    var ids = list.Select(obj => obj.ID.Value).ToArray();

                    retModel.BaseList = list.Select(obj => new SPA_ScoringInfoExportModelBase(obj)).ToList();

                    retModel.Tab1List =
                        (from item in this._detailMgr.GetList_Module1(ids)
                         join mainItem in list
                             on item.SIID equals mainItem.ID
                         select new SPA_ScoringInfoExportTab1Model(item, mainItem)).ToList();

                    retModel.Tab2List =
                        (from item in this._detailMgr.GetList_Module2(ids)
                         join mainItem in list
                             on item.SIID equals mainItem.ID
                         select new SPA_ScoringInfoExportTab2Model(item, mainItem)).ToList();

                    retModel.Tab3List =
                      (from item in this._detailMgr.GetList_Module3(ids)
                       join mainItem in list
                           on item.SIID equals mainItem.ID
                       select new SPA_ScoringInfoExportTab3Model(item, mainItem)).ToList();

                    retModel.Tab4List = list.Select(obj => new SPA_ScoringInfoExportTab4Model(obj)).ToList();
                    retModel.Tab5List = list.Select(obj => new SPA_ScoringInfoExportTab5Model(obj)).ToList();

                    retModel.Tab6List =
                      (from item in this._detailMgr.GetList_Module4(ids)
                       join mainItem in list
                           on item.SIID equals mainItem.ID
                       select new SPA_ScoringInfoExportTab6Model(item, mainItem)).ToList();
                    //----- 明細資料 -----


                    //--- 處理輸出資料 ---
                    //var retList = new List<SPA_ScoringInfoExportModel>();
                    //var empList = this._userMgr.GetUserList(list.Select(obj => obj.Filler));


                    //foreach (var mainItem in list)
                    //{
                    //    var thisDetailList = detailList.Where(obj => obj.CSID == mainItem.ID).ToList();

                    //    // 如果沒有明細，仍產生一筆資料
                    //    if (!thisDetailList.Any())
                    //    {
                    //        var item = new SPA_ScoringInfoExportModel()
                    //        {
                    //            Period = mainItem.Period,
                    //        };

                    //        // 填寫者
                    //        if (item.Filler != null)
                    //            item.FillerFullName = empList.Where(obj => item.Filler == obj.UserID).Select(obj => $"{obj.FirstNameEN} {obj.LastNameEN}({obj.EmpID})").FirstOrDefault();

                    //        // 起始結束時間
                    //        var periodDate = PeriodUtil.ParsePeriod(item.Period);
                    //        item.PeriodStart = periodDate?.StartDate?.ToString("yyyy-MM-dd");
                    //        item.PeriodEnd = periodDate?.EndDate?.ToString("yyyy-MM-dd");

                    //        retList.Add(item);
                    //    }
                    //    else
                    //    {
                    //        foreach (var detail in thisDetailList)
                    //        {
                    //            var item = new SPA_ScoringInfoExportModel()
                    //            {
                    //                Period = mainItem.Period,
                    //                Filler = mainItem.Filler,
                    //                Source = detail.Source,
                    //                IsEvaluate = detail.IsEvaluate,
                    //                BU = detail.BU,
                    //                ServiceFor = detail.ServiceFor,
                    //                BelongTo = detail.BelongTo,
                    //                POSource = detail.POSource,
                    //                AssessmentItem = detail.AssessmentItem,
                    //                PriceDeflator = detail.PriceDeflator,
                    //                PaymentTerm = detail.PaymentTerm,
                    //                Cooperation = detail.Cooperation,
                    //                Advantage = detail.Advantage,
                    //                Improved = detail.Improved,
                    //                Comment = detail.Comment,
                    //                Remark = detail.Remark,
                    //            };

                    //            // 填寫者
                    //            if (item.Filler != null)
                    //                item.FillerFullName = empList.Where(obj => item.Filler == obj.UserID).Select(obj => $"{obj.FirstNameEN} {obj.LastNameEN}({obj.EmpID})").FirstOrDefault();

                    //            // 起始結束時間
                    //            var periodDate = PeriodUtil.ParsePeriod(item.Period);
                    //            item.PeriodStart = periodDate?.StartDate?.ToString("yyyy-MM-dd");
                    //            item.PeriodEnd = periodDate?.EndDate?.ToString("yyyy-MM-dd");

                    //            retList.Add(item);
                    //        }
                    //    }
                    //}
                    //--- 處理輸出資料 ---


                    return retModel;
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
        /// <summary> 依評鑑項目，查出指定的設定 </summary>
        /// <param name="bu"></param>
        /// <param name="serviceItem"></param>
        /// <param name="userId"></param>
        /// <param name="cDate"></param>
        /// <returns></returns>
        public List<string> GetSetupInfoConfirm(string bu, string serviceItem, string userId, DateTime cDate)
        {
            // 取得設定
            var setupList = this._spa_ApproverSetupManager.GetList(new List<Guid>(), new List<Guid>(), userId, cDate, new Pager() { AllowPaging = false });


            var accounts = setupList.Where(obj => obj.BUText == bu && obj.ServiceItemText == serviceItem).Select(obj => obj.InfoConfirm).Distinct();
            return accounts.ToList();
        }


        /// <summary> 寫入檔案，並回傳附件 </summary>
        /// <param name="detailId"></param>
        /// <param name="file"></param>
        /// <param name="userID"></param>
        /// <param name="cDate"></param>
        /// <returns></returns>
        private TET_SPA_ScoringInfoAttachments WriteFile(Guid detailId, FileContent file, string userID, DateTime cDate)
        {
            // 將前端傳入的檔案寫入檔案系統及資料表
            // 計算起始路徑，如果不是上傳資料夾根目錄，要附加在最前面
            string rootFolder = MediaFileManager.GetRootFolder();
            string filePath = Path.Combine(rootFolder, ModuleConfig.FolderPath);

            if (!filePath.StartsWith(rootFolder, StringComparison.OrdinalIgnoreCase))
                filePath = Path.Combine(rootFolder, ModuleConfig.FolderPath);

            string folderPath = HostingEnvironment.MapPath("~/" + filePath);

            var newFileName = FileUtility.Upload(file, folderPath);
            var entity = new TET_SPA_ScoringInfoAttachments()
            {
                ID = Guid.NewGuid(),
                SIID = detailId,
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
        private void DropFile(IEnumerable<TET_SPA_ScoringInfoAttachments> attachments)
        {
            foreach (var item in attachments)
            {
                FileUtility.DeleteFile(item.FilePath);
            }
        }
        #endregion
    }
}
