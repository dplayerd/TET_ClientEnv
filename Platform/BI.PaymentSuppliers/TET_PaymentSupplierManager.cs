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
using Platform.Auth.Models;
using BI.PaymentSuppliers.Flows;
using BI.PaymentSuppliers.Validators;
using BI.Shared;
using Newtonsoft.Json;

namespace BI.PaymentSuppliers
{
    public class TET_PaymentSupplierManager
    {
        private Logger _logger = new Logger();

        private TET_PaymentSupplierContactManager _contactMgr = new TET_PaymentSupplierContactManager();
        private TET_PaymentSupplierAttachmentManager _attachmentMgr = new TET_PaymentSupplierAttachmentManager();
        private UserManager _userMgr = new UserManager();
        private UserRoleManager _userRoleMgr = new UserRoleManager();
        private TET_ParametersManager _paraMgr = new TET_ParametersManager();

        #region Queryable
        /// <summary> 組合過濾條件 </summary>
        /// <param name="query"> 原本的查詢 </param>
        /// <param name="queryParams"> 查詢條件 </param>
        /// <returns></returns>
        private IQueryable<TET_PaymentSupplier> BuildQueryCondition(IQueryable<TET_PaymentSupplier> query, ISupplierListQueryInput queryParams)
        {
            if (!string.IsNullOrWhiteSpace(queryParams.caption))
                query = query.Where(obj => obj.CName.Contains(queryParams.caption) || obj.EName.Contains(queryParams.caption) || obj.VenderCode.Contains(queryParams.caption));

            if (!string.IsNullOrWhiteSpace(queryParams.taxNo))
                query = query.Where(obj => obj.TaxNo.Contains(queryParams.taxNo) || obj.IdNo.Contains(queryParams.taxNo));

            return query;
        }

        /// <summary> 組合轉換條件 </summary>
        /// <param name="orgQuery"></param>
        /// <returns></returns>
        private IQueryable<TET_PaymentSupplierModel> BuildSelectModel(IQueryable<TET_PaymentSupplier> orgQuery)
        {
            var query =
                from item in orgQuery
                select
                    new TET_PaymentSupplierModel()
                    {
                        ID = item.ID,
                        CoSignApprover_Text = item.CoSignApprover,
                        ApplyReason = item.ApplyReason,
                        VenderCode = item.VenderCode,
                        RegisterDate_Date = item.RegisterDate,
                        CName = item.CName,
                        EName = item.EName,
                        Country = item.Country.ToUpper(),
                        TaxNo = item.TaxNo,
                        IdNo = item.IdNo,
                        Address = item.Address,
                        OfficeTel = item.OfficeTel,
                        Charge = item.Charge,
                        PaymentTerm = item.PaymentTerm.ToUpper(),
                        BillingDocument = item.BillingDocument.ToUpper(),
                        Incoterms = item.Incoterms.ToUpper(),
                        Remark = item.Remark,
                        BankCountry = item.BankCountry.ToUpper(),
                        BankName = item.BankName,
                        BankCode = item.BankCode,
                        BankBranchName = item.BankBranchName,
                        BankBranchCode = item.BankBranchCode,
                        Currency = item.Currency.ToUpper(),
                        BankAccountName = item.BankAccountName,
                        BankAccountNo = item.BankAccountNo,
                        CompanyCity = item.CompanyCity,
                        BankAddress = item.BankAddress,
                        SwiftCode = item.SwiftCode,
                        Version = item.Version,
                        IsLastVersion = item.IsLastVersion,
                        ApproveStatus = item.ApproveStatus,
                        CreateUser = item.CreateUser,
                        CreateDate = item.CreateDate,
                        ModifyUser = item.ModifyUser,
                        ModifyDate = item.ModifyDate,
                        IsActive = item.IsActive,
                    };

            return query;
        }
        #endregion

