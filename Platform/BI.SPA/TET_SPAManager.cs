using System;
using System.Collections.Generic;
using System.Linq;
using Platform.AbstractionClass;
using Platform.Infra;
using Platform.LogService;
using Platform.ORM;
using BI.SPA.Models;
using BI.SPA.Enums;
using BI.SPA.Utils;
using Platform.Auth;
using BI.SPA.Validators;

namespace BI.SPA
{
    public class TET_SPAManager
    {
        private Logger _logger = new Logger();
        private UserManager _userMgr = new UserManager();
        private UserRoleManager _userRoleMgr = new UserRoleManager();


        #region Read
        /// <summary>
        /// 取得 供應商 SPA 清單
        /// </summary>
        /// <param name="belongTo">供應商</param>
        /// <param name="period">評鑑期間</param>
        /// <param name="bu">評鑑單位</param>
        /// <param name="assessmentItem">評鑑項目</param>
        /// <param name="performanceLevel">Performance Level</param>
        /// <param name="userID"> 目前登入者 </param>
        /// <param name="cDate"> 目前時間 </param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<TET_SupplierSPAModel> GetSPAList(string[] belongTo, string[] period, string[] bu, string[] assessmentItem, string[] performanceLevel, string userID, DateTime cDate, Pager pager)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    string completedText = ApprovalStatus.Completed.ToText();

                    var orgQuery =
                        from item in context.TET_SupplierSPA
                        from item1 in context.TET_Parameters
                        from item2 in context.TET_Parameters
                        from item3 in context.TET_Parameters
                        from item4 in context.TET_Parameters
                        where item.ApproveStatus == completedText &&
                              item.ServiceFor == item1.ID.ToString() &&
                              item.AssessmentItem == item2.ID.ToString() &&
                              item.PerformanceLevel == item3.ID.ToString() &&
                              item.BU == item4.ID.ToString()
                        select new
                        {
                            item.ID,
                            item.BelongTo,
                            item.Period,
                            BUID = item4.ID,
                            BU = item4.Item,
                            ServiceFor = item1.Item,
                            AssessmentItemID = item2.ID,
                            AssessmentItem = item2.Item,
                            PerformanceLevelID = item3.ID,
                            PerformanceLevel = item3.Item,
                            TotalScore = item.TotalScore,
                            TScore = item.TScore,
                            DScore = item.DScore,
                            QScore = item.QScore,
                            CScore = item.CScore,
                            SScore = item.SScore,
                            SPAComment = item.Comment,
                            item.ApproveStatus,
                            item.CreateUser,
                            item.CreateDate,
                            item.ModifyUser,
                            item.ModifyDate
                        };

                    orgQuery =
                        orgQuery.Union(
                            from item in context.TET_SupplierSPA
                            from item1 in context.TET_Parameters
                            from item2 in context.TET_Parameters
                            from item3 in context.TET_Parameters
                            from item4 in context.TET_Parameters
                            where
                                item.ApproveStatus != completedText &&
                                (item.CreateUser == userID || item.ModifyUser == userID) &&
                                item.ServiceFor == item1.ID.ToString() &&
                                item.AssessmentItem == item2.ID.ToString() &&
                                item.PerformanceLevel == item3.ID.ToString() &&
                                item.BU == item4.ID.ToString()
                            select new
                            {
                                item.ID,
                                item.BelongTo,
                                item.Period,
                                BUID = item4.ID,
                                BU=item4.Item,
                                ServiceFor = item1.Item,
                                AssessmentItemID = item2.ID,
                                AssessmentItem = item2.Item,
                                PerformanceLevelID = item3.ID,
                                PerformanceLevel = item3.Item,
                                TotalScore = item.TotalScore,
                                TScore = item.TScore,
                                DScore = item.DScore,
                                QScore = item.QScore,
                                CScore = item.CScore,
                                SScore = item.SScore,
                                SPAComment = item.Comment,
                                item.ApproveStatus,
                                item.CreateUser,
                                item.CreateDate,
                                item.ModifyUser,
                                item.ModifyDate
                            });



