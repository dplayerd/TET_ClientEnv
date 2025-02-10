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
using BI.Suppliers.Validators;
using BI.Shared;
using BI.STQA.Models;
using Newtonsoft.Json;

namespace BI.Suppliers
{
    public class TET_SupplierManager
    {
        private Logger _logger = new Logger();

        private TET_SupplierContactManager _contactMgr = new TET_SupplierContactManager();
        private TET_SupplierAttachmentManager _attachmentMgr = new TET_SupplierAttachmentManager();
        private SupplierTradeManager _tradeMgr = new SupplierTradeManager();
        private UserManager _userMgr = new UserManager();
        private UserRoleManager _userRoleMgr = new UserRoleManager();
        private TET_ParametersManager _paraMgr = new TET_ParametersManager();

        #region Queryable
        /// <summary> 組合過濾條件 </summary>
        /// <param name="query"> 原本的查詢 </param>
        /// <param name="queryParams"> 查詢條件 </param>
        /// <returns></returns>
        private IQueryable<TET_Supplier> BuildQueryCondition(IQueryable<TET_Supplier> query, ISupplierListQueryInput queryParams)
        {
            if (!string.IsNullOrWhiteSpace(queryParams.caption))
                query = query.Where(obj => obj.CName.Contains(queryParams.caption) || obj.EName.Contains(queryParams.caption) || obj.VenderCode.Contains(queryParams.caption));

            if (!string.IsNullOrWhiteSpace(queryParams.taxNo))
                query = query.Where(obj => obj.TaxNo.Contains(queryParams.taxNo));

            if (queryParams.belongTo != null && queryParams.belongTo.Length > 0)
                query = query.Where(obj => queryParams.belongTo.Contains(obj.BelongTo));

            if (queryParams.supplierCategory != null)
                query = query.Where(obj => queryParams.supplierCategory.Any(item => obj.SupplierCategory.Contains(item)));

            if (queryParams.businessCategory != null && queryParams.businessCategory.Length > 0)
                query = query.Where(obj => queryParams.businessCategory.Any(item => obj.BusinessCategory.Contains(item)));

            if (queryParams.businessAttribute != null && queryParams.businessAttribute.Length > 0)
                query = query.Where(obj => queryParams.businessAttribute.Any(item => obj.BusinessAttribute.Contains(item)));

            if (queryParams.buyer != null && queryParams.buyer.Length > 0)
                query = query.Where(obj => queryParams.buyer.Any(item => obj.Buyer.Contains(item)));

            if (queryParams.mainProduct != null && queryParams.mainProduct.Length > 0)
                query = query.Where(obj => obj.MainProduct.Contains(queryParams.mainProduct));

            if (!string.IsNullOrWhiteSpace(queryParams.searchKey))
                query = query.Where(obj => obj.SearchKey.Contains(queryParams.searchKey));

            if (!string.IsNullOrWhiteSpace(queryParams.keySupplier))
                query = query.Where(obj => obj.KeySupplier == queryParams.keySupplier);

            return query;
        }

        /// <summary> 組合轉換條件 </summary>
        /// <param name="orgQuery"></param>
        /// <returns></returns>
        private IQueryable<TET_SupplierModel> BuildSelectModel(IQueryable<TET_Supplier> orgQuery)
        {
            var query =
                from item in orgQuery
                select
                    new TET_SupplierModel()
                    {
                        ID = item.ID,
                        CoSignApprover_Text = item.CoSignApprover,
                        IsSecret = item.IsSecret,
                        IsNDA = item.IsNDA,
                        ApplyReason = item.ApplyReason,
                        BelongTo = item.BelongTo,
                        VenderCode = item.VenderCode,
                        SupplierCategory_Text = item.SupplierCategory.ToUpper(),
                        BusinessCategory_Text = item.BusinessCategory.ToUpper(),
                        BusinessAttribute_Text = item.BusinessAttribute.ToUpper(),
                        SearchKey = item.SearchKey,
                        RelatedDept_Text = item.RelatedDept.ToUpper(),
                        Buyer_Text = item.Buyer,
                        RegisterDate_Date = item.RegisterDate,
                        CName = item.CName,
                        EName = item.EName,
                        Country = item.Country.ToUpper(),
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
                        IsActive = item.IsActive,
                    };

            return query;
        }
        #endregion