        #region Read
        /// <summary>
        /// 取得 一般付款對象 清單
        /// </summary>
        /// <param name="queryParams">各種過濾條件</param>
        /// <param name="userID"> 目前登入者 </param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<PaymentSupplierListModel> GetTET_PaymentSupplierList(ISupplierListQueryInput queryParams, string userID, Pager pager)
        {
            // 檢查目前登入者是否有指定角色的身份
            var isACC_First = _userRoleMgr.IsRole(userID, ApprovalRole.ACC_First.ToID().Value);
            var isACC_Second = _userRoleMgr.IsRole(userID, ApprovalRole.ACC_Second.ToID().Value);
            var isACC_Last = _userRoleMgr.IsRole(userID, ApprovalRole.ACC_Last.ToID().Value);

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    // 原始查詢
                    var orgQuery =
                        from item in context.TET_PaymentSupplier
                        where item.Version == 0
                        select item;

                    if (!isACC_First && !isACC_Second && !isACC_Last)
                        orgQuery = orgQuery.Where(obj => obj.CreateUser == userID);


                    orgQuery = BuildQueryCondition(orgQuery, queryParams);  // 組合過濾條件

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
                                ApproveStatus = item.ApproveStatus,
                                Version = item.Version,
                                IsLastVersion = item.IsLastVersion,
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

        /// <summary>
        /// 取得 一般付款對象 清單
        /// </summary>
        /// <param name="queryParams">各種過濾條件</param>
        /// <param name="userID"> 目前登入者 </param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<TET_PaymentSupplierModel> GetTET_PaymentSupplierLastVersionList(ISupplierListQueryInput queryParams, string userID, Pager pager)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    // 原始查詢
                    var orgQuery =
                        from item in context.TET_PaymentSupplier
                        where item.IsLastVersion == "Y"
                        select item;

                    orgQuery = BuildQueryCondition(orgQuery, queryParams);  // 組合過濾條件
                    var query = this.BuildSelectModel(orgQuery);            // 組合轉換條件

                    query = query.OrderByDescending(obj => obj.CreateDate);
                    var list = query.ProcessPager(pager).ToList();

                    //--- 處理輸出資料 ---

                    // 常用參數
                    var paraList = this._paraMgr.GetAllParametersKeyTextList();

                    foreach (var item in list)
                    {
                        //國家別
                        if (item.Country != null)
                        {
                            if (item.CountryFullNameList != null)
                                item.CountryFullNameList = paraList.Where(obj => item.Country.Contains(obj.ID.ToString().ToUpper().Replace("{", "").Replace("}", ""))).Select(obj => $"{obj.Item}").ToList();
                        }

                        //付款條件
                        if (item.PaymentTerm != null)
                        {
                            if (item.PaymentTermFullNameList != null)
                                item.PaymentTermFullNameList = paraList.Where(obj => item.PaymentTerm.Contains(obj.ID.ToString().ToUpper().Replace("{", "").Replace("}", ""))).Select(obj => $"{obj.Item}").ToList();
                        }

                        //交易條件
                        if (item.Incoterms != null)
                        {
                            if (item.IncotermsFullNameList != null)
                                item.IncotermsFullNameList = paraList.Where(obj => item.Incoterms.Contains(obj.ID.ToString().ToUpper().Replace("{", "").Replace("}", ""))).Select(obj => $"{obj.Item}").ToList();
                        }

                        //請款憑證
                        if (item.BillingDocument != null)
                        {
                            if (item.BillingDocumentFullNameList != null)
                                item.BillingDocumentFullNameList = paraList.Where(obj => item.BillingDocument.Contains(obj.ID.ToString().ToUpper().Replace("{", "").Replace("}", ""))).Select(obj => $"{obj.Item}").ToList();
                        }

                        //幣別
                        if (item.Currency != null)
                        {
                            if (item.CurrencyFullNameList != null)
                                item.CurrencyFullNameList = paraList.Where(obj => item.Currency.Contains(obj.ID.ToString().ToUpper().Replace("{", "").Replace("}", ""))).Select(obj => $"{obj.Item}").ToList();
                        }

                        //銀行國別
                        if (item.BankCountry != null)
                        {
                            if (item.BankCountryFullNameList != null)
                                item.BankCountryFullNameList = paraList.Where(obj => item.BankCountry.Contains(obj.ID.ToString().ToUpper().Replace("{", "").Replace("}", ""))).Select(obj => $"{obj.Item}").ToList();
                        }
                    }
                    //--- 處理輸出資料 ---

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
        /// 取得 一般付款對象 清單
        /// </summary>
        /// <param name="queryParams">各種過濾條件</param>
        /// <param name="userID"> 目前登入者 </param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<TET_PaymentSupplierModel> GetTET_PaymentSupplierActiveLastVersionList(ISupplierListQueryInput queryParams, string userID, Pager pager)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    // 原始查詢
                    var orgQuery =
                        from item in context.TET_PaymentSupplier
                        where item.IsActive == "Active" && item.IsLastVersion == "Y"
                        select item;

                    orgQuery = BuildQueryCondition(orgQuery, queryParams);  // 組合過濾條件
                    var query = this.BuildSelectModel(orgQuery);            // 組合轉換條件

                    query = query.OrderByDescending(obj => obj.CreateDate);
                    var list = query.ProcessPager(pager).ToList();

                    //--- 處理輸出資料 ---

                    // 常用參數
                    var paraList = this._paraMgr.GetAllParametersKeyTextList();

                    foreach (var item in list)
                    {
                        //國家別
                        if (item.Country != null)
                        {
                            if (item.CountryFullNameList != null)
                                item.CountryFullNameList = paraList.Where(obj => item.Country.Contains(obj.ID.ToString().ToUpper().Replace("{", "").Replace("}", ""))).Select(obj => $"{obj.Item}").ToList();
                        }

                        //付款條件
                        if (item.PaymentTerm != null)
                        {
                            if (item.PaymentTermFullNameList != null)
                                item.PaymentTermFullNameList = paraList.Where(obj => item.PaymentTerm.Contains(obj.ID.ToString().ToUpper().Replace("{", "").Replace("}", ""))).Select(obj => $"{obj.Item}").ToList();
                        }

                        //交易條件
                        if (item.Incoterms != null)
                        {
                            if (item.IncotermsFullNameList != null)
                                item.IncotermsFullNameList = paraList.Where(obj => item.Incoterms.Contains(obj.ID.ToString().ToUpper().Replace("{", "").Replace("}", ""))).Select(obj => $"{obj.Item}").ToList();
                        }

                        //請款憑證
                        if (item.BillingDocument != null)
                        {
                            if (item.BillingDocumentFullNameList != null)
                                item.BillingDocumentFullNameList = paraList.Where(obj => item.BillingDocument.Contains(obj.ID.ToString().ToUpper().Replace("{", "").Replace("}", ""))).Select(obj => $"{obj.Item}").ToList();
                        }

                        //幣別
                        if (item.Currency != null)
                        {
                            if (item.CurrencyFullNameList != null)
                                item.CurrencyFullNameList = paraList.Where(obj => item.Currency.Contains(obj.ID.ToString().ToUpper().Replace("{", "").Replace("}", ""))).Select(obj => $"{obj.Item}").ToList();
                        }

                        //銀行國別
                        if (item.BankCountry != null)
                        {
                            if (item.BankCountryFullNameList != null)
                                item.BankCountryFullNameList = paraList.Where(obj => item.BankCountry.Contains(obj.ID.ToString().ToUpper().Replace("{", "").Replace("}", ""))).Select(obj => $"{obj.Item}").ToList();
                        }
                    }
                    //--- 處理輸出資料 ---

                    return list;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                return default;
            }
        }

