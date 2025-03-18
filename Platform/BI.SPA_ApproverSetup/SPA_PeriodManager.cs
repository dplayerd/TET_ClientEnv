using BI.Shared;
using BI.Shared.Utils;
using BI.SPA_ApproverSetup.Enums;
using BI.SPA_ApproverSetup.Models;
using BI.SPA_ApproverSetup.Utils;
using BI.SPA_ApproverSetup.Validators;
using Newtonsoft.Json;
using Platform.AbstractionClass;
using Platform.Auth;
using Platform.Infra;
using Platform.LogService;
using Platform.Messages;
using Platform.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace BI.SPA_ApproverSetup
{
    public class SPA_PeriodManager
    {
        private Logger _logger = new Logger();
        private UserManager _userMgr = new UserManager();
        private UserRoleManager _roleMgr = new UserRoleManager();
        private TET_ParametersManager _paramMgr = new TET_ParametersManager();
        private string _serviceItem_Safety = "Safety";
        private string _bu_EHS = "EHS";

        private const string _paraKey_AssessmentItems = "SPA評鑑項目";
        private const string _paraKey_EvaluationUnit = "SPA評鑑單位";


        /// <summary> 取得 供應商SPA評鑑期間設定 清單 </summary>
        /// <param name="periodText">評鑑期間(模糊搜尋)</param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<TET_SPA_PeriodModel> GetList(string periodText, string userID, DateTime cDate, Pager pager)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var baseQuery =
                        from item in context.TET_SPA_Period
                        select item;

                    if (!string.IsNullOrWhiteSpace(periodText))
                        baseQuery = baseQuery.Where(obj => obj.Period.Contains(periodText));

                    var query =
                        from item in baseQuery
                        orderby item.Period descending
                        select new TET_SPA_PeriodModel
                        {
                            ID = item.ID,
                            Period = item.Period,
                            Status = item.Status,
                            CreateUser = item.CreateUser,
                            CreateDate = item.CreateDate,
                            ModifyUser = item.ModifyUser,
                            ModifyDate = item.ModifyDate,
                        };

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

        /// <summary> 查詢 已啟動的供應商SPA評鑑期間設定 資料 </summary>
        /// <param name="periodId"> 評鑑期間 </param>
        /// <returns></returns>
        public TET_SPA_PeriodModel GetStartingDetail()
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    string status = SPA_Period_Status.Executing.ToText();

                    var query =
                        from item in context.TET_SPA_Period
                        where
                            item.Status == status
                        select new TET_SPA_PeriodModel
                        {
                            Period = item.Period,
                            Status = item.Status,
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


        /// <summary> 查詢 供應商SPA評鑑期間設定 資料 </summary>
        /// <param name="periodId"> 評鑑期間 </param>
        /// <returns></returns>
        public TET_SPA_PeriodModel GetDetail(Guid periodId)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in context.TET_SPA_Period
                        where
                            item.ID == periodId
                        select new TET_SPA_PeriodModel
                        {
                            Period = item.Period,
                            Status = item.Status,
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

        #region CUD
        /// <summary> 新增 供應商SPA評鑑期間設定 </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public TET_SPA_PeriodModel Create(TET_SPA_PeriodModel model, string userID, DateTime cDate)
        {
            if (model == null)
                throw new ArgumentNullException("Model is required.");

            // 新增前，先檢查是否能通過商業邏輯
            if (!SPA_PeriodValidator.Valid(model, out List<string> msgList))
                throw new ArgumentException(string.Join(Environment.NewLine, msgList));

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                        (from item in context.TET_SPA_Period
                         where item.Period.ToUpper() == model.Period.ToUpper()
                         select item).FirstOrDefault();

                    if (dbModel != null)
                        throw new Exception($"{model.Period} 為重覆資料.");


                    var entity = new TET_SPA_Period()
                    {
                        ID = Guid.NewGuid(),
                        Period = model.Period.ToUpper(),
                        Status = SPA_Period_Status.Ready.ToText(),

                        CreateUser = userID,
                        CreateDate = cDate,
                        ModifyUser = userID,
                        ModifyDate = cDate,
                    };

                    context.TET_SPA_Period.Add(entity);
                    context.SaveChanges();
                    return model;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> 修改 供應商SPA評鑑期間設定 </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void Modify(TET_SPA_PeriodModel model, string userID, DateTime cDate)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Period))
                throw new ArgumentNullException("Period is required.");

            // 修改前，先檢查是否能通過商業邏輯
            if (!SPA_PeriodValidator.Valid(model, out List<string> msgList))
                throw new ArgumentException(string.Join(Environment.NewLine, msgList));

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                        (from item in context.TET_SPA_Period
                         where item.ID == model.ID
                         select item).FirstOrDefault();

                    if (dbModel == null)
                        throw new Exception($"{model.Period} doesn't existed.");


                    // 檢查重複資料
                    var dbModel_existed = context.TET_SPA_Period.Where(item => item.Period.ToUpper() == model.Period.ToUpper()).FirstOrDefault();
                    if (dbModel_existed != null)
                        throw new Exception($"{model.Period} 為重覆資料.");


                    dbModel.Period = model.Period;
                    //dbModel.Status = model.Status;

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
        #endregion

        #region Change-Status
        /// <summary> 修改 供應商SPA評鑑期間設定 </summary>
        /// <param name="id"> 評鑑期間代碼 </param>
        /// <param name="status"> 評鑑狀態 </param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void ChangeStatus(Guid id, SPA_Period_Status status, string userID, DateTime cDate)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                        (from item in context.TET_SPA_Period
                         where item.ID == id
                         select item).FirstOrDefault();

                    if (dbModel == null)
                        throw new Exception($"{id} doesn't existed.");


                    // 檢查是否有其它評鑑期間執行中
                    var isAnyPeriodStarted = this.IsAnyPeriodStarted(id);
                    if (isAnyPeriodStarted)
                        throw new Exception($"Have Period is started. ");

                    if (status == SPA_Period_Status.Executing)
                    {
                        dbModel.Status = status.ToText();

                        // 寄送通知信 (SRI_SS 、 QSM)
                        var srissList = this._roleMgr.GetUserListInRole(ApprovalRole.SRI_SS.ToID().Value).ToList();
                        var srissglList = this._roleMgr.GetUserListInRole(ApprovalRole.SRI_SS_GL.ToID().Value).ToList();
                        var qsmList = this._roleMgr.GetUserListInRole(ApprovalRole.QSM.ToID().Value).ToList();
                        var receiver1_MailList = srissList.Union(srissglList).Select(obj => obj.EMail).Distinct().ToList();
                        var cc_MailList = qsmList.Select(obj => obj.EMail).Distinct().ToList();

                        MailUtil.Send_SRISS_Mail(receiver1_MailList, cc_MailList, dbModel.Period, userID, cDate);


                        // 寄送通知信 (供應商SPA評鑑審核者)
                        var buList = this._paramMgr.GetTET_ParametersList(SPA_PeriodManager._paraKey_EvaluationUnit);
                        var serviceItemList = this._paramMgr.GetTET_ParametersList(SPA_PeriodManager._paraKey_AssessmentItems);
                        var bu = buList.Where(obj => obj.Item == _bu_EHS).FirstOrDefault()?.ID;
                        var serviceItem = serviceItemList.Where(obj => obj.Item == _serviceItem_Safety).FirstOrDefault()?.ID;

                        var spaApproverSetup = context.TET_SPA_ApproverSetup.Where(obj => obj.BUID == bu && obj.ServiceItemID == serviceItem).FirstOrDefault();


                        var receiver2_MailList = qsmList.Select(obj => obj.EMail).ToList();
                        if (spaApproverSetup != null)
                        {
                            var infoFill = spaApproverSetup.InfoFill;
                            var empIdList = JsonConvert.DeserializeObject<List<string>>(infoFill);
                            var empList = this._userMgr.GetUserList(empIdList);

                            var MailList = empList.Select(obj => obj.EMail).Distinct().ToList();
                            MailUtil.Send_SafetyAndEhs_Mail(MailList, receiver2_MailList, dbModel.Period, userID, cDate);
                        }
                    }
                    else if (status == SPA_Period_Status.Completed)
                        dbModel.Status = status.ToText();

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

        /// <summary> 是否有任何評鑑期間執行中 </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool IsAnyPeriodStarted(Guid? id)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    string execText = SPA_Period_Status.Executing.ToText();

                    // 檢查是否有其它評鑑期間執行中
                    var query =
                        (from item in context.TET_SPA_Period
                         where item.Status == execText
                         select item);

                    if (id.HasValue && id != Guid.Empty)
                        query = query.Where(obj => obj.ID != id);

                    var result = query.Any();
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
    }
}
