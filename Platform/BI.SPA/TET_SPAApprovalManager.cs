using Platform.AbstractionClass;
using Platform.LogService;
using Platform.ORM;
using Platform.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using BI.SPA.Models;
using BI.SPA.Enums;
using BI.SPA.Utils;
using BI.SPA.Flows;
using BI.SPA.Validators;
using Platform.Auth;
using Platform.Auth.Models;
using System.Xml.Linq;
using Platform.Messages;

namespace BI.SPA
{
    public class TET_SPAApprovalManager
    {
        private Logger _logger = new Logger();
        private UserManager _userMgr = new UserManager();
        private UserRoleManager _roleMgr = new UserRoleManager();

        #region Read
        /// <summary>
        /// 取得 供應商審核資料 清單
        /// </summary>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<TET_SupplierSPAApprovalModel> GetApprovalList(string userID, DateTime cDate, Pager pager)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                    from item in context.TET_SupplierSPAApproval
                    where
                        item.Approver == userID &&
                        item.Result == null
                    orderby item.CreateDate descending
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
        /// 取得 供應商審核資料 清單
        /// </summary>
        /// <param name="SPAID">SPA ID</param>
        /// <param name="isCompleted"> TRUE: 過濾 Result 不為空 / FALSE 過濾 Result 為空 / NULL: 不過濾 </param>
        /// <param name="cDate">目前時間</param>
        /// <returns></returns>
        public List<TET_SupplierSPAApprovalModel> GetApprovalList(Guid SPAID, bool? isCompleted, DateTime cDate)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                    from item in context.TET_SupplierSPAApproval
                    where
                        item.SPAID == SPAID
                    orderby item.CreateDate ascending
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