        /// <summary> 查詢一般付款對象 </summary>
        /// <param name="ID"> 一般付款對象 ID </param>
        /// <returns></returns>
        public TET_PaymentSupplierModel GetTET_PaymentSupplier(Guid ID)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in context.TET_PaymentSupplier
                        where item.ID == ID
                        select item;

                    var result = this.BuildSelectModel(query).FirstOrDefault();
                    if (result != null)
                    {
                        result.ContactList = this._contactMgr.GetTET_PaymentSupplierContactList(context, ID);
                        result.AttachmentList = this._attachmentMgr.GetTET_PaymentSupplierAttachmentsList(context, ID);
                        result.ApprovalList = this.GetApprovalList(context, result);
                    }

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

        #region OtherRead
        /// <summary> 是否能開啟一般付款對象資料 </summary>
        /// <param name="cUser"> 目前登入者 </param>
        /// <returns></returns>
        public bool CanExport(string cUser)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    // 檢查目前登入者是否有指定角色的身份
                    var isACC_First = _userRoleMgr.IsRole(cUser, ApprovalRole.ACC_First.ToID().Value);
                    var isACC_Second = _userRoleMgr.IsRole(cUser, ApprovalRole.ACC_Second.ToID().Value);
                    var isACC_Last = _userRoleMgr.IsRole(cUser, ApprovalRole.ACC_Last.ToID().Value);

                    // 如果任一條件達成，就可以閱讀
                    var result = isACC_First || isACC_Second || isACC_Last;
                    return result;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> 是否能開啟一般付款對象資料 </summary>
        /// <param name="psID"> 一般付款對象代碼 </param>
        /// <param name="cUser"> 目前登入者 </param>
        /// <returns></returns>
        public bool CanRead(Guid psID, string cUser)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var supplier = this.GetTET_PaymentSupplier(psID);

