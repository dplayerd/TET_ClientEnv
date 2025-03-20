using BI.Shared.Utils;
using BI.Shared.Extensions;
using BI.SPA_ApproverSetup;
using BI.SPA_ApproverSetup.Models;
using BI.SPA_CostService.Enums;
using BI.SPA_CostService.Flows;
using BI.SPA_CostService.Models;
using BI.SPA_CostService.Utils;
using Platform.AbstractionClass;
using Platform.Auth;
using Platform.Auth.Models;
using Platform.Infra;
using Platform.LogService;
using Platform.Messages;
using Platform.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;

namespace BI.SPA_CostService
{
    public class SPA_CostServiceApprovalManager
    {
        private Logger _logger = new Logger();
        private UserManager _userMgr = new UserManager();
        private UserRoleManager _roleMgr = new UserRoleManager();
        private SPA_ApproverSetupManager _spa_ApproverSetupManager = new SPA_ApproverSetupManager();
        private const string _typeText = "Cost&Service資料審核";
        private const string _isEvaluateText = "評鑑";
        private const string _prevText = "前期匯入";
        private const string _empStatus_Leave_Text = "離職";
        private const string _empStatus_Stay_Text = "在職";
        private const string _empStatus_New_Text = "新進";


        /// <summary> 查詢 Cost&Service 審核資料 </summary>
        /// <returns></returns>
        public SPA_CostServiceApprovalModel GetDetail(Guid ID)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var result =
                    (from item in context.TET_SPA_CostServiceApproval
                     where
                        item.ID == ID
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
                        }
                        ).FirstOrDefault();

                    return result;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> 送出 </summary>
        /// <param name="id"> Cost&Service資料維護代碼 </param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void Submit(Guid id, string userID, DateTime cDate)
        {
            if (Guid.Empty == id)
                throw new ArgumentNullException("Id is required.");

            var user = _userMgr.GetUser(userID);
            if (user == null)
                throw new Exception("User is required.");


            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    // 先查出原資料
                    var dbModel =
                        (from item in context.TET_SPA_CostService
                         where item.ID == id
                         select item).FirstOrDefault();

                    if (dbModel == null)
                        throw new NullReferenceException($"{id} don't exists.");

                    //--- 調整原資料的審核狀態 ---
                    // 如果審核狀態為 NULL 或是已退回，才允許送出申請
                    if (!string.IsNullOrWhiteSpace(dbModel.ApproveStatus) &&
                        ApprovalUtils.ParseApprovalStatus(dbModel.ApproveStatus) != ApprovalStatus.Rejected)
                        throw new Exception("送審狀態必須為空，或是已退回.");

                    // 更改原資料的審核狀態
                    dbModel.ApproveStatus = ApprovalStatus.Verify.ToText();
                    //--- 調整原資料的審核狀態 ---


                    //--- 新增審核資料 ---
                    var approverList = this._roleMgr.GetUserListInRole(ApprovalRole.SRI_SS_GL.ToID().Value);
                    foreach (var item in approverList)
                    {
                        string email = item.EMail;
                        string approver = item.EmpID;

                        // 建立新的申請資訊 
                        var entityApproval = new TET_SPA_CostServiceApproval()
                        {
                            ID = Guid.NewGuid(),
                            CSID = dbModel.ID,
                            Type = _typeText,
                            Level = ApprovalLevel.SRI_SS_GL.ToText(),
                            Description = $"{_typeText}_{dbModel.Period}_{user.UnitName}_{user.FirstNameEN} {user.LastNameEN} ({user.EmpID})",
                            Approver = approver,
                            CreateUser = userID,
                            CreateDate = cDate,
                            ModifyUser = userID,
                            ModifyDate = cDate,
                        };

                        // 寄送通知信
                        ApprovalMailUtil.SendNewVerifyMail(user, item, entityApproval, dbModel, ApprovalLevel.SRI_SS_GL.ToDisplayText(), userID, cDate);
                        context.TET_SPA_CostServiceApproval.Add(entityApproval);
                    }
                    //--- 新增審核資料 ---

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }


