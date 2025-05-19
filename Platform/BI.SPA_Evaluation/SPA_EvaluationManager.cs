using BI.SPA_Evaluation.Models;
using Platform.AbstractionClass;
using Platform.Infra;
using Platform.Auth;
using Platform.LogService;
using Platform.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using BI.Shared.Extensions;
using BI.SPA_Evaluation.Enums;
using BI.SPA_ScoringInfo.Models;
using BI.SPA_Violation.Models;
using BI.SPA_CostService.Models;
using BI.Shared.Utils;
using BI.Suppliers;
using BI.Shared;
using BI.SPA_Evaluation.Validators;
using Platform.FileSystem;
using System.IO;
using System.Web.Hosting;
using BI.SPA_Evaluation.Utils;
using BI.SPA_ApproverSetup;
using BI.SPA_Evaluation.Models.Exporting;
using BI.SPA_ApproverSetup.Enums;

namespace BI.SPA_Evaluation
{
    public class SPA_EvaluationManager
    {
        private Logger _logger = new Logger();
        private Calculator _caculator = new Calculator();
        private UserManager _userMgr = new UserManager();
        private UserRoleManager _userRoleMgr = new UserRoleManager();
        private TET_SupplierManager _supplierMgr = new TET_SupplierManager();
        private TET_ParametersManager _parameterMgr = new TET_ParametersManager();
        private SPA_ApproverSetupManager _setupMgr = new SPA_ApproverSetupManager();

        #region Read SPA_Eva_Period
        /// <summary> 取得 供應商SPA評鑑 清單 </summary>
        /// <param name="id">Key</param>
        /// <returns></returns>
        public SPA_Eva_PeriodModel GetOnePeriod(Guid id)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in context.TET_SPA_Period
                        where item.ID == id
                        select new SPA_Eva_PeriodModel
                        {
                            ID = item.ID,
                            Period = item.Period,
                            Status=item.Status,
                            CreateUser = item.CreateUser,
                            CreateDate = item.CreateDate,
                            ModifyUser = item.ModifyUser,
                            ModifyDate = item.ModifyDate,
                        };

                    var result = query.FirstOrDefault();

                    if (result != null)
                    {
                        // SPA_ScoringInfo
                        var detailQuery =
                            from item in context.TET_SPA_ScoringInfo
                            where item.Period == result.Period
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
                            };

                        var scoringInfoList = detailQuery.ToList();
                        result.ScoringInfoList = scoringInfoList;

                        // SPA_Violation
                        var violationQuery =
                            from item in context.TET_SPA_Violation
                            where item.Period == result.Period
                            select new SPA_ViolationModel
                            {
                                ID = item.ID,
                                Period = item.Period,
                                ApproveStatus = item.ApproveStatus
                            };

                        var violationList = violationQuery.ToList();
                        result.ViolationList = violationList;
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
        #endregion