                    if (supplier == null)
                        return false;


                    // 檢查目前登入者是否有指定角色的身份
                    var isACC_First = _userRoleMgr.IsRole(cUser, ApprovalRole.ACC_First.ToID().Value);
                    var isACC_Second = _userRoleMgr.IsRole(cUser, ApprovalRole.ACC_Second.ToID().Value);
                    var isACC_Last = _userRoleMgr.IsRole(cUser, ApprovalRole.ACC_Last.ToID().Value);
                    var roleResult = isACC_First || isACC_Second || isACC_Last;


                    // 檢查是否為建立或修改者
                    var creatorResult = false;
                    if (string.Compare(cUser, supplier.CreateUser, true) == 0 ||
                       string.Compare(cUser, supplier.ModifyUser, true) == 0)
                        creatorResult = true;


                    // 只有審核中的需要檢查
                    bool approvalResult = true;
                    if (string.Compare(ApprovalStatus.Verify.ToText(), supplier.ApproveStatus, true) == 0)
                    {
                        var approvalQuery =
                            from item in context.TET_PaymentSupplierApproval
                            where
                                item.PSID == psID && item.Approver == cUser
                            select item.ID;
                        approvalResult = approvalQuery.Any();
                    }

                    // 如果任一條件達成，就可以閱讀
                    var result = creatorResult || approvalResult || roleResult;
                    return result;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> 取得簽核人資料 </summary>
        /// <param name="context"></param>
        /// <param name="mainModel"></param>
        /// <returns></returns>
        private List<TET_PaymentSupplierApprovalModel> GetApprovalList(PlatformContextModel context, TET_PaymentSupplierModel mainModel)
        {
            if (mainModel?.ID == null)
                return new List<TET_PaymentSupplierApprovalModel>();

            var sourceList =
                (from item in context.TET_PaymentSupplierApproval
                 where item.PSID == mainModel.ID.Value
                 orderby item.CreateDate
                 select item).ToList();

            var approverKeyList = sourceList
                .Where(obj => !string.IsNullOrWhiteSpace(obj.Approver))
                .Select(obj => obj.Approver)
                .Distinct()
                .ToList();

            var userList =
                (from item in context.Users
                 where approverKeyList.Contains(item.EmpID) || approverKeyList.Contains(item.UserID)
                 select new UserAccountModel()
                 {
                     ID = item.UserID,
                     EmpID = item.EmpID,
                     FirstNameEN = item.FirstNameEN,
                     LastNameEN = item.LastNameEN,
                 }).ToList();

            var result = sourceList
                .Select(item =>
                {
                    var approverInfo = userList.FirstOrDefault(obj =>
                        string.Compare(obj.EmpID, item.Approver, true) == 0 ||
                        string.Compare(obj.ID, item.Approver, true) == 0);
                    var level = ApprovalUtils.ParseApprovalLevel(item.Level);

                    return new TET_PaymentSupplierApprovalModel()
                    {
                        ID = item.ID,
                        PSID = item.PSID,
                        Type = item.Type,
                        Description = item.Description,
                        Level = item.Level,
                        PreviewLevel = this.GetLevelDisplayName(item.Approver, level, mainModel.CoSignApprover_Text),
                        Approver = this.FormatApproverName(approverInfo, item.Approver),
                        Result = item.Result,
                        Comment = item.Comment,
                        CreateUser = item.CreateUser,
                        CreateDate = item.CreateDate,
                        ModifyUser = item.ModifyUser,
                        ModifyDate = item.ModifyDate,
                    };
                }).ToList();

            this.AppendPendingApprovalSteps(result, sourceList, mainModel);

            return result;
        }

