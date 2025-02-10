using BI.Suppliers.Enums;
using BI.Suppliers.Flows;
using BI.Suppliers.Models;
using BI.Suppliers.Utils;
using BI.Suppliers.Validators;
using Newtonsoft.Json;
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

namespace BI.Suppliers
{
    public class TET_SupplierApprovalManager
    {
        private Logger _logger = new Logger();
        private TET_SupplierContactManager _contactMgr = new TET_SupplierContactManager();
        private TET_SupplierAttachmentManager _attachmentMgr = new TET_SupplierAttachmentManager();
        private TET_SupplierManager _supplierMgr = new TET_SupplierManager();
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
        public List<TET_SupplierApprovalModel> GetTET_SupplierApprovalList(string userID, DateTime cDate, Pager pager)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                    from item in context.TET_SupplierApproval
                    where
                        item.Approver == userID &&
                        item.Result == null
                    orderby item.CreateDate descending
                    select
                    new TET_SupplierApprovalModel()
                    {
                        ID = item.ID,
                        SupplierID = item.SupplierID,
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
        /// <param name="supplierID">供應商 ID</param>
        /// <param name="isCompleted"> TRUE: 過濾 Result 不為空 / FALSE 過濾 Result 為空 / NULL: 不過濾 </param>
        /// <param name="cDate">目前時間</param>
        /// <returns></returns>
        public List<TET_SupplierApprovalModel> GetTET_SupplierApprovalList(Guid supplierID, bool? isCompleted, DateTime cDate)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                    from item in context.TET_SupplierApproval
                    where
                        item.SupplierID == supplierID
                    orderby item.CreateDate ascending
                    select
                    new TET_SupplierApprovalModel()
                    {
                        ID = item.ID,
                        SupplierID = item.SupplierID,
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
        /// <param name="supplierID"> 供應商編號 </param>
        /// <returns></returns>
        public TET_SupplierApprovalModel GetTET_SupplierApproval(Guid supplierID, string userID, DateTime cDate)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var result =
                    (from item in context.TET_SupplierApproval
                     where
                        item.SupplierID == supplierID &&
                        item.Approver == userID &&
                        item.Result == null
                     select
                        new TET_SupplierApprovalModel()
                        {
                            ID = item.ID,
                            SupplierID = item.SupplierID,
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

        /// <summary> 查詢供應商審核資料 </summary>
        /// <returns></returns>
        public TET_SupplierApprovalModel GetTET_SupplierApproval(Guid ID)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var result =
                    (from item in context.TET_SupplierApproval
                     where
                        item.ID == ID
                     select
                        new TET_SupplierApprovalModel()
                        {
                            ID = item.ID,
                            SupplierID = item.SupplierID,
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
        private List<TET_SupplierApproval> GetSameLevelApprovalList(PlatformContextModel context, TET_SupplierApprovalModel model)
        {
            var query =
                (from item in context.TET_SupplierApproval
                 where
                    item.SupplierID == model.SupplierID &&
                    item.ID != model.ID &&
                    item.Result == null &&
                    item.Level == model.Level
                 select item);

            var result = query.ToList();
            return result;
        }
        #endregion

        #region Approval
        /// <summary> 修改 供應商審核資料 </summary>
        /// <param name="model"> 供應商審核 </param>
        /// <param name="supplierModel">供應商</param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void SubmitTET_SupplierApproval(TET_SupplierApprovalModel model, TET_SupplierModel supplierModel, string userID, DateTime cDate)
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
                if (!ApprovalValidator.ValidSupplier(supplierModel, model.Level, out List<string> msgList))
                    throw new ArgumentException(string.Join(", ", msgList));
            }


            List<TET_Supplier> willSaveSupplier = new List<TET_Supplier>();

            List<TET_SupplierApproval> willSaveSupplierApproval = new List<TET_SupplierApproval>();
            List<TET_SupplierApproval> willDeleteSupplierApproval = new List<TET_SupplierApproval>();


            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                        (from item in context.TET_SupplierApproval
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

                    willSaveSupplierApproval.AddWhenNotContains(dbModel);



                    // 刪除其它簽核設定 (如果是 User_GL 及同意就不刪除，因為是所有人同意才同意)
                    if (approvalLevel == ApprovalLevel.User_GL && result == ApprovalResult.Agree)
                    { }
                    else
                    {
                        var otherApprovalList = this.GetSameLevelApprovalList(context, model);
                        context.TET_SupplierApproval.RemoveRange(otherApprovalList);

                        willDeleteSupplierApproval.AddWhenNotContains(otherApprovalList);
                    }

                    // 檢查供應商代碼是否已存在
                    if (approvalType == ApprovalType.New && approvalLevel == ApprovalLevel.ACC_Second)
                    {
                        if (this._supplierMgr.IsVendorCodeExisted(context, supplierModel))
                            throw new ArgumentException($"此供應商代碼已存在!");
                    }

                    // 將供應商資料回寫欄位中
                    var dbSupplierModel =
                        (from item in context.TET_Supplier
                         where item.ID == model.SupplierID
                         select item).FirstOrDefault();

                    if (dbSupplierModel == null)
                        throw new NullReferenceException($"{model.ID} don't exists.");

                    willSaveSupplier.AddWhenNotContains(dbSupplierModel);

                    dbSupplierModel.IsSecret = supplierModel.IsSecret;
                    dbSupplierModel.IsNDA = supplierModel.IsNDA;
                    dbSupplierModel.ApplyReason = supplierModel.ApplyReason;
                    dbSupplierModel.BelongTo = supplierModel.BelongTo;
                    dbSupplierModel.VenderCode = supplierModel.VenderCode;
                    dbSupplierModel.SupplierCategory = supplierModel.SupplierCategory_Text;
                    dbSupplierModel.BusinessCategory = supplierModel.BusinessCategory_Text;
                    dbSupplierModel.BusinessAttribute = supplierModel.BusinessAttribute_Text;
                    dbSupplierModel.SearchKey = supplierModel.SearchKey;
                    dbSupplierModel.RelatedDept = supplierModel.RelatedDept_Text;
                    dbSupplierModel.Buyer = supplierModel.Buyer_Text;
                    dbSupplierModel.RegisterDate = supplierModel.RegisterDate_Date;
                    dbSupplierModel.CName = supplierModel.CName;
                    dbSupplierModel.EName = supplierModel.EName;
                    dbSupplierModel.Country = supplierModel.Country;
                    dbSupplierModel.TaxNo = supplierModel.TaxNo;
                    dbSupplierModel.Address = supplierModel.Address;
                    dbSupplierModel.OfficeTel = supplierModel.OfficeTel;
                    dbSupplierModel.ISO = supplierModel.ISO;
                    dbSupplierModel.Email = supplierModel.Email;
                    dbSupplierModel.Website = supplierModel.Website;
                    dbSupplierModel.CapitalAmount = supplierModel.CapitalAmount;
                    dbSupplierModel.MainProduct = supplierModel.MainProduct;
                    dbSupplierModel.Employees = supplierModel.Employees;
                    dbSupplierModel.Charge = supplierModel.Charge;
                    dbSupplierModel.PaymentTerm = supplierModel.PaymentTerm;
                    dbSupplierModel.BillingDocument = supplierModel.BillingDocument;
                    dbSupplierModel.Incoterms = supplierModel.Incoterms;
                    dbSupplierModel.Remark = supplierModel.Remark;
                    dbSupplierModel.BankCountry = supplierModel.BankCountry;
                    dbSupplierModel.BankName = supplierModel.BankName;
                    dbSupplierModel.BankCode = supplierModel.BankCode;
                    dbSupplierModel.BankBranchName = supplierModel.BankBranchName;
                    dbSupplierModel.BankBranchCode = supplierModel.BankBranchCode;
                    dbSupplierModel.Currency = supplierModel.Currency;
                    dbSupplierModel.BankAccountName = supplierModel.BankAccountName;
                    dbSupplierModel.BankAccountNo = supplierModel.BankAccountNo;
                    dbSupplierModel.CompanyCity = supplierModel.CompanyCity;
                    dbSupplierModel.BankAddress = supplierModel.BankAddress;
                    dbSupplierModel.SwiftCode = supplierModel.SwiftCode;
                    dbSupplierModel.NDANo = supplierModel.NDANo;
                    dbSupplierModel.Contract = supplierModel.Contract;
                    dbSupplierModel.IsSign1 = supplierModel.IsSign1;
                    dbSupplierModel.SignDate1 = supplierModel.SignDate1;
                    dbSupplierModel.IsSign2 = supplierModel.IsSign2;
                    dbSupplierModel.SignDate2 = supplierModel.SignDate2;
                    dbSupplierModel.STQAApplication = supplierModel.STQAApplication;
                    dbSupplierModel.KeySupplier = supplierModel.KeySupplier;
                    dbSupplierModel.Version = supplierModel.Version;
                    dbSupplierModel.RevisionType = supplierModel.RevisionType;
                    dbSupplierModel.ModifyUser = userID;
                    dbSupplierModel.ModifyDate = cDate;

                    //_contactMgr.WriteTET_SupplierContact(context, dbSupplierModel.ID, supplierModel.ContactList, userID, cDate);                                        // 寫入供應商
                    //_attachmentMgr.WriteTET_SupplierAttachment(context, dbSupplierModel.ID, supplierModel.AttachmentList, supplierModel.UploadFiles, userID, cDate);    // 寫入聯絡人



                    // 查出已完成的簽核歷程
                    var approvalHistoryList = this.GetTET_SupplierApprovalList(model.SupplierID, true, cDate);
                    supplierModel.ApprovalList = approvalHistoryList;
                    supplierModel.ApprovalList.Add(model);              // 將目前的簽核也加入歷程中
                    string nextLevelName = string.Empty;
                    string nextLevelDisplayName = string.Empty;
                    var nextApporverList = new List<UserAccountModel>();            // 產生下一階段簽核人
                    bool isCompleted = false;
                    bool isStart = false;

                    if (approvalType == ApprovalType.New)
                    {
                        //--- 下一審核階段 ---
                        var cLevelModel = NewSupplierFlow.GetCurrentFlow(model, supplierModel, userID);
                        var nextLevelModel = NewSupplierFlow.GetNextFlow(model, supplierModel, userID);
                        var prevLevelModel = NewSupplierFlow.GetPrevFlow(model, supplierModel, userID);

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
                            this.SendRejectToStartMail(supplierModel, model, userID, cDate);
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
                                this.SendRejectToStartMail(supplierModel, model, userID, cDate);
                                dbSupplierModel.ApproveStatus = ApprovalStatus.Rejected.ToText();

                                // 不產生下階段簽核人
                            }
                            //--- 檢查審核階段決定信件: (審核關卡=Any、審核結果=退回申請人) or (審核關卡=User_GL、審核結果=退回上一關) ---

                            //--- 檢查審核階段決定信件: (審核關卡!=User_GL、審核結果=退回上一關) ---
                            if (cLevelModel.Level != ApprovalLevel.User_GL)
                            {
                                // 如果上一關是 User_GL ，要產生加簽者資料
                                if (prevLevelModel.Level == ApprovalLevel.User_GL)
                                {
                                    var coSignList = this._userMgr.GetUserList_AccountModel(supplierModel.CoSignApprover);
                                    nextApporverList.AddRange(coSignList);

                                    var accList = this.GetLevelApproverList(prevLevelModel, dbSupplierModel.CreateUser);
                                    nextApporverList.AddRange(accList);

                                    foreach (var acc in accList)
                                    {
                                        var lvlName = this.GetLevelDisplayName(acc.ID, prevLevelModel.Level, dbSupplierModel.CoSignApprover);
                                        this.SendRejectToPrevMail(lvlName, new List<UserAccountModel>() { acc }, supplierModel, model, userID, cDate);
                                    }
                                }
                                else
                                {
                                    var accList = this.GetLevelApproverList(prevLevelModel, dbSupplierModel.CreateUser);
                                    nextApporverList.AddRange(accList);
                                    this.SendRejectToPrevMail(nextLevelDisplayName, accList, supplierModel, model, userID, cDate);
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
                                    this.SendAgreeMail(nextLevelDisplayName, accList, supplierModel, model, userID, cDate);
                                }
                            }
                            else if (cLevelModel.Level != ApprovalLevel.ACC_Last)
                            {
                                var accList = this.GetLevelApproverList(nextLevelModel, dbSupplierModel.CreateUser);
                                nextApporverList.AddRange(accList);
                                this.SendAgreeMail(nextLevelDisplayName, accList, supplierModel, model, userID, cDate);
                            }
                            //--- 檢查審核階段決定信件: (審核關卡!= ACC覆核、審核結果=同意) ---

                            //--- 檢查審核階段決定信件: (審核關卡= ACC覆核、審核結果=同意) ---
                            if (cLevelModel.Level == ApprovalLevel.ACC_Last)
                            {
                                SendCompleteMail(nextLevelDisplayName, supplierModel, model, userID, cDate);
                            }
                            //--- 檢查審核階段決定信件: (審核關卡= ACC覆核、審核結果=同意) ---
                        }
                        //--- 審核結果=同意 ---


                        //--- 檢查審核階段決定信件: (審核關卡= SRI_SS_GL、審核結果=同意、STQA Application欄位=Yes) ---
                        if (cLevelModel.Level == ApprovalLevel.SRI_SS_GL && result == ApprovalResult.Agree && supplierModel.STQAApplication == "YES")
                        {
                            SendSTQAMail(nextLevelDisplayName, supplierModel, model, userID, cDate);
                        }
                        //--- 檢查審核階段決定信件: (審核關卡= SRI_SS_GL、審核結果=同意、STQA Application欄位=Yes) ---
                    }
                    else if (approvalType == ApprovalType.Modify)
                    {
                        //--- 下一審核階段 ---
                        var cLevelModel = ModifySupplierFlow.GetCurrentFlow(model, supplierModel, userID);
                        var nextLevelModel = ModifySupplierFlow.GetNextFlow(model, supplierModel, userID);
                        var prevLevelModel = ModifySupplierFlow.GetPrevFlow(model, supplierModel, userID);

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

                        // 是否具 SRI_SS 身份
                        bool isSRI_SS =
                            this._roleMgr.IsRole(supplierModel.CreateUser, ApprovalRole.SRI_SS.ToID().Value, ApprovalRole.SRI_SS_Approval.ToID().Value);


                        //--- 檢查審核階段決定信件: (審核關卡=Any、審核結果=退回申請人) or (申請者未擁有SRI_SS角色、審核關卡=User_GL、審核結果=退回上一關) or (申請者擁有SRI_SS角色、審核關卡=SRI_SS_GL、審核結果=退回上一關) ---
                        if (
                            result == ApprovalResult.RejectToStart ||
                            (!isSRI_SS && cLevelModel.Level == ApprovalLevel.User_GL && result == ApprovalResult.RejectToPrev) ||
                            (isSRI_SS && cLevelModel.Level == ApprovalLevel.SRI_SS_GL && result == ApprovalResult.RejectToPrev)
                            )
                        {
                            this.SendModifyRejectToStartMail(supplierModel, model, userID, cDate);
                            dbSupplierModel.ApproveStatus = ApprovalStatus.Rejected.ToText();

                            // 不產生下階段簽核人
                        }
                        //--- 檢查審核階段決定信件: (審核關卡=Any、審核結果=退回申請人) or (申請者未擁有SRI_SS角色、審核關卡=User_GL、審核結果=退回上一關) or (申請者擁有SRI_SS角色、審核關卡=SRI_SS_GL、審核結果=退回上一關) ---

                        //--- 檢查審核階段決定信件: (申請者未擁有SRI_SS角色、審核關卡!=User_GL、審核結果=退回上一關) or (申請者擁有SRI_SS角色、審核關卡=!SRI_SS_GL、審核結果=退回上一關) ---
                        if (
                            (!isSRI_SS && cLevelModel.Level != ApprovalLevel.User_GL && result == ApprovalResult.RejectToPrev) ||
                            (isSRI_SS && cLevelModel.Level != ApprovalLevel.SRI_SS_GL && result == ApprovalResult.RejectToPrev)
                            )
                        {
                            var accList = this.GetLevelApproverList(prevLevelModel, dbSupplierModel.CreateUser);
                            nextApporverList.AddRange(accList);
                            this.SendModifyRejectToPrevMail(nextLevelName, accList, supplierModel, model, userID, cDate);
                        }
                        //--- 檢查審核階段決定信件: (申請者未擁有SRI_SS角色、審核關卡!=User_GL、審核結果=退回上一關) or (申請者擁有SRI_SS角色、審核關卡=!SRI_SS_GL、審核結果=退回上一關) ---

                        //--- 檢查審核階段決定信件: (RevisionType=1、審核關卡!= SRI_SS_GL、審核結果=同意) or (RevisionType=2、審核關卡!= ACC覆核、審核結果=同意) ---
                        if (
                            (supplierModel.RevisionType == RevisionType.Same.ToText() && cLevelModel.Level != ApprovalLevel.SRI_SS_GL && result == ApprovalResult.Agree) ||
                            (supplierModel.RevisionType == RevisionType.Changed.ToText() && cLevelModel.Level != ApprovalLevel.ACC_Last && result == ApprovalResult.Agree)
                            )
                        {
                            var accList = this.GetLevelApproverList(nextLevelModel, dbSupplierModel.CreateUser);
                            nextApporverList.AddRange(accList);
                            this.SendModifyAgreeMail(nextLevelName, accList, supplierModel, model, userID, cDate);
                        }
                        //--- 檢查審核階段決定信件: (RevisionType=1、審核關卡!= SRI_SS_GL、審核結果=同意) or (RevisionType=2、審核關卡!= ACC覆核、審核結果=同意) ---

                        //--- 檢查審核階段決定信件: (審核關卡= ACC覆核、審核結果=同意) ---
                        if (
                            (supplierModel.RevisionType == RevisionType.Same.ToText() && cLevelModel.Level == ApprovalLevel.SRI_SS_GL && result == ApprovalResult.Agree) ||
                            (supplierModel.RevisionType == RevisionType.Changed.ToText() && cLevelModel.Level == ApprovalLevel.ACC_Last && result == ApprovalResult.Agree)
                            )
                        {
                            this.SendModifyCompleteMail(nextLevelName, supplierModel, model, userID, cDate);
                        }
                        //--- 檢查審核階段決定信件: (審核關卡= ACC覆核、審核結果=同意) ---
                    }

                    //--- 新增審核資料 ---
                    if (!isCompleted)
                    {
                        // 建立新的申請資訊
                        foreach (var item in nextApporverList)
                        {
                            var entity = new TET_SupplierApproval()
                            {
                                ID = Guid.NewGuid(),
                                SupplierID = dbModel.SupplierID,
                                Type = dbModel.Type,
                                Level = nextLevelName,
                                Description = dbModel.Description,
                                Approver = item.EmpID,
                                CreateUser = userID,
                                CreateDate = cDate,
                                ModifyUser = userID,
                                ModifyDate = cDate,
                            };

                            context.TET_SupplierApproval.Add(entity);
                            willSaveSupplierApproval.AddWhenNotContains(entity);
                        }
                    }
                    else
                    {
                        dbSupplierModel.ApproveStatus = ApprovalStatus.Completed.ToText();
                        dbSupplierModel.IsLastVersion = "Y";
                        dbSupplierModel.IsActive = "Active";


                        var otherVerdorCodeSupplier =
                            from item in context.TET_Supplier
                            where
                                item.VenderCode == dbSupplierModel.VenderCode &&
                                item.ID != dbSupplierModel.ID
                            select item;

                        foreach (var item in otherVerdorCodeSupplier)
                        {
                            item.IsLastVersion = "N";
                            willSaveSupplier.Add(item);
                        }
                    }
                    //--- 新增審核資料 ---


                    this.SaveContents(supplierModel, willSaveSupplier, willSaveSupplierApproval, willDeleteSupplierApproval, userID, cDate);

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

        #region Save

        /// <summary> 為了避免版本衝突問題，所以將 Entity 挪至此處存檔 </summary>
        /// <param name="supplierModel"></param>
        /// <param name="willSaveSupplier"></param>
        /// <param name="willSaveSupplierApproval"></param>
        /// <param name="willDeleteSupplierApproval"></param>
        /// <param name="cUser"></param>
        /// <param name="cDate"></param>
        private void SaveContents(TET_SupplierModel supplierModel, List<TET_Supplier> willSaveSupplier, List<TET_SupplierApproval> willSaveSupplierApproval, List<TET_SupplierApproval> willDeleteSupplierApproval, string cUser, DateTime cDate)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    // 更新或新增供應商資料
                    var supplierIds = willSaveSupplier.Select(obj => obj.ID).ToList();
                    var supplierQuery =
                        from item in context.TET_Supplier
                        where supplierIds.Contains(item.ID)
                        select item;

                    var dicSuppliers = supplierQuery.ToDictionary(obj => obj.ID, obj => obj);

                    foreach (var item in willSaveSupplier)
                    {
                        TET_Supplier dbEntity;

                        if (dicSuppliers.ContainsKey(item.ID))
                            dbEntity = dicSuppliers[item.ID];
                        else
                        {
                            dbEntity = new TET_Supplier() { ID = Guid.NewGuid(), CreateUser = cUser, CreateDate = cDate };
                            context.TET_Supplier.Add(dbEntity);
                        }

                        dbEntity.CoSignApprover = item.CoSignApprover;
                        dbEntity.IsSecret = item.IsSecret;
                        dbEntity.IsNDA = item.IsNDA;
                        dbEntity.ApplyReason = item.ApplyReason;
                        dbEntity.BelongTo = item.BelongTo;
                        dbEntity.VenderCode = item.VenderCode;
                        dbEntity.SupplierCategory = item.SupplierCategory;
                        dbEntity.BusinessCategory = item.BusinessCategory;
                        dbEntity.BusinessAttribute = item.BusinessAttribute;
                        dbEntity.SearchKey = item.SearchKey;
                        dbEntity.RelatedDept = item.RelatedDept;
                        dbEntity.Buyer = item.Buyer;
                        dbEntity.RegisterDate = item.RegisterDate;
                        dbEntity.CName = item.CName;
                        dbEntity.EName = item.EName;
                        dbEntity.Country = item.Country;
                        dbEntity.TaxNo = item.TaxNo;
                        dbEntity.Address = item.Address;
                        dbEntity.OfficeTel = item.OfficeTel;
                        dbEntity.ISO = item.ISO;
                        dbEntity.Email = item.Email;
                        dbEntity.Website = item.Website;
                        dbEntity.CapitalAmount = item.CapitalAmount;
                        dbEntity.MainProduct = item.MainProduct;
                        dbEntity.Employees = item.Employees;
                        dbEntity.Charge = item.Charge;
                        dbEntity.PaymentTerm = item.PaymentTerm;
                        dbEntity.BillingDocument = item.BillingDocument;
                        dbEntity.Incoterms = item.Incoterms;
                        dbEntity.Remark = item.Remark;
                        dbEntity.BankCountry = item.BankCountry;
                        dbEntity.BankName = item.BankName;
                        dbEntity.BankCode = item.BankCode;
                        dbEntity.BankBranchName = item.BankBranchName;
                        dbEntity.BankBranchCode = item.BankBranchCode;
                        dbEntity.Currency = item.Currency;
                        dbEntity.BankAccountName = item.BankAccountName;
                        dbEntity.BankAccountNo = item.BankAccountNo;
                        dbEntity.CompanyCity = item.CompanyCity;
                        dbEntity.BankAddress = item.BankAddress;
                        dbEntity.SwiftCode = item.SwiftCode;
                        dbEntity.NDANo = item.NDANo;
                        dbEntity.Contract = item.Contract;
                        dbEntity.IsSign1 = item.IsSign1;
                        dbEntity.SignDate1 = item.SignDate1;
                        dbEntity.IsSign2 = item.IsSign2;
                        dbEntity.SignDate2 = item.SignDate2;
                        dbEntity.STQAApplication = item.STQAApplication;
                        dbEntity.KeySupplier = item.KeySupplier;
                        dbEntity.Version = item.Version;
                        dbEntity.RevisionType = item.RevisionType;
                        dbEntity.IsLastVersion = item.IsLastVersion;
                        dbEntity.ApproveStatus = item.ApproveStatus;
                        dbEntity.ModifyUser = item.ModifyUser;
                        dbEntity.ModifyDate = item.ModifyDate;
                        dbEntity.IsActive = item.IsActive;
                    }



                    // 更新或新增供應商簽核資料
                    var supplierApprovalIds = willSaveSupplierApproval.Select(obj => obj.ID).ToList();
                    var supplierApprovalQuery =
                        from item in context.TET_SupplierApproval
                        where supplierApprovalIds.Contains(item.ID)
                        select item;

                    var dicSupplierApprovals = supplierApprovalQuery.ToDictionary(obj => obj.ID, obj => obj);


                    foreach (var item in willSaveSupplierApproval)
                    {
                        TET_SupplierApproval dbEntity;

                        if (dicSupplierApprovals.ContainsKey(item.ID))
                            dbEntity = dicSupplierApprovals[item.ID];
                        else
                        {
                            dbEntity = new TET_SupplierApproval() { ID = Guid.NewGuid(), CreateUser = cUser, CreateDate = cDate };
                            context.TET_SupplierApproval.Add(dbEntity);
                        }

                        dbEntity.SupplierID = item.SupplierID;
                        dbEntity.Type = item.Type;
                        dbEntity.Level = item.Level;
                        dbEntity.Result = item.Result;
                        dbEntity.Description = item.Description;
                        dbEntity.Comment = item.Comment;
                        dbEntity.Approver = item.Approver;
                        dbEntity.ModifyUser = cUser;
                        dbEntity.ModifyDate = cDate;
                    }


                    // 刪除供應商簽核資料
                    supplierApprovalIds = willDeleteSupplierApproval.Select(obj => obj.ID).ToList();
                    supplierApprovalQuery =
                        from item in context.TET_SupplierApproval
                        where supplierApprovalIds.Contains(item.ID)
                        select item;

                    var willDeleteList = supplierApprovalQuery.ToList();
                    context.TET_SupplierApproval.RemoveRange(willDeleteList);


                    // 寫入附件和聯絡人
                    _contactMgr.WriteTET_SupplierContact(context, supplierModel.ID.Value, supplierModel.ContactList, cUser, cDate);                                        // 寫入供應商聯絡人
                    _attachmentMgr.WriteTET_SupplierAttachment(context, supplierModel.ID.Value, supplierModel.AttachmentList, supplierModel.UploadFiles, cUser, cDate);    // 寫入附件

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
        private void SendRejectToStartMail(TET_SupplierModel supplierModel, TET_SupplierApprovalModel approvalModel, string userID, DateTime cDate)
        {
            var applicant = _userMgr.GetUser(supplierModel.CreateUser);
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/Supplier/Index";

            var approver = _userMgr.GetUser(approvalModel.Approver);
            var approverName = $"{approver?.FirstNameEN} {approver?.LastNameEN}";
            var comment = approvalModel.Comment.ReplaceNewLine(true);

            EMailContent content = new EMailContent()
            {
                Title = $"[審核退回通知] {approvalModel.Description}",
                Body =
                $@"
                流程名稱: 新增供應商審核 <br/>
                退件者: {approverName} <br/>
                退件意見: ${comment} <br/>
                <br/>          
                請點擊<a href=""{pageUrl}"" target=""_blank"">新供應商申請</a>追蹤此流程，謝謝 <br/>
                <br/>
                " + this.BuildApproveLogTable(supplierModel, supplierModel.ApprovalList)
            };
            MailPoolManager.WritePool(applicant.EMail, content, userID, cDate);
        }

        /// <summary> (審核關卡!=User_GL、審核結果=退回上一關) </summary>
        /// <param name="supplierModel"></param>
        /// <param name="approvalModel"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        private void SendRejectToPrevMail(string nextLevel, IEnumerable<UserAccountModel> receiverMailList, TET_SupplierModel supplierModel, TET_SupplierApprovalModel approvalModel, string userID, DateTime cDate)
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
                流程名稱: 新增供應商審核 <br/>
                流程發起時間: {supplierCreateTime} <br/>
                審核關卡: {nextLevel} <br/>
                審核開始時間: {createTime} <br/>
                <br/>
                " + this.BuildApproveLogTable(supplierModel, supplierModel.ApprovalList)
            };

            var mailList = receiverMailList.Select(obj => obj.EMail).ToList();
            MailPoolManager.WritePool(mailList, content, userID, cDate);
        }

        /// <summary> (審核關卡!=User_GL、審核結果=退回上一關) </summary>
        /// <param name="supplierModel"></param>
        /// <param name="approvalModel"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        private void SendAgreeMail(string nextLevel, List<UserAccountModel> receiverMailList, TET_SupplierModel supplierModel, TET_SupplierApprovalModel approvalModel, string userID, DateTime cDate)
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
                流程名稱: 新增供應商審核 <br/>
                流程發起時間: {supplierCreateTime} <br/>
                審核關卡: {nextLevel} <br/>
                審核開始時間: {createTime} <br/>
                <br/>
                " + this.BuildApproveLogTable(supplierModel, supplierModel.ApprovalList)
            };

            var mailList = receiverMailList.Select(obj => obj.EMail).ToList();
            MailPoolManager.WritePool(mailList, content, userID, cDate);
        }

