using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Platform.AbstractionClass;
using Platform.Infra;
using Platform.LogService;
using Platform.ORM;
using BI.PaymentSuppliers.Models;
using BI.PaymentSuppliers.Enums;
using BI.PaymentSuppliers.Utils;
using Platform.Auth;
using System.Xml.Linq;
using Platform.Auth.Models;
using BI.PaymentSuppliers.Validators;

namespace BI.PaymentSuppliers
{
    public class TET_PaymentSupplierRevisionManager
    {
        private Logger _logger = new Logger();
        private TET_PaymentSupplierManager _supplierMgr = new TET_PaymentSupplierManager();
        private TET_PaymentSupplierContactManager _contactMgr = new TET_PaymentSupplierContactManager();
        private TET_PaymentSupplierAttachmentManager _attachmentMgr = new TET_PaymentSupplierAttachmentManager();
        private UserManager _userMgr = new UserManager();
        private UserRoleManager _userRoleMgr = new UserRoleManager();

        #region Read
        /// <summary> 取得 一般付款對象 清單 </summary>
        /// <param name="caption">中文名稱 / 英文名稱 / 供應商代碼</param>
        /// <param name="taxNo">統一編號</param>
        /// <param name="userID"> 目前登入者 </param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<PaymentSupplierListModel> GetTET_PaymentSupplierReversionList(string caption, string taxNo, string userID, Pager pager)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var orgQuery = context.vwTET_PaymentSupplier_LastVersion.AsQueryable();

                    //--- 組合過濾條件 ---
                    if (!string.IsNullOrWhiteSpace(caption))
                        orgQuery = orgQuery.Where(obj => obj.CName.Contains(caption) || obj.EName.Contains(caption) || obj.VenderCode.Contains(caption));

                    if (!string.IsNullOrWhiteSpace(taxNo))
                        //orgQuery = orgQuery.Where(obj => obj.TaxNo.Contains(taxNo) || obj.IdNo.Contains(taxNo))
                        orgQuery = orgQuery.Where(obj => obj.TaxNo.Contains(taxNo));
                    //--- 組合過濾條件 ---


                    var query =
                        from item in orgQuery
                        let appItem = context.TET_PaymentSupplierApproval.Where(obj => obj.PSID == item.ID && string.IsNullOrEmpty(obj.Result)).FirstOrDefault()
                        orderby item.CreateDate descending
                        select
                            new PaymentSupplierListModel()
                            {
                                ID = item.ID,
                                VenderCode = item.VenderCode,
                                CName = item.CName,
                                EName = item.EName,
                                TaxNo = item.TaxNo,
                                IdNo = item.IdNo,
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
        /// <summary> 修改 一般付款對象 </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        public void ModifyTET_PaymentSupplier(TET_PaymentSupplierModel model, string userID, DateTime cDate)
        {
            if (model == null)
                throw new ArgumentNullException("Model is required.");

            // 新增前，先檢查是否能通過商業邏輯
            if (!PaymentSupplierRevisionValidator.Valid(model, out List<string> msgList))
                throw new ArgumentException(string.Join(Environment.NewLine, msgList));

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                        (from item in context.TET_PaymentSupplier
                         where item.ID == model.ID
                         select item).FirstOrDefault();

                    var dbModel_LastVersion =
                        (from item in context.TET_PaymentSupplier
                         where
                            item.VenderCode == dbModel.VenderCode &&
                            item.Version == dbModel.Version - 1
                         select item).FirstOrDefault();

                    if (dbModel == null)
                        throw new NullReferenceException($"{model.ID} don't exists.");

                    dbModel.ApplyReason = model.ApplyReason;
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
                    dbModel.CompanyCity = model.CompanyCity;
                    dbModel.BankAddress = model.BankAddress;
                    dbModel.SwiftCode = model.SwiftCode;
                    dbModel.ModifyUser = userID;
                    dbModel.ModifyDate = cDate;

                    _contactMgr.WriteTET_PaymentSupplierContact(context, dbModel.ID, model.ContactList, userID, cDate);                              // 寫入供應商
                    _attachmentMgr.WriteTET_PaymentSupplierAttachment(context, dbModel.ID, model.AttachmentList, model.UploadFiles, userID, cDate);  // 寫入聯絡人

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
        public void SubmitTET_PaymentSupplierRevision(TET_PaymentSupplierModel model, string userID, DateTime cDate)
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
                    (from item in context.TET_PaymentSupplier
                     where item.ID == model.ID
                     select item).FirstOrDefault();

                    if (dbModel == null)
                        throw new NullReferenceException($"{model.ID} don't exists.");

                    var dbModel_LastVersion =
                        (from item in context.TET_PaymentSupplier
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

                    startFlow = ApprovalLevel.User_GL;
                    approverList.Add(this._userMgr.GetUserLeader(userID));

                    //--- 新增審核資料 ---
                    List<TET_PaymentSupplierApproval> paymentsupplierApprovalList = new List<TET_PaymentSupplierApproval>();
                    if (approverList.Count > 0)
                    {
                        // 建立新的申請資訊
                        foreach (var item in approverList)
                        {
                            var entity = new TET_PaymentSupplierApproval()
                            {
                                ID = Guid.NewGuid(),
                                PSID = dbModel.ID,
                                Type = ApprovalType.Modify.ToText(),
                                Level = startFlow.ToText(),
                                Description = $"{ApprovalType.Modify.ToText()}_{dbModel.CName}_{user.UnitName}_{user.FirstNameEN} {user.LastNameEN}",
                                Approver = item.EmpID,
                                CreateUser = userID,
                                CreateDate = cDate,
                                ModifyUser = userID,
                                ModifyDate = cDate,
                            };
                            paymentsupplierApprovalList.Add(entity);
                        }

                        // 寄送通知信
                        ApprovalMailUtil.SendRevisionVerifyMail(approverList.Select(obj => obj.EMail).ToList(), paymentsupplierApprovalList[0], userID, cDate);
                        context.TET_PaymentSupplierApproval.AddRange(paymentsupplierApprovalList);
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


        /// <summary> 複製一般付款對象 </summary>
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
                    var model = this._supplierMgr.GetTET_PaymentSupplier(id);

                    var entity = new TET_PaymentSupplier()
                    {
                        ID = Guid.NewGuid(),
                        ApplyReason = model.ApplyReason,
                        VenderCode = model.VenderCode,
                        RegisterDate = model.RegisterDate_Date,
                        CName = model.CName,
                        EName = model.EName,
                        Country = model.Country,
                        TaxNo = model.TaxNo,
                        IdNo = model.IdNo,
                        Address = model.Address,
                        OfficeTel = model.OfficeTel,
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


                    context.TET_PaymentSupplier.Add(entity);
                    _contactMgr.CopyTET_PaymentSupplierContact(context, entity.ID, model.ContactList, userID, cDate);                              // 複製供應商聯絡人

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
    }
}