        /// <summary> 補上一般付款對象審核中的預計後續關卡 </summary>
        /// <param name="result">畫面顯示用審核清單</param>
        /// <param name="sourceList">資料庫原始審核清單</param>
        /// <param name="mainModel">一般付款對象資料</param>
        private void AppendPendingApprovalSteps(List<TET_PaymentSupplierApprovalModel> result, List<TET_PaymentSupplierApproval> sourceList, TET_PaymentSupplierModel mainModel)
        {
            if (mainModel?.ID == null)
                return;

            if (string.Compare(mainModel.ApproveStatus, ApprovalStatus.Verify.ToText(), true) != 0)
                return;

            if (!sourceList.Any())
                return;

            var type = sourceList.Select(obj => obj.Type).FirstOrDefault(obj => !string.IsNullOrWhiteSpace(obj));
            var flowList = this.GetApprovalFlowList(type);
            if (!flowList.Any())
                return;

            var pendingLevelList = sourceList
                .Where(obj => string.IsNullOrWhiteSpace(obj.Result))
                .Select(obj => ApprovalUtils.ParseApprovalLevel(obj.Level))
                .Where(obj => obj != ApprovalLevel.Empty)
                .ToList();

            var baseLevelList = pendingLevelList.Any()
                ? pendingLevelList
                : sourceList.Select(obj => ApprovalUtils.ParseApprovalLevel(obj.Level)).Where(obj => obj != ApprovalLevel.Empty).ToList();

            var baseFlowIndex = baseLevelList
                .Select(obj => flowList.FindIndex(flow => flow.Level == obj))
                .Where(obj => obj >= 0)
                .DefaultIfEmpty(-1)
                .Max();

            if (baseFlowIndex < 0)
                return;

            var description = sourceList.Select(obj => obj.Description).FirstOrDefault(obj => !string.IsNullOrWhiteSpace(obj));
            if (string.IsNullOrWhiteSpace(type))
                type = ApprovalType.New.ToText();

            foreach (var flow in flowList.Skip(baseFlowIndex + 1))
            {
                var approverList = this.GetLevelApproverList(flow, mainModel.CreateUser)
                    .Where(obj => obj != null)
                    .ToList();

                if (!approverList.Any())
                    approverList.Add(null);

                foreach (var approver in approverList)
                {
                    result.Add(new TET_PaymentSupplierApprovalModel()
                    {
                        ID = Guid.Empty,
                        PSID = mainModel.ID.Value,
                        Type = type,
                        Description = description,
                        Level = flow.Level.ToText(),
                        PreviewLevel = this.GetLevelDisplayName(approver?.EmpID, flow.Level, mainModel.CoSignApprover_Text),
                        Approver = this.FormatApproverName(approver, string.Empty),
                        PreviewResult = "未來審核關卡",
                        IsSimulated = true,
                    });
                }
            }
        }

        /// <summary> 取得一般付款對象審核流程 </summary>
        /// <param name="type">審核類型</param>
        /// <returns></returns>
        private List<FlowModel> GetApprovalFlowList(string type)
        {
            if (string.Compare(type, ApprovalType.New.ToText(), true) == 0)
                return NewPaymentSupplierFlow.GetFlowList();

            if (string.Compare(type, ApprovalType.Modify.ToText(), true) == 0)
                return ModifyPaymentSupplierFlow.GetFlowList();

            return new List<FlowModel>();
        }

        /// <summary> 依照關卡取得預計簽核人 </summary>
        /// <param name="flow">流程</param>
        /// <param name="applicantID">申請人帳號</param>
        /// <returns></returns>
        private List<UserAccountModel> GetLevelApproverList(FlowModel flow, string applicantID)
        {
            switch (flow.Level)
            {
                case ApprovalLevel.User_GL:
                    var manager = this._userMgr.GetUserLeader(applicantID);
                    return manager == null ? new List<UserAccountModel>() : new List<UserAccountModel>() { manager };

                case ApprovalLevel.ACC_First:
                    return this._userRoleMgr.GetUserListInRole(ApprovalRole.ACC_First.ToID().Value);

                case ApprovalLevel.ACC_Second:
                    return this._userRoleMgr.GetUserListInRole(ApprovalRole.ACC_Second.ToID().Value);

                case ApprovalLevel.ACC_Last:
                    return this._userRoleMgr.GetUserListInRole(ApprovalRole.ACC_Last.ToID().Value);

                case ApprovalLevel.Empty:
                default:
                    return new List<UserAccountModel>();
            }
        }

        /// <summary> 格式化審核者顯示名稱 </summary>
        /// <param name="user">使用者資料</param>
        /// <param name="fallback">查無使用者時的顯示文字</param>
        /// <returns></returns>
        private string FormatApproverName(UserAccountModel user, string fallback)
        {
            if (user == null)
                return fallback;

            var name = $"{user.FirstNameEN} {user.LastNameEN}".Trim();
            if (string.IsNullOrWhiteSpace(name))
                return $"({user.EmpID})";

            return $"{name}({user.EmpID})";
        }