        /// <summary> 查詢供應商審核資料 </summary>
        /// <param name="SPAID"> SPA ID </param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        /// <returns></returns>
        public TET_SupplierSPAApprovalModel GetDetail(Guid SPAID, string userID, DateTime cDate)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var result =
                    (from item in context.TET_SupplierSPAApproval
                     where
                        item.SPAID == SPAID &&
                        item.Approver == userID &&
                        item.Result == null
                     select
                        new TET_SupplierSPAApprovalModel()
                        {
                            ID = item.ID,
                            SPAID = item.ID,
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

        /// <summary> 查詢供應商 SPA 審核資料 </summary>
        /// <returns></returns>
        public TET_SupplierSPAApprovalModel GetDetail(Guid ID)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var result =
                    (from item in context.TET_SupplierSPAApproval
                     where
                        item.ID == ID
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

        /// <summary> 取得同一關卡的其它簽核資訊 (排除自己) (只取未簽核) </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private List<TET_SupplierSPAApproval> GetSameLevelApprovalList(PlatformContextModel context, TET_SupplierSPAApprovalModel model)
        {
            var query =
                (from item in context.TET_SupplierSPAApproval
                 where
                    item.SPAID == model.SPAID &&
                    item.ID != model.ID &&
                    item.Result == null &&
                    item.Level == model.Level
                 select item);

            var result = query.ToList();
            return result;
        }
        #endregion


        #region Approval
        /// <summary> 送出 供應商審核資料 </summary>
        /// <param name="model"> 供應商審核 </param>
        /// <param name="SPAModel">供應商</param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void Approve(TET_SupplierSPAApprovalModel model, TET_SupplierSPAModel SPAModel, string userID, DateTime cDate)
        {
            if (model == null)
                throw new ArgumentNullException("Model is required.");

            var user = _userMgr.GetUser(userID);
            if (user == null)
                throw new Exception("User is required.");

            if (!ApprovalValidator.Valid(model, out List<string> msgList))
                throw new ArgumentException(string.Join(", ", msgList));


            // 將簽核結果轉換為 Enum
            ApprovalResult result = ApprovalUtils.ParseApprovalResult(model.Result);
            if (result == ApprovalResult.Empty)
                throw new Exception(ApprovalUtils.ParseApprovalResultError);


            // 將簽核種類轉換為 Enum
            ApprovalType approvalType = ApprovalUtils.ParseApprovalType(model.Type);
            if (approvalType == ApprovalType.Empty)
                throw new Exception(ApprovalUtils.ParseApprovalTypeError);


            // 將簽核階段轉換為 Enum
            ApprovalLevel approvalLevel = ApprovalUtils.ParseApprovalLevel(model.Level);
            if (approvalLevel == ApprovalLevel.Empty)
                throw new Exception(ApprovalUtils.ParseApprovalLevelError);


            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                        (from item in context.TET_SupplierSPAApproval
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
                    context.TET_SupplierSPAApproval.RemoveRange(otherApprovalList);

                    // 將供應商資料回寫欄位中
                    var dbSPAModel =
                        (from item in context.TET_SupplierSPA
                         where item.ID == model.SPAID
                         select item).FirstOrDefault();

                    if (dbSPAModel == null)
                        throw new NullReferenceException($"{model.ID} don't exists.");

                    dbModel.SPAID = model.SPAID;
                    dbModel.Type = model.Type;
                    dbModel.Description = model.Description;
                    dbModel.Level = model.Level;
                    dbModel.Approver = model.Approver;
                    dbModel.Result = model.Result;
                    dbModel.Comment = model.Comment;
                    dbSPAModel.ModifyUser = userID;
                    dbSPAModel.ModifyDate = cDate;



                    // 查出已完成的簽核歷程
                    var approvalHistoryList = this.GetApprovalList(model.SPAID, true, cDate);
                    SPAModel.ApprovalList = approvalHistoryList;
                    SPAModel.ApprovalList.Add(model);              // 將目前的簽核也加入歷程中
                    string nextLevelName = string.Empty;
                    var nextApporverList = new List<UserAccountModel>();            // 產生下一階段簽核人
                    bool isCompleted = false;
                    bool isStart = false;


                    //--- 下一審核階段 ---
                    var cLevelModel = NewApprovalFlow.GetCurrentFlow(model, SPAModel, userID);
                    var nextLevelModel = NewApprovalFlow.GetNextFlow(model, SPAModel, userID);
                    var prevLevelModel = NewApprovalFlow.GetPrevFlow(model, SPAModel, userID);

                    isCompleted = (result == ApprovalResult.Agree) && cLevelModel.IsLast;
                    isStart = cLevelModel.IsStart;
                    if (result == ApprovalResult.Agree)
                    {
                        if (!isCompleted)
                            nextLevelName = nextLevelModel?.Level.ToText();
                    }
                    else if (result == ApprovalResult.RejectToPrev)
                    {
                        if (!isStart)
                            nextLevelName = prevLevelModel?.Level.ToText();
                    }
                    //--- 下一審核階段 ---


                    // 審核結果=退回
                    if (result == ApprovalResult.RejectToPrev)
                    {
                        this.SendRejectMail(SPAModel, model, userID, cDate);
                    }
                    // 審核結果=同意
                    else if (result == ApprovalResult.Agree)
                    {
                        this.SendAgreeMail(SPAModel, model, userID, cDate);
                    }


                    //--- 新增審核資料 ---
                    if (isCompleted)
                    {
                        dbSPAModel.ApproveStatus = ApprovalStatus.Completed.ToText();
                    }
                    else if (isStart)
                    {
                        dbSPAModel.ApproveStatus = ApprovalStatus.Rejected.ToText();
                    }
                    else
                    {
                        // 建立新的申請資訊
                        foreach (var item in nextApporverList)
                        {
                            var entity = new TET_SupplierSPAApproval()
                            {
                                ID = Guid.NewGuid(),
                                SPAID = dbModel.SPAID,
                                Type = dbModel.Type,
                                Level = nextLevelName,
                                Description = dbModel.Description,
                                Approver = item.EmpID,
                                CreateUser = userID,
                                CreateDate = cDate,
                                ModifyUser = userID,
                                ModifyDate = cDate,
                            };

                            context.TET_SupplierSPAApproval.Add(entity);
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

        #region Send-NewSPA-Mail
        /// <summary> (審核結果=退回) </summary>
        /// <param name="mainModel"></param>
        /// <param name="apprModel"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param> 
        private void SendRejectMail(TET_SupplierSPAModel mainModel, TET_SupplierSPAApprovalModel apprModel, string userID, DateTime cDate)
        {
            var applicant = _userMgr.GetUser(mainModel.CreateUser);
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/SPA/Index";

            var approver = _userMgr.GetUser(apprModel.Approver);
            var approverName = $"{approver.FirstNameEN} {approver.LastNameEN}";
            var comment = apprModel.Comment.ReplaceNewLine(true);

            EMailContent content = new EMailContent()
            {
                Title = $"[審核退回通知] 新增SPA資料_{mainModel.Period}_{mainModel.BelongTo}",
                Body =
                $@"
流程名稱: 退回供應商SPA資料 <br/>
退件者: {approverName} <br/>
退件意見: {comment} <br/>
<br/>
請點擊<a href=""{pageUrl}"" target=""_blank"">供應商 SPA 資料維護</a>追蹤此流程，謝謝 <br/>
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



            foreach (var item in mainModel.ApprovalList)
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

            var mailList = new List<string>() { applicant.EMail };
            MailPoolManager.WritePool(mailList, content, userID, cDate);
        }

        /// <summary> (審核結果=同意) </summary>
        /// <param name="mainModel"></param>
        /// <param name="apprModel"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        private void SendAgreeMail(TET_SupplierSPAModel mainModel, TET_SupplierSPAApprovalModel apprModel, string userID, DateTime cDate)
        {
            var applicant = _userMgr.GetUser(mainModel.CreateUser);
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/SPA/Index";

            EMailContent content = new EMailContent()
            {
                Title = $"[審核完成通知] 新增SPA資料_{mainModel.Period}_{mainModel.BelongTo}",
                Body =
                $@"
流程名稱: 完成供應商SPA審核 <br/>
<br/>
請點擊<a href=""{pageUrl}"" target=""_blank"">供應商 SPA 資料維護</a>追蹤此流程，謝謝 <br/>
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



            foreach (var item in mainModel.ApprovalList)
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

            var mailList = new List<string>() { applicant.EMail };
            MailPoolManager.WritePool(mailList, content, userID, cDate);
        }
        #endregion

        #region GetUsers
        /// <summary> 依照關卡取得簽核人 (User_GL 會回傳管理者) </summary>
        /// <param name="flow"> 流程 </param>
        /// <param name="applicantID"> 申請人帳號 </param>
        /// <returns></returns>
        private List<UserAccountModel> GetLevelApproverList(FlowModel flow, string applicantID)
        {
            switch (flow.Level)
            {
                case ApprovalLevel.Empty:
                    return new List<UserAccountModel>();

                case ApprovalLevel.User_GL:
                    var manager = this._userMgr.GetUserLeader(applicantID);
                    return new List<UserAccountModel>() { manager };

                case ApprovalLevel.SRI_SS:
                    return this._roleMgr.GetUserListInRole(ApprovalRole.SRI_SS_Approval.ToID().Value);

                case ApprovalLevel.SRI_SS_GL:
                    return this._roleMgr.GetUserListInRole(ApprovalRole.SRI_SS_GL.ToID().Value);

                case ApprovalLevel.ACC_First:
                    return this._roleMgr.GetUserListInRole(ApprovalRole.ACC_First.ToID().Value);

                case ApprovalLevel.ACC_Second:
                    return this._roleMgr.GetUserListInRole(ApprovalRole.ACC_Second.ToID().Value);

                case ApprovalLevel.ACC_Last:
                    return this._roleMgr.GetUserListInRole(ApprovalRole.ACC_Last.ToID().Value);

                default:
                    return new List<UserAccountModel>();
            }
        }
        #endregion
    }
}