                    //--- 組合過濾條件 ---
                    if (belongTo != null && belongTo.Length > 0)
                        orgQuery = orgQuery.Where(obj => belongTo.Contains(obj.BelongTo));

                    if (period != null && period.Length > 0)
                        orgQuery = orgQuery.Where(obj => period.Contains(obj.Period));

                    if (bu != null && bu.Length > 0)
                        orgQuery = orgQuery.Where(obj => bu.Contains(obj.BUID.ToString().ToUpper()));

                    if (assessmentItem != null && assessmentItem.Length > 0)
                        orgQuery = orgQuery.Where(obj => assessmentItem.Contains(obj.AssessmentItemID.ToString().ToUpper()));

                    if (performanceLevel != null && performanceLevel.Length > 0)
                        orgQuery = orgQuery.Where(obj => performanceLevel.Contains(obj.PerformanceLevelID.ToString().ToUpper()));
                    //--- 組合過濾條件 ---


                    var query =
                        from item in orgQuery
                        select
                            new TET_SupplierSPAModel()
                            {
                                ID = item.ID,
                                BelongTo = item.BelongTo,
                                Period = item.Period,
                                BU = item.BU,
                                ServiceFor = item.ServiceFor,
                                AssessmentItem = item.AssessmentItem,
                                PerformanceLevel = item.PerformanceLevel,
                                TotalScore = item.TotalScore,
                                TScore = item.TScore,
                                DScore = item.DScore,
                                QScore = item.QScore,
                                CScore = item.CScore,
                                SScore = item.SScore,
                                SPAComment = item.SPAComment,
                                ApproveStatus = item.ApproveStatus,
                                CreateUser = item.CreateUser,
                                CreateDate = item.CreateDate,
                                ModifyUser = item.ModifyUser,
                                ModifyDate = item.ModifyDate,
                            };

                    query = query.OrderByDescending(obj => obj.CreateDate);
                    var list = query.ProcessPager(pager).ToList();

                    foreach (var item in list)
                    {
                        if (double.TryParse(item.TScore, out double tmpTScore))
                            item.TScore = tmpTScore.ToString("N2");

                        if (double.TryParse(item.DScore, out double tmpDScore))
                            item.DScore = tmpDScore.ToString("N2");

                        if (double.TryParse(item.QScore, out double tmpQScore))
                            item.QScore = tmpQScore.ToString("N2");

                        if (double.TryParse(item.CScore, out double tmpCScore))
                            item.CScore = tmpCScore.ToString("N2");

                        if (double.TryParse(item.SScore, out double tmpSScore))
                            item.SScore = tmpSScore.ToString("N2");

                        if (double.TryParse(item.TotalScore, out double tmpTotalScore))
                            item.TotalScore = tmpTotalScore.ToString("N2");
                    }