        /// <summary> 檢查供應商代碼是否已存在 </summary>
        /// <param name="context"></param>
        /// <param name="supplierModel"></param>
        /// <returns></returns>
        internal bool IsVendorCodeExisted(PlatformContextModel context, TET_PaymentSupplierModel supplierModel)
        {
            var completedStatusText = ApprovalStatus.Completed.ToText();
            var query =
                (from item in context.TET_PaymentSupplier
                 where
                    item.ID != supplierModel.ID &&
                    !string.IsNullOrEmpty(item.VenderCode) &&
                    item.VenderCode == supplierModel.VenderCode &&
                    item.ApproveStatus == completedStatusText &&
                    item.IsLastVersion == "Y"
                 select item);

            var isExist = query.Any();
            return isExist;
        }



        /// <summary> 取得要呈現的關卡名稱
        /// (因為加簽的人，要顯示不同的關卡)
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="approvalId"></param>
        /// <returns></returns>
        public string GetLevelDisplayName(Guid supplierId, Guid approvalId)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    // Supplier
                    var supplierQuery =
                        (from item in context.TET_PaymentSupplier
                         where item.ID == supplierId
                         select new { item.CoSignApprover });

                    var supplier = supplierQuery.FirstOrDefault();

                    // Approver
                    var appQuery =
                        from item in context.TET_PaymentSupplierApproval
                        where item.ID == approvalId
                        select new
                        {
                            item.Level,
                            item.Approver
                        };
                    var approver = appQuery.FirstOrDefault();


                    // 任一查不到，關卡名稱用空字串
                    if (supplier == null || approver == null)
                        return string.Empty;


                    // 如果是 User_GL 這關，而且是加簽人，將關卡名稱換掉
                    var lvl = ApprovalUtils.ParseApprovalLevel(approver.Level);
                    if (supplier != null)
                        return GetLevelDisplayName(approver.Approver, lvl, supplier.CoSignApprover);