        /// <summary> (審核關卡= ACC覆核、審核結果=同意) </summary>
        /// <param name="supplierModel"></param>
        /// <param name="approvalModel"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        private void SendCompleteMail(string nextLevel, TET_SupplierModel supplierModel, TET_SupplierApprovalModel approvalModel, string userID, DateTime cDate)
        {
            var applicant = _userMgr.GetUser(supplierModel.CreateUser);
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/Supplier/Index";

            EMailContent content = new EMailContent()
            {
                Title = $"[審核完成通知] 新增供應商審核完成通知_{supplierModel.CName}_{supplierModel.VenderCode}_{applicant.UnitName}_{applicant.FirstNameEN} {applicant.LastNameEN}",
                Body =
                $@"
                流程名稱: 新增供應商審核 <br/>
                請點擊<a href=""{pageUrl}"" target=""_blank"">新供應商申請</a>追蹤此流程，謝謝 <br/>
                <br/>
                " + this.BuildApproveLogTable(supplierModel, supplierModel.ApprovalList)
            };

            MailPoolManager.WritePool(applicant.EMail, content, userID, cDate);
        }

        /// <summary> 審核關卡= SRI_SS_GL、審核結果=同意、STQA Application欄位=Yes </summary>
        /// <param name="supplierModel"></param>
        /// <param name="approvalModel"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        private void SendSTQAMail(string nextLevel, TET_SupplierModel supplierModel, TET_SupplierApprovalModel approvalModel, string userID, DateTime cDate)
        {
            var applicant = _userMgr.GetUser(supplierModel.CreateUser);

            // 找出申請人、QSM角色、QSM_GL角色所有人的 Email
            var member_QSM = this._roleMgr.GetUserListInRole(ApprovalRole.QSM.ToID().Value);
            var member_QSM_GL = this._roleMgr.GetUserListInRole(ApprovalRole.QSM_GL.ToID().Value);
            var allMemberEmailList = member_QSM.Union(member_QSM_GL).Select(obj => obj.EMail).ToList();
            allMemberEmailList.Add(applicant.EMail);

            EMailContent content = new EMailContent()
            {
                Title = $"[STQA實施要求通知信] {supplierModel.CName}_{applicant.UnitName}_{applicant.FirstNameEN} {applicant.LastNameEN}",
                Body =
                $@"
                您好, <br/>
                <br/>
                您申請新增的供應商需進行 STQA實施作業,請先填寫STQA實施要求書,進行STQA實施作業程序<br/>
                <br/>
                申請流程 : <br/>
                申請者填寫申請內容、供應商及交易物情報 ⇒ 申請者部門主管承認 ⇒ SS確認申請內容及情報完整性 ⇒ QSM主管判定STQA實施方式⇒ STQA-L確認Assessment項目及執行團隊成員 (Type1 & Type2) ⇒ QSM主管確認實施結果&結案<br/>
                <br/>
                *相關表格請到公司首頁內的[各式表格]下載,SRI>Assessment 實施要求書(供應商認定申請書)<br/>
                "
            };

            MailPoolManager.WritePool(allMemberEmailList, content, userID, cDate);
        }
        #endregion

