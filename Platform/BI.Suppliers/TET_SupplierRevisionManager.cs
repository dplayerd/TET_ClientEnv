using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Platform.AbstractionClass;
using Platform.Infra;
using Platform.LogService;
using Platform.ORM;
using BI.Suppliers.Models;
using BI.Suppliers.Enums;
using BI.Suppliers.Utils;
using Platform.Auth;
using System.Xml.Linq;
using Platform.Auth.Models;
using BI.Suppliers.Validators;
using Newtonsoft.Json;

namespace BI.Suppliers
{
    public class TET_SupplierRevisionManager
    {
        private Logger _logger = new Logger();
        private TET_SupplierManager _supplierMgr = new TET_SupplierManager();
        private TET_SupplierContactManager _contactMgr = new TET_SupplierContactManager();
        private TET_SupplierAttachmentManager _attachmentMgr = new TET_SupplierAttachmentManager();
        private UserManager _userMgr = new UserManager();
        private UserRoleManager _userRoleMgr = new UserRoleManager();

        #region Read
        /// <summary> 取得 供應商 清單 </summary>
        /// <param name="caption">中文名稱 / 英文名稱 / 供應商代碼</param>
        /// <param name="taxNo">統一編號</param>
        /// <param name="userID"> 目前登入者 </param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<SupplierListModel> GetTET_SupplierReversionList(string caption, string taxNo, string userID, Pager pager)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var orgQuery = context.vwTET_Supplier_LastVersion.AsQueryable();

                    //--- 組合過濾條件 ---
                    if (!string.IsNullOrWhiteSpace(caption))
                        orgQuery = orgQuery.Where(obj => obj.CName.Contains(caption) || obj.EName.Contains(caption) || obj.VenderCode.Contains(caption));

                    if (!string.IsNullOrWhiteSpace(taxNo))
                        orgQuery = orgQuery.Where(obj => obj.TaxNo.Contains(taxNo));
                    //--- 組合過濾條件 ---