        #region Read SPA_EvaluationReport
        /// <summary> 取得 SPA 績效評鑑報告維護  清單 </summary>
        /// <param name="period">評鑑期間</param>
        /// <param name="userID"> 目前登入者 </param>
        /// <param name="cDate"> 目前時間 </param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<SPA_EvaluationReportModel> GetList(string period, string userID, DateTime cDate, Pager pager)
        {
            // 目前登入者是否具 QSM 身份
            bool isQSM = this._userRoleMgr.IsRole(userID, BI.SPA_Evaluation.Enums.ApprovalRole.QSM.ToID().Value);
            bool isSS = this._userRoleMgr.IsRole(userID, BI.SPA_Evaluation.Enums.ApprovalRole.SRI_SS.ToID().Value);
            bool isSS_GL = this._userRoleMgr.IsRole(userID, BI.SPA_Evaluation.Enums.ApprovalRole.SRI_SS_GL.ToID().Value);

            var setupList = this._setupMgr.GetList(new List<Guid>(), new List<Guid>(), userID, cDate, new Pager() { AllowPaging = false });

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var orgQuery =
                        from item in context.TET_SPA_EvaluationReport
                        select item;

                    //--- 組合過濾條件 ---
                    if (!string.IsNullOrWhiteSpace(period))
                        orgQuery = orgQuery.Where(obj => obj.Period.Contains(period));
                    //--- 組合過濾條件 ---

                    var query =
                        from item in orgQuery
                        select
                            new SPA_EvaluationReportModel()
                            {
                                ID = item.ID,
                                Period = item.Period,
                                BU = item.BU,
                                CreateUser = item.CreateUser,
                                CreateDate = item.CreateDate,
                                ModifyUser = item.ModifyUser,
                                ModifyDate = item.ModifyDate,
                            };

                    query = query.OrderByDescending(obj => obj.CreateDate);
                    var list = query.ToList();



                    var neededPeriodList = list.Select(obj => obj.Period);
                    var periodList = context.TET_SPA_Period.Where(obj => neededPeriodList.Contains(obj.Period)).ToList();

                    var retList = new List<SPA_EvaluationReportModel>();
                    foreach (var item in list)
                    {
                        item.IsQSM = isQSM;
                        // 若登入者擁有QSM角色、SRI_SS、SRI_SS_GL，可查詢全部資料。
                        if (isQSM || isSS || isSS_GL)
                            retList.Add(item);
                        else
                        {
                            // 若登入者沒有QSM角色，登入者只能查詢到在供應商SPA評鑑審核者設定功能中，對應的評鑑項目、評鑑單位設定的計分資料填寫者、計分資料確認者、第一關審核者、第二關審核者欄位有維護登入者工號的評鑑單位資料。
                            var setups = setupList.Where(obj => obj.BUText == item.BU);

                            foreach (var item2 in setups)
                            {
                                var configUserIds = new List<string>() { item2.InfoConfirm, item2.Lv1Apprvoer, item2.Lv2Apprvoer };
                                configUserIds.AddRange(item2.InfoFills);

                                if (configUserIds.Contains(userID))
                                    retList.Add(item);
                            }
                        }

                        var periodItem = periodList.Where(obj => obj.Period == item.Period).FirstOrDefault();
                        if (periodItem == null || periodItem.Status != BI.SPA_ApproverSetup.Enums.SPA_Period_Status.Executing.ToText())
                            item.CanEdit = false;
                        else
                            item.CanEdit = true;
                    }

                    retList = retList.ProcessPager(pager).ToList();
                    return retList;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                return default;
            }
        }

