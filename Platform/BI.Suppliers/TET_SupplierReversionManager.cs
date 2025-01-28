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

namespace BI.Suppliers
{
    public class TET_SupplierReversionManager
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
        public List<TET_SupplierModel> GetTET_SupplierReversionList(string caption, string taxNo, string userID, Pager pager)
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
                        orderby item.CreateDate descending
                        select
                            new TET_SupplierModel()
                            {
                                ID = item.ID,
                                IsSecret = item.IsSecret,
                                IsNDA = item.IsNDA,
                                ApplyReason = item.ApplyReason,
                                BelongTo = item.BelongTo,
                                VenderCode = item.VenderCode,
                                SupplierCategory_Text = item.SupplierCategory,
                                BusinessCategory_Text = item.BusinessCategory,
                                BusinessAttribute_Text = item.BusinessAttribute,
                                SearchKey = item.SearchKey,
                                RelatedDept = item.RelatedDept,
                                Buyer = item.Buyer,
                                RegisterDate_Date = item.RegisterDate,
                                CName = item.CName,
                                EName = item.EName,
                                Country = item.Country,
                                TaxNo = item.TaxNo,
                                Address = item.Address,
                                OfficeTel = item.OfficeTel,
                                ISO = item.ISO,
                                Email = item.Email,
                                Website = item.Website,
                                CapitalAmount = item.CapitalAmount,
                                MainProduct = item.MainProduct,
                                Employees = item.Employees,
                                Charge = item.Charge,
                                PaymentTerm = item.PaymentTerm,
                                BillingDocument = item.BillingDocument,
                                Incoterms = item.Incoterms,
                                Remark = item.Remark,
                                BankCountry = item.BankCountry,
                                BankName = item.BankName,
                                BankCode = item.BankCode,
                                BankBranchName = item.BankBranchName,
                                BankBranchCode = item.BankBranchCode,
                                Currency = item.Currency,
                                BankAccountName = item.BankAccountName,
                                BankAccountNo = item.BankAccountNo,
                                CompanyCity = item.CompanyCity,
                                BankAddress = item.BankAddress,
                                SwiftCode = item.SwiftCode,
                                NDANo = item.NDANo,
                                Contract = item.Contract,
                                IsSign1 = item.IsSign1,
                                SignDate1 = item.SignDate1,
                                IsSign2 = item.IsSign2,
                                SignDate2 = item.SignDate2,
                                STQAApplication = item.STQAApplication,
                                KeySupplier = item.KeySupplier,
                                Version = item.Version,
                                RevisionType = item.RevisionType,
                                IsLastVersion = item.IsLastVersion,
                                ApproveStatus = item.ApproveStatus,
                                CreateUser = item.CreateUser,
                                CreateDate = item.CreateDate,
                                ModifyUser = item.ModifyUser,
                                ModifyDate = item.ModifyDate,
                            };

                    query = query.OrderByDescending(obj => obj.CreateDate);
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
            if (!SupplierValidator.ValidModify(model, out List<string> msgList))
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

                    dbModel.IsSecret = model.IsSecret;
                    dbModel.IsNDA = model.IsNDA;
                    dbModel.ApplyReason = model.ApplyReason;
                    dbModel.BelongTo = model.BelongTo;
                    dbModel.VenderCode = model.VenderCode;
                    dbModel.SupplierCategory = model.SupplierCategory_Text;
                    dbModel.BusinessCategory = model.BusinessCategory_Text;
                    dbModel.BusinessAttribute = model.BusinessAttribute_Text;
                    dbModel.SearchKey = model.SearchKey;
                    dbModel.RelatedDept = model.RelatedDept;
                    dbModel.Buyer = model.Buyer;
                    dbModel.RegisterDate = model.RegisterDate_Date;
                    dbModel.CName = model.CName;
                    dbModel.EName = model.EName;
                    dbModel.Country = model.Country;
                    dbModel.TaxNo = model.TaxNo;
                    dbModel.Address = model.Address;
                    dbModel.OfficeTel = model.OfficeTel;
                    dbModel.ISO = model.ISO;
                    dbModel.Email = model.Email;
                    dbModel.Website = model.Website;
                    dbModel.CapitalAmount = model.CapitalAmount;
                    dbModel.MainProduct = model.MainProduct;
                    dbModel.Employees = model.Employees;
                    dbModel.Charge = model.Charge;
                    dbModel.PaymentTerm = model.PaymentTerm;
                    dbModel.BillingDocument = model.BillingDocument;
                    dbModel.Incoterms = model.Incoterms;
                    dbModel.Remark = model.Remark;
                    dbModel.BankCountry = model.BankCountry;
                    dbModel.BankName = model.BankName;
                    dbModel.BankCode = model.BankCode;
                    dbModel.BankBranchName = model.BankBranchName;
                    dbModel.BankBranchCode = model.BankBranchCode;
                    dbModel.Currency = model.Currency;
                    dbModel.BankAccountName = model.BankAccountName;
                    dbModel.BankAccountNo = model.BankAccountNo;
                    dbModel.CompanyCity = model.CompanyCity;
                    dbModel.BankAddress = model.BankAddress;
                    dbModel.SwiftCode = model.SwiftCode;
                    dbModel.NDANo = model.NDANo;
                    dbModel.Contract = model.Contract;
                    dbModel.IsSign1 = model.IsSign1;
                    dbModel.SignDate1 = model.SignDate1;
                    dbModel.IsSign2 = model.IsSign2;
                    dbModel.SignDate2 = model.SignDate2;
                    dbModel.STQAApplication = model.STQAApplication;
                    dbModel.KeySupplier = model.KeySupplier;
                    dbModel.Version = model.Version;
                    dbModel.RevisionType = model.RevisionType;
                    dbModel.IsLastVersion = model.IsLastVersion;
                    dbModel.ApproveStatus = model.ApproveStatus;
                    dbModel.ModifyUser = userID;
                    dbModel.ModifyDate = cDate;