                    return list;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                return default;
            }
        }

        /// <summary> 查詢供應商 SPA </summary>
        /// <param name="ID"> 供應商 ID </param>
        /// <param name="includeApprovalList"> 是否要包含審核歷程 </param>
        /// <returns></returns>
        public TET_SupplierSPAModel GetSPA(Guid ID, bool includeApprovalList = false)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var result =
                    (from item in context.TET_SupplierSPA
                     where
                        item.ID == ID
                     select
                        new TET_SupplierSPAModel()
                        {
                            ID = item.ID,
                            BelongTo = item.BelongTo,
                            Period = item.Period,
                            BU = item.BU,
                            ServiceFor = item.ServiceFor,
                            AssessmentItem = item.AssessmentItem,
                            PerformanceLevel = item.PerformanceLevel,
                            TotalScore = item.TotalScore,
                            TScore = item.TScore,
                            DScore = item.DScore,
                            QScore = item.QScore,
                            CScore = item.CScore,
                            SScore = item.SScore,
                            SPAComment = item.Comment,
                            ApproveStatus = item.ApproveStatus,
                            CreateUser = item.CreateUser,
                            CreateDate = item.CreateDate,
                            ModifyUser = item.ModifyUser,
                            ModifyDate = item.ModifyDate,
                        }
                    ).FirstOrDefault();

                    if (double.TryParse(result.TScore, out double tmpTScore))
                        result.TScore = tmpTScore.ToString("N2");

                    if (double.TryParse(result.DScore, out double tmpDScore))
                        result.DScore = tmpDScore.ToString("N2");

                    if (double.TryParse(result.QScore, out double tmpQScore))
                        result.QScore = tmpQScore.ToString("N2");

                    if (double.TryParse(result.CScore, out double tmpCScore))
                        result.CScore = tmpCScore.ToString("N2");

                    if (double.TryParse(result.SScore, out double tmpSScore))
                        result.SScore = tmpSScore.ToString("N2");

                    if (double.TryParse(result.TotalScore, out double tmpTotalScore))
                        result.TotalScore = tmpTotalScore.ToString("N2");

                    if (includeApprovalList)
                    {
                        var approvalQuery =
                            from item in context.TET_SupplierSPAApproval
                            where
                                item.SPAID == ID &&
                                item.Result != null
                            orderby item.CreateDate
                            select
                                new TET_SupplierSPAApprovalModel()
                                {
                                    ID = item.ID,
                                    SPAID = item.SPAID,
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

        /// <summary> 取得評鑑期間 </summary>
        /// <param name="onlyCompleted"> 是否要過濾，讓它只剩已完成 </param>
        /// <returns></returns>
        public List<KeyTextModel> GetPeriodList(bool onlyCompleted = false)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    string status = ApprovalStatus.Completed.ToText();

                    var orgQuery =
                        from item in context.TET_SupplierSPA
                        select item;

                    if (onlyCompleted)
                        orgQuery = orgQuery.Where(item => item.ApproveStatus == status);

                    var orgQuery2 =
                        (from item in orgQuery
                         orderby item.Period
                         select item.Period).Distinct();

                    var query =
                        orgQuery2.Select(item =>
                            new KeyTextModel()
                            {
                                Key = item,
                                Text = item
                            });

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
        #endregion

        #region Advanced Read
        /// <summary> 是否能開啟 SPA 資料 </summary>
        /// <param name="ID"> SPA 代碼 </param>
        /// <param name="cUser"> 目前登入者 </param>
        /// <returns></returns>
        public bool CanRead(Guid ID, string cUser)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var supplier = this.GetSPA(ID);

                    if (supplier == null)
                        return false;

                    // 檢查是否為建立或修改者
                    var creatorResult = false;
                    if (string.Compare(cUser, supplier.CreateUser, true) == 0 ||
                       string.Compare(cUser, supplier.ModifyUser, true) == 0)
                        creatorResult = true;


                    // 只有審核中的需要檢查
                    bool approvalResult = true;
                    if (string.Compare(ApprovalStatus.Verify.ToText(), supplier.ApproveStatus, true) == 0)
                    {
                        var approvalQuery =
                            from item in context.TET_SupplierSPAApproval
                            where
                                item.SPAID == ID && item.Approver == cUser
                            select item.ID;
                        approvalResult = approvalQuery.Any();
                    }

                    var result = creatorResult || approvalResult;
                    return result;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }

        /// <summary>
        /// 取得 供應商 SPA 清單
        /// </summary>
        /// <param name="belongTo">供應商</param>
        /// <param name="period">評鑑期間</param>
        /// <param name="bu">評鑑單位</param>
        /// <param name="assessmentItem">評鑑項目</param>
        /// <param name="performanceLevel">Performance Level</param>
        /// <param name="userID"> 目前登入者 </param>
        /// <param name="cDate"> 目前時間 </param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<TET_SupplierSPAModel> GetSingleQuerySPAList(string[] belongTo, string[] period, string[] bu, string[] assessmentItem, string[] performanceLevel, string userID, DateTime cDate, Pager pager)
        {
            // 評鑑期間  單選、必選
            // 評鑑項目  單選、必選
            if (
                (period == null || period.Length == 0) ||
                (assessmentItem == null || assessmentItem.Length == 0)
                )
                throw new ArgumentException("評鑑期間、評鑑項目 為必要項目");


            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var orgQuery =
                        from item in context.TET_SupplierSPA
                        from item1 in context.TET_Parameters
                        from item2 in context.TET_Parameters
                        from item3 in context.TET_Parameters
                        from item4 in context.TET_Parameters
                        where item.ServiceFor == item1.ID.ToString() &&
                              item.AssessmentItem == item2.ID.ToString() &&
                              item.PerformanceLevel == item3.ID.ToString() &&
                              item.BU == item4.ID.ToString()
                        select new
                        {
                            item.ID,
                            item.BelongTo,
                            item.Period,
                            BUID = item4.ID,
                            BU = item4.Item,
                            ServiceFor = item1.Item,
                            AssessmentItemID = item2.ID,
                            AssessmentItem = item2.Item,
                            PerformanceLevelID = item3.ID,
                            PerformanceLevel = item3.Item,
                            TotalScore = item.TotalScore,
                            TScore = item.TScore,
                            DScore = item.DScore,
                            QScore = item.QScore,
                            CScore = item.CScore,
                            SScore = item.SScore,
                            SPAComment = item.Comment,
                            item.ApproveStatus,
                            item.CreateUser,
                            item.CreateDate,
                            item.ModifyUser,
                            item.ModifyDate
                        };

                    //--- 組合過濾條件 ---
                    if (belongTo != null && belongTo.Length > 0)
                        orgQuery = orgQuery.Where(obj => belongTo.Contains(obj.BelongTo));

                    if (period != null && period.Length > 0)
                        orgQuery = orgQuery.Where(obj => period.Contains(obj.Period));

                    if (bu != null && bu.Length > 0)
                        orgQuery = orgQuery.Where(obj => bu.Contains(obj.BUID.ToString().ToUpper()));

                    if (assessmentItem != null && assessmentItem.Length > 0)
                        orgQuery = orgQuery.Where(obj => assessmentItem.Contains(obj.AssessmentItemID.ToString().ToUpper()));

                    if (performanceLevel != null && performanceLevel.Length > 0)
                        orgQuery = orgQuery.Where(obj => performanceLevel.Contains(obj.PerformanceLevelID.ToString().ToUpper()));
                    //--- 組合過濾條件 ---


                    var query =
                        from item in orgQuery
                        select
                            new TET_SupplierSPAModel()
                            {
                                ID = item.ID,
                                BelongTo = item.BelongTo,
                                Period = item.Period,
                                BU = item.BU,
                                ServiceFor = item.ServiceFor,
                                AssessmentItem = item.AssessmentItem,
                                PerformanceLevel = item.PerformanceLevel,
                                TotalScore = item.TotalScore,
                                TScore = item.TScore,
                                DScore = item.DScore,
                                QScore = item.QScore,
                                CScore = item.CScore,
                                SScore = item.SScore,
                                CreateUser = item.CreateUser,
                                CreateDate = item.CreateDate,
                                ModifyUser = item.ModifyUser,
                                ModifyDate = item.ModifyDate,
                            };

                    query = query.OrderByDescending(obj => obj.CreateDate);
                    var list = query.ProcessPager(pager).ToList();

                    foreach(var item in list)
                    {
                        if (double.TryParse(item.TScore, out double tmpTScore))
                            item.TScore = tmpTScore.ToString("N2");

                        if (double.TryParse(item.DScore, out double tmpDScore))
                            item.DScore = tmpDScore.ToString("N2");

                        if (double.TryParse(item.QScore, out double tmpQScore))
                            item.QScore = tmpQScore.ToString("N2");

                        if (double.TryParse(item.CScore, out double tmpCScore))
                            item.CScore = tmpCScore.ToString("N2");

                        if (double.TryParse(item.SScore, out double tmpSScore))
                            item.SScore = tmpSScore.ToString("N2");

                        if (double.TryParse(item.TotalScore, out double tmpTotalScore))
                            item.TotalScore = tmpTotalScore.ToString("N2");
                    }

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
        /// 取得 供應商 SPA 清單
        /// </summary>
        /// <param name="belongTo">供應商</param>
        /// <param name="period">評鑑期間</param>
        /// <param name="bu">評鑑單位</param>
        /// <param name="assessmentItem">評鑑項目</param>
        /// <param name="userID"> 目前登入者 </param>
        /// <param name="cDate"> 目前時間 </param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<TET_SupplierSPAModel> GetMultiQuerySPAList(string[] belongTo, string[] period, string[] bu, string[] assessmentItem, string userID, DateTime cDate, Pager pager)
        {
            // 評鑑期間  複選、必選
            // 評鑑項目  複選、必選
            if (
                (period == null || period.Length == 0) ||
                (assessmentItem == null || assessmentItem.Length == 0)
                )
                throw new ArgumentException("評鑑期間、評鑑項目 為必要項目");


            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var orgQuery =
                        from item in context.TET_SupplierSPA
                        from item1 in context.TET_Parameters
                        from item2 in context.TET_Parameters
                        from item3 in context.TET_Parameters
                        from item4 in context.TET_Parameters
                        where item.ServiceFor == item1.ID.ToString() &&
                              item.AssessmentItem == item2.ID.ToString() &&
                              item.PerformanceLevel == item3.ID.ToString() &&
                              item.BU == item4.ID.ToString()
                        select new
                        {
                            item.ID,
                            item.BelongTo,
                            item.Period,
                            BUID = item4.ID,
                            BU = item4.Item,
                            ServiceFor = item1.Item,
                            AssessmentItemID = item2.ID,
                            AssessmentItem = item2.Item,
                            PerformanceLevelID = item3.ID,
                            PerformanceLevel = item3.Item,
                            TotalScore = item.TotalScore,
                            TScore = item.TScore,
                            DScore = item.DScore,
                            QScore = item.QScore,
                            CScore = item.CScore,
                            SScore = item.SScore,
                            SPAComment = item.Comment,
                            item.ApproveStatus,
                            item.CreateUser,
                            item.CreateDate,
                            item.ModifyUser,
                            item.ModifyDate
                        };


                    //--- 組合過濾條件 ---
                    if (belongTo != null && belongTo.Length > 0)
                        orgQuery = orgQuery.Where(obj => belongTo.Contains(obj.BelongTo));

                    if (period != null && period.Length > 0)
                        orgQuery = orgQuery.Where(obj => period.Contains(obj.Period));

                    if (bu != null && bu.Length > 0)
                        orgQuery = orgQuery.Where(obj => bu.Contains(obj.BUID.ToString().ToUpper()));

                    if (assessmentItem != null && assessmentItem.Length > 0)
                        orgQuery = orgQuery.Where(obj => assessmentItem.Contains(obj.AssessmentItemID.ToString().ToUpper()));

                    //--- 組合過濾條件 ---


                    var query =
                        from item in orgQuery
                        select
                            new TET_SupplierSPAModel()
                            {
                                ID = item.ID,
                                BelongTo = item.BelongTo,
                                Period = item.Period,
                                BU = item.BU,
                                ServiceFor = item.ServiceFor,
                                AssessmentItemID = item.AssessmentItemID.ToString().ToUpper(),
                                AssessmentItem = item.AssessmentItem,
                                PerformanceLevel = item.PerformanceLevel,
                                TotalScore = item.TotalScore,
                                TScore = item.TScore,
                                DScore = item.DScore,
                                QScore = item.QScore,
                                CScore = item.CScore,
                                SScore = item.SScore,
                                CreateUser = item.CreateUser,
                                CreateDate = item.CreateDate,
                                ModifyUser = item.ModifyUser,
                                ModifyDate = item.ModifyDate,
                            };

                    query = query.OrderByDescending(obj => obj.CreateDate);
                    var list = query.ProcessPager(pager).ToList();

                    foreach (var item in list)
                    {
                        if (double.TryParse(item.TScore, out double tmpTScore))
                            item.TScore = tmpTScore.ToString("N2");

                        if (double.TryParse(item.DScore, out double tmpDScore))
                            item.DScore = tmpDScore.ToString("N2");

                        if (double.TryParse(item.QScore, out double tmpQScore))
                            item.QScore = tmpQScore.ToString("N2");

                        if (double.TryParse(item.CScore, out double tmpCScore))
                            item.CScore = tmpCScore.ToString("N2");

                        if (double.TryParse(item.SScore, out double tmpSScore))
                            item.SScore = tmpSScore.ToString("N2");

                        if (double.TryParse(item.TotalScore, out double tmpTotalScore))
                            item.TotalScore = tmpTotalScore.ToString("N2");
                    }

                    return list;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                return default;
            }
        }
        #endregion

        #region CUD
        /// <summary> 新增 供應商 SPA </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public Guid CreateSPA(TET_SupplierSPAModel model, string userID, DateTime cDate)
        {
            if (model == null)
                throw new ArgumentNullException("Supplier is required.");

            // 新增前，先檢查是否能通過商業邏輯
            if (!SupplierSPAValidator.Valid(model, out List<string> msgList))
                throw new ArgumentException(string.Join(Environment.NewLine, msgList));

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var entity = new TET_SupplierSPA()
                    {
                        ID = Guid.NewGuid(),
                        BelongTo = model.BelongTo,
                        Period = model.Period,
                        BU = model.BU,
                        ServiceFor = model.ServiceFor,
                        AssessmentItem = model.AssessmentItem,
                        PerformanceLevel = model.PerformanceLevel,
                        TotalScore = model.TotalScore,
                        TScore = model.TScore,
                        DScore = model.DScore,
                        QScore = model.QScore,
                        CScore = model.CScore,
                        SScore = model.SScore,
                        Comment = model.SPAComment,
                        ApproveStatus = model.ApproveStatus,

                        CreateUser = userID,
                        CreateDate = cDate,
                        ModifyUser = userID,
                        ModifyDate = cDate,
                    };

                    context.TET_SupplierSPA.Add(entity);
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

        /// <summary> 修改 供應商 SPA </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        public void ModifySPA(TET_SupplierSPAModel model, string userID, DateTime cDate)
        {
            if (model == null)
                throw new ArgumentNullException("Model is required.");

            // 修改前，先檢查是否能通過商業邏輯
            if (!SupplierSPAValidator.Valid(model, out List<string> msgList))
                throw new ArgumentException(string.Join(Environment.NewLine, msgList));

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                    (from item in context.TET_SupplierSPA
                     where item.ID == model.ID
                     select item).FirstOrDefault();

                    if (dbModel == null)
                        throw new NullReferenceException($"{model.ID} don't exists.");

                    dbModel.BelongTo = model.BelongTo;
                    dbModel.Period = model.Period;
                    dbModel.BU = model.BU;
                    dbModel.ServiceFor = model.ServiceFor;
                    dbModel.AssessmentItem = model.AssessmentItem;
                    dbModel.PerformanceLevel = model.PerformanceLevel;
                    dbModel.TotalScore = model.TotalScore;
                    dbModel.TScore = model.TScore;
                    dbModel.DScore = model.DScore;
                    dbModel.QScore = model.QScore;
                    dbModel.CScore = model.CScore;
                    dbModel.SScore = model.SScore;
                    dbModel.Comment = model.SPAComment;
                    dbModel.ApproveStatus = model.ApproveStatus;
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

        /// <summary> 改版 供應商 SPA </summary>
        /// <param name="id">主鍵</param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void Revision(Guid id, string userID, DateTime cDate)
        {
            if (id == Guid.Empty || string.IsNullOrWhiteSpace(userID))
                throw new ArgumentNullException("ID, UserID is required");

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                    (from item in context.TET_SupplierSPA
                     where item.ID == id
                     select item).FirstOrDefault();

                    // 資料如果不存在，就視為已刪除
                    if (dbModel == null)
                        throw new NullReferenceException($"{id} don't exists.");

                    dbModel.ApproveStatus = null;
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

        #region Applicant
        /// <summary> 送出申請 </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void SubmitSPA(TET_SupplierSPAModel model, string userID, DateTime cDate)
        {
            if (model == null)
                throw new ArgumentNullException("Model is required.");

            var user = _userMgr.GetUser(userID);
            if (user == null)
                throw new Exception("User is required.");

            var leader = this._userMgr.GetUserLeader(userID);
            if (leader == null)
                throw new Exception($"{userID} 沒有主管資料");


            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    // 先查出原資料
                    var dbModel =
                    (from item in context.TET_SupplierSPA
                     where item.ID == model.ID
                     select item).FirstOrDefault();

                    if (dbModel == null)
                        throw new NullReferenceException($"{model.ID} don't exists.");

                    //--- 調整原資料的審核狀態 ---
                    // 如果審核狀態為 NULL 或是已退回，才允許送出申請
                    if (!string.IsNullOrWhiteSpace(dbModel.ApproveStatus) &&
                        ApprovalUtils.ParseApprovalStatus(dbModel.ApproveStatus) != ApprovalStatus.Rejected)
                        throw new Exception("送審狀態必須為空，或是已退回.");

                    // 更改原資料的審核狀態
                    dbModel.ApproveStatus = ApprovalStatus.Verify.ToText();
                    //--- 調整原資料的審核狀態 ---


                    //--- 新增審核資料 ---
                    // 建立新的申請資訊
                    var entity = new TET_SupplierSPAApproval()
                    {
                        ID = Guid.NewGuid(),
                        SPAID = dbModel.ID,
                        Type = ApprovalType.New.ToText(),
                        Level = ApprovalLevel.User_GL.ToText(),
                        Description = $"{ApprovalType.New.ToText()}_{dbModel.BelongTo}",
                        Approver = leader.ID,
                        CreateUser = userID,
                        CreateDate = cDate,
                        ModifyUser = userID,
                        ModifyDate = cDate,
                    };


                    // 寄送通知信
                    ApprovalMailUtil.SendNewApprovalMail(leader.EMail, entity, dbModel, userID, cDate);

                    context.TET_SupplierSPAApproval.Add(entity);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> 刪除 SPA </summary>
        /// <param name="id">主鍵</param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void DeleteTET_SPA(Guid id, string userID, DateTime cDate)
        {
            if (id == Guid.Empty || string.IsNullOrWhiteSpace(userID))
                throw new ArgumentNullException("ID, UserID is required");

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                        (from item in context.TET_SupplierSPA
                         where item.ID == id
                         select item).FirstOrDefault();

                    // 資料如果不存在，就視為已刪除
                    if (dbModel == null)
                        return;

                    context.TET_SupplierSPA.Remove(dbModel);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> 中止 供應商 審核</summary>
        /// <param name="id">主鍵</param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void AbordTET_SPA(Guid id, string userID, DateTime cDate)
        {
            if (id == Guid.Empty || string.IsNullOrWhiteSpace(userID))
                throw new ArgumentNullException("ID, UserID is required");

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                        (from item in context.TET_SupplierSPA
                         where item.ID == id
                         select item).FirstOrDefault();

                    // 資料如果不存在
                    if (dbModel == null)
                        return;

                    // 只有審核中的能中止
                    if (ApprovalUtils.ParseApprovalStatus(dbModel.ApproveStatus) != ApprovalStatus.Verify)
                        return;

                    // 改為未送出的狀態
                    dbModel.ApproveStatus = null;

                    // 審核資料
                    var approvalList =
                        (from item in context.TET_SupplierSPAApproval
                         where item.SPAID == id
                         select item).ToList();

                    // 刪除尚未審核的審核資料
                    var nonResultApprovalList = approvalList.Where(item => string.IsNullOrEmpty(item.Result)).ToList();
                    context.TET_SupplierSPAApproval.RemoveRange(nonResultApprovalList);
                    context.SaveChanges();

                    // 寄送通知信
                    var receivers = approvalList.Select(obj => obj.Approver).ToList();
                    var userList = this._userMgr.GetUserList(receivers);
                    var mailList = userList.Select(obj => obj.EMail).ToList();


                    ApprovalMailUtil.SendAbordMail(mailList, dbModel, userID, cDate);
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }
        #endregion
    }
}