        #region Send-ModifySupplier-Mail
        /// <summary> (申請者未擁有SRI_SS角色、審核關卡=User_GL、審核結果=退回上一關) OR (申請者擁有SRI_SS角色、審核關卡=SRI_SS_GL、審核結果=退回上一關) </summary>
        /// <param name="supplierModel"></param>
        /// <param name="approvalModel"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        private void SendModifyRejectToStartMail(TET_SupplierModel supplierModel, TET_SupplierApprovalModel approvalModel, string userID, DateTime cDate)
        {
            var applicant = _userMgr.GetUser(supplierModel.CreateUser);
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/SupplierRevision/Index";

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
請點擊<a href=""{pageUrl}"" target=""_blank"">供應商改版申請</a>追蹤此流程，謝謝 <br/>
<br/>
                " + this.BuildApproveLogTable(supplierModel, supplierModel.ApprovalList)
            };

            MailPoolManager.WritePool(applicant.EMail, content, userID, cDate);
        }

        /// <summary> (審核關卡!=User_GL、審核結果=退回上一關) </summary>
        /// <param name="supplierModel"></param>
        /// <param name="approvalModel"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        private void SendModifyRejectToPrevMail(string nextLevel, IEnumerable<UserAccountModel> receiverMailList, TET_SupplierModel supplierModel, TET_SupplierApprovalModel approvalModel, string userID, DateTime cDate)
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
流程名稱: {ApprovalType.Modify.ToText()} <br/>
流程發起時間: {supplierCreateTime} <br/>
審核關卡: {nextLevel} <br/>
審核開始時間: {createTime} <br/>
<br/>
                " + this.BuildApproveLogTable(supplierModel, supplierModel.ApprovalList)
            };

            var mailList = receiverMailList.Select(obj => obj.EMail).ToList();
            MailPoolManager.WritePool(mailList, content, userID, cDate);
        }

        /// <summary> (審核關卡!=User_GL、審核結果=退回上一關) </summary>
        /// <param name="supplierModel"></param>
        /// <param name="approvalModel"></param>
        private void SendModifyAgreeMail(string nextLevel, List<UserAccountModel> receiverMailList, TET_SupplierModel supplierModel, TET_SupplierApprovalModel approvalModel, string userID, DateTime cDate)
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
流程名稱: {ApprovalType.Modify.ToText()} <br/>
流程發起時間: {supplierCreateTime} <br/>
審核關卡: {nextLevel} <br/>
審核開始時間: {createTime} <br/>
<br/>
                " + this.BuildApproveLogTable(supplierModel, supplierModel.ApprovalList)
            };

            var mailList = receiverMailList.Select(obj => obj.EMail).ToList();
            MailPoolManager.WritePool(mailList, content, userID, cDate);
        }

        /// <summary> (審核關卡= ACC覆核、審核結果=同意) </summary>
        /// <param name="supplierModel"></param>
        /// <param name="approvalModel"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        private void SendModifyCompleteMail(string nextLevel, TET_SupplierModel supplierModel, TET_SupplierApprovalModel approvalModel, string userID, DateTime cDate)
        {
            var applicant = _userMgr.GetUser(supplierModel.CreateUser);
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/Supplier/Index";

            EMailContent content = new EMailContent()
            {
                Title = $"[審核完成通知] 供應商改版_{supplierModel.CName}_{supplierModel.VenderCode}_{applicant.UnitName}_{applicant.FirstNameEN} {applicant.LastNameEN}",
                Body =
                $@"
流程名稱: {ApprovalType.Modify.ToText()} <br/>
請點擊<a href=""{pageUrl}"" target=""_blank"">新供應商申請</a>追蹤此流程，謝謝 <br/>
<br/>
                " + BuildApproveLogTable(supplierModel, supplierModel.ApprovalList)
            };

            MailPoolManager.WritePool(applicant.EMail, content, userID, cDate);
        }
        #endregion

        #region Shared
        /// <summary> 組合簽核紀錄 </summary>
        /// <param name="supplierModel"></param>
        /// <param name="approvalList"></param>
        /// <returns></returns>
        private string BuildApproveLogTable(TET_SupplierModel supplierModel, List<TET_SupplierApprovalModel> approvalList)
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
                var lvl = ApprovalUtils.ParseApprovalLevel(item.Level);
                string lvlName = this.GetLevelDisplayName(item.Approver, lvl, supplierModel.CoSignApprover_Text);
                mailBody +=
                $@"
                    <tr>
                        <td>{approverInfo?.FirstNameEN} {approverInfo?.LastNameEN}</td>
                        <td>{lvlName}</td>
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

        /// <summary> 取得要呈現的關卡名稱
        /// (因為加簽的人，要顯示不同的關卡)
        /// </summary>
        /// <param name="approver"></param>
        /// <param name="level"></param>
        /// <param name="coSignApproverText"></param>
        /// <returns></returns>
        private string GetLevelDisplayName(string approver, ApprovalLevel level, string coSignApproverText)
        {
            if (level == ApprovalLevel.User_GL)
            {
                var coSigns = JsonConvert.DeserializeObject<List<string>>(coSignApproverText);
                if (coSigns.Contains(approver))
                    return ModuleConfig.CoSignApproverLevelName;
            }

            return level.ToDisplayText();
        }
        #endregion
    }
}
