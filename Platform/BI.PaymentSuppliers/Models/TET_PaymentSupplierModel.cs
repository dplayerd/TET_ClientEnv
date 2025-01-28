using Newtonsoft.Json;
using Platform.AbstractionClass;
using Platform.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.PaymentSuppliers.Models
{
    /// <summary> 一般付款對象 Model </summary>
    public partial class TET_PaymentSupplierModel
    {
        #region 原生欄位
        /// <summary> ID </summary>
        public Guid? ID { get; set; }

        /// <summary> ApplyReason </summary>
        public string ApplyReason { get; set; }

        /// <summary> VenderCode </summary>
        public string VenderCode { get; set; }

        /// <summary> CName </summary>
        public string CName { get; set; }

        /// <summary> EName </summary>
        public string EName { get; set; }

        /// <summary> Country </summary>
        public string Country { get; set; }

        /// <summary> TaxNo </summary>
        public string TaxNo { get; set; }

        /// <summary> IdNo </summary>
        public string IdNo { get; set; }

        /// <summary> Address </summary>
        public string Address { get; set; }

        /// <summary> OfficeTel </summary>
        public string OfficeTel { get; set; }

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

        /// <summary> Version </summary>
        public int Version { get; set; }

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

        #endregion

        #region 外部資料表

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

        /// <summary> 聯絡人清單 </summary>
        public List<TET_PaymentSupplierContactModel> ContactList { get; set; } = new List<TET_PaymentSupplierContactModel>();

        /// <summary> 上傳檔案清單 </summary>
        public List<TET_PaymentSupplierAttachmentModel> AttachmentList { get; set; } = new List<TET_PaymentSupplierAttachmentModel>();

        /// <summary> 簽核歷程 </summary>
        public List<TET_PaymentSupplierApprovalModel> ApprovalList { get; set; } = new List<TET_PaymentSupplierApprovalModel>();

        #endregion

        #region 檔案上傳

        /// <summary> 本次上傳的檔案 </summary>
        public List<FileContent> UploadFiles { get; set; } = new List<FileContent>();

        #endregion
    }
}
