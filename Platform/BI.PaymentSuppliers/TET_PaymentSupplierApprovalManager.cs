using Platform.AbstractionClass;
using Platform.LogService;
using Platform.ORM;
using Platform.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using BI.PaymentSuppliers.Enums;
using BI.PaymentSuppliers.Utils;
using Platform.Auth;
using BI.PaymentSuppliers.Flows;
using Platform.Auth.Models;
using BI.PaymentSuppliers.Validators;
using Platform.Messages;
using BI.PaymentSuppliers.Models;
using Newtonsoft.Json;

namespace BI.PaymentSuppliers
{
    public class TET_PaymentSupplierApprovalManager
    {
        private Logger _logger = new Logger();
        private TET_PaymentSupplierContactManager _contactMgr = new TET_PaymentSupplierContactManager();
        private TET_PaymentSupplierAttachmentManager _attachmentMgr = new TET_PaymentSupplierAttachmentManager();
        private TET_PaymentSupplierManager _supplierMgr = new TET_PaymentSupplierManager();
        private UserManager _userMgr = new UserManager();
        private UserRoleManager _roleMgr = new UserRoleManager();

        #region Read
        /// <summary>
        /// 取得 付款單位審核資料 清單
        /// </summary>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<TET_PaymentSupplierApprovalModel> GetTET_SupplierApprovalList(string userID, DateTime cDate, Pager pager)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                    from item in context.TET_PaymentSupplierApproval
                    where
                        item.Approver == userID &&
                        item.Result == null
                    orderby item.CreateDate descending
                    select
                    new TET_PaymentSupplierApprovalModel()
                    {
                        ID = item.ID,
                        PSID = item.PSID,
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
        /// 取得 付款單位審核資料 清單
        /// </summary>
        /// <param name="psID">付款單位 ID</param>
        /// <param name="isCompleted"> TRUE: 過濾 Result 不為空 / FALSE 過濾 Result 為空 / NULL: 不過濾 </param>
        /// <param name="cDate">目前時間</param>
        /// <returns></returns>
        public List<TET_PaymentSupplierApprovalModel> GetTET_PaymentSupplierApprovalList(Guid psID, bool? isCompleted, DateTime cDate)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                    from item in context.TET_PaymentSupplierApproval
                    where
                        item.PSID == psID
                    orderby item.CreateDate ascending
                    select
                    new TET_PaymentSupplierApprovalModel()
                    {
                        ID = item.ID,
                        PSID = item.PSID,
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

        /// <summary> 查詢一般付款對象審核資料 </summary>
        /// <param name="psID"> 供應商編號 </param>
        /// <returns></returns>
        public TET_PaymentSupplierApprovalModel GetTET_PaymentSupplierApproval(Guid psID, string userID, DateTime cDate)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var result =
                    (from item in context.TET_PaymentSupplierApproval
                     where
                        item.PSID == psID &&
                        item.Approver == userID &&
                        item.Result == null
                     select
                        new TET_PaymentSupplierApprovalModel()
                        {
                            ID = item.ID,
                            PSID = item.PSID,
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

        /// <summary> 查詢一般付款對象審核資料 </summary>
        /// <returns></returns>
        public TET_PaymentSupplierApprovalModel GetTET_PaymentSupplierApproval(Guid ID)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var result =
                    (from item in context.TET_PaymentSupplierApproval
                     where
                        item.ID == ID
                     select
                        new TET_PaymentSupplierApprovalModel()
                        {
                            ID = item.ID,
                            PSID = item.PSID,
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
        private List<TET_PaymentSupplierApproval> GetSameLevelApprovalList(PlatformContextModel context, TET_PaymentSupplierApprovalModel model)
        {
            var query =
                (from item in context.TET_PaymentSupplierApproval
                 where
                    item.PSID == model.PSID &&
                    item.ID != model.ID &&
                    item.Result == null &&
                    item.Level == model.Level
                 select item);

            var result = query.ToList();
            return result;
        }
        #endregion

        #region Approval
        /// <summary> 修改 一般付款對象審核資料 </summary>
        /// <param name="model"> 一般付款對象審核 </param>
        /// <param name="paymentsupplierModel">付款單位</param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void SubmitTET_PaymentSupplierApproval(TET_PaymentSupplierApprovalModel model, TET_PaymentSupplierModel paymentsupplierModel, string userID, DateTime cDate)
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


            // 將簽核種類轉換為 Enum
            ApprovalType approvalType = ApprovalUtils.ParseApprovalType(model.Type);
            if (approvalType == ApprovalType.Empty)
                throw new Exception(ApprovalUtils.ParseApprovalTypeError);


            // 將簽核階段轉換為 Enum
            ApprovalLevel approvalLevel = ApprovalUtils.ParseApprovalLevel(model.Level);
            if (approvalLevel == ApprovalLevel.Empty)
                throw new Exception(ApprovalUtils.ParseApprovalLevelError);


            // 如果是退回上一關或退回申請人，都不用檢查供應商的必填
            // 同意時才需要檢查
            if (result == ApprovalResult.Agree)
            {
                if (!ApprovalValidator.ValidSupplier(paymentsupplierModel, model.Level, out List<string> msgList))
                    throw new ArgumentException(string.Join(", ", msgList));
            }

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                        (from item in context.TET_PaymentSupplierApproval
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

                    // 刪除其它簽核設定 (如果是 User_GL 及同意就不刪除，因為是所有人同意才同意)
                    if (approvalLevel == ApprovalLevel.User_GL && result == ApprovalResult.Agree)
                    { }
                    else
                    {
                        var otherApprovalList = this.GetSameLevelApprovalList(context, model);
                        context.TET_PaymentSupplierApproval.RemoveRange(otherApprovalList);
                    }

                    // 檢查供應商代碼是否已存在
                    if (approvalType == ApprovalType.New && approvalLevel == ApprovalLevel.ACC_Second)
                    {
                        if (this._supplierMgr.IsVendorCodeExisted(context, paymentsupplierModel))
                            throw new ArgumentException($"此供應商代碼已存在!");
                    }

                    // 將一般付款對象資料回寫欄位中
                    var dbSupplierModel =
                        (from item in context.TET_PaymentSupplier
                         where item.ID == model.PSID
                         select item).FirstOrDefault();

                    if (dbSupplierModel == null)
                        throw new NullReferenceException($"{model.ID} don't exists.");

                    dbSupplierModel.ApplyReason = paymentsupplierModel.ApplyReason;
                    dbSupplierModel.VenderCode = paymentsupplierModel.VenderCode;
                    dbSupplierModel.RegisterDate = paymentsupplierModel.RegisterDate_Date;
                    dbSupplierModel.CName = paymentsupplierModel.CName;
                    dbSupplierModel.EName = paymentsupplierModel.EName;
                    dbSupplierModel.Country = paymentsupplierModel.Country;
                    dbSupplierModel.TaxNo = paymentsupplierModel.TaxNo;
                    dbSupplierModel.Address = paymentsupplierModel.Address;
                    dbSupplierModel.OfficeTel = paymentsupplierModel.OfficeTel;
                    dbSupplierModel.Charge = paymentsupplierModel.Charge;
                    dbSupplierModel.PaymentTerm = paymentsupplierModel.PaymentTerm;
                    dbSupplierModel.BillingDocument = paymentsupplierModel.BillingDocument;
                    dbSupplierModel.Incoterms = paymentsupplierModel.Incoterms;
                    dbSupplierModel.Remark = paymentsupplierModel.Remark;
                    dbSupplierModel.BankCountry = paymentsupplierModel.BankCountry;
                    dbSupplierModel.BankName = paymentsupplierModel.BankName;
                    dbSupplierModel.BankCode = paymentsupplierModel.BankCode;
                    dbSupplierModel.BankBranchName = paymentsupplierModel.BankBranchName;
                    dbSupplierModel.BankBranchCode = paymentsupplierModel.BankBranchCode;
                    dbSupplierModel.Currency = paymentsupplierModel.Currency;
                    dbSupplierModel.BankAccountName = paymentsupplierModel.BankAccountName;
                    dbSupplierModel.BankAccountNo = paymentsupplierModel.BankAccountNo;
                    dbSupplierModel.CompanyCity = paymentsupplierModel.CompanyCity;
                    dbSupplierModel.BankAddress = paymentsupplierModel.BankAddress;
                    dbSupplierModel.SwiftCode = paymentsupplierModel.SwiftCode;
                    dbSupplierModel.Version = paymentsupplierModel.Version;
                    dbSupplierModel.ModifyUser = userID;
                    dbSupplierModel.ModifyDate = cDate;

                    _contactMgr.WriteTET_PaymentSupplierContact(context, dbSupplierModel.ID, paymentsupplierModel.ContactList, userID, cDate);                                        // 寫入供應商
                    _attachmentMgr.WriteTET_PaymentSupplierAttachment(context, dbSupplierModel.ID, paymentsupplierModel.AttachmentList, paymentsupplierModel.UploadFiles, userID, cDate);    // 寫入聯絡人

                    // 查出已完成的簽核歷程
                    var approvalHistoryList = this.GetTET_PaymentSupplierApprovalList(model.PSID, true, cDate);
                    paymentsupplierModel.ApprovalList = approvalHistoryList;
                    paymentsupplierModel.ApprovalList.Add(model);              // 將目前的簽核也加入歷程中
                    string nextLevelName = string.Empty;
                    string nextLevelDisplayName = string.Empty;
                    var nextApporverList = new List<UserAccountModel>();            // 產生下一階段簽核人
                    bool isCompleted = false;
                    bool isStart = false;

                    //--- 下一審核階段 ---
                    var cLevelModel = NewPaymentSupplierFlow.GetCurrentFlow(model, paymentsupplierModel, userID);
                    var nextLevelModel = NewPaymentSupplierFlow.GetNextFlow(model, paymentsupplierModel, userID);
                    var prevLevelModel = NewPaymentSupplierFlow.GetPrevFlow(model, paymentsupplierModel, userID);

                    bool hasCoSignLevel = !string.IsNullOrWhiteSpace(dbSupplierModel.CoSignApprover);
                    isCompleted = (result == ApprovalResult.Agree) && cLevelModel.IsLast;
                    isStart = cLevelModel.IsStart;

                    if (result == ApprovalResult.Agree)
                    {
                        if (!isCompleted)
                        {
                            nextLevelName = nextLevelModel?.Level.ToText();
                            nextLevelDisplayName = nextLevelModel?.Level.ToDisplayText();
                        }
                    }
                    else if (result == ApprovalResult.RejectToPrev)
                    {
                        if (!isStart)
                        {
                            nextLevelName = prevLevelModel?.Level.ToText();
                            nextLevelDisplayName = nextLevelModel?.Level.ToDisplayText();
                        }
                    }
                    //--- 下一審核階段 ---


                    //--- 檢查審核階段決定信件: (審核關卡=Any、審核結果=退回申請人) ---
                    if (result == ApprovalResult.RejectToStart)
                    {
                        this.SendRejectToStartMail(paymentsupplierModel, model, userID, cDate);
                        dbSupplierModel.ApproveStatus = ApprovalStatus.Rejected.ToText();

                        // 不產生下階段簽核人
                    }
                    //--- 檢查審核階段決定信件: (審核關卡=Any、審核結果=退回申請人) ---


                    //--- 審核結果=退回上一關 ---
                    if (result == ApprovalResult.RejectToPrev)
                    {
                        //--- 檢查審核階段決定信件: (審核關卡=Any、審核結果=退回申請人) or (審核關卡=User_GL、審核結果=退回上一關) ---
                        if (cLevelModel.Level == ApprovalLevel.User_GL)
                        {
                            this.SendRejectToStartMail(paymentsupplierModel, model, userID, cDate);
                            dbSupplierModel.ApproveStatus = ApprovalStatus.Rejected.ToText();

                            // 不產生下階段簽核人
                        }
                        //--- 檢查審核階段決定信件: (審核關卡=Any、審核結果=退回申請人) or (審核關卡=User_GL、審核結果=退回上一關) ---

                        //--- 檢查審核階段決定信件: (審核關卡!=User_GL、審核結果=退回上一關) ---
                        else if (cLevelModel.Level != ApprovalLevel.User_GL)
                        {
                            // 如果上一關是 User_GL ，要產生加簽者資料
                            if (prevLevelModel.Level == ApprovalLevel.User_GL)
                            {
                                var coSignList = this._userMgr.GetUserList_AccountModel(paymentsupplierModel.CoSignApprover);
                                nextApporverList.AddRange(coSignList);

                                foreach (var acc in coSignList)
                                {
                                    var lvlName = this.GetLevelDisplayName(acc.ID, prevLevelModel.Level, dbSupplierModel.CoSignApprover);
                                    this.SendRejectToPrevMail(lvlName, new List<UserAccountModel>() { acc }, paymentsupplierModel, model, userID, cDate);
                                }

                                var accList = this.GetLevelApproverList(prevLevelModel, dbSupplierModel.CreateUser);

                                foreach (var acc in accList)
                                {
                                    var lvlName = this.GetLevelDisplayName(acc.ID, prevLevelModel.Level, dbSupplierModel.CoSignApprover);
                                    this.SendRejectToPrevMail(lvlName, new List<UserAccountModel>() { acc }, paymentsupplierModel, model, userID, cDate);
                                }

                                nextApporverList.AddRange(accList);
                            }
                            else
                            {
                                var accList = this.GetLevelApproverList(prevLevelModel, dbSupplierModel.CreateUser);
                                nextApporverList.AddRange(accList);
                                this.SendRejectToPrevMail(nextLevelDisplayName, accList, paymentsupplierModel, model, userID, cDate);
                            }
                        }
                        //--- 檢查審核階段決定信件: (審核關卡!=User_GL、審核結果=退回上一關) ---
                    }
                    //--- 審核結果=退回上一關 ---

                    //--- 審核結果=同意 ---
                    if (result == ApprovalResult.Agree)
                    {
                        //--- 檢查審核階段決定信件: (審核關卡!= ACC覆核、審核結果=同意) ---
                        if (cLevelModel.Level == ApprovalLevel.User_GL)
                        {
                            var otherApprovers = this.GetSameLevelApprovalList(context, model);
                            if (!otherApprovers.Where(obj => obj.Result == null).Any())
                            {
                                // 全部同意才前往下一關
                                var accList = this.GetLevelApproverList(nextLevelModel, dbSupplierModel.CreateUser);
                                nextApporverList.AddRange(accList);
                                this.SendAgreeMail(nextLevelDisplayName, accList, paymentsupplierModel, model, userID, cDate);
                            }
                        }
                        else if (cLevelModel.Level != ApprovalLevel.ACC_Last)
                        {
                            var accList = this.GetLevelApproverList(nextLevelModel, dbSupplierModel.CreateUser);
                            nextApporverList.AddRange(accList);
                            this.SendAgreeMail(nextLevelDisplayName, accList, paymentsupplierModel, model, userID, cDate);
                        }
                        //--- 檢查審核階段決定信件: (審核關卡!= ACC覆核、審核結果=同意) ---

                        //--- 檢查審核階段決定信件: (審核關卡= ACC覆核、審核結果=同意) ---
                        if (cLevelModel.Level == ApprovalLevel.ACC_Last)
                        {
                            SendCompleteMail(nextLevelDisplayName, paymentsupplierModel, model, userID, cDate);
                        }
                        //--- 檢查審核階段決定信件: (審核關卡= ACC覆核、審核結果=同意) ---
                    }
                    //--- 審核結果=同意 ---

                    //--- 檢查審核階段決定信件: (審核關卡= SRI_SS_GL、審核結果=同意、STQA Application欄位=Yes) ---

                    //--- 新增審核資料 ---
                    if (!isCompleted)
                    {
                        // 建立新的申請資訊
                        foreach (var item in nextApporverList)
                        {
                            var entity = new TET_PaymentSupplierApproval()
                            {
                                ID = Guid.NewGuid(),
                                PSID = dbModel.PSID,
                                Type = dbModel.Type,
                                Level = nextLevelName,
                                Description = dbModel.Description,
                                Approver = item.EmpID,
                                CreateUser = userID,
                                CreateDate = cDate,
                                ModifyUser = userID,
                                ModifyDate = cDate,
                            };

                            context.TET_PaymentSupplierApproval.Add(entity);
                        }
                    }
                    else
                    {
                        dbSupplierModel.ApproveStatus = ApprovalStatus.Completed.ToText();
                        dbSupplierModel.IsLastVersion = "Y";
                        dbSupplierModel.IsActive = "Active";

                        var otherVerdorCodeSupplier =
                            from item in context.TET_PaymentSupplier
                            where
                                item.VenderCode == dbSupplierModel.VenderCode &&
                                item.ID != dbSupplierModel.ID
                            select item;

                        foreach (var item in otherVerdorCodeSupplier)
                        {
                            item.IsLastVersion = "N";
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

        #region Send-NewSupplier-Mail
        /// <summary> (審核關卡=Any、審核結果=退回申請人) or (審核關卡=User_GL、審核結果=退回上一關) </summary>
        /// <param name="supplierModel"></param>
        /// <param name="approvalModel"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        private void SendRejectToStartMail(TET_PaymentSupplierModel supplierModel, TET_PaymentSupplierApprovalModel approvalModel, string userID, DateTime cDate)
        {
            var applicant = _userMgr.GetUser(supplierModel.CreateUser);
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/PaymentSupplier/Index";

            var approver = _userMgr.GetUser(approvalModel.Approver);
            var approverName = $"{approver?.FirstNameEN} {approver?.LastNameEN}";
            var comment = approvalModel.Comment.ReplaceNewLine(true);

            EMailContent content = new EMailContent()
            {
                Title = $"[審核退回通知] {approvalModel.Description}",
                Body =
                $@"
                流程名稱: 新增付款單位審核 <br/>
                退件者: {approverName} <br/>
                退件意見: ${comment} <br/>
                <br/>          
                請點擊<a href=""{pageUrl}"" target=""_blank"">付款單位申請</a>追蹤此流程，謝謝 <br/>
                <br/>
                " + this.BuildApproveLogTable(supplierModel.ApprovalList)
            };
            MailPoolManager.WritePool(applicant.EMail, content, userID, cDate);
        }

        /// <summary> (審核關卡!=User_GL、審核結果=退回上一關) </summary>
        /// <param name="supplierModel"></param>
        /// <param name="approvalModel"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        private void SendRejectToPrevMail(string nextLevel, IEnumerable<UserAccountModel> receiverMailList, TET_PaymentSupplierModel supplierModel, TET_PaymentSupplierApprovalModel approvalModel, string userID, DateTime cDate)
        {
            var supplierCreateTime = supplierModel.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/SupplierApproval/Index";
            var createTime = approvalModel.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");

            EMailContent content = new EMailContent()
            {
                Title = $"[審核通知] {approvalModel.Description}",
                Body =
                $@"
                您好, <br/>
                請點「<a href=""{pageUrl}"" target=""_blank"">待審清單</a>」，謝謝 <br/>
                <br/>
                流程名稱: 新增付款單位審核 <br/>
                流程發起時間: {supplierCreateTime} <br/>
                審核關卡: {nextLevel} <br/>
                審核開始時間: {createTime} <br/>
                <br/>
                " + this.BuildApproveLogTable(supplierModel.ApprovalList)
            };

            var mailList = receiverMailList.Select(obj => obj.EMail).ToList();
            MailPoolManager.WriteMailWithCC(mailList, content, userID, cDate);
        }

        /// <summary> (審核關卡!=User_GL、審核結果=退回上一關) </summary>
        /// <param name="supplierModel"></param>
        /// <param name="approvalModel"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        private void SendAgreeMail(string nextLevel, List<UserAccountModel> receiverMailList, TET_PaymentSupplierModel supplierModel, TET_PaymentSupplierApprovalModel approvalModel, string userID, DateTime cDate)
        {
            var supplierCreateTime = supplierModel.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/SupplierApproval/Index";
            var createTime = approvalModel.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");

            EMailContent content = new EMailContent()
            {
                Title = $"[審核通知] {approvalModel.Description}",
                Body =
                $@"
                您好, <br/>
                請點擊「<a href=""{pageUrl}"" target=""_blank"">待審清單</a>」，謝謝 <br/>
                <br/>
                流程名稱: 新增一般付款對象審核 <br/>
                流程發起時間: {supplierCreateTime} <br/>
                審核關卡: {nextLevel} <br/>
                審核開始時間: {createTime} <br/>
                <br/>
                " + this.BuildApproveLogTable(supplierModel.ApprovalList)
            };

            var mailList = receiverMailList.Select(obj => obj.EMail).ToList();
            MailPoolManager.WriteMailWithCC(mailList, content, userID, cDate);
        }

        /// <summary> (審核關卡= ACC覆核、審核結果=同意) </summary>
        /// <param name="supplierModel"></param>
        /// <param name="approvalModel"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        private void SendCompleteMail(string nextLevel, TET_PaymentSupplierModel supplierModel, TET_PaymentSupplierApprovalModel approvalModel, string userID, DateTime cDate)
        {
            var applicant = _userMgr.GetUser(supplierModel.CreateUser);
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/PaymentSupplier/Index";

            EMailContent content = new EMailContent()
            {
                Title = $"[審核完成通知] 新增一般付款對象審核完成通知_{supplierModel.CName}_{supplierModel.VenderCode}_{applicant.UnitName}_{applicant.FirstNameEN} {applicant.LastNameEN}",
                Body =
                $@"
                流程名稱: 新增一般付款對象審核 <br/>
                請點擊<a href=""{pageUrl}"" target=""_blank"">一般付款對象申請</a>追蹤此流程，謝謝 <br/>
                <br/>
                " + this.BuildApproveLogTable(supplierModel.ApprovalList)
            };

            MailPoolManager.WritePool(applicant.EMail, content, userID, cDate);
        }

        #endregion

        #region Send-ModifySupplier-Mail
        /// <summary> (申請者未擁有SRI_SS角色、審核關卡=User_GL、審核結果=退回上一關) OR (申請者擁有SRI_SS角色、審核關卡=SRI_SS_GL、審核結果=退回上一關) </summary>
        /// <param name="supplierModel"></param>
        /// <param name="approvalModel"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        private void SendModifyRejectToStartMail(TET_PaymentSupplierModel supplierModel, TET_PaymentSupplierApprovalModel approvalModel, string userID, DateTime cDate)
        {
            var applicant = _userMgr.GetUser(supplierModel.CreateUser);
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/PaymentSupplierRevision/Index";

            var approver = _userMgr.GetUser(approvalModel.Approver);
            var approverName = $"{approver.FirstNameEN} {approver.LastNameEN}";
            var comment = approvalModel.Comment.ReplaceNewLine(true);

            EMailContent content = new EMailContent()
            {
                Title = $"[審核退回通知] {approvalModel.Description}",
                Body =
                $@"
                流程名稱: {ApprovalType.Modify.ToText()} <br/>
                退件者: {approverName} <br/>
                退件意見: {comment.ReplaceNewLine(true)} <br/>
                <br/>          
                請點擊<a href=""{pageUrl}"" target=""_blank"">一般付款對象資訊異動申請</a>追蹤此流程，謝謝 <br/>
                <br/>
                " + this.BuildApproveLogTable(supplierModel.ApprovalList)
            };

            MailPoolManager.WritePool(applicant.EMail, content, userID, cDate);
        }

        /// <summary> (審核關卡!=User_GL、審核結果=退回上一關) </summary>
        /// <param name="supplierModel"></param>
        /// <param name="approvalModel"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        private void SendModifyRejectToPrevMail(string nextLevel, IEnumerable<UserAccountModel> receiverMailList, TET_PaymentSupplierModel supplierModel, TET_PaymentSupplierApprovalModel approvalModel, string userID, DateTime cDate)
        {
            var supplierCreateTime = supplierModel.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/SupplierApproval/Index";
            var createTime = approvalModel.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");

            var lvl = ApprovalUtils.ParseApprovalLevel(nextLevel);
            var lvlName = this.GetLevelDisplayName(approvalModel.Approver, lvl);

            EMailContent content = new EMailContent()
            {
                Title = $"[審核通知] {approvalModel.Description}",
                Body =
                $@"
                您好, <br/>
                請點擊「<a href=""{pageUrl}"" target=""_blank"">待審清單</a>」，謝謝 <br/>
                <br/>
                流程名稱: {ApprovalType.Modify.ToText()} <br/>
                流程發起時間: {supplierCreateTime} <br/>
                審核關卡: {lvlName} <br/>
                審核開始時間: {createTime} <br/>
                <br/>
                " + this.BuildApproveLogTable(supplierModel.ApprovalList)
            };

            var mailList = receiverMailList.Select(obj => obj.EMail).ToList();
            MailPoolManager.WriteMailWithCC(mailList, content, userID, cDate);
        }

        /// <summary> (審核關卡!=User_GL、審核結果=退回上一關) </summary>
        /// <param name="supplierModel"></param>
        /// <param name="approvalModel"></param>
        private void SendModifyAgreeMail(string nextLevel, List<UserAccountModel> receiverMailList, TET_PaymentSupplierModel supplierModel, TET_PaymentSupplierApprovalModel approvalModel, string userID, DateTime cDate)
        {
            var supplierCreateTime = supplierModel.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/SupplierApproval/Index";
            var createTime = approvalModel.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");

            var lvl = ApprovalUtils.ParseApprovalLevel(nextLevel);
            var lvlName = this.GetLevelDisplayName(approvalModel.Approver, lvl);

            EMailContent content = new EMailContent()
            {
                Title = $"[審核通知] {approvalModel.Description}",
                Body =
                $@"
                您好, <br/>
                請點擊「<a href=""{pageUrl}"" target=""_blank"">待審清單</a>」，謝謝 <br/>
                <br/>
                流程名稱: {ApprovalType.Modify.ToText()} <br/>
                流程發起時間: {supplierCreateTime} <br/>
                審核關卡: {lvlName} <br/>
                審核開始時間: {createTime} <br/>
                <br/>
                " + this.BuildApproveLogTable(supplierModel.ApprovalList)
            };

            var mailList = receiverMailList.Select(obj => obj.EMail).ToList();
            MailPoolManager.WriteMailWithCC(mailList, content, userID, cDate);
        }

        /// <summary> (審核關卡= ACC覆核、審核結果=同意) </summary>
        /// <param name="supplierModel"></param>
        /// <param name="approvalModel"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        private void SendModifyCompleteMail(string nextLevel, TET_PaymentSupplierModel supplierModel, TET_PaymentSupplierApprovalModel approvalModel, string userID, DateTime cDate)
        {
            var applicant = _userMgr.GetUser(supplierModel.CreateUser);
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/PaymentSupplierRevision/Index";

            EMailContent content = new EMailContent()
            {
                Title = $"[審核完成通知] 一般付款對象資訊異動審核完成通知_{supplierModel.CName}_{supplierModel.VenderCode}_{applicant.UnitName}_{applicant.FirstNameEN} {applicant.LastNameEN}",
                Body =
                $@"
                流程名稱: {ApprovalType.Modify.ToText()} <br/>
                請點擊<a href=""{pageUrl}"" target=""_blank"">一般付款對象資訊異動申請</a>追蹤此流程，謝謝 <br/>
                <br/>
                " + BuildApproveLogTable(supplierModel.ApprovalList)
            };

            MailPoolManager.WritePool(applicant.EMail, content, userID, cDate);
        }
        #endregion

        #region Shared
        /// <summary> 組合簽核紀錄 </summary>
        /// <param name="approvalList"></param>
        /// <returns></returns>
        private string BuildApproveLogTable(List<TET_PaymentSupplierApprovalModel> approvalList)
        {
            // 表頭
            var mailBody =
            $@"
            <table border=""1"" cellpadding=""2"" cellspacing=""0"">
                <tr style=""background-color: black; text-align:center; color:white"">
                    <th>審核者 </th>        
                    <th>審核關卡</th>       
                    <th>建立時間</th>
                    <th>審核時間</th>
                    <th>審核結果</th>
                    <th>審核意見</th>
                </tr>
            ";

            // 表身
            foreach (var item in approvalList)
            {
                var approverInfo = this._userMgr.GetUser(item.Approver);
                mailBody +=
                $@"
                    <tr>
                        <td>{approverInfo?.FirstNameEN} {approverInfo?.LastNameEN}</td>
                        <td>{item.Level}</td>
                        <td>{item.CreateDate.ToString("yyyy/MM/dd HH:mm:ss")}</td>
                        <td>{item.ModifyDate.ToString("yyyy/MM/dd HH:mm:ss")}</td>
                        <td>{item.Result}</td>
                        <td>{item.Comment?.ReplaceNewLine(true)}</td>
                    </tr>
                ";
            }

            // 表尾
            mailBody += "</table>";
            return mailBody;
        }

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

        /// <summary> 取得要呈現的關卡名稱
        /// (因為加簽的人，要顯示不同的關卡)
        /// </summary>
        /// <param name="approver"></param>
        /// <param name="level"></param>
        /// <param name="coSignApproverText"></param>
        /// <returns></returns>
        private string GetLevelDisplayName(string approver, ApprovalLevel level, string coSignApproverText = "[]")
        {
            if (level == ApprovalLevel.User_GL)
            {
                // 如果沒有加簽者
                if (string.IsNullOrWhiteSpace(coSignApproverText) || string.Compare("[]", coSignApproverText, true) == 0)
                    return level.ToDisplayText();

                var coSigns = JsonConvert.DeserializeObject<List<string>>(coSignApproverText);
                if (coSigns.Contains(approver))
                    return ModuleConfig.CoSignApproverLevelName;
            }

            return level.ToDisplayText();
        }
        #endregion
    }
}