                    return lvl.ToDisplayText();
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
        /// <summary> 新增 一般付款對象 </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public Guid CreateTET_PaymentSupplier(TET_PaymentSupplierModel model, string userID, DateTime cDate)
        {
            if (model == null)
                throw new ArgumentNullException("Supplier is required.");

            // 新增前，先檢查是否能通過商業邏輯
            if (!PaymentSupplierValidator.ValidCreate(model, out List<string> msgList))
                throw new ArgumentException(string.Join(Environment.NewLine, msgList));

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var entity = new TET_PaymentSupplier()
                    {
                        ID = Guid.NewGuid(),
                        CoSignApprover = model.CoSignApprover_Text,
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
                        ApproveStatus = model.ApproveStatus,
                        CreateUser = userID,
                        CreateDate = cDate,
                        ModifyUser = userID,
                        ModifyDate = cDate,
                    };

                    // 剛建立時
                    entity.Version = 0;            // 版本強制為 0
                    entity.IsLastVersion = "N";    // 是否為最新版本為 N
                    entity.RegisterDate = cDate;   // 填入當下時間

                    context.TET_PaymentSupplier.Add(entity);
                    _contactMgr.WriteTET_PaymentSupplierContact(context, entity.ID, model.ContactList, userID, cDate);                              // 寫入供應商
                    _attachmentMgr.WriteTET_PaymentSupplierAttachment(context, entity.ID, model.AttachmentList, model.UploadFiles, userID, cDate);  // 寫入聯絡人

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

        /// <summary> 修改 一般付款對象 </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        public void ModifyTET_PaymentSupplier(TET_PaymentSupplierModel model, string userID, DateTime cDate)
        {
            if (model == null)
                throw new ArgumentNullException("Model is required.");

            // 新增前，先檢查是否能通過商業邏輯
            if (!PaymentSupplierValidator.ValidModify(model, out List<string> msgList))
                throw new ArgumentException(string.Join(Environment.NewLine, msgList));

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                        (from item in context.TET_PaymentSupplier
                         where item.ID == model.ID
                         select item).FirstOrDefault();

                    if (dbModel == null)
                        throw new NullReferenceException($"{model.ID} don't exists.");

                    dbModel.CoSignApprover = model.CoSignApprover_Text;
                    dbModel.ApplyReason = model.ApplyReason;
                    dbModel.VenderCode = model.VenderCode;
                    dbModel.RegisterDate = model.RegisterDate_Date;
                    dbModel.CName = model.CName;
                    dbModel.EName = model.EName;
                    dbModel.Country = model.Country;
                    dbModel.TaxNo = model.TaxNo;
                    dbModel.IdNo = model.IdNo;
                    dbModel.Address = model.Address;
                    dbModel.OfficeTel = model.OfficeTel;
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
                    dbModel.Version = model.Version;
                    dbModel.IsLastVersion = model.IsLastVersion;
                    dbModel.ApproveStatus = model.ApproveStatus;
                    dbModel.IsActive = model.IsActive;
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


        /// <summary> 修改 一般付款對象 </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        public void ModifyTET_PaymentSupplier_Query(TET_PaymentSupplierModel model, string userID, DateTime cDate)
        {
            if (model == null)
                throw new ArgumentNullException("Model is required.");

            // 新增前，先檢查是否能通過商業邏輯
            if (!QueryValidator.Valid(model, out List<string> msgList))
                throw new ArgumentException(string.Join(Environment.NewLine, msgList));

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                        (from item in context.TET_PaymentSupplier
                         where item.ID == model.ID
                         select item).FirstOrDefault();

                    if (dbModel == null)
                        throw new NullReferenceException($"{model.ID} don't exists.");

                    dbModel.IdNo = model.IdNo;
                    dbModel.Address = model.Address;
                    dbModel.OfficeTel = model.OfficeTel;
                    dbModel.Country = model.Country;
                    dbModel.Charge = model.Charge;
                    dbModel.PaymentTerm = model.PaymentTerm;
                    dbModel.Incoterms = model.Incoterms;
                    dbModel.BillingDocument = model.BillingDocument;
                    dbModel.Remark = model.Remark;
                    dbModel.IsActive = model.IsActive;
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

        /// <summary> 刪除 一般付款對象 </summary>
        /// <param name="id">主鍵</param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void DeleteTET_PaymentSupplier(Guid id, string userID, DateTime cDate)
        {
            if (id == Guid.Empty || string.IsNullOrWhiteSpace(userID))
                throw new ArgumentNullException("ID, UserID is required");

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                    (from item in context.TET_PaymentSupplier
                     where item.ID == id
                     select item).FirstOrDefault();

                    // 資料如果不存在，就視為已刪除
                    if (dbModel == null)
                        return;

                    this._contactMgr.DeleteTET_PaymentSupplierContact(context, id, userID, cDate);
                    this._attachmentMgr.DeleteTET_PaymentSupplierAttachment(context, id, userID, cDate);

                    context.TET_PaymentSupplier.Remove(dbModel);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> Active 一般付款對象資料 </summary>
        /// <param name="id">主鍵</param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void ActiveTET_PaymentSupplier(Guid id, string userID, DateTime cDate)
        {
            if (id == Guid.Empty || string.IsNullOrWhiteSpace(userID))
                throw new ArgumentNullException("ID, UserID is required");

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                    (from item in context.TET_PaymentSupplier
                     where item.ID == id
                     select item).FirstOrDefault();

                    // 資料如果不存在，就視為已刪除
                    if (dbModel == null)
                        return;

                    dbModel.IsActive = "Active";
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

        /// <summary> InaActive 一般付款對象資料 </summary>
        /// <param name="id">主鍵</param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void InactiveTET_PaymentSupplier(Guid id, string userID, DateTime cDate)
        {
            if (id == Guid.Empty || string.IsNullOrWhiteSpace(userID))
                throw new ArgumentNullException("ID, UserID is required");

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                    (from item in context.TET_PaymentSupplier
                     where item.ID == id
                     select item).FirstOrDefault();

                    // 資料如果不存在，就視為已刪除
                    if (dbModel == null)
                        return;

                    dbModel.IsActive = "Inactive";
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

        #region Applicant
        /// <summary> 送出申請 </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void SubmitTET_PaymentSupplierApproval(TET_PaymentSupplierModel model, string userID, DateTime cDate)
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

                    //--- 調整原資料的審核狀態 ---
                    // 如果審核狀態為 NULL 或是已退回，才允許送出申請
                    if (!string.IsNullOrWhiteSpace(dbModel.ApproveStatus) &&
                        ApprovalUtils.ParseApprovalStatus(dbModel.ApproveStatus) != ApprovalStatus.Rejected)
                        throw new Exception("送審狀態必須為空，或是已退回.");

                    // 更改原資料的審核狀態
                    dbModel.ApproveStatus = ApprovalStatus.Verify.ToText();
                    //--- 調整原資料的審核狀態 ---

                    //--- 新增送審資料 ---
                    var entityApplier = new TET_PaymentSupplierApproval()
                    {
                        ID = Guid.NewGuid(),
                        PSID = dbModel.ID,
                        Type = ApprovalType.New.ToText(),
                        Level = ApprovalLevel.Applicant.ToText(),
                        Description = $"{ApprovalType.New.ToText()}_{dbModel.CName}_{user.UnitName}_{user.FirstNameEN} {user.LastNameEN}",
                        Approver = userID,
                        Result = ApprovalResult.SentToApproval.ToText(),
                        CreateUser = userID,
                        CreateDate = cDate.AddSeconds(-1),
                        ModifyUser = userID,
                        ModifyDate = cDate.AddSeconds(-1),
                    };
                    context.TET_PaymentSupplierApproval.Add(entityApplier);
                    //--- 新增送審資料資料 ---

                    //--- 新增審核資料 - 主管 ---
                    var lvl = ApprovalLevel.User_GL;

                    // 建立新的申請資訊
                    var entity = new TET_PaymentSupplierApproval()
                    {
                        ID = Guid.NewGuid(),
                        PSID = dbModel.ID,
                        Type = ApprovalType.New.ToText(),
                        Level = ApprovalLevel.User_GL.ToText(),
                        Description = $"{ApprovalType.New.ToText()}_{dbModel.CName}_{user.UnitName}_{user.FirstNameEN} {user.LastNameEN}",
                        Approver = leader.ID,
                        CreateUser = userID,
                        CreateDate = cDate,
                        ModifyUser = userID,
                        ModifyDate = cDate,
                    };

                    // 寄送通知信
                    var lvlName = this.GetLevelDisplayName(leader.ID, lvl, dbModel.CoSignApprover);
                    ApprovalMailUtil.SendNewVerifyMail(leader.EMail, entity, lvlName, userID, cDate);
                    context.TET_PaymentSupplierApproval.Add(entity);
                    //--- 新增審核資料 - 主管 ---

                    //--- 新增審核資料 - 加簽人 ---
                    var coSignUser = this._userMgr.GetUserList(model.CoSignApprover);
                    foreach (var item in coSignUser)
                    {
                        string email = item.EMail;
                        string approver = item.EmpID;

                        // 建立新的申請資訊 
                        var entity_CoSign = new TET_PaymentSupplierApproval()
                        {
                            ID = Guid.NewGuid(),
                            PSID = dbModel.ID,
                            Type = ApprovalType.New.ToText(),
                            Level = lvl.ToText(),
                            Description = $"{ApprovalType.New.ToText()}_{dbModel.CName}_{user.UnitName}_{user.FirstNameEN} {user.LastNameEN}",
                            Approver = approver,
                            CreateUser = userID,
                            CreateDate = cDate,
                            ModifyUser = userID,
                            ModifyDate = cDate,
                        };

                        // 寄送通知信
                        var lvlName2 = this.GetLevelDisplayName(approver, lvl, dbModel.CoSignApprover);
                        ApprovalMailUtil.SendNewVerifyMail(email, entity_CoSign, lvlName2, userID, cDate);
                        context.TET_PaymentSupplierApproval.Add(entity_CoSign);
                    }
                    //--- 新增審核資料 - 加簽人 ---

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }



        /// <summary> 中止 供應商 審核</summary>
        /// <param name="id">主鍵</param>
        /// <param name="reason">中止原因</param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void AbordTET_PaymentSupplier(Guid id, string reason, string userID, DateTime cDate)
        {
            if (id == Guid.Empty || string.IsNullOrWhiteSpace(userID))
                throw new ArgumentNullException("ID, UserID is required");

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                        (from item in context.TET_PaymentSupplier
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
                        (from item in context.TET_PaymentSupplierApproval
                         where item.PSID == id
                         select item).ToList();

                    // 刪除尚未審核的審核資料
                    var nonResultApprovalList = approvalList.Where(item => string.IsNullOrEmpty(item.Result)).ToList();
                    context.TET_PaymentSupplierApproval.RemoveRange(nonResultApprovalList);
                    context.SaveChanges();

                    // 寄送通知信
                    var receivers = approvalList.Select(obj => obj.Approver).ToList();
                    var subTitle = (approvalList.Count > 0) ? approvalList[0].Description : string.Empty;
                    var userList = this._userMgr.GetUserList(receivers);
                    var mailList = userList.Select(obj => obj.EMail).ToList();


                    ApprovalMailUtil.SendAbordMail(mailList, subTitle, reason, userID, cDate);
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
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