                    // 若改版調整到銀行匯款資訊區塊的11個欄位
                    if (HasDiffInBankFields(dbModel, dbModel_LastVersion))
                        dbModel.RevisionType = "2";     //有調整
                    else
                        dbModel.RevisionType = "1";     //未調整

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
                        string.Compare(ApprovalStatus.Rejected.ToText(), dbModel.ApproveStatus, true) != 0)
                        throw new Exception("送審狀態必須為空，或是已退回.");

                    // 更改原資料的審核狀態
                    dbModel.ApproveStatus = ApprovalStatus.Verify.ToText();
                    //--- 調整原資料的審核狀態 ---



                    ApprovalLevel startFlow;
                    List<UserAccountModel> approverList = new List<UserAccountModel>();

                    // 登入者是否擁有SRI_SS角色
                    if (this._userRoleMgr.IsRole(userID, ApprovalRole.SRI_SS.ToID().Value))     // 當登入者擁有SRI_SS角色
                    {
                        startFlow = ApprovalLevel.User_GL;
                        approverList.Add(this._userMgr.GetUserLeader(userID));
                    }
                    else    // 當登入者未擁有SRI_SS角色
                    {
                        startFlow = ApprovalLevel.SRI_SS_GL;
                        approverList.AddRange(this._userRoleMgr.GetUserListInRole(ApprovalRole.SRI_SS.ToID().Value));
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
                                Level = ApprovalLevel.User_GL.ToText(),
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
                        SendVerifyMail(approverList.Select(obj => obj.EMail).ToList(), supplierApprovalList[0], cDate, cDate);
                        context.TET_SupplierApproval.AddRange(supplierApprovalList);
                    }
                    //--- 新增審核資料 ---


                    // 若改版調整到銀行匯款資訊區塊的11個欄位
                    if (HasDiffInBankFields(dbModel, dbModel_LastVersion))
                        dbModel.RevisionType = "2";     //有調整
                    else
                        dbModel.RevisionType = "1";     //未調整


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
                        RelatedDept = model.RelatedDept,
                        Buyer = model.Buyer,
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
                    _attachmentMgr.CopyTET_SupplierAttachment(context, entity.ID, model.AttachmentList, userID, cDate);                     // 複製附件

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
            if (newVersion.BankAddress != oldVersion.BankAddress)
                hasChanged = true;

            // SWIFT CODE 
            if (newVersion.SwiftCode != oldVersion.SwiftCode)
                hasChanged = true;

            // 公司註冊地城市
            if (newVersion.CompanyCity != oldVersion.CompanyCity)
                hasChanged = true;

            return hasChanged;
        }

        /// <summary> 寄信新的簽核人 </summary>
        /// <param name="receiverMailList"></param>
        /// <param name="approvalModel"></param>
        /// <param name="createTime"></param>
        private void SendVerifyMail(List<string> receiverMailList, TET_SupplierApproval approvalModel, DateTime createTime, DateTime thisApprovalCreateTime)
        {
            var pageUrl = $"{ModuleConfig.EmailRootUrl}/SupplierApproval/Index";

            EMailContent content = new EMailContent()
            {
                Title = $"[審核通知] {approvalModel.Description}",
                Body =
                $@"
您好,<br/>
請點「<a href=""{pageUrl}"" target=""_blank"">待審清單</a>」，謝謝 <br/>
<br/>
流程名稱: {ApprovalType.Modify.ToText()} <br/>
流程發起時間: {createTime.ToString("yyyy-MM-dd HH:mm:ss")} <br/>
審核關卡: {approvalModel.Level} <br/>
審核開始時間: {thisApprovalCreateTime.ToString("yyyy-MM-dd HH:mm:ss")} <br/>
                "
            };

            EMailSender.Send(receiverMailList, content);
        }
        #endregion
    }
}