        /// <summary> 取得 SPA 績效評鑑報告維護 </summary>
        /// <param name="id">Key</param>
        /// <param name="userID"> 目前登入者 </param>
        /// <param name="cDate"> 目前時間 </param>
        /// <returns></returns>
        public SPA_EvaluationReportModel GetOne(Guid id, string userID, DateTime cDate)
        {
            // 目前登入者是否具 QSM 身份
            bool isQSM = this._userRoleMgr.IsRole(userID, BI.SPA_Evaluation.Enums.ApprovalRole.QSM.ToID().Value);


            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in context.TET_SPA_EvaluationReport
                        where item.ID == id
                        select new SPA_EvaluationReportModel
                        {
                            ID = item.ID,
                            Period = item.Period,
                            BU = item.BU,
                            CreateUser = item.CreateUser,
                            CreateDate = item.CreateDate,
                            ModifyUser = item.ModifyUser,
                            ModifyDate = item.ModifyDate,
                        };

                    var result = query.FirstOrDefault();

                    if (result != null)
                    {
                        var attachmentQuery =
                            from item in context.TET_SPA_EvaluationReportAttachments
                            where item.EPID == id
                            select new SPA_EvaluationReportAttachmentModel
                            {
                                ID = item.ID,
                                EPID = item.EPID,
                                FileCategory = item.FileCategory,
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

                        if (!isQSM)
                        {
                            var allText = FileCategory.All.ToText();
                            attachmentQuery = attachmentQuery.Where(obj => obj.FileCategory == allText);
                        }

                        result.AttachmentList = attachmentQuery.ToList();
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
        #endregion


        #region Exporting
        /// <summary>
        /// 取得 供應商SPA評鑑 匯出清單
        /// </summary>
        /// <param name="period">評鑑期間</param>
        /// <param name="userID"> 目前登入者 </param>
        /// <param name="cDate"> 目前時間 </param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<SPA_EvaluationExportModel> GetExportList(string period, string userID, DateTime cDate)
        {
            if (!string.IsNullOrWhiteSpace(period) && !PeriodUtil.IsPeriodFormat(period))
                throw new Exception("請輸入正確的評鑑期間");

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    //----- 主表資料 -----
                    var query =
                        from item in context.TET_SPA_Evaluation
                        select item;


                    //--- 組合過濾條件 ---
                    if (!string.IsNullOrWhiteSpace(period))
                        query = query.Where(obj => obj.Period.Contains(period));
                    //--- 組合過濾條件 ---

                    query = query.OrderByDescending(obj => obj.CreateDate);
                    var list = query.ToList();
                    //----- 主表資料 -----


                    //--- 處理輸出資料 ---
                    var retList = new List<SPA_EvaluationExportModel>();

                    foreach (var mainItem in list)
                    {
                        var item = new SPA_EvaluationExportModel()
                        {
                            ID = mainItem.ID,
                            Period = mainItem.Period,
                            BU = mainItem.BU,
                            ServiceItem = mainItem.ServiceItem,
                            ServiceFor = mainItem.ServiceFor,
                            BelongTo = mainItem.BelongTo,
                            POSource = mainItem.POSource,
                            PerformanceLevel = mainItem.PerformanceLevel,
                            TotalScore = mainItem.TotalScore,
                            TScore = mainItem.TScore,
                            DScore = mainItem.DScore,
                            QScore = mainItem.QScore,
                            CScore = mainItem.CScore,
                            SScore = mainItem.SScore,
                            TScore1 = mainItem.TScore1,
                            TScore2 = mainItem.TScore2,
                            DScore1 = mainItem.DScore1,
                            DScore2 = mainItem.DScore2,
                            QScore1 = mainItem.QScore1,
                            QScore2 = mainItem.QScore2,
                            CScore1 = mainItem.CScore1,
                            CScore2 = mainItem.CScore2,
                            SScore1 = mainItem.SScore1,
                            SScore2 = mainItem.SScore2,
                        };

                        // 起始結束時間
                        var periodDate = PeriodUtil.ParsePeriod(item.Period);
                        item.PeriodStart = periodDate?.StartDate?.ToString("yyyy-MM-dd");
                        item.PeriodEnd = periodDate?.EndDate?.ToString("yyyy-MM-dd");

                        retList.Add(item);
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


        #region CUD - Evaluation
        /// <summary> 計算分數 </summary>
        /// <param name="period"> 評鑑期間 </param>
        /// <param name="userID"> 目前登入者 </param>
        /// <param name="cDate"> 現在時間 </param>
        public void ComputeScore(string period, string userID, DateTime cDate)
        {
            if (!PeriodUtil.IsPeriodFormat(period))
                throw new Exception("請輸入正確的評鑑期間");


            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in context.TET_SPA_Period
                        where item.Period == period
                        select new SPA_Eva_PeriodModel
                        {
                            ID = item.ID,
                            Period = item.Period,
                        };

                    var result = query.FirstOrDefault();

                    // 取得原資料
                    if (result != null)
                    {
                        // SPA_ScoringInfo
                        result.ScoringInfoList = GetScoringInfoList(context, period);

                        // SPA_Violation
                        result.ViolationList = GetViolationList(context, period);

                        // SPA_CostService
                        result.CostServiceList = GetCostServiceList(context, period);
                    }

                    // 計算分數
                    var evalutionList = this._caculator.ComputeAllScore(result, userID, cDate);


                    //--- 將計分資料存到SPA 評鑑結果(TET_SPA_Evaluation) ---
                    foreach (var item in evalutionList)
                    {
                        var dbEntity =
                            (from obj in context.TET_SPA_Evaluation
                             where
                                 obj.Period == item.Period &&
                                 obj.BU == item.BU &&
                                 obj.ServiceItem == item.ServiceItem &&
                                 obj.ServiceFor == item.ServiceFor &&
                                 obj.BelongTo == item.BelongTo &&
                                 obj.POSource == item.POSource
                             select obj).FirstOrDefault();

                        // 如果查不到就新增，查得到就更新屬性
                        if (dbEntity == null)
                        {
                            context.TET_SPA_Evaluation.Add(item);
                            continue;
                        }
                        else
                        {
                            dbEntity.PerformanceLevel = item.PerformanceLevel;
                            dbEntity.TotalScore = item.TotalScore;

                            dbEntity.TScore = item.TScore;
                            dbEntity.DScore = item.DScore;
                            dbEntity.QScore = item.QScore;
                            dbEntity.CScore = item.CScore;
                            dbEntity.SScore = item.SScore;

                            dbEntity.TScore1 = item.TScore1;
                            dbEntity.TScore2 = item.TScore2;
                            dbEntity.DScore1 = item.DScore1;
                            dbEntity.DScore2 = item.DScore2;
                            dbEntity.QScore1 = item.QScore1;
                            dbEntity.QScore2 = item.QScore2;
                            dbEntity.CScore1 = item.CScore1;
                            dbEntity.CScore2 = item.CScore2;
                            dbEntity.SScore1 = item.SScore1;
                            dbEntity.SScore2 = item.SScore2;

                            dbEntity.ModifyUser = userID;
                            dbEntity.ModifyDate = cDate;
                        }
                    }
                    //--- 將計分資料存到SPA 評鑑結果(TET_SPA_Evaluation) ---


                    //--- 將計分資料存到供應商平台的SPA資料 (TET_SupplierSPA) ---
                    var belongToList = this._supplierMgr.GetBelongToList();
                    var param_buList = this._parameterMgr.GetTET_ParametersList("SPA評鑑單位");
                    var param_serviceForList = this._parameterMgr.GetTET_ParametersList("SPA服務對象");
                    var param_assessmentItemList = this._parameterMgr.GetTET_ParametersList("SPA評鑑項目");
                    var param_spaLevelList = this._parameterMgr.GetTET_ParametersList("SPA Level");

                    foreach (var item in evalutionList)
                    {
                        var belongTo = item.BelongTo;
                        var bu = param_buList.Where(obj => obj.Item == item.BU).FirstOrDefault().ID.ToString();
                        var serviceFor = param_serviceForList.Where(obj => obj.Item == item.ServiceFor).FirstOrDefault().ID.ToString();
                        var assessmentItem = param_assessmentItemList.Where(obj => obj.Item == item.ServiceItem).FirstOrDefault().ID.ToString();
                        var spaLevel = param_spaLevelList.Where(obj => obj.Item == item.PerformanceLevel).FirstOrDefault()?.ID.ToString();

                        var dbEntity =
                            (from obj in context.TET_SupplierSPA
                             where
                                 obj.Period == item.Period &&
                                 obj.BelongTo == belongTo &&
                                 obj.BU == bu &&
                                 obj.ServiceFor == serviceFor &&
                                 obj.AssessmentItem == assessmentItem
                             select obj).FirstOrDefault();

                        // 如果查不到就新增，查得到就更新屬性
                        if (dbEntity == null)
                        {
                            TET_SupplierSPA supplierSPA = new TET_SupplierSPA()
                            {
                                ID = Guid.NewGuid(),
                                Period = item.Period,
                                BelongTo = belongTo,
                                BU = bu,
                                ServiceFor = serviceFor,
                                AssessmentItem = assessmentItem,

                                PerformanceLevel = spaLevel ?? "NA",
                                TotalScore = item.TotalScore ?? "NA",
                                TScore = item.TScore ?? "NA",
                                DScore = item.DScore ?? "NA",
                                QScore = item.QScore ?? "NA",
                                CScore = item.CScore ?? "NA",
                                SScore = item.SScore ?? "NA",
                                CreateUser = userID,
                                CreateDate = cDate,
                                ModifyUser = userID,
                                ModifyDate = cDate,
                            };

                            context.TET_SupplierSPA.Add(supplierSPA);
                        }
                        else
                        {
                            dbEntity.PerformanceLevel = spaLevel ?? "NA";
                            dbEntity.TotalScore = item.TotalScore ?? "NA";
                            dbEntity.TScore = item.TScore ?? "NA";
                            dbEntity.DScore = item.DScore ?? "NA";
                            dbEntity.QScore = item.QScore ?? "NA";
                            dbEntity.CScore = item.CScore ?? "NA";
                            dbEntity.SScore = item.SScore ?? "NA";
                            dbEntity.ModifyUser = userID;
                            dbEntity.ModifyDate = cDate;
                        }
                    }
                    //--- 將計分資料存到供應商平台的SPA資料 (TET_SupplierSPA) ---


                    // 依該評鑑期間中包含的評鑑單位，個別產生一筆績效評鑑報告資料 (TET_SPA_EvaluationReport)
                    var buList = result.ScoringInfoList.Select(obj => obj.BU).Distinct().ToList();
                    foreach (var item in buList)
                    {
                        var dbEntity =
                            (from obj in context.TET_SPA_EvaluationReport
                             where
                                 obj.Period == period &&
                                 obj.BU == item
                             select obj).FirstOrDefault();

                        if (dbEntity == null)
                        {
                            var report = new TET_SPA_EvaluationReport()
                            {
                                ID = Guid.NewGuid(),
                                BU = item,
                                Period = period,

                                CreateUser = userID,
                                CreateDate = cDate,
                                ModifyUser = userID,
                                ModifyDate = cDate,
                            };

                            context.TET_SPA_EvaluationReport.Add(report);
                        }
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> 讀取指定評鑑期間的 Violation </summary>
        /// <param name="context"></param>
        /// <param name="period"> 評鑑期間 </param>
        /// <returns></returns>
        private List<SPA_ViolationModel> GetViolationList(PlatformContextModel context, string period)
        {
            // SPA_Violation
            var mainQuery =
                from item in context.TET_SPA_Violation
                where item.Period == period && item.ApproveStatus == "已完成"
                select new SPA_ViolationModel
                {
                    ID = item.ID,
                    Period = item.Period,
                    ApproveStatus = item.ApproveStatus
                };

            var list = mainQuery.ToList();
            var idList = list.Select(obj => obj.ID).ToList();


            // 查詢明細
            var detailQuery =
                from item in context.TET_SPA_ViolationDetail
                where idList.Contains(item.ViolationID)
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
                };

            var detailList = detailQuery.ToList();
            foreach (var item in list)
            {
                item.DetailList = detailList.Where(obj => obj.ViolationID == item.ID).ToList();
            }

            return list;
        }

        /// <summary> 讀取指定評鑑期間的 ScoringInfo </summary>
        /// <param name="context"></param>
        /// <param name="period"> 評鑑期間 </param>
        /// <returns></returns>
        private List<SPA_ScoringInfoModel> GetScoringInfoList(PlatformContextModel context, string period)
        {
            var mainQuery =
                from item in context.TET_SPA_ScoringInfo
                where item.Period == period && item.ApproveStatus=="已完成"
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
                };

            var scoringInfoList = mainQuery.ToList();
            var idList = scoringInfoList.Select(obj => obj.ID).ToList();

            var m1Query =
                from item in context.TET_SPA_ScoringInfoModule1
                where idList.Contains(item.SIID)
                select new SPA_ScoringInfoModule1Model
                {
                    ID = item.ID,
                    SIID = item.SIID,
                    Source = item.Source,
                    Type = item.Type,
                    Supplier = item.Supplier,
                    EmpName = item.EmpName,
                    MajorJob = item.MajorJob,
                    IsIndependent = item.IsIndependent,
                    SkillLevel = item.SkillLevel,
                    EmpStatus = item.EmpStatus,
                    TELSeniorityY = item.TELSeniorityY,
                    TELSeniorityM = item.TELSeniorityM,
                    Remark = item.Remark,
                };

            var m2Query =
                from item in context.TET_SPA_ScoringInfoModule2
                where idList.Contains(item.SIID)
                select new SPA_ScoringInfoModule2Model
                {
                    ID = item.ID,
                    SIID = item.SIID,
                    ServiceFor = item.ServiceFor,
                    WorkItem = item.WorkItem,
                    MachineName = item.MachineName,
                    MachineNo = item.MachineNo,
                    OnTime = item.OnTime,
                    Remark = item.Remark,
                };

            var m3Query =
                from item in context.TET_SPA_ScoringInfoModule3
                where idList.Contains(item.SIID)
                select new SPA_ScoringInfoModule3Model
                {
                    ID = item.ID,
                    SIID = item.SIID,
                    Date = item.Date,
                    Location = item.Location,
                    TELLoss = item.TELLoss,
                    CustomerLoss = item.CustomerLoss,
                    Accident = item.Accident,
                    Description = item.Description,
                };

            var m4Query =
                from item in context.TET_SPA_ScoringInfoModule4
                where idList.Contains(item.SIID)
                select new SPA_ScoringInfoModule4Model
                {
                    ID = item.ID,
                    SIID = item.SIID,
                    Date = item.Date,
                    Location = item.Location,
                    IsDamage = item.IsDamage,
                    Description = item.Description,
                };


            foreach (var item in scoringInfoList)
            {
                item.Module1List = m1Query.Where(obj => obj.SIID == item.ID).ToList();
                item.Module2List = m2Query.Where(obj => obj.SIID == item.ID).ToList();
                item.Module3List = m3Query.Where(obj => obj.SIID == item.ID).ToList();
                item.Module4List = m4Query.Where(obj => obj.SIID == item.ID).ToList();
            }


            return scoringInfoList;
        }

        /// <summary> 讀取指定評鑑期間的 CostService </summary>
        /// <param name="context"></param>
        /// <param name="period"> 評鑑期間 </param>
        /// <returns></returns>
        private List<SPA_CostServiceModel> GetCostServiceList(PlatformContextModel context, string period)
        {
            var query =
                from item in context.TET_SPA_CostService
                where item.Period == period && item.ApproveStatus == "已完成"
                select new SPA_CostServiceModel
                {
                    ID = item.ID,
                    Period = item.Period,
                    Filler = item.Filler,
                    ApproveStatus = item.ApproveStatus,
                };

            var list = query.ToList();
            var idList = list.Select(obj => obj.ID).ToList();


            // 查詢明細
            var detailQuery =
                from item in context.TET_SPA_CostServiceDetail
                where idList.Contains(item.CSID)
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
                };

            var detailList = detailQuery.ToList();
            foreach (var item in list)
            {
                item.DetailList = detailList.Where(obj => obj.CSID == item.ID).ToList();
            }

            return list;
        }
        #endregion


        #region CUD - EvaluationReport
        /// <summary> 修改 SPA 績效評鑑報告維護 </summary>
        /// <param name="model"></param>
        /// <param name="attachmentList_QSM"></param>
        /// <param name="attachmentList_All"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void Modify(SPA_EvaluationReportModel model, List<FileContent> attachmentList_QSM, List<FileContent> attachmentList_All, string userID, DateTime cDate)
        {
            if (model == null)
                throw new ArgumentNullException("Model is required.");


            //--- 先檢查是否能通過商業邏輯 ---
            List<string> msgList = new List<string>();
            var validResult = SPA_EvaluationReportValidator.Valid(model, out msgList);
            if (!validResult)
                throw new ArgumentException(string.Join(Environment.NewLine, msgList));
            //--- 先檢查是否能通過商業邏輯 ---


            // 目前登入者是否具 QSM 身份
            bool isQSM = this._userRoleMgr.IsRole(userID, BI.SPA_Evaluation.Enums.ApprovalRole.QSM.ToID().Value);


            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                        (from item in context.TET_SPA_EvaluationReport
                         where item.ID == model.ID
                         select item).FirstOrDefault();

                    if (dbModel == null)
                        throw new NullReferenceException($"{model.ID} don't exists.");


                    // 附件清單
                    var attachQuery = context.TET_SPA_EvaluationReportAttachments.Where(item => item.EPID == model.ID);
                    var allText = FileCategory.All.ToText();

                    // 如果是 QSM 可以管理全部檔案，不然只能管理 All
                    if (!isQSM)
                        attachQuery = attachQuery.Where(obj => obj.FileCategory == allText);
                    var attachList = attachQuery.ToList();
                    var attachIdList = attachList.Select(obj => obj.ID).ToList();
                    var inpAttachIdList = model.AttachmentList.Select(obj => obj.ID).ToList();

                    //--- 找到已移除的附檔，並刪除 ---
                    var notExistDetailId = attachIdList.Except(inpAttachIdList).ToList();
                    var willRemoveAttachment = attachList.Where(item => notExistDetailId.Contains(item.ID));
                    if (willRemoveAttachment.Any())
                    {
                        this.DropFile(willRemoveAttachment);
                        context.TET_SPA_EvaluationReportAttachments.RemoveRange(willRemoveAttachment);
                    }
                    //--- 找到已移除的附檔，並刪除 ---


                    // 新增檔案 - QSM
                    foreach (var fileItem in attachmentList_QSM)
                    {
                        var attachmEnttity = this.WriteFile(dbModel.ID, fileItem, FileCategory.QSM, userID, cDate);
                        context.TET_SPA_EvaluationReportAttachments.Add(attachmEnttity);
                    }

                    // 新增檔案 - All
                    foreach (var fileItem in attachmentList_All)
                    {
                        var attachmEnttity = this.WriteFile(dbModel.ID, fileItem, FileCategory.All, userID, cDate);
                        context.TET_SPA_EvaluationReportAttachments.Add(attachmEnttity);
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }


        /// <summary> 發送 SPA 績效評鑑報告維護 通知 </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void SendMessage(SPA_EvaluationReportModel model, string userID, DateTime cDate)
        {
            if (model == null)
                throw new ArgumentNullException("Model is required.");


            //--- 先檢查是否能通過商業邏輯 ---
            List<string> msgList = new List<string>();
            var validResult = SPA_EvaluationReportValidator.Valid(model, out msgList);
            if (!validResult)
                throw new ArgumentException(string.Join(Environment.NewLine, msgList));
            //--- 先檢查是否能通過商業邏輯 ---

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                        (from item in context.TET_SPA_EvaluationReport
                         where item.ID == model.ID
                         select item).FirstOrDefault();

                    if (dbModel == null)
                        throw new NullReferenceException($"{model.ID} don't exists.");


                    var setupList = this._setupMgr.GetList(new List<Guid>(), new List<Guid>(), userID, cDate, new Pager() { AllowPaging = false });


                    foreach (var item in setupList)
                    {
                        if (item.BUText == dbModel.BU)
                        {
                            var receivers = new List<string>() { item.InfoConfirm, item.Lv1Apprvoer, item.Lv2Apprvoer };

                            var users = this._userMgr.GetUserList_AccountModel(receivers.ToArray());

                            MailUtil.SendMessageMail(users, dbModel, userID, cDate);
                        }
                    }
                    //context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }
        #endregion


        #region Attachment
        /// <summary> 查詢附件 </summary>
        /// <param name="id"> 附件 Id </param>
        /// <returns></returns>
        public SPA_EvaluationReportAttachmentModel GetAttachment(Guid id)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    // 查詢附件
                    var query =
                        from item in context.TET_SPA_EvaluationReportAttachments
                        where item.ID == id
                        select new SPA_EvaluationReportAttachmentModel
                        {
                            ID = item.ID,
                            EPID = item.EPID,
                            FileCategory = item.FileCategory,
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


        #region Private
        /// <summary> 寫入檔案，並回傳附件 </summary>
        /// <param name="reportId"></param>
        /// <param name="file"></param>
        /// <param name="category"></param>
        /// <param name="userID"></param>
        /// <param name="cDate"></param>
        /// <returns></returns>
        private TET_SPA_EvaluationReportAttachments WriteFile(Guid reportId, FileContent file, FileCategory category, string userID, DateTime cDate)
        {
            // 將前端傳入的檔案寫入檔案系統及資料表
            // 計算起始路徑，如果不是上傳資料夾根目錄，要附加在最前面
            string rootFolder = MediaFileManager.GetRootFolder();
            string filePath = Path.Combine(rootFolder, ModuleConfig.FolderPath);

            if (!filePath.StartsWith(rootFolder, StringComparison.OrdinalIgnoreCase))
                filePath = Path.Combine(rootFolder, ModuleConfig.FolderPath);

            string folderPath = HostingEnvironment.MapPath("~/" + filePath);

            var newFileName = FileUtility.Upload(file, folderPath);
            var entity = new TET_SPA_EvaluationReportAttachments()
            {
                ID = Guid.NewGuid(),
                EPID = reportId,
                FileName = newFileName,
                FileCategory = category.ToText(),
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
        private void DropFile(IEnumerable<TET_SPA_EvaluationReportAttachments> attachments)
        {
            foreach (var item in attachments)
            {
                FileUtility.DeleteFile(item.FilePath);
            }
        }
        #endregion
    }
}
