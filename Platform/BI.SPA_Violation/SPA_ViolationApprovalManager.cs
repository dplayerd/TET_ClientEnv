using BI.Shared;
using BI.SPA_ApproverSetup;
using BI.SPA_ApproverSetup.Models;
using BI.SPA_Violation.Enums;
using BI.SPA_Violation.Flows;
using BI.SPA_Violation.Models;
using BI.SPA_Violation.Utils;
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

namespace BI.SPA_Violation
{
    public class SPA_ViolationApprovalManager
    {
        private Logger _logger = new Logger();
        private UserManager _userMgr = new UserManager();
        private UserRoleManager _roleMgr = new UserRoleManager();
        private SPA_ApproverSetupManager _spa_ApproverSetupManager = new SPA_ApproverSetupManager();
        private TET_ParametersManager _paramMgr = new TET_ParametersManager();
        private const string _typeText = "違規紀錄資料審核";
        private string _serviceItem_Safety = "Safety";
        private string _bu_EHS = "EHS";
        private const string _paraKey_AssessmentItems = "SPA評鑑項目";
        private const string _paraKey_EvaluationUnit = "SPA評鑑單位";

        #region Submit

        /// <summary> 查詢 Cost&Service 審核資料 </summary>
        /// <returns></returns>
        public SPA_ViolationApprovalModel GetDetail(Guid ID)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var result =
                    (from item in context.TET_SPA_ViolationApproval
                     where
                        item.ID == ID
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
                        (from item in context.TET_SPA_Violation
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

                    //--- 新增第一關審核者資料 ---
                    var approverList = this.GetApprover("Level_1", userID, cDate);
                    foreach (var item in approverList)
                    {
                        foreach (var userModel in item.Value)
                        {
                            // 建立新的申請資訊 
                            var entityApproval = new TET_SPA_ViolationApproval()
                            {
                                ID = Guid.NewGuid(),
                                ViolationID = dbModel.ID,
                                Type = _typeText,
                                Level = ApprovalLevel.Level_1.ToText(),
                                Description = $"{_typeText}_{dbModel.Period}",
                                Approver = userModel.EmpID,
                                CreateUser = userID,
                                CreateDate = cDate,
                                ModifyUser = userID,
                                ModifyDate = cDate,
                            };

                            context.TET_SPA_ViolationApproval.Add(entityApproval);

                            // 寄送通知信
                            ApprovalMailUtil.SendNewVerifyMail(userModel, entityApproval, dbModel, userID, cDate);
                        }
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

        #region Abord

        /// <summary> 中止違規紀錄資料審核</summary>
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
                        (from item in context.TET_SPA_Violation
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
                        (from item in context.TET_SPA_ViolationApproval
                         where item.ViolationID == id
                         select item).ToList();

                    // 刪除尚未審核的審核資料
                    var nonResultApprovalList = approvalList.Where(item => string.IsNullOrEmpty(item.Result)).ToList();
                    context.TET_SPA_ViolationApproval.RemoveRange(nonResultApprovalList);
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

        #endregion

        #region Approval

        /// <summary> 送出違規紀錄資料審核資料 </summary>
        /// <param name="model"> 違規紀錄資料維護審核 </param>
        /// <param name="mainModel">違規紀錄資料維護</param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void Approve(SPA_ViolationApprovalModel model, SPA_ViolationModel mainModel, string userID, DateTime cDate)
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
                        (from item in context.TET_SPA_ViolationApproval
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

                    // 將違規紀錄資料回寫欄位中
                    var dbMainModel =
                        (from item in context.TET_SPA_Violation
                         where item.ID == model.ViolationID
                         select item).FirstOrDefault();

                    if (dbMainModel == null)
                        throw new NullReferenceException($"{model.ID} don't exists.");

                    dbMainModel.ModifyUser = userID;
                    dbMainModel.ModifyDate = cDate;

                    // 查出已完成的簽核歷程
                    var approvalHistoryList = this.GetTET_SupplierApprovalList(model.ViolationID, true, cDate);
                    mainModel.ApprovalList = approvalHistoryList;
                    mainModel.ApprovalList.Add(model);              // 將目前的簽核也加入歷程中
                    string nextLevelName = string.Empty;
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
                        else
                            dbMainModel.ApproveStatus = ApprovalStatus.Completed.ToText();
                    }
                    //--- 下一審核階段 ---

                    //--- 檢查審核階段決定信件: (審核關卡=Any、審核結果=退回申請人) ---
                    else if (result == ApprovalResult.RejectToStart)
                    {
                        this.SendRejectToStartMail(mainModel, model, userID, cDate);
                        dbMainModel.ApproveStatus = ApprovalStatus.Rejected.ToText();

                        // 不產生下階段簽核人
                    }
                    //--- 檢查審核階段決定信件: (審核關卡=Any、審核結果=退回申請人) ---


                    //--- 審核結果=同意 ---
                    if (result == ApprovalResult.Agree)
                    {
                        // 查出 QSM 關卡的審核者資料
                        var accList = this._roleMgr.GetUserListInRole(ApprovalRole.QSM.ToID().Value);

                        //--- 檢查審核階段決定信件: (審核關卡=第一關審核者、審核結果=同意) ---
                        if (cLevelModel.Level == ApprovalLevel.Level_1)
                        {
                            //--- 查詢是否有第二關審核者資料 ---
                            var approverList = this.GetApprover("Level_2", userID, cDate);

                            //--- 有第二關審核者資料 ---
                            if (approverList != null || approverList.Count > 0)
                            {
                                //--- 新增第二關審核者資料 ---
                                foreach (var item in approverList)
                                {
                                    if (item.Value.Count > 0)
                                    {
                                        foreach (var userModel in item.Value)
                                        {
                                            // 建立新的申請資訊 
                                            var entityApproval = new TET_SPA_ViolationApproval()
                                            {
                                                ID = Guid.NewGuid(),
                                                ViolationID = dbModel.ViolationID,
                                                Type = _typeText,
                                                Level = ApprovalLevel.Level_2.ToText(),
                                                Description = dbModel.Description,
                                                Approver = userModel.EmpID,
                                                CreateUser = userID,
                                                CreateDate = cDate,
                                                ModifyUser = userID,
                                                ModifyDate = cDate,
                                            };

                                            context.TET_SPA_ViolationApproval.Add(entityApproval);

                                            // 寄送通知信
                                            ApprovalMailUtil.SendApprovalMail(ApprovalLevel.Level_2.ToDisplayText(), userModel, entityApproval, userID, cDate);
                                        }
                                    }
                                    //--- 沒有第二關審核者資料 ---
                                    else
                                    {
                                        //--- 新增QSM審核者資料 ---
                                        foreach (var item1 in accList)
                                        {
                                            var entityApproval = new TET_SPA_ViolationApproval()
                                            {
                                                ID = Guid.NewGuid(),
                                                ViolationID = dbModel.ViolationID,
                                                Type = _typeText,
                                                Level = ApprovalLevel.QSM.ToText(),
                                                Description = dbModel.Description,
                                                Approver = item1.EmpID,
                                                CreateUser = userID,
                                                CreateDate = cDate,
                                                ModifyUser = userID,
                                                ModifyDate = cDate,
                                            };

                                            context.TET_SPA_ViolationApproval.Add(entityApproval);

                                            // 寄送通知信
                                            ApprovalMailUtil.SendApprovalMail(ApprovalLevel.QSM.ToDisplayText(), item1, entityApproval, userID, cDate);
                                        }
                                    }
                                }
                            }
                        }
                        else if (cLevelModel.Level == ApprovalLevel.Level_2)
                        {
                            //--- 新增QSM審核者資料 ---
                            foreach (var item in accList)
                            {
                                var entityApproval = new TET_SPA_ViolationApproval()
                                {
                                    ID = Guid.NewGuid(),
                                    ViolationID = dbModel.ViolationID,
                                    Type = _typeText,
                                    Level = ApprovalLevel.QSM.ToText(),
                                    Description = dbModel.Description,
                                    Approver = item.EmpID,
                                    CreateUser = userID,
                                    CreateDate = cDate,
                                    ModifyUser = userID,
                                    ModifyDate = cDate,
                                };

                                context.TET_SPA_ViolationApproval.Add(entityApproval);

                                // 寄送通知信
                                ApprovalMailUtil.SendApprovalMail(ApprovalLevel.QSM.ToDisplayText(), item, entityApproval, userID, cDate);
                            }
                        }
                        //--- 檢查審核階段決定信件: (審核關卡= QSM、審核結果=同意) ---
                        else if (cLevelModel.Level == ApprovalLevel.QSM)
                        {
                            this.ClearNoneResultQSMLevelList(context, model);
                            this.SendCompleteMail(nextLevelName, mainModel, model, userID, cDate);
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


        #endregion

        #region Private

        /// <summary> 取得同一關卡的其它簽核資訊 (排除自己) (只取未簽核) </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private void ClearNoneResultQSMLevelList(PlatformContextModel context, SPA_ViolationApprovalModel model)
        {
            var qsm = ApprovalLevel.QSM.ToText();

            var query =
                (from item in context.TET_SPA_ViolationApproval
                 where
                    item.ViolationID == model.ViolationID &&
                    item.ID != model.ID &&
                    item.Result == null &&
                    item.Level == qsm
                 select item);

            var list = query.ToList();
            context.TET_SPA_ViolationApproval.RemoveRange(list);
        }

        /// <summary>
        /// 取得 違規紀錄資料審核資料 清單
        /// </summary>
        /// <param name="violationId">違規紀錄資料 ID</param>
        /// <param name="isCompleted"> TRUE: 過濾 Result 不為空 / FALSE 過濾 Result 為空 / NULL: 不過濾 </param>
        /// <param name="cDate">目前時間</param>
        /// <returns></returns>
        public List<SPA_ViolationApprovalModel> GetTET_SupplierApprovalList(Guid violationId, bool? isCompleted, DateTime cDate)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                    from item in context.TET_SPA_ViolationApproval
                    where
                        item.ViolationID == violationId
                    orderby item.CreateDate ascending
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

        /// <sumbuListmary> 從供應商SPA評鑑審核者設定取得審核者 </summary>
        /// <param name="level"></param>
        /// <param name="userId"></param>
        /// <param name="cDate"></param>
        /// <returns></returns>
        public Dictionary<TET_SPA_ApproverSetupModel, List<UserAccountModel>> GetApprover(string level, string userId, DateTime cDate)
        {
            var buList = this._paramMgr.GetTET_ParametersList(SPA_ViolationApprovalManager._paraKey_EvaluationUnit);
            var serviceItemList = this._paramMgr.GetTET_ParametersList(SPA_ViolationApprovalManager._paraKey_AssessmentItems);
            var bu = buList.Where(obj => obj.Item == _bu_EHS).FirstOrDefault()?.ID;
            var serviceItem = serviceItemList.Where(obj => obj.Item == _serviceItem_Safety).FirstOrDefault()?.ID;

            var setupList = this._spa_ApproverSetupManager.GetList(new List<Guid>(), new List<Guid>(), userId, cDate, new Pager() { AllowPaging = false });
            var selectedItem = setupList.Where(obj => obj.BUID == bu && obj.ServiceItemID == serviceItem).FirstOrDefault();
            var dic = new Dictionary<TET_SPA_ApproverSetupModel, List<UserAccountModel>>();

            if (selectedItem != null)
            {
                if (level == "Level_1")
                {
                    var users = this._userMgr.GetUserList_AccountModel(selectedItem.Lv1Apprvoer);
                    dic.Add(selectedItem, users);
                }
                else if (level == "Level_2")
                {
                    var users = this._userMgr.GetUserList_AccountModel(selectedItem.Lv2Apprvoer);
                    dic.Add(selectedItem, users);
                }
            }

            return dic;
        }

        /// <summary> 依照關卡取得簽核人 (User_GL 會回傳管理者) </summary>
        /// <param name="flow"> 流程 </param>
        /// <param name="dbMainModel"> 主要資料 </param>
        /// <returns></returns>
        private List<UserAccountModel> GetLevelApproverList(FlowModel flow, TET_SPA_CostService dbMainModel)
        {
            switch (flow.Level)
            {
                case ApprovalLevel.QSM:
                    return this._roleMgr.GetUserListInRole(ApprovalRole.QSM.ToID().Value);
                default:
                    return new List<UserAccountModel>();
            }
        }

        #endregion

        #region SendMail

        /// <summary> 拒絕 </summary>
        /// <param name="main"></param>
        /// <param name="approvalModel"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        private void SendRejectToStartMail(SPA_ViolationModel main, SPA_ViolationApprovalModel approvalModel, string userID, DateTime cDate)
        {
            var applicant = _userMgr.GetUser(main.CreateUser);
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/SPA_Violation/Index";

            var approver = _userMgr.GetUser(approvalModel.Approver);
            var approverName = $"{approver?.FirstNameEN} {approver?.LastNameEN}";
            var comment = approvalModel.Comment.ReplaceNewLine(true);

            EMailContent content = new EMailContent()
            {
                Title = $"[審核退回通知] {approvalModel.Description}",
                Body =
                $@"
                流程名稱: 違規紀錄資料審核 <br/>
                退件者: {approverName} <br/>
                退件意見: {comment} <br/>
                <br/>          
                請點擊<a href=""{pageUrl}"" target=""_blank"">供應商SPA違規紀錄資料維護</a>追蹤此流程，謝謝 <br/>
                <br/>
                "
            };
            MailPoolManager.WritePool(applicant.EMail, content, userID, cDate);
        }

        /// <summary> (審核關卡QSM、審核結果=同意) </summary>
        /// <param name="main"></param>
        /// <param name="approvalModel"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        private void SendCompleteMail(string nextLevel, SPA_ViolationModel main, SPA_ViolationApprovalModel approvalModel, string userID, DateTime cDate)
        {
            var applicant = _userMgr.GetUser(main.CreateUser);
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/SPA_Violation/Index";

            EMailContent content = new EMailContent()
            {
                Title = $"[審核完成通知] {_typeText}_{main.Period}",
                Body =
                $@"
                流程名稱: {_typeText} <br/>
                請點擊<a href=""{pageUrl}"" target=""_blank"">供應商SPA違規紀錄資料維護</a>追蹤此流程，謝謝 <br/>
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
    }
}