                    var query =
                        from item in orgQuery
                        let appItem = context.TET_SupplierApproval.Where(obj => obj.SupplierID == item.ID && string.IsNullOrEmpty(obj.Result)).FirstOrDefault()
                        orderby item.CreateDate descending
                        select
                            new SupplierListModel()
                            {
                                ID = item.ID,
                                VenderCode = item.VenderCode,
                                CName = item.CName,
                                EName = item.EName,
                                TaxNo = item.TaxNo,
                                Version = item.Version,
                                IsLastVersion = item.IsLastVersion,
                                ApproveStatus = item.ApproveStatus,
                                CreateUser = item.CreateUser,

                                Level = appItem.Level,
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
        #endregion

        #region CUD
        /// <summary> 修改 供應商 </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        public void ModifyTET_Supplier(TET_SupplierModel model, string userID, DateTime cDate)
        {
            if (model == null)
                throw new ArgumentNullException("Model is required.");

            // 新增前，先檢查是否能通過商業邏輯
            if (!RevisionValidator.Valid(model, out List<string> msgList))
                throw new ArgumentException(string.Join(Environment.NewLine, msgList));

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                        (from item in context.TET_Supplier
                         where item.ID == model.ID
                         select item).FirstOrDefault();

                    var dbModel_LastVersion =
                        (from item in context.TET_Supplier
                         where
                            item.VenderCode == dbModel.VenderCode &&
                            item.Version == dbModel.Version - 1
                         select item).FirstOrDefault();

                    if (dbModel == null)
                        throw new NullReferenceException($"{model.ID} don't exists.");

                    dbModel.ApplyReason = model.ApplyReason;
                    dbModel.SupplierCategory = model.SupplierCategory_Text;
                    dbModel.BusinessCategory = model.BusinessCategory_Text;
                    dbModel.BusinessAttribute = model.BusinessAttribute_Text;
                    dbModel.Buyer = model.Buyer_Text;
                    dbModel.CName = model.CName;
                    dbModel.EName = model.EName;
                    dbModel.TaxNo = model.TaxNo;
                    dbModel.Charge = model.Charge;
                    dbModel.PaymentTerm = model.PaymentTerm;
                    dbModel.BankCountry = model.BankCountry;
                    dbModel.BankName = model.BankName;
                    dbModel.BankCode = model.BankCode;
                    dbModel.BankBranchName = model.BankBranchName;
                    dbModel.BankBranchCode = model.BankBranchCode;
                    dbModel.Currency = model.Currency;
                    dbModel.BankAccountName = model.BankAccountName;
                    dbModel.BankAccountNo = model.BankAccountNo;

                    if (!String.IsNullOrEmpty(model.CompanyCity))
                        dbModel.CompanyCity = model.CompanyCity;
                    else
                        dbModel.CompanyCity = null;

                    if (!String.IsNullOrEmpty(model.BankAddress))
                        dbModel.BankAddress = model.BankAddress;
                    else
                        dbModel.BankAddress = null;

                    if (!String.IsNullOrEmpty(model.SwiftCode))
                        dbModel.SwiftCode = model.SwiftCode;
                    else
                        dbModel.SwiftCode = null;

                    //dbModel.NDANo = model.NDANo;
                    //dbModel.Contract = model.Contract;
                    dbModel.ModifyUser = userID;
                    dbModel.ModifyDate = cDate;

                    // 若改版調整到銀行匯款資訊區塊的11個欄位
                    if (HasDiffInBankFields(dbModel, dbModel_LastVersion))
                        dbModel.RevisionType = RevisionType.Changed.ToText();     //有調整
                    else
                        dbModel.RevisionType = RevisionType.Same.ToText();        //未調整

                    _contactMgr.WriteTET_SupplierContact(context, dbModel.ID, model.ContactList, userID, cDate);                              // 寫入供應商
                    _attachmentMgr.WriteTET_SupplierAttachment(context, dbModel.ID, model.AttachmentList, model.UploadFiles, userID, cDate);  // 寫入聯絡人

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

        /// <summary> 送出申請 </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void SubmitTET_SupplierRevision(TET_SupplierModel model, string userID, DateTime cDate)
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
                    (from item in context.TET_Supplier
                     where item.ID == model.ID
                     select item).FirstOrDefault();

                    if (dbModel == null)
                        throw new NullReferenceException($"{model.ID} don't exists.");

                    var dbModel_LastVersion =
                        (from item in context.TET_Supplier
                         where
                            item.VenderCode == dbModel.VenderCode &&
                            item.Version == dbModel.Version - 1
                         select item).FirstOrDefault();


                    //--- 調整原資料的審核狀態 ---
                    // 如果審核狀態為 NULL 或是已退回，才允許送出申請
                    if (!string.IsNullOrWhiteSpace(dbModel.ApproveStatus) &&
                        ApprovalUtils.ParseApprovalStatus(dbModel.ApproveStatus) != ApprovalStatus.Rejected)
                        throw new Exception("送審狀態必須為空，或是已退回.");

                    // 更改原資料的審核狀態
                    dbModel.ApproveStatus = ApprovalStatus.Verify.ToText();
                    //--- 調整原資料的審核狀態 ---



                    ApprovalLevel startFlow;
                    List<UserAccountModel> approverList = new List<UserAccountModel>();

                    // 登入者是否擁有SRI_SS角色
                    if (this._userRoleMgr.IsRole(userID, ApprovalRole.SRI_SS.ToID().Value, ApprovalRole.SRI_SS_Approval.ToID().Value))
                    {       
                        // 當登入者擁有SRI_SS角色
                        startFlow = ApprovalLevel.SRI_SS_GL;
                        approverList.AddRange(this._userRoleMgr.GetUserListInRole(ApprovalRole.SRI_SS_GL.ToID().Value));
                    }
                    else
                    {       
                        // 當登入者未擁有SRI_SS角色
                        startFlow = ApprovalLevel.User_GL;
                        approverList.Add(this._userMgr.GetUserLeader(userID));
                    }

                    //--- 新增審核資料 ---
                    List<TET_SupplierApproval> supplierApprovalList = new List<TET_SupplierApproval>();
                    if (approverList.Count > 0)
                    {
                        // 建立新的申請資訊
                        foreach (var item in approverList)
                        {
                            var entity = new TET_SupplierApproval()
                            {
                                ID = Guid.NewGuid(),
                                SupplierID = dbModel.ID,
                                Type = ApprovalType.Modify.ToText(),
                                Level = startFlow.ToText(),
                                Description = $"{ApprovalType.Modify.ToText()}_{dbModel.CName}_{user.UnitName}_{user.FirstNameEN} {user.LastNameEN}",
                                Approver = item.EmpID,
                                CreateUser = userID,
                                CreateDate = cDate,
                                ModifyUser = userID,
                                ModifyDate = cDate,
                            };
                            supplierApprovalList.Add(entity);
                        }

                        // 寄送通知信
                        var lvlName = this.GetLevelDisplayName(supplierApprovalList[0].Approver, startFlow, dbModel.CoSignApprover);
                        ApprovalMailUtil.SendRevisionVerifyMail(approverList.Select(obj => obj.EMail).ToList(), supplierApprovalList[0], lvlName, userID, cDate);
                        context.TET_SupplierApproval.AddRange(supplierApprovalList);
                    }
                    //--- 新增審核資料 ---


                    // 若改版調整到銀行匯款資訊區塊的11個欄位
                    if (HasDiffInBankFields(dbModel, dbModel_LastVersion))
                        dbModel.RevisionType = RevisionType.Changed.ToText();     //有調整
                    else
                        dbModel.RevisionType = RevisionType.Same.ToText();        //未調整


                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }


        /// <summary> 複製供應商 </summary>
        /// <param name="id"> 要複製的供應商 ID </param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        /// <returns> 新供應商 ID </returns>
        public Guid CopyCurrentReversion(Guid id, string userID, DateTime cDate)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var model = this._supplierMgr.GetTET_Supplier(id, true);

                    var entity = new TET_Supplier()
                    {
                        ID = Guid.NewGuid(),
                        IsSecret = model.IsSecret,
                        IsNDA = model.IsNDA,
                        ApplyReason = model.ApplyReason,
                        BelongTo = model.BelongTo,
                        VenderCode = model.VenderCode,
                        SupplierCategory = model.SupplierCategory_Text,
                        BusinessCategory = model.BusinessCategory_Text,
                        BusinessAttribute = model.BusinessAttribute_Text,
                        SearchKey = model.SearchKey,
                        RelatedDept = model.RelatedDept_Text,
                        Buyer = model.Buyer_Text,
                        RegisterDate = model.RegisterDate_Date,
                        CName = model.CName,
                        EName = model.EName,
                        Country = model.Country,
                        TaxNo = model.TaxNo,
                        Address = model.Address,
                        OfficeTel = model.OfficeTel,
                        ISO = model.ISO,
                        Email = model.Email,
                        Website = model.Website,
                        CapitalAmount = model.CapitalAmount,
                        MainProduct = model.MainProduct,
                        Employees = model.Employees,
                        Charge = model.Charge,
                        PaymentTerm = model.PaymentTerm,
                        BillingDocument = model.BillingDocument,
                        Incoterms = model.Incoterms,
                        Remark = model.Remark,
                        BankCountry = model.BankCountry,
                        BankName = model.BankName,
                        BankCode = model.BankCode,
                        BankBranchName = model.BankBranchName,
                        BankBranchCode = model.BankBranchCode,
                        Currency = model.Currency,
                        BankAccountName = model.BankAccountName,
                        BankAccountNo = model.BankAccountNo,
                        CompanyCity = model.CompanyCity,
                        BankAddress = model.BankAddress,
                        SwiftCode = model.SwiftCode,
                        NDANo = model.NDANo,
                        Contract = model.Contract,
                        IsSign1 = model.IsSign1,
                        SignDate1 = model.SignDate1,
                        IsSign2 = model.IsSign2,
                        SignDate2 = model.SignDate2,
                        STQAApplication = model.STQAApplication,
                        KeySupplier = model.KeySupplier,
                        RevisionType = model.RevisionType,
                        CreateUser = userID,
                        CreateDate = cDate,
                        ModifyUser = userID,
                        ModifyDate = cDate,
                    };

                    // 複製時，資料要調整
                    entity.Version = model.Version + 1;   // 版本為原版號 + 1
                    entity.IsLastVersion = null;          // 是否為最新版本為空
                    entity.ApplyReason = null;
                    entity.ApproveStatus = null;


                    context.TET_Supplier.Add(entity);
                    _contactMgr.CopyTET_SupplierContact(context, entity.ID, model.ContactList, userID, cDate);                              // 複製供應商聯絡人
                    //_attachmentMgr.CopyTET_SupplierAttachment(context, entity.ID, model.AttachmentList, userID, cDate);                     // 複製附件

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

        #region Private methods
        /// <summary> 檢查銀行相關欄位是否有變動過 </summary>
        /// <param name="newVersion"></param>
        /// <param name="oldVersion"></param>
        /// <returns></returns>
        private bool HasDiffInBankFields(TET_Supplier newVersion, TET_Supplier oldVersion)
        {
            bool hasChanged = false;

            if (oldVersion == null)
                return false;

            // 銀行名稱 
            if (newVersion.BankName != oldVersion.BankName)
                hasChanged = true;

            // 銀行代碼 
            if (newVersion.BankCode != oldVersion.BankCode)
                hasChanged = true;

            // 分行名稱 
            if (newVersion.BankBranchName != oldVersion.BankBranchName)
                hasChanged = true;

            // 分行代碼
            if (newVersion.BankBranchCode != oldVersion.BankBranchCode)
                hasChanged = true;

            // 帳號 
            if (newVersion.BankAccountNo != oldVersion.BankAccountNo)
                hasChanged = true;

            // 帳戶名稱 
            if (newVersion.BankAccountName != oldVersion.BankAccountName)
                hasChanged = true;

            // 幣別
            if (newVersion.Currency != oldVersion.Currency)
                hasChanged = true;

            // 銀行國別
            if (newVersion.BankCountry != oldVersion.BankCountry)
                hasChanged = true;

            // 銀行地址
            if ((newVersion.BankAddress!=null ? newVersion.BankAddress : "") != (oldVersion.BankAddress != null ? oldVersion.BankAddress : ""))
                hasChanged = true;

            // SWIFT CODE 
            if ((newVersion.SwiftCode != null ? newVersion.SwiftCode : "") != (oldVersion.SwiftCode != null ? oldVersion.SwiftCode : ""))
                hasChanged = true;

            // 公司註冊地城市
            if ((newVersion.CompanyCity != null ? newVersion.CompanyCity : "") != (oldVersion.CompanyCity != null ? oldVersion.CompanyCity : ""))
                hasChanged = true;

            return hasChanged;
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
