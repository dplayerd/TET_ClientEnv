using BI.SPA_ScoringInfo;
using BI.SPA_ScoringInfo.Models;
using BI.SPA_ScoringInfo.Enums;
using BI.SPA_ScoringInfo.Flows;
using BI.SPA_ScoringInfo.Models;
using BI.SPA_ScoringInfo.Utils;
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
using BI.SPA_ApproverSetup.Models;
using BI.SPA_ApproverSetup;

namespace BI.SPA_ScoringInfo
{
    public class SPA_ScoringInfoApprovalManager
    {
        private const string _typeText = "SPA評鑑計分資料審核";
        private const string _isEvaluateText = "評鑑";

        private Logger _logger = new Logger();
        private UserManager _userMgr = new UserManager();
        private UserRoleManager _userRoleMgr = new UserRoleManager();
        private SPA_ApproverSetupManager _spa_ApproverSetupManager = new SPA_ApproverSetupManager();


        /// <summary> 查詢 SPA_ScoringInfo 審核資料 </summary>
        /// <returns></returns>
        public SPA_ScoringInfoApprovalModel GetOne(Guid ID)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var result =
                    (from item in context.TET_SPA_ScoringInfoApproval
                     where
                        item.ID == ID
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
        /// <param name="id"> SPA_ScoringInfo資料維護代碼 </param>
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
                        (from item in context.TET_SPA_ScoringInfo
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
                    var dic = this.GetSetupDic(dbModel, userID, cDate);

                    if(dic.ContainsKey(ApprovalLevel.FirstApproval))
                    {
                        var approver = dic[ApprovalLevel.FirstApproval].FirstOrDefault();

                        string email = approver.EMail;
                        string approverId = approver.EmpID;

                        // 建立新的申請資訊 
                        var entityApproval = new TET_SPA_ScoringInfoApproval()
                        {
                            ID = Guid.NewGuid(),
                            SIID = dbModel.ID,
                            Type = _typeText,
                            Level = ApprovalLevel.FirstApproval.ToText(),
                            Description = $"{_typeText}_{dbModel.Period}_{dbModel.BU}_{dbModel.ServiceItem}_{dbModel.BelongTo}",
                            Approver = approverId,
                            CreateUser = userID,
                            CreateDate = cDate,
                            ModifyUser = userID,
                            ModifyDate = cDate,
                        };


                        var qsm = this._userRoleMgr.GetUserListInRole(ApprovalRole.QSM.ToID().Value);

                        // 寄送通知信
                        ApprovalMailUtil.SendNewVerifyMail(approver, qsm.FirstOrDefault(), entityApproval, dbModel, userID, cDate);
                        context.TET_SPA_ScoringInfoApproval.Add(entityApproval);
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


        /// <summary> 中止 SPA_ScoringInfo 審核</summary>
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
                        (from item in context.TET_SPA_ScoringInfo
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
                        (from item in context.TET_SPA_ScoringInfoApproval
                         where item.SIID == id
                         select item).ToList();

                    // 刪除尚未審核的審核資料
                    var nonResultApprovalList = approvalList.Where(item => string.IsNullOrEmpty(item.Result)).ToList();
                    context.TET_SPA_ScoringInfoApproval.RemoveRange(nonResultApprovalList);
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
        /// <summary> 送出 SPA_ScoringInfo資料維護審核資料 </summary>
        /// <param name="model"> SPA_ScoringInfo資料維護審核 </param>
        /// <param name="mainModel">SPA_ScoringInfo資料維護</param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void Approve(SPA_ScoringInfoApprovalModel model, SPA_ScoringInfoModel mainModel, string userID, DateTime cDate)
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
                        (from item in context.TET_SPA_ScoringInfoApproval
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
                    var otherApprovalList = this.GetSameLevelApprovalList(context, model);
                    context.TET_SPA_ScoringInfoApproval.RemoveRange(otherApprovalList);


                    // 將SPA_ScoringInfo資料維護資料回寫欄位中
                    var dbMainModel =
                        (from item in context.TET_SPA_ScoringInfo
                         where item.ID == model.SIID
                         select item).FirstOrDefault();

                    if (dbMainModel == null)
                        throw new NullReferenceException($"{model.ID} don't exists.");

                    //dbMainModel.Period = mainModel.Period;
                    dbMainModel.ModifyUser = userID;
                    dbMainModel.ModifyDate = cDate;


                    // 查出已完成的簽核歷程
                    var approvalHistoryList = this.GetTET_SupplierApprovalList(model.SIID, true, cDate);
                    mainModel.ApprovalList = approvalHistoryList;
                    mainModel.ApprovalList.Add(model);                      // 將目前的簽核也加入歷程中
                    string nextLevelName = string.Empty;
                    var nextApporverList = new List<UserAccountModel>();    // 產生下一階段簽核人
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

                        // 不產生下階段簽核人
                    }
                    //--- 檢查審核階段決定信件: (審核關卡=Any、審核結果=退回申請人) ---


                    //--- 審核結果=同意 ---
                    if (result == ApprovalResult.Agree)
                    {
                        //--- 檢查審核階段決定信件: (審核關卡= 第一關、審核結果=同意) ---
                        if (cLevelModel.Level == ApprovalLevel.FirstApproval)
                        {
                            // 查出 第二關關卡的審核者資料 (如果沒有，就跑 QSM)
                            var dic = this.GetSetupDic(dbMainModel, userID, cDate);

                            if (dic.ContainsKey(ApprovalLevel.SecondApproval))
                            {
                                var accList = dic[ApprovalLevel.SecondApproval];
                                nextApporverList.AddRange(accList);
                                var qsm = this._userRoleMgr.GetUserListInRole(ApprovalRole.QSM.ToID().Value).ToList();
                                this.SendSecondMail(nextLevelName, accList, qsm, mainModel, model, userID, cDate);
                            }
                            else
                            {
                                nextLevelName = ApprovalLevel.QSM.ToText();

                                var accList = this._userRoleMgr.GetUserListInRole(ApprovalRole.QSM.ToID().Value);
                                nextApporverList.AddRange(accList);
                                this.SendQSMMail(nextLevelName, accList, mainModel, model, userID, cDate);
                            }
                        }
                        //--- 檢查審核階段決定信件: (審核關卡= SRI_SS_GL、審核結果=同意) ---


                        //--- 檢查審核階段決定信件: (審核關卡= 第二關、審核結果=同意) ---
                        if (cLevelModel.Level == ApprovalLevel.SecondApproval)
                        {
                            nextLevelName = ApprovalLevel.QSM.ToText();

                            var accList = this._userRoleMgr.GetUserListInRole(ApprovalRole.QSM.ToID().Value);
                            nextApporverList.AddRange(accList);
                            this.SendQSMMail(nextLevelName, accList, mainModel, model, userID, cDate);
                        }
                        //--- 檢查審核階段決定信件: (審核關卡= 第二關、審核結果=同意) ---

                        //--- 檢查審核階段決定信件: (審核關卡= QSM、審核結果=同意) ---
                        if (cLevelModel.Level == ApprovalLevel.QSM)
                        {
                        }
                        //--- 檢查審核階段決定信件: (審核關卡= QSM、審核結果=同意) ---
                    }
                    //--- 審核結果=同意 ---


                    //--- 新增審核資料 ---
                    if (!isCompleted)
                    {
                        // 建立新的申請資訊
                        foreach (var item in nextApporverList)
                        {
                            var entity = new TET_SPA_ScoringInfoApproval()
                            {
                                ID = Guid.NewGuid(),
                                SIID = dbMainModel.ID,
                                Type = _typeText,
                                Level = nextLevelName,
                                Description = dbModel.Description,
                                Approver = item.EmpID,
                                CreateUser = userID,
                                CreateDate = cDate,
                                ModifyUser = userID,
                                ModifyDate = cDate,
                            };

                            context.TET_SPA_ScoringInfoApproval.Add(entity);
                        }
                    }
                    else
                    {
                        dbMainModel.ApproveStatus = ApprovalStatus.Completed.ToText();
                        this.SendCompleteMail(nextLevelName, mainModel, model, userID, cDate);
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
        /// <summary> 依評鑑項目，查出指定的設定 </summary>
        /// <param name="mainModel"></param>
        /// <param name="userId"></param>
        /// <param name="cDate"></param>
        /// <returns></returns>
        public Dictionary<ApprovalLevel, List<UserAccountModel>> GetSetupDic(TET_SPA_ScoringInfo mainModel, string userId, DateTime cDate)
        {
            // 取得設定
            var setupList = this._spa_ApproverSetupManager.GetList(new List<Guid>(), new List<Guid>(), userId, cDate, new Pager() { AllowPaging = false });

            var dic = new Dictionary<ApprovalLevel, List<UserAccountModel>>();

            var selectedItem = setupList.Where(obj => obj.BUText == mainModel.BU && obj.ServiceItemText == mainModel.ServiceItem).FirstOrDefault();

            if (selectedItem == null)
                return dic;

            if (!string.IsNullOrWhiteSpace(selectedItem.Lv1Apprvoer))
                dic.Add(ApprovalLevel.FirstApproval, this._userMgr.GetUserList_AccountModel(selectedItem.Lv1Apprvoer));

            if (!string.IsNullOrWhiteSpace(selectedItem.Lv2Apprvoer))
                dic.Add(ApprovalLevel.SecondApproval, this._userMgr.GetUserList_AccountModel(selectedItem.Lv2Apprvoer));

            return dic;
        }


        /// <summary>
        /// 取得 SPA_ScoringInfo資料審核資料 清單
        /// </summary>
        /// <param name="csId">SPA_ScoringInfo資料 ID</param>
        /// <param name="isCompleted"> TRUE: 過濾 Result 不為空 / FALSE 過濾 Result 為空 / NULL: 不過濾 </param>
        /// <param name="cDate">目前時間</param>
        /// <returns></returns>
        public List<SPA_ScoringInfoApprovalModel> GetTET_SupplierApprovalList(Guid csId, bool? isCompleted, DateTime cDate)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                    from item in context.TET_SPA_ScoringInfoApproval
                    where
                        item.SIID == csId
                    orderby item.CreateDate ascending
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
        private List<TET_SPA_ScoringInfoApproval> GetSameLevelApprovalList(PlatformContextModel context, SPA_ScoringInfoApprovalModel model)
        {
            var query =
                (from item in context.TET_SPA_ScoringInfoApproval
                 where
                    item.SIID == model.SIID &&
                    item.ID != model.ID &&
                    item.Result == null &&
                    item.Level == model.Level
                 select item);

            var result = query.ToList();
            return result;
        }

        /// <summary> 依照關卡取得簽核人 (User_GL 會回傳管理者) </summary>
        /// <param name="flow"> 流程 </param>
        /// <param name="dbMainModel"> 主要資料 </param>
        /// <returns></returns>
        private List<UserAccountModel> GetLevelApproverList(FlowModel flow, TET_SPA_ScoringInfo dbMainModel)
        {
            switch (flow.Level)
            {
                case ApprovalLevel.Empty:
                    return new List<UserAccountModel>();

                case ApprovalLevel.FirstApproval:
                    return new List<UserAccountModel>();

                case ApprovalLevel.SecondApproval:
                    return new List<UserAccountModel>();

                case ApprovalLevel.QSM:
                    return this._userRoleMgr.GetUserListInRole(ApprovalRole.QSM.ToID().Value);

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
        private void SendRejectToStartMail(SPA_ScoringInfoModel main, SPA_ScoringInfoApprovalModel approvalModel, string userID, DateTime cDate)
        {
            var applicant = _userMgr.GetUser(main.CreateUser);
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/SPA_ScoringInfo/Index";

            var approver = _userMgr.GetUser(approvalModel.Approver);
            var approverName = $"{approver?.FirstNameEN} {approver?.LastNameEN}";
            var comment = approvalModel.Comment.ReplaceNewLine(true);

            EMailContent content = new EMailContent()
            {
                Title = $"[審核退回通知] {approvalModel.Description}",
                Body =
                $@"
                流程名稱: SPA評鑑計分資料審核 <br/>
                退件者: {approverName} <br/>
                退件意見: {comment} <br/>
                <br/>          
                請點擊<a href=""{pageUrl}"" target=""_blank"">供應商SPA評鑑計分資料維護</a>追蹤此流程，謝謝 <br/>
                <br/>
                "
            };
            MailPoolManager.WritePool(applicant.EMail, content, userID, cDate);
        }


        /// <summary> (審核關卡=第一關、審核結果=同意) </summary>
        /// <param name="main"></param>
        /// <param name="approvalModel"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        private void SendSecondMail(string nextLevel, List<UserAccountModel> receiverMailList, List<UserAccountModel> ccList, SPA_ScoringInfoModel main, SPA_ScoringInfoApprovalModel approvalModel, string userID, DateTime cDate)
        {
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/SupplierApproval/Index";
            var createTime = approvalModel.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
            var firstApprovalTime = main.ApprovalList.OrderBy(obj => obj.CreateDate).Select(obj => obj.CreateDate).FirstOrDefault();
            var firstApprovalTimeText = (firstApprovalTime != null) ? firstApprovalTime.ToString("yyyy-MM-dd HH:mm:ss") : "";

            EMailContent content = new EMailContent()
            {
                Title = $"[SPA評鑑計分資料審核] {approvalModel.Description}",
                Body =
                $@"
                您好, <br/>
                請點「<a href=""{pageUrl}"" target=""_blank"">待審清單</a>」，謝謝 <br/>
                <br/>
                流程名稱: SPA評鑑計分資料審核 <br/>
                流程發起時間: {firstApprovalTimeText} <br/>
                審核關卡: {nextLevel} <br/>
                審核開始時間: {createTime} <br/>
                "
            };

            var mailList = receiverMailList.Select(obj => obj.EMail).ToList();
            var ccMailList = ccList.Select(obj => obj.EMail).ToList();
            MailPoolManager.WriteMailWithCC(mailList, ccMailList, content, userID, cDate);
        }

        /// <summary> (審核關卡=QSM、審核結果=同意) </summary>
        /// <param name="main"></param>
        /// <param name="approvalModel"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        private void SendQSMMail(string nextLevel, List<UserAccountModel> receiverMailList, SPA_ScoringInfoModel main, SPA_ScoringInfoApprovalModel approvalModel, string userID, DateTime cDate)
        {
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/SupplierApproval/Index";
            var createTime = approvalModel.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
            var firstApprovalTime = main.ApprovalList.OrderBy(obj => obj.CreateDate).Select(obj => obj.CreateDate).FirstOrDefault();
            var firstApprovalTimeText = (firstApprovalTime != null) ? firstApprovalTime.ToString("yyyy-MM-dd HH:mm:ss") : "";

            EMailContent content = new EMailContent()
            {
                Title = $"[SPA評鑑計分資料審核] {approvalModel.Description}",
                Body =
                $@"
                您好, <br/>
                請點「<a href=""{pageUrl}"" target=""_blank"">待審清單</a>」，謝謝 <br/>
                <br/>
                流程名稱: SPA評鑑計分資料審核 <br/>
                流程發起時間: {firstApprovalTimeText} <br/>
                審核關卡: {nextLevel} <br/>
                審核開始時間: {createTime} <br/>
                "
            };

            var mailList = receiverMailList.Select(obj => obj.EMail).ToList();
            var ccMailList = new List<string>();
            MailPoolManager.WriteMailWithCC(mailList, ccMailList, content, userID, cDate);
        }

        /// <summary> (審核關卡= QSM、審核結果=同意) </summary>
        /// <param name="main"></param>
        /// <param name="approvalModel"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        private void SendCompleteMail(string nextLevel, SPA_ScoringInfoModel main, SPA_ScoringInfoApprovalModel approvalModel, string userID, DateTime cDate)
        {
            var applicant = _userMgr.GetUser(main.CreateUser);
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/SPA_ScoringInfo/Index";

            EMailContent content = new EMailContent()
            {
                Title = $"[審核完成通知] {_typeText}_{main.Period}_{main.BU}_{main.ServiceItem}_{main.BelongTo}",
                Body =
                $@"
                流程名稱: {_typeText} <br/>
                請點擊<a href=""{pageUrl}"" target=""_blank"">供應商SPA評鑑計分資料維護</a>追蹤此流程，謝謝 <br/>
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
