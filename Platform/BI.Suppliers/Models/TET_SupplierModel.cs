using Newtonsoft.Json;
using Platform.AbstractionClass;
using Platform.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Suppliers.Models
{
    /// <summary> 供應商 Model </summary>
    public partial class TET_SupplierModel
    {
        #region 原生欄位
        /// <summary> ID </summary>
        public Guid? ID { get; set; }

        /// <summary> IsSecret </summary>
        public string IsSecret { get; set; }

        /// <summary> IsNDA </summary>
        public string IsNDA { get; set; }

        /// <summary> ApplyReason </summary>
        public string ApplyReason { get; set; }

        /// <summary> BelongTo </summary>
        public string BelongTo { get; set; }

        /// <summary> VenderCode </summary>
        public string VenderCode { get; set; }

        /// <summary> SearchKey </summary>
        public string SearchKey { get; set; }

        ///// <summary> Buyer </summary>
        //public string Buyer { get; set; }

        /// <summary> CName </summary>
        public string CName { get; set; }

        /// <summary> EName </summary>
        public string EName { get; set; }

        /// <summary> Country </summary>
        public string Country { get; set; }

        /// <summary> TaxNo </summary>
        public string TaxNo { get; set; }

        /// <summary> Address </summary>
        public string Address { get; set; }

        /// <summary> OfficeTel </summary>
        public string OfficeTel { get; set; }

        /// <summary> ISO </summary>
        public string ISO { get; set; }

        /// <summary> Email </summary>
        public string Email { get; set; }

        /// <summary> Website </summary>
        public string Website { get; set; }

        /// <summary> CapitalAmount </summary>
        public string CapitalAmount { get; set; }

        /// <summary> MainProduct </summary>
        public string MainProduct { get; set; }

        /// <summary> Employees </summary>
        public string Employees { get; set; }

        /// <summary> Charge </summary>
        public string Charge { get; set; }

        /// <summary> PaymentTerm </summary>
        public string PaymentTerm { get; set; }

        /// <summary> BillingDocument </summary>
        public string BillingDocument { get; set; }

        /// <summary> Incoterms </summary>
        public string Incoterms { get; set; }

        /// <summary> Remark </summary>
        public string Remark { get; set; }

        /// <summary> BankCountry </summary>
        public string BankCountry { get; set; }

        /// <summary> BankName </summary>
        public string BankName { get; set; }

        /// <summary> BankCode </summary>
        public string BankCode { get; set; }

        /// <summary> BankBranchName </summary>
        public string BankBranchName { get; set; }

        /// <summary> BankBranchCode </summary>
        public string BankBranchCode { get; set; }

        /// <summary> Currency </summary>
        public string Currency { get; set; }

        /// <summary> BankAccountName </summary>
        public string BankAccountName { get; set; }

        /// <summary> BankAccountNo </summary>
        public string BankAccountNo { get; set; }

        /// <summary> CompanyCity </summary>
        public string CompanyCity { get; set; }

        /// <summary> BankAddress </summary>
        public string BankAddress { get; set; }

        /// <summary> SwiftCode </summary>
        public string SwiftCode { get; set; }

        /// <summary> NDANo </summary>
        public string NDANo { get; set; }

        /// <summary> Contract </summary>
        public string Contract { get; set; }

        /// <summary> IsSign1 </summary>
        public string IsSign1 { get; set; }

        /// <summary> SignDate1 </summary>
        public DateTime? SignDate1 { get; set; }

        /// <summary> IsSign2 </summary>
        public string IsSign2 { get; set; }

        /// <summary> SignDate2 </summary>
        public DateTime? SignDate2 { get; set; }

        /// <summary> STQAApplication </summary>
        public string STQAApplication { get; set; }

        /// <summary> KeySupplier </summary>
        public string KeySupplier { get; set; }

        /// <summary> Version </summary>
        public int Version { get; set; }

        /// <summary> RevisionType </summary>
        public string RevisionType { get; set; }

        /// <summary> IsLastVersion </summary>
        public string IsLastVersion { get; set; }

        /// <summary> ApproveStatus </summary>
        public string ApproveStatus { get; set; }

        /// <summary> CreateUser </summary>
        public string CreateUser { get; set; }

        /// <summary> CreateDate </summary>
        public DateTime CreateDate { get; set; }

        /// <summary> ModifyUser </summary>
        public string ModifyUser { get; set; }

        /// <summary> ModifyDate </summary>
        public DateTime ModifyDate { get; set; }

        /// <summary> IsActive </summary>
        public string IsActive { get; set; }
        #endregion

        #region 日期用欄位
        /// <summary> 日期格式 - RegisterDate </summary>
        public DateTime? RegisterDate_Date { get; set; }

        /// <summary> RegisterDate </summary>
        public string RegisterDate
        {
            get
            {
                if (!this.RegisterDate_Date.HasValue)
                    return string.Empty;

                return this.RegisterDate_Date.Value.ToString("yyyy-MM-dd");
            }
            set
            {
                if (DateTime.TryParse(value, out DateTime temp))
                    this.RegisterDate_Date = temp;
            }
        }
        #endregion

        #region 複選用欄位
        private string[] ParseJson(string txt)
        {
            if (string.IsNullOrWhiteSpace(txt) || 
                "null" == txt)
                return new string[0];

            try
            {
                return JsonConvert.DeserializeObject<string[]>(txt);
            }
            catch
            {
                return new string[0];
            }
        }

        /// <summary> 文字格式 - SupplierCategory </summary>
        public string SupplierCategory_Text { get; set; }

        /// <summary> 文字格式 - BusinessCategory </summary>
        public string BusinessCategory_Text { get; set; }

        /// <summary> 文字格式 - BusinessAttribute </summary>
        public string BusinessAttribute_Text { get; set; }

        /// <summary> 文字格式 - RelatedDept </summary>
        public string RelatedDept_Text { get; set; }

        /// <summary> 文字格式 - 採購擔當 </summary>
        public string Buyer_Text { get; set; }

        /// <summary> 文字格式 - 加簽人員 </summary>
        public string CoSignApprover_Text { get; set; }

        /// <summary> 加簽人員 </summary>
        public string[] CoSignApprover
        {
            get
            {
                if (this.CoSignApprover_Text == null)
                    this.CoSignApprover_Text = string.Empty;
                return ParseJson(this.CoSignApprover_Text);
            }
            set
            {
                this.CoSignApprover_Text = JsonConvert.SerializeObject(value);
            }
        }

        /// <summary> SupplierCategory </summary>
        public string[] SupplierCategory
        {
            get
            {
                if (this.SupplierCategory_Text == null)
                    this.SupplierCategory_Text = string.Empty;
                return ParseJson(this.SupplierCategory_Text);
            }
            set
            {
                this.SupplierCategory_Text = JsonConvert.SerializeObject(value);
            }
        }

        /// <summary> BusinessCategory </summary>
        public string[] BusinessCategory
        {
            get
            {
                if (this.BusinessCategory_Text == null)
                    this.BusinessCategory_Text = string.Empty;
                return ParseJson(this.BusinessCategory_Text);
            }
            set
            {
                this.BusinessCategory_Text = JsonConvert.SerializeObject(value);
            }
        }

        /// <summary> BusinessAttribute </summary>
        public string[] BusinessAttribute
        {
            get
            {
                if (this.BusinessAttribute_Text == null)
                    this.BusinessAttribute_Text = string.Empty;
                return ParseJson(this.BusinessAttribute_Text);
            }
            set
            {
                this.BusinessAttribute_Text = JsonConvert.SerializeObject(value);
            }
        }

        /// <summary> RelatedDept </summary>
        public string[] RelatedDept
        {
            get
            {
                if (this.RelatedDept_Text == null)
                    this.RelatedDept_Text = string.Empty;
                return ParseJson(this.RelatedDept_Text);
            }
            set
            {
                this.RelatedDept_Text = JsonConvert.SerializeObject(value);
            }
        }

        /// <summary> 採購擔當 </summary>
        public string[] Buyer
        {
            get
            {
                if (this.Buyer_Text == null)
                    this.Buyer_Text = string.Empty;
                return ParseJson(this.Buyer_Text);
            }
            set
            {
                this.Buyer_Text = JsonConvert.SerializeObject(value);
            }
        }
        #endregion

        #region 外部資料表
        /// <summary> 採購擔當的全名 </summary>
        public List<string> BuyerFullNameList { get; set; } = new List<string>();

        /// <summary> 廠商類別的全名 </summary>
        public List<string> SupplierCategoryFullNameList { get; set; } = new List<string>();

        /// <summary> 交易主類別的全名 </summary>
        public List<string> BusinessCategoryFullNameList { get; set; } = new List<string>();

        /// <summary> 交易子類別的全名 </summary>
        public List<string> BusinessAttributeFullNameList { get; set; } = new List<string>();

        /// <summary> 相關BU的全名 </summary>
        public List<string> RelatedDeptFullNameList { get; set; } = new List<string>();

        /// <summary> 國家別的全名 </summary>
        public List<string> CountryFullNameList { get; set; } = new List<string>();

        /// <summary> 付款條件的全名 </summary>
        public List<string> PaymentTermFullNameList { get; set; } = new List<string>();

        /// <summary> 交易條件的全名 </summary>
        public List<string> IncotermsFullNameList { get; set; } = new List<string>();

        /// <summary> 請款憑證的全名 </summary>
        public List<string> BillingDocumentFullNameList { get; set; } = new List<string>();

        /// <summary> 幣別的全名 </summary>
        public List<string> CurrencyFullNameList { get; set; } = new List<string>();

        /// <summary> 銀行國別的全名 </summary>
        public List<string> BankCountryFullNameList { get; set; } = new List<string>();

        /// <summary> 供應商狀態的全名 </summary>
        public List<string> KeySupplierFullNameList { get; set; } = new List<string>();

        /// <summary> STQA 認證 </summary>
        public string IsSTQA { get; set; }

        /// <summary> 聯絡人清單 </summary>
        public List<TET_SupplierContactModel> ContactList { get; set; } = new List<TET_SupplierContactModel>();

        /// <summary> 上傳檔案清單 </summary>
        public List<TET_SupplierAttachmentModel> AttachmentList { get; set; } = new List<TET_SupplierAttachmentModel>();

        /// <summary> 簽核歷程 </summary>
        public List<TET_SupplierApprovalModel> ApprovalList { get; set; } = new List<TET_SupplierApprovalModel>();

        /// <summary> STQA 資訊 </summary>
        public List<StqaModel> STQAList { get; set; } = new List<StqaModel>();

        /// <summary> 交易資訊 </summary>
        public List<GroupedTradeModel> TradeList { get; set; } = new List<GroupedTradeModel>();
        #endregion

        #region 檔案上傳
        /// <summary> 本次上傳的檔案 </summary>
        public List<FileContent> UploadFiles { get; set; } = new List<FileContent>();
        #endregion
    }
}