        #region Read
        /// <summary>
        /// 取得 供應商 清單
        /// </summary>
        /// <param name="queryParams">各種過濾條件</param>
        /// <param name="userID"> 目前登入者 </param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<SupplierListModel> GetTET_SupplierList(ISupplierListQueryInput queryParams, string userID, Pager pager)
        {
            // 檢查目前登入者是否有指定角色的身份
            var isSRI_SS = _userRoleMgr.IsRole(userID, ApprovalRole.SRI_SS.ToID().Value);
            var isSRI_SS_GL = _userRoleMgr.IsRole(userID, ApprovalRole.SRI_SS_GL.ToID().Value);


            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    // 原始查詢
                    var orgQuery =
                        from item in context.TET_Supplier
                        where item.Version == 0
                        select item;

                    if (!isSRI_SS && !isSRI_SS_GL)
                        orgQuery = orgQuery.Where(obj => obj.CreateUser == userID);


                    orgQuery = BuildQueryCondition(orgQuery, queryParams);  // 組合過濾條件

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
        /// 取得 供應商 清單
        /// </summary>
        /// <param name="queryParams">各種過濾條件</param>
        /// <param name="userID"> 目前登入者 </param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<TET_SupplierModel> GetTET_SupplierLastVersionList(ISupplierListQueryInput queryParams, string userID, Pager pager)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    // 原始查詢
                    var orgQuery =
                        from item in context.TET_Supplier
                        where item.IsLastVersion == "Y"
                        select item;


                    //--- 過濾 STQA 認證欄位 ----
                    string completedText = ApprovalStatus.Completed.ToText();
                    if (string.Compare("Y", queryParams.stqaCertified, true) == 0)
                    {
                        orgQuery = orgQuery.Where(item => context.TET_SupplierSTQA.Where(obj => obj.ApproveStatus == completedText).Select(obj => obj.BelongTo).Contains(item.BelongTo));
                    }
                    else if (string.Compare("N", queryParams.stqaCertified, true) == 0)
                    {
                        orgQuery = orgQuery.Where(item => !context.TET_SupplierSTQA.Where(obj => obj.ApproveStatus == completedText).Select(obj => obj.BelongTo).Contains(item.BelongTo));
                    }
                    //--- 過濾 STQA 認證欄位 ----


                    orgQuery = BuildQueryCondition(orgQuery, queryParams);  // 組合過濾條件
                    var query = this.BuildSelectModel(orgQuery);            // 組合轉換條件

                    query = query.OrderByDescending(obj => obj.CreateDate);
                    var list = query.ProcessPager(pager).ToList();

                    //--- 處理輸出資料 ---
                    // 是否為 STQA
                    var stqaList = context.TET_SupplierSTQA.Where(obj => obj.ApproveStatus == completedText).Select(obj => obj.BelongTo).ToList();

                    // 採購擔當
                    var userIdList = list.Where(obj => obj.Buyer != null).SelectMany(obj => obj.Buyer).Select(obj => obj).Distinct();
                    var empList = this._userMgr.GetUserList(userIdList);

                    // 常用參數
                    var paraList = this._paraMgr.GetAllParametersKeyTextList();

                    foreach (var item in list)
                    {
                        // 是否為 STQA
                        item.IsSTQA = (stqaList.Contains(item.BelongTo)) ? "Y" : "N";

                        // 採購擔當
                        if (item.Buyer != null)
                            item.BuyerFullNameList = empList.Where(obj => item.Buyer.Contains(obj.UserID)).Select(obj => $"{obj.FirstNameEN} {obj.LastNameEN}({obj.EmpID})").ToList();

                        // 廠商類別
                        if (item.SupplierCategory != null)
                            item.SupplierCategoryFullNameList = paraList.Where(obj => item.SupplierCategory.Contains(obj.ID.ToString().ToUpper().Replace("{", "").Replace("}", ""))).Select(obj => $"{obj.Item}").ToList();

                        // 交易主類別
                        if (item.BusinessCategoryFullNameList != null)
                            item.BusinessCategoryFullNameList = paraList.Where(obj => item.BusinessCategory.Contains(obj.ID.ToString().ToUpper().Replace("{", "").Replace("}", ""))).Select(obj => $"{obj.Item}").ToList();

                        // 交易子類別
                        if (item.BusinessAttributeFullNameList != null)
                            item.BusinessAttributeFullNameList = paraList.Where(obj => item.BusinessAttribute.Contains(obj.ID.ToString().ToUpper().Replace("{", "").Replace("}", ""))).Select(obj => $"{obj.Item}").ToList();

                        //相關BU
                        if (item.RelatedDept != null)
                        {
                            if (item.RelatedDeptFullNameList != null)
                                item.RelatedDeptFullNameList = paraList.Where(obj => item.RelatedDept.Contains(obj.ID.ToString().ToUpper().Replace("{", "").Replace("}", ""))).Select(obj => $"{obj.Item}").ToList();
                        }

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

                        //供應商狀態
                        if (item.KeySupplier != null && item.KeySupplier != "" && item.KeySupplier != "Y")
                        {
                            if (item.KeySupplierFullNameList != null)
                                item.KeySupplierFullNameList = paraList.Where(obj => item.KeySupplier.Contains(obj.ID.ToString().ToUpper().Replace("{", "").Replace("}", ""))).Select(obj => $"{obj.Item}").ToList();
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
        /// 取得 供應商 清單
        /// </summary>
        /// <param name="queryParams">各種過濾條件</param>
        /// <param name="userID"> 目前登入者 </param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<TET_SupplierModel> GetTET_SupplierActiveLastVersionList(ISupplierListQueryInput queryParams, string userID, Pager pager)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    // 原始查詢
                    var orgQuery =
                        from item in context.TET_Supplier
                        where item.IsActive == "Active" && item.IsLastVersion == "Y"
                        select item;


                    //--- 過濾 STQA 認證欄位 ----
                    string completedText = ApprovalStatus.Completed.ToText();
                    if (string.Compare("Y", queryParams.stqaCertified, true) == 0)
                    {
                        orgQuery = orgQuery.Where(item => context.TET_SupplierSTQA.Where(obj => obj.ApproveStatus == completedText).Select(obj => obj.BelongTo).Contains(item.BelongTo));
                    }
                    else if (string.Compare("N", queryParams.stqaCertified, true) == 0)
                    {
                        orgQuery = orgQuery.Where(item => !context.TET_SupplierSTQA.Where(obj => obj.ApproveStatus == completedText).Select(obj => obj.BelongTo).Contains(item.BelongTo));
                    }
                    //--- 過濾 STQA 認證欄位 ----


                    orgQuery = BuildQueryCondition(orgQuery, queryParams);  // 組合過濾條件
                    var query = this.BuildSelectModel(orgQuery);            // 組合轉換條件

                    query = query.OrderByDescending(obj => obj.CreateDate);
                    var list = query.ProcessPager(pager).ToList();

                    //--- 處理輸出資料 ---
                    // 是否為 STQA
                    var stqaList = context.TET_SupplierSTQA.Where(obj => obj.ApproveStatus == completedText).Select(obj => obj.BelongTo).ToList();

                    // 採購擔當
                    var userIdList = list.Where(obj => obj.Buyer != null).SelectMany(obj => obj.Buyer).Select(obj => obj).Distinct();
                    var empList = this._userMgr.GetUserList(userIdList);

                    foreach (var item in list)
                    {
                        // 是否為 STQA
                        item.IsSTQA = (stqaList.Contains(item.BelongTo)) ? "Y" : "N";

                        // 採購擔當
                        if (item.Buyer != null)
                            item.BuyerFullNameList = empList.Where(obj => item.Buyer.Contains(obj.UserID)).Select(obj => $"{obj.FirstNameEN} {obj.LastNameEN}({obj.EmpID})").ToList();
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

        /// <summary> 查詢供應商 </summary>
        /// <param name="ID"> 供應商 ID </param>
        /// <param name="includeApprovalList"> 是否要包含審核歷程 </param>
        /// <param name="includeSTQAList"> 是否要包含 STQA 表 </param>
        /// <param name="includeTradeList"> 是否要包含交易紀錄表 </param>
        /// <param name="isSS"> 是否為採購 </param>
        /// <returns></returns>
        public TET_SupplierModel GetTET_Supplier(Guid ID, bool includeApprovalList = false, bool includeSTQAList = false, bool includeTradeList = false, bool isSS = false)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in context.TET_Supplier
                        where item.ID == ID
                        select item;

                    var result = this.BuildSelectModel(query).FirstOrDefault();
                    if (result != null)
                    {
                        result.ContactList = this._contactMgr.GetTET_SupplierContactList(context, ID);
                        result.AttachmentList = this._attachmentMgr.GetTET_SupplierAttachmentsList(context, ID);

                        // 如果需要包含審核紀錄再查出來，避免速度過慢
                        if (includeApprovalList)
                            result.ApprovalList = this.GetApprovalList(context, ID);

                        // 如果需要包含 STQA 再查出來，避免速度過慢
                        if (includeSTQAList)
                            result.STQAList = this.GetCompletedSTQAList(context, result.BelongTo);

                        // 如果需要包含 交易 再查出來，避免速度過慢
                        if (includeTradeList)
                            result.TradeList = this._tradeMgr.GetGroupedTradeList(result.VenderCode, isSS);
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
                        (from item in context.TET_Supplier
                         where item.ID == supplierId
                         select new { item.CoSignApprover });

                    var supplier = supplierQuery.FirstOrDefault();

                    // Approver
                    var appQuery =
                        from item in context.TET_SupplierApproval
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
                        return this.GetLevelDisplayName(approver.Approver, lvl, supplier.CoSignApprover);

                    return lvl.ToDisplayText();
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                return default;
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

        #region OtherRead
        private List<StqaModel> GetCompletedSTQAList(PlatformContextModel context, string belongTo)
        {
            string completedText = ApprovalStatus.Completed.ToText();

            var query =
                (from item in context.TET_SupplierSTQA
                 join item1 in context.TET_Parameters
                     on item.Purpose equals item1.ID.ToString()
                 join item2 in context.TET_Parameters
                     on item.BusinessTerm equals item2.ID.ToString()
                 join item3 in context.TET_Parameters
                     on item.Type equals item3.ID.ToString()
                 join item4 in context.TET_Parameters
                     on item.UnitALevel equals item4.ID.ToString()
                 join item5 in context.TET_Parameters
                     on item.UnitCLevel equals item5.ID.ToString()
                 join item6 in context.TET_Parameters
                     on item.UnitDLevel equals item6.ID.ToString()

                 where
                     item.BelongTo == belongTo &&
                     item.ApproveStatus == completedText
                 orderby item.CreateDate
                 select new
                 {
                     BelongTo = item.BelongTo,
                     Purpose = item1.Item,
                     BusinessTerm = item2.Item,
                     Date = item.Date,
                     Type = item3.Item,
                     UnitALevel = item4.Item,
                     UnitCLevel = item5.Item,
                     UnitDLevel = item6.Item
                 });

            var sourceList = query.ToList();
            var list =
                sourceList.Select(item =>
                    new StqaModel()
                    {
                        BelongTo = item.BelongTo,
                        Purpose = item.Purpose,
                        BusinessTerm = item.BusinessTerm,
                        Date = item.Date.ToString("yyyy-MM-dd"),
                        Type = item.Type,
                        UnitALevel = item.UnitALevel,
                        UnitCLevel = item.UnitCLevel,
                        UnitDLevel = item.UnitDLevel,
                    }
                ).ToList();

            return list;
        }

        /// <summary> 是否能開啟供應商資料 </summary>
        /// <param name="supplierID"> 供應商代碼 </param>
        /// <param name="cUser"> 目前登入者 </param>
        /// <returns></returns>
        public bool CanRead(Guid supplierID, string cUser)
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var supplier = this.GetTET_Supplier(supplierID);

                    if (supplier == null)
                        return false;


                    // 檢查目前登入者是否有指定角色的身份
                    var isSRI_SS = this._userRoleMgr.IsRole(cUser, ApprovalRole.SRI_SS.ToID().Value);
                    var isSRI_SS_GL = this._userRoleMgr.IsRole(cUser, ApprovalRole.SRI_SS_GL.ToID().Value);
                    var roleResult = isSRI_SS || isSRI_SS_GL;


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
                            from item in context.TET_SupplierApproval
                            where
                                item.SupplierID == supplierID && item.Approver == cUser
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
        /// <param name="ID"></param>
        /// <returns></returns>
        private List<TET_SupplierApprovalModel> GetApprovalList(PlatformContextModel context, Guid ID)
        {
            var query =
                (from item in context.TET_SupplierApproval
                 where
                     item.SupplierID == ID &&
                     item.Result != null
                 orderby item.CreateDate
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
                     });


            var result = query.ToList();


            // 如果是 User_GL 這關，而且是加簽人，要把關卡名稱換了
            var supplierCoSign =
                (from item in context.TET_Supplier
                 where item.ID == ID
                 select item.CoSignApprover).FirstOrDefault();

            if (supplierCoSign != null)
            {
                foreach (var ritem in result)
                {
                    if (ApprovalUtils.ParseApprovalLevel(ritem.Level) == ApprovalLevel.User_GL)
                    {
                        var coSigns = JsonConvert.DeserializeObject<List<string>>(supplierCoSign);
                        if (coSigns.Contains(ritem.Approver))
                            ritem.Level = ModuleConfig.CoSignApproverLevelName;
                    }
                }
            }

            return result;
        }

        /// <summary> 取得最新版所有供應商的歸屬公司 </summary>
        /// <returns></returns>
        public List<KeyTextModel> GetBelongToList()
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var query =
                        from item in context.TET_Supplier
                        where item.IsLastVersion == "Y"
                        orderby item.BelongTo
                        select
                            new KeyTextModel()
                            {
                                Key = item.BelongTo,
                                Text = item.BelongTo
                            };

                    var list = query.Distinct().ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                return default;
            }
        }

        /// <summary> 查出已經有建立 STQA 的供應商資料 </summary>
        /// <returns></returns>
        public List<KeyTextModel> GetSuppliersWithSTQA()
        {
            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    string completedText = ApprovalStatus.Completed.ToText();
                    var query =
                        from item in context.TET_Supplier
                        where
                            (from obj in context.TET_SupplierSTQA
                             where obj.ApproveStatus == completedText
                             select obj.BelongTo
                             ).Contains(item.BelongTo)
                        orderby item.BelongTo
                        select
                            new KeyTextModel()
                            {
                                Key = item.BelongTo,
                                Text = item.BelongTo
                            };

                    var list = query.Distinct().ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                return default;
            }
        }

        /// <summary> 檢查供應商代碼是否已存在 </summary>
        /// <param name="context"></param>
        /// <param name="supplierModel"></param>
        /// <returns></returns>
        internal bool IsVendorCodeExisted(PlatformContextModel context, TET_SupplierModel supplierModel)
        {
            var completedStatusText = ApprovalStatus.Completed.ToText();
            var query =
                (from item in context.TET_Supplier
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
        #endregion

        #region CUD
        /// <summary> 新增 供應商 </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public Guid CreateTET_Supplier(TET_SupplierModel model, string userID, DateTime cDate)
        {
            if (model == null)
                throw new ArgumentNullException("Supplier is required.");

            // 新增前，先檢查是否能通過商業邏輯
            if (!SupplierValidator.ValidCreate(model, out List<string> msgList))
                throw new ArgumentException(string.Join(Environment.NewLine, msgList));

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var entity = new TET_Supplier()
                    {
                        ID = Guid.NewGuid(),
                        CoSignApprover = model.CoSignApprover_Text,
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

                    context.TET_Supplier.Add(entity);
                    _contactMgr.WriteTET_SupplierContact(context, entity.ID, model.ContactList, userID, cDate);                              // 寫入供應商
                    _attachmentMgr.WriteTET_SupplierAttachment(context, entity.ID, model.AttachmentList, model.UploadFiles, userID, cDate);  // 寫入聯絡人

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

                    if (dbModel == null)
                        throw new NullReferenceException($"{model.ID} don't exists.");

                    dbModel.CoSignApprover = model.CoSignApprover_Text;
                    dbModel.IsSecret = model.IsSecret;
                    dbModel.IsNDA = model.IsNDA;
                    dbModel.ApplyReason = model.ApplyReason;
                    dbModel.BelongTo = model.BelongTo;
                    dbModel.VenderCode = model.VenderCode;
                    dbModel.SupplierCategory = model.SupplierCategory_Text;
                    dbModel.BusinessCategory = model.BusinessCategory_Text;
                    dbModel.BusinessAttribute = model.BusinessAttribute_Text;
                    dbModel.SearchKey = model.SearchKey;
                    dbModel.RelatedDept = model.RelatedDept_Text;
                    dbModel.Buyer = model.Buyer_Text;
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


        /// <summary> 修改 供應商 </summary>
        /// <param name="model"></param>
        /// <param name="userID">目前登入者</param>
        public void ModifyTET_Supplier_QuerySS(TET_SupplierModel model, string userID, DateTime cDate)
        {
            if (model == null)
                throw new ArgumentNullException("Model is required.");

            // 新增前，先檢查是否能通過商業邏輯
            if (!QuerySSValidator.Valid(model, out List<string> msgList))
                throw new ArgumentException(string.Join(Environment.NewLine, msgList));

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                        (from item in context.TET_Supplier
                         where item.ID == model.ID
                         select item).FirstOrDefault();

                    if (dbModel == null)
                        throw new NullReferenceException($"{model.ID} don't exists.");

                    dbModel.RelatedDept = model.RelatedDept_Text;
                    dbModel.BelongTo = model.BelongTo;
                    dbModel.SupplierCategory = model.SupplierCategory_Text;
                    dbModel.BusinessCategory = model.BusinessCategory_Text;
                    dbModel.BusinessAttribute = model.BusinessAttribute_Text;
                    dbModel.Buyer = model.Buyer_Text;
                    dbModel.EName = model.EName;
                    dbModel.PaymentTerm = model.PaymentTerm;
                    dbModel.NDANo = model.NDANo;
                    dbModel.Contract = model.Contract;
                    dbModel.Buyer = model.Buyer_Text;
                    dbModel.SearchKey = model.SearchKey;
                    dbModel.Country = model.Country;
                    dbModel.Address = model.Address;
                    dbModel.OfficeTel = model.OfficeTel;
                    dbModel.Email = model.Email;
                    dbModel.Website = model.Website;
                    dbModel.ISO = model.ISO;
                    dbModel.CapitalAmount = model.CapitalAmount;
                    dbModel.Employees = model.Employees;
                    dbModel.Incoterms = model.Incoterms;
                    dbModel.BillingDocument = model.BillingDocument;
                    dbModel.MainProduct = model.MainProduct;
                    dbModel.Remark = model.Remark;
                    dbModel.SignDate1 = model.SignDate1;
                    dbModel.SignDate2 = model.SignDate2;
                    dbModel.KeySupplier = model.KeySupplier;

                    dbModel.ModifyUser = userID;
                    dbModel.ModifyDate = cDate;


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

        /// <summary> 刪除 供應商 </summary>
        /// <param name="id">主鍵</param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void DeleteTET_Supplier(Guid id, string userID, DateTime cDate)
        {
            if (id == Guid.Empty || string.IsNullOrWhiteSpace(userID))
                throw new ArgumentNullException("ID, UserID is required");

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                    (from item in context.TET_Supplier
                     where item.ID == id
                     select item).FirstOrDefault();

                    // 資料如果不存在，就視為已刪除
                    if (dbModel == null)
                        return;

                    this._contactMgr.DeleteTET_SupplierContact(context, id, userID, cDate);
                    this._attachmentMgr.DeleteTET_SupplierAttachment(context, id, userID, cDate);

                    context.TET_Supplier.Remove(dbModel);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this._logger.WriteError(ex);
                throw;
            }
        }

        /// <summary> Active 供應商資料 </summary>
        /// <param name="id">主鍵</param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void ActiveTET_Supplier(Guid id, string userID, DateTime cDate)
        {
            if (id == Guid.Empty || string.IsNullOrWhiteSpace(userID))
                throw new ArgumentNullException("ID, UserID is required");

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                    (from item in context.TET_Supplier
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

        /// <summary> Active 供應商資料 </summary>
        /// <param name="id">主鍵</param>
        /// <param name="userID">目前登入者</param>
        /// <param name="cDate">目前時間</param>
        public void InactiveTET_Supplier(Guid id, string userID, DateTime cDate)
        {
            if (id == Guid.Empty || string.IsNullOrWhiteSpace(userID))
                throw new ArgumentNullException("ID, UserID is required");

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                    (from item in context.TET_Supplier
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
        public void SubmitTET_SupplierApproval(TET_SupplierModel model, string userID, DateTime cDate)
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

                    //--- 調整原資料的審核狀態 ---
                    // 如果審核狀態為 NULL 或是已退回，才允許送出申請
                    if (!string.IsNullOrWhiteSpace(dbModel.ApproveStatus) &&
                        ApprovalUtils.ParseApprovalStatus(dbModel.ApproveStatus) != ApprovalStatus.Rejected)
                        throw new Exception("送審狀態必須為空，或是已退回.");

                    // 更改原資料的審核狀態
                    dbModel.ApproveStatus = ApprovalStatus.Verify.ToText();
                    //--- 調整原資料的審核狀態 ---


                    //--- 新增審核資料 - 主管 ---
                    var lvl = ApprovalLevel.User_GL;

                    // 建立新的申請資訊
                    var entity = new TET_SupplierApproval()
                    {
                        ID = Guid.NewGuid(),
                        SupplierID = dbModel.ID,
                        Type = ApprovalType.New.ToText(),
                        Level = lvl.ToText(),
                        Description = $"{ApprovalType.New.ToText()}_{dbModel.CName}_{user.UnitName}_{user.FirstNameEN} {user.LastNameEN}",
                        Approver = leader.ID,
                        CreateUser = userID,
                        CreateDate = cDate,
                        ModifyUser = userID,
                        ModifyDate = cDate,
                    };

                    // 寄送通知信
                    var lvlName = GetLevelDisplayName(entity.Approver, lvl, dbModel.CoSignApprover);
                    ApprovalMailUtil.SendNewVerifyMail(leader.EMail, entity, lvlName, userID, cDate);
                    context.TET_SupplierApproval.Add(entity);
                    //--- 新增審核資料 - 主管 ---


                    //--- 新增審核資料 - 加簽人 ---
                    var coSignUser = this._userMgr.GetUserList(model.CoSignApprover);
                    foreach (var item in coSignUser)
                    {
                        string email = item.EMail;
                        string approver = item.EmpID;
                        
                        // 建立新的申請資訊 
                        var entity_CoSign = new TET_SupplierApproval()
                        {
                            ID = Guid.NewGuid(),
                            SupplierID = dbModel.ID,
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
                        var lvlName1 = GetLevelDisplayName(entity_CoSign.Approver, lvl, dbModel.CoSignApprover);
                        ApprovalMailUtil.SendNewVerifyMail(email, entity_CoSign, lvlName1, userID, cDate);
                        context.TET_SupplierApproval.Add(entity_CoSign);
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
        public void AbordTET_Supplier(Guid id, string reason, string userID, DateTime cDate)
        {
            if (id == Guid.Empty || string.IsNullOrWhiteSpace(userID))
                throw new ArgumentNullException("ID, UserID is required");

            try
            {
                using (PlatformContextModel context = new PlatformContextModel())
                {
                    var dbModel =
                        (from item in context.TET_Supplier
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
                        (from item in context.TET_SupplierApproval
                         where item.SupplierID == id
                         select item).ToList();

                    // 刪除尚未審核的審核資料
                    var nonResultApprovalList = approvalList.Where(item => string.IsNullOrEmpty(item.Result)).ToList();
                    context.TET_SupplierApproval.RemoveRange(nonResultApprovalList);
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
        #endregion
    }
}