        /// <summary> 中止 Cost&Service 審核</summary>
        /// <param name="id">主鍵</param>
        /// <param name="reason">中止原因</param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void Abord(Guid id, string reason, string userID, DateTime cDate)
        {
            if (id == Guid.Empty || string.IsNullOrWhiteSpace(userID))
                throw new ArgumentNullException("ID, UserID is required");

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                        (from item in context.TET_SPA_CostService
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
                        (from item in context.TET_SPA_CostServiceApproval
                         where item.CSID == id
                         select item).ToList();

                    // 刪除尚未審核的審核資料
                    var nonResultApprovalList = approvalList.Where(item => string.IsNullOrEmpty(item.Result)).ToList();
                    context.TET_SPA_CostServiceApproval.RemoveRange(nonResultApprovalList);
                    context.SaveChanges();

                    // 寄送通知信
                    var receivers = approvalList.Select(obj => obj.Approver).ToList();
                    var userList = this._userMgr.GetUserList(receivers);
                    var mailList = userList.Select(obj => obj.EMail).ToList();
                    var subTitle = (approvalList.Count > 0) ? approvalList[0].Description : string.Empty;
                    ApprovalMailUtil.SendAbordMail(mailList, subTitle, reason, userID, cDate);
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }


        #region Approval
        /// <summary> 送出 Cost&Service資料維護審核資料 </summary>
        /// <param name="model"> Cost&Service資料維護審核 </param>
        /// <param name="mainModel">Cost&Service資料維護</param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void Approve(SPA_CostServiceApprovalModel model, SPA_CostServiceModel mainModel, string userID, DateTime cDate)
        {
            if (model == null)
                throw new ArgumentNullException("Model is required.");

            var user = _userMgr.GetUser(userID);
            if (user == null)
                throw new Exception("User is required.");



            // 將簽核結果轉換為 Enum
            ApprovalResult result = ApprovalUtils.ParseApprovalResult(model.Result);
            if (result == ApprovalResult.Empty)
                throw new Exception(ApprovalUtils.ParseApprovalResultError);

            // 將簽核階段轉換為 Enum
            ApprovalLevel approvalLevel = ApprovalUtils.ParseApprovalLevel(model.Level);
            if (approvalLevel == ApprovalLevel.Empty)
                throw new Exception(ApprovalUtils.ParseApprovalLevelError);



            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                        (from item in context.TET_SPA_CostServiceApproval
                         where
                            item.ID == model.ID &&
                            item.Result == null
                         select item).FirstOrDefault();

                    if (dbModel == null)
                        throw new NullReferenceException($"{model.ID} don't exists.");

                    // 將簽核值回寫欄位中
                    dbModel.Result = model.Result;
                    dbModel.Comment = model.Comment;
                    dbModel.ModifyUser = userID;
                    dbModel.ModifyDate = cDate;

                    // 刪除其它簽核設定 
                    // 客製化：如果是 BU 人員關卡，並不需要刪除同關的資料
                    if (approvalLevel != ApprovalLevel.BU)
                    {
                        var otherApprovalList = this.GetSameLevelApprovalList(context, model);
                        context.TET_SPA_CostServiceApproval.RemoveRange(otherApprovalList);
                    }


                    // 將Cost&Service資料維護資料回寫欄位中
                    var dbMainModel =
                        (from item in context.TET_SPA_CostService
                         where item.ID == model.CSID
                         select item).FirstOrDefault();

                    if (dbMainModel == null)
                        throw new NullReferenceException($"{model.ID} don't exists.");

                    //dbMainModel.Period = mainModel.Period;
                    dbMainModel.ModifyUser = userID;
                    dbMainModel.ModifyDate = cDate;


                    // 查出已完成的簽核歷程
                    var approvalHistoryList = this.GetTET_SupplierApprovalList(model.CSID, true, cDate);
                    mainModel.ApprovalList = approvalHistoryList;
                    mainModel.ApprovalList.Add(model);              // 將目前的簽核也加入歷程中
                    string nextLevelName = string.Empty;
                    var nextApporverList = new List<UserAccountModel>();            // 產生下一階段簽核人
                    bool isCompleted = false;
                    bool isStart = false;


                    //--- 下一審核階段 ---
                    var cLevelModel = NewApprovalFlow.GetCurrentFlow(model, mainModel, userID);
                    var nextLevelModel = NewApprovalFlow.GetNextFlow(model, mainModel, userID);
                    var prevLevelModel = NewApprovalFlow.GetPrevFlow(model, mainModel, userID);

                    isCompleted = (result == ApprovalResult.Agree) && cLevelModel.IsLast;
                    isStart = cLevelModel.IsStart;

                    if (result == ApprovalResult.Agree)
                    {
                        if (!isCompleted)
                            nextLevelName = nextLevelModel?.Level.ToText();
                    }
                    //else if (result == ApprovalResult.RejectToPrev)
                    //{
                    //    if (!isStart)
                    //        nextLevelName = prevLevelModel?.Level.ToText();
                    //}
                    //--- 下一審核階段 ---


                    // 目前沒有退回上一關
                    //--- 檢查審核階段決定信件: (審核關卡=Any、審核結果=退回申請人) ---
                    if (result == ApprovalResult.RejectToStart)
                    {
                        this.SendRejectToStartMail(mainModel, model, userID, cDate);
                        dbMainModel.ApproveStatus = ApprovalStatus.Rejected.ToText();

                        if (cLevelModel.Level == ApprovalLevel.QSM)
                        {
                            this.ClearNoneResultBuLevelList(context, model);
                        }

                        // 不產生下階段簽核人
                    }
                    //--- 檢查審核階段決定信件: (審核關卡=Any、審核結果=退回申請人) ---


                    //--- 審核結果=同意 ---
                    if (result == ApprovalResult.Agree)
                    {
                        //--- 檢查審核階段決定信件: (審核關卡= SRI_SS_GL、審核結果=同意) ---
                        if (cLevelModel.Level == ApprovalLevel.SRI_SS_GL)
                        {
                            // 查出 供應商SPA評鑑審核者設定，並且依設定，產生新的 BU 關卡審核資料
                            var dicSetting = this.GetSetupDic(mainModel, userID, cDate);
                            this.SendBUMail(mainModel, model, dicSetting, userID, cDate);
                            string approver = "";

                            // 建立 BU 的審核資訊
                            foreach (var item in dicSetting)
                            {
                                foreach (var userModel in item.Value)
                                {
                                    if (!approver.Contains(userModel.EmpID))
                                    {
                                        approver += userModel.EmpID + ",";

                                        var entity = new TET_SPA_CostServiceApproval()
                                        {
                                            ID = Guid.NewGuid(),
                                            CSID = dbMainModel.ID,
                                            Type = _typeText,
                                            Level = ApprovalLevel.BU.ToText(),
                                            Description = dbModel.Description,
                                            Approver = userModel.EmpID,
                                            CreateUser = userID,
                                            CreateDate = cDate,
                                            ModifyUser = userID,
                                            ModifyDate = cDate,
                                        };

                                        context.TET_SPA_CostServiceApproval.Add(entity);
                                    }
                                }
                            }


                            // 由於 BU 人員是虛擬關卡，所以直接跳到 QSM
                            nextLevelModel = NewApprovalFlow.GetTargetFlow(ApprovalLevel.QSM);
                            nextLevelName = nextLevelModel?.Level.ToText();

                            // 查出 QSM 關卡的審核者資料
                            var accList = this.GetLevelApproverList(nextLevelModel, dbMainModel);
                            nextApporverList.AddRange(accList);
                            this.SendQSMMail(nextLevelName, accList, mainModel, model, userID, cDate);
                        }
                        //--- 檢查審核階段決定信件: (審核關卡= SRI_SS_GL、審核結果=同意) ---

                        //--- 檢查審核階段決定信件: (審核關卡= QSM、審核結果=同意) ---
                        if (cLevelModel.Level == ApprovalLevel.QSM)
                        {
                            // 查出 供應商SPA評鑑審核者設定，並且依設定，產生新的 BU 關卡審核資料
                            var dicSetting = this.GetSetupDic(mainModel, userID, cDate);

                            this.BuildNewScoreInfo(context, mainModel, userID, cDate);      // 產生對應的SPA評鑑計分資料主檔資料
                            this.SendBUCompleteMail(mainModel, model, dicSetting, userID, cDate);
                            this.SendCompleteMail(nextLevelName, mainModel, model, userID, cDate);
                        }
                        //--- 檢查審核階段決定信件: (審核關卡= QSM、審核結果=同意) ---
                    }
                    //--- 審核結果=同意 ---


                    //--- 檢查審核階段決定信件: (審核關卡= BU、審核結果=確認) ---
                    if (cLevelModel.Level == ApprovalLevel.BU)
                    {
                        // 如果是 BU 關，什麼都不要管
                        // 只存入備註和結果就好
                    }
                    //--- 檢查審核階段決定信件: (審核關卡= BU、審核結果=確認) ---


                    //--- 新增審核資料 ---
                    if (!isCompleted)
                    {
                        // 建立新的申請資訊
                        foreach (var item in nextApporverList)
                        {
                            var entity = new TET_SPA_CostServiceApproval()
                            {
                                ID = Guid.NewGuid(),
                                CSID = dbMainModel.ID,
                                Type = _typeText,
                                Level = nextLevelName,
                                Description = dbModel.Description,
                                Approver = item.EmpID,
                                CreateUser = userID,
                                CreateDate = cDate,
                                ModifyUser = userID,
                                ModifyDate = cDate,
                            };

                            context.TET_SPA_CostServiceApproval.Add(entity);
                        }
                    }
                    else
                    {
                        this.ClearNoneResultBuLevelList(context, model);
                        dbMainModel.ApproveStatus = ApprovalStatus.Completed.ToText();
                    }
                    //--- 新增審核資料 ---


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



        #region Private
        /// <summary>
        /// 取得 Cost&Service資料審核資料 清單
        /// </summary>
        /// <param name="csId">Cost&Service資料 ID</param>
        /// <param name="isCompleted"> TRUE: 過濾 Result 不為空 / FALSE 過濾 Result 為空 / NULL: 不過濾 </param>
        /// <param name="cDate">目前時間</param>
        /// <returns></returns>
        public List<SPA_CostServiceApprovalModel> GetTET_SupplierApprovalList(Guid csId, bool? isCompleted, DateTime cDate)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                    from item in context.TET_SPA_CostServiceApproval
                    where
                        item.CSID == csId
                    orderby item.CreateDate ascending
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

                    // 如果有過濾條件，就補上
                    if (isCompleted.HasValue)
                    {
                        if (isCompleted.Value)
                            query = query.Where(obj => obj.Result != null);
                        else
                            query = query.Where(obj => obj.Result == null);
                    }

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

        /// <summary> 取得同一關卡的其它簽核資訊 (排除自己) (只取未簽核) </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private List<TET_SPA_CostServiceApproval> GetSameLevelApprovalList(PlatformContextModel context, SPA_CostServiceApprovalModel model)
        {
            var query =
                (from item in context.TET_SPA_CostServiceApproval
                 where
                    item.CSID == model.CSID &&
                    item.ID != model.ID &&
                    item.Result == null &&
                    item.Level == model.Level
                 select item);

            var result = query.ToList();
            return result;
        }

        /// <summary> 取得同一關卡的其它簽核資訊 (排除自己) (只取未簽核) </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private void ClearNoneResultBuLevelList(PlatformContextModel context, SPA_CostServiceApprovalModel model)
        {
            var bu = ApprovalLevel.BU.ToText();

            var query =
                (from item in context.TET_SPA_CostServiceApproval
                 where
                    item.CSID == model.CSID &&
                    item.ID != model.ID &&
                    item.Result == null &&
                    item.Level == bu
                 select item);

            var list = query.ToList();
            context.TET_SPA_CostServiceApproval.RemoveRange(list);
        }


        /// <summary> 依照關卡取得簽核人 (User_GL 會回傳管理者) </summary>
        /// <param name="flow"> 流程 </param>
        /// <param name="dbMainModel"> 主要資料 </param>
        /// <returns></returns>
        private List<UserAccountModel> GetLevelApproverList(FlowModel flow, TET_SPA_CostService dbMainModel)
        {
            switch (flow.Level)
            {
                case ApprovalLevel.Empty:
                    return new List<UserAccountModel>();

                case ApprovalLevel.SRI_SS_GL:
                    return this._roleMgr.GetUserListInRole(ApprovalRole.SRI_SS_GL.ToID().Value);

                case ApprovalLevel.BU:
                    {
                        var applicantUser = this._userMgr.GetUser(dbMainModel.CreateUser);
                        List<UserAccountModel> retList = new List<UserAccountModel>();

                        if (applicantUser != null)
                        {
                            retList.Add(
                            new UserAccountModel()
                            {
                                ID = applicantUser.UserID,
                                EmpID = applicantUser.EmpID,
                                EMail = applicantUser.EMail,
                                FirstNameCH = applicantUser.FirstNameCH,
                                LastNameCH = applicantUser.LastNameCH,
                                FirstNameEN = applicantUser.FirstNameEN,
                                LastNameEN = applicantUser.LastNameEN,
                                UnitCode = applicantUser.UnitCode,
                                UnitName = applicantUser.UnitName,
                                LeaderID = applicantUser.LeaderID,
                            });
                        }

                        return retList;
                    }

                case ApprovalLevel.QSM:
                    return this._roleMgr.GetUserListInRole(ApprovalRole.QSM.ToID().Value);

                default:
                    return new List<UserAccountModel>();
            }
        }

        /// <summary> 依評鑑項目，查出指定的設定 </summary>
        /// <param name="mainModel"></param>
        /// <param name="userId"></param>
        /// <param name="cDate"></param>
        /// <returns></returns>
        public Dictionary<TET_SPA_ApproverSetupModel, List<UserAccountModel>> GetSetupDic(SPA_CostServiceModel mainModel, string userId, DateTime cDate)
        {
            // 取得設定
            var setupList = this._spa_ApproverSetupManager.GetList(new List<Guid>(), new List<Guid>(), userId, cDate, new Pager() { AllowPaging = false });

            var dic = new Dictionary<TET_SPA_ApproverSetupModel, List<UserAccountModel>>();
            foreach (var item in mainModel.DetailList)
            {
                //只有「評鑑」的要產生資料，如果是「不評鑑」就不用
                if (item.IsEvaluate != _isEvaluateText)
                    continue;

                // 如果沒有設定，也不需要產生資料
                var selectedItem = setupList.Where(obj => obj.BUText == item.BU && obj.ServiceItemText == item.AssessmentItem).FirstOrDefault();

                if (selectedItem == null ||
                    selectedItem.InfoFills == null ||
                    selectedItem.InfoFills.Length == 0)
                {
                    continue;
                }

                var users = this._userMgr.GetUserList_AccountModel(selectedItem.InfoFills);

                if (!dic.ContainsKey(selectedItem))
                    dic.Add(selectedItem, users);
            }

            return dic;
        }
        #endregion

        #region SendMail
        private void SendBUMail(SPA_CostServiceModel main, SPA_CostServiceApprovalModel approvalModel, Dictionary<TET_SPA_ApproverSetupModel, List<UserAccountModel>> dic, string userID, DateTime cDate)
        {
            var qsmList = this._roleMgr.GetUserListInRole(ApprovalRole.QSM.ToID().Value);
            var qsmMailList = qsmList.Select(obj => obj.EMail).ToList();
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/SupplierApproval/Index";

            var createTime = approvalModel.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
            var firstApprovalTime = main.ApprovalList.OrderBy(obj => obj.CreateDate).Select(obj => obj.CreateDate).FirstOrDefault();
            var firstApprovalTimeText = (firstApprovalTime != null) ? firstApprovalTime.ToString("yyyy-MM-dd HH:mm:ss") : "";

            foreach (var item in dic)
            {
                EMailContent content = new EMailContent()
                {
                    Title = $"[Cost&Service資料確認] {main.Period}_{item.Key.ServiceItemText}_{item.Key.BUText}",
                    Body =
                    $@"
                    您好,
                    <br/>
                    請點擊「<a href=""{pageUrl}"" target=""_blank"">待審清單</a>」，謝謝 <br/>
                    <br/>
                    流程名稱: Cost&Service資料審核 <br/>

                    流程發起時間: {firstApprovalTimeText} <br/>
                    審核關卡: {ApprovalLevel.BU.ToDisplayText()} <br/>
                    審核開始時間: {createTime} <br/>
                    "
                };

                var buEmail = item.Value.Select(obj => obj.EMail).ToList();
                MailPoolManager.WriteMailWithCC(buEmail, qsmMailList, content, userID, cDate);
            }
        }

        /// <summary> 拒絕 </summary>
        /// <param name="main"></param>
        /// <param name="approvalModel"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        private void SendRejectToStartMail(SPA_CostServiceModel main, SPA_CostServiceApprovalModel approvalModel, string userID, DateTime cDate)
        {
            var applicant = _userMgr.GetUser(main.CreateUser);
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/SPA_CostService/Index";

            var approver = _userMgr.GetUser(approvalModel.Approver);
            var approverName = $"{approver?.FirstNameEN} {approver?.LastNameEN}";
            var comment = approvalModel.Comment.ReplaceNewLine(true);

            EMailContent content = new EMailContent()
            {
                Title = $"[審核退回通知] {approvalModel.Description}",
                Body =
                $@"
                流程名稱: Cost&Service資料審核 <br/>
                退件者: {approverName} <br/>
                退件意見: {comment} <br/>
                <br/>          
                請點擊<a href=""{pageUrl}"" target=""_blank"">Cost & Service資料維護</a>追蹤此流程，謝謝 <br/>
                <br/>
                "
            };
            MailPoolManager.WritePool(applicant.EMail, content, userID, cDate);
        }

        /// <summary> (審核關卡=QSM、審核結果=同意) </summary>
        /// <param name="main"></param>
        /// <param name="approvalModel"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        private void SendQSMMail(string nextLevel, List<UserAccountModel> receiverMailList, SPA_CostServiceModel main, SPA_CostServiceApprovalModel approvalModel, string userID, DateTime cDate)
        {
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/SupplierApproval/Index";
            var createTime = approvalModel.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
            var firstApprovalTime = main.ApprovalList.OrderBy(obj => obj.CreateDate).Select(obj => obj.CreateDate).FirstOrDefault();
            var firstApprovalTimeText = (firstApprovalTime != null) ? firstApprovalTime.ToString("yyyy-MM-dd HH:mm:ss") : "";

            EMailContent content = new EMailContent()
            {
                Title = $"[Cost&Service資料確認] {approvalModel.Description}",
                Body =
                $@"
                您好, <br/>
                請點擊「<a href=""{pageUrl}"" target=""_blank"">待審清單</a>」，謝謝 <br/>
                <br/>
                流程名稱: Cost&Service資料審核 <br/>
                流程發起時間: {firstApprovalTimeText} <br/>
                審核關卡: {nextLevel} <br/>
                審核開始時間: {createTime} <br/>
                "
            };

            var mailList = receiverMailList.Select(obj => obj.EMail).ToList();
            var ccMailList = new List<string>();
            //MailPoolManager.WriteMailWithCC(mailList, ccMailList, content, userID, cDate);
            MailPoolManager.WriteMailWithCC(mailList, new List<string>(), content, userID, cDate);
        }


        private void SendBUCompleteMail(SPA_CostServiceModel main, SPA_CostServiceApprovalModel approvalModel, Dictionary<TET_SPA_ApproverSetupModel, List<UserAccountModel>> dic, string userID, DateTime cDate)
        {
            var qsmList = this._roleMgr.GetUserListInRole(ApprovalRole.QSM.ToID().Value);
            var qsmMailList = qsmList.Select(obj => obj.EMail).ToList();
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/SPA_ScoringInfo/Index";

            var createTime = approvalModel.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
            var firstApprovalTime = main.ApprovalList.OrderBy(obj => obj.CreateDate).Select(obj => obj.CreateDate).FirstOrDefault();
            var firstApprovalTimeText = (firstApprovalTime != null) ? firstApprovalTime.ToString("yyyy-MM-dd HH:mm:ss") : "";

            foreach (var item in dic)
            {
                EMailContent content = new EMailContent()
                {
                    Title = $"[供應商SPA評鑑計分資料填寫] {main.Period}_{item.Key.ServiceItemText}_{item.Key.BUText}",
                    Body =
                    $@"
                    您好,
                    <br/>
                    請點擊「<a href=""{pageUrl}"" target=""_blank"">供應商SPA評鑑計分資料維護</a>」填寫計分資料，謝謝待審清單，謝謝 <br/>
                    "
                };

                var buEmail = item.Value.Select(obj => obj.EMail).ToList();
                MailPoolManager.WriteMailWithCC(buEmail, qsmMailList, content, userID, cDate);
            }
        }

        /// <summary> (審核關卡= ACC覆核、審核結果=同意) </summary>
        /// <param name="main"></param>
        /// <param name="approvalModel"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        private void SendCompleteMail(string nextLevel, SPA_CostServiceModel main, SPA_CostServiceApprovalModel approvalModel, string userID, DateTime cDate)
        {
            var applicant = _userMgr.GetUser(main.CreateUser);
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/SPA_CostService/Index";

            EMailContent content = new EMailContent()
            {
                Title = $"[審核完成通知] {_typeText}_{main.Period}_{applicant.UnitName}_{applicant.FirstNameEN} {applicant.LastNameEN}",
                Body =
                $@"
                流程名稱: {_typeText} <br/>
                請點擊<a href=""{pageUrl}"" target=""_blank"">Cost & Service資料維護</a>追蹤此流程，謝謝 <br/>
                <br/>
                <table border=""1"" cellpadding=""2"" cellspacing=""0"">
                    <tr style=""background-color: black; text-align:center; color:white"">
                        <th>審核者 </th>        
                        <th>審核關卡</th>       
                        <th>建立時間</th>
                        <th>審核時間</th>
                        <th>審核結果</th>
                        <th>審核意見</th>
                    </tr>
                "
            };


            foreach (var item in main.ApprovalList)
            {
                var approverInfo = this._userMgr.GetUser(item.Approver);

                content.Body +=
                $@"
                    <tr>
                        <td>{approverInfo.FirstNameEN} {approverInfo.LastNameEN}</td>
                        <td>{item.Level}</td>
                        <td>{item.CreateDate.ToString("yyyy/MM/dd HH:mm:ss")}</td>
                        <td>{item.ModifyDate.ToString("yyyy/MM/dd HH:mm:ss")}</td>
                        <td>{item.Result}</td>
                        <td>{item.Comment?.ReplaceNewLine(true)}</td>
                    </tr>
                ";
            }

            content.Body += "</table>";

            MailPoolManager.WritePool(applicant.EMail, content, userID, cDate);
        }
        #endregion

        #region BuildNew
        /// <summary> 產生對應的SPA評鑑計分資料主檔資料 </summary>
        /// <param name="main"></param>
        /// <param name="userId"></param>
        /// <param name="cDate"></param>
        private void BuildNewScoreInfo(PlatformContextModel context, SPA_CostServiceModel main, string userId, DateTime cDate)
        {
            // 找出前期的 SPA評鑑計分資料 、人力盤點資料
            var prevPeriod = PeriodUtil.ParsePeriod(main.Period).GetPrevPeriod().PeriodText;

            var prevPeriod_ScoringInfoList =
                (from scoringItem in context.TET_SPA_ScoringInfo
                 where scoringItem.Period == prevPeriod
                 select scoringItem).ToList();
            var prevPeriod_ScoringInfoList_IdList = prevPeriod_ScoringInfoList.Select(obj => obj.ID).ToList();

            var prevPeriod_Module1List =
                (from item in context.TET_SPA_ScoringInfoModule1
                 where prevPeriod_ScoringInfoList_IdList.Contains(item.SIID)
                 select item).ToList();


            foreach (var item in main.DetailList.Where(obj => obj.IsEvaluate == _isEvaluateText))
            {
                // 產生新的 SPA評鑑計分資料
                var scoringDbEntity = new TET_SPA_ScoringInfo()
                {
                    ID = Guid.NewGuid(),
                    Period = main.Period,
                    BU = item.BU,
                    ServiceItem = item.AssessmentItem,
                    ServiceFor = item.ServiceFor,
                    POSource = item.POSource,
                    BelongTo = item.BelongTo,
                    CreateUser = userId,
                    CreateDate = cDate,
                    ModifyUser = userId,
                    ModifyDate = cDate,
                };

                context.TET_SPA_ScoringInfo.Add(scoringDbEntity);


                //--- 加入前期的人力盤點項目 ---
                // 前期的 SPA評鑑計分資料
                var scoringInfo = prevPeriod_ScoringInfoList.Where(obj => obj.BU == item.BU && obj.ServiceItem == item.AssessmentItem && obj.POSource == item.POSource && obj.BelongTo == item.BelongTo).FirstOrDefault();

                if (scoringInfo == null)
                    continue;

                // 前期的人力評鑑
                var module1List = prevPeriod_Module1List.Where(obj => obj.SIID == scoringInfo.ID).ToList();

                foreach (var module1Item in module1List)
                {
                    // 前一期資料的員工狀態=離職，不需複製
                    if (string.Compare(_empStatus_Leave_Text, module1Item.EmpStatus, true) == 0)
                        continue;

                    // 計算新的年資
                    int TELSeniorityY;
                    int TELSeniorityM;
                    int.TryParse(module1Item.TELSeniorityY, out TELSeniorityY);
                    int.TryParse(module1Item.TELSeniorityM, out TELSeniorityM);

                    TELSeniorityM += 6;
                    if (TELSeniorityM > 11)
                    {
                        TELSeniorityY += 1;
                        TELSeniorityM -= 12;
                    }

                    // 前一期資料的員工狀態=新進，複製時改為在職
                    string empStatus = _empStatus_Stay_Text;
                    if(string.Compare(_empStatus_New_Text, module1Item.EmpStatus, true) == 0)
                        empStatus = _empStatus_Stay_Text;

                    // 產生新的 SPA評鑑計分 - 人力盤點資料
                    var module1DbEntity = new TET_SPA_ScoringInfoModule1()
                    {
                        ID = Guid.NewGuid(),
                        SIID = scoringDbEntity.ID,

                        Source = _prevText,
                        Type = module1Item.Type,
                        Supplier = module1Item.Supplier,
                        EmpName = module1Item.EmpName,
                        MajorJob = module1Item.MajorJob,
                        IsIndependent = module1Item.IsIndependent,
                        SkillLevel = module1Item.SkillLevel,
                        EmpStatus = empStatus,
                        TELSeniorityY = TELSeniorityY.ToString(),
                        TELSeniorityM = TELSeniorityM.ToString(),
                        // 備註欄位不用複製
                        //Remark = module1Item.Remark,          

                        CreateUser = userId,
                        CreateDate = cDate,
                        ModifyUser = userId,
                        ModifyDate = cDate,
                    };

                    context.TET_SPA_ScoringInfoModule1.Add(module1DbEntity);
                }
                //--- 加入前期的人力盤點項目 ---
            }
        }
        #endregion
    }
}
