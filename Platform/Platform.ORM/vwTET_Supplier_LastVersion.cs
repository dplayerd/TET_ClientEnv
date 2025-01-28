namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class vwTET_Supplier_LastVersion
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        [StringLength(8)]
        public string IsSecret { get; set; }

        [Required]
        [StringLength(32)]
        public string IsNDA { get; set; }

        [StringLength(512)]
        public string ApplyReason { get; set; }

        [StringLength(128)]
        public string BelongTo { get; set; }

        [StringLength(32)]
        public string VenderCode { get; set; }

        [Required]
        [StringLength(512)]
        public string SupplierCategory { get; set; }

        [Required]
        [StringLength(512)]
        public string BusinessCategory { get; set; }

        [Required]
        [StringLength(512)]
        public string BusinessAttribute { get; set; }

        [StringLength(256)]
        public string SearchKey { get; set; }

        [StringLength(256)]
        public string RelatedDept { get; set; }

        [StringLength(64)]
        public string Buyer { get; set; }

        [Column(TypeName = "date")]
        public DateTime? RegisterDate { get; set; }

        [Required]
        [StringLength(128)]
        public string CName { get; set; }

        [Required]
        [StringLength(128)]
        public string EName { get; set; }

        [Required]
        [StringLength(64)]
        public string Country { get; set; }

        [Required]
        [StringLength(16)]
        public string TaxNo { get; set; }

        [Required]
        [StringLength(128)]
        public string Address { get; set; }

        [Required]
        [StringLength(32)]
        public string OfficeTel { get; set; }

        [StringLength(128)]
        public string ISO { get; set; }

        [StringLength(128)]
        public string Email { get; set; }

        [StringLength(128)]
        public string Website { get; set; }

        [Required]
        [StringLength(32)]
        public string CapitalAmount { get; set; }

        [Required]
        [StringLength(512)]
        public string MainProduct { get; set; }

        [StringLength(32)]
        public string Employees { get; set; }

        [Required]
        [StringLength(32)]
        public string Charge { get; set; }

        [Required]
        [StringLength(64)]
        public string PaymentTerm { get; set; }

        [Required]
        [StringLength(64)]
        public string BillingDocument { get; set; }

        [Required]
        [StringLength(64)]
        public string Incoterms { get; set; }

        [StringLength(512)]
        public string Remark { get; set; }

        [Required]
        [StringLength(32)]
        public string BankCountry { get; set; }

        [Required]
        [StringLength(128)]
        public string BankName { get; set; }

        [Required]
        [StringLength(16)]
        public string BankCode { get; set; }

        [Required]
        [StringLength(128)]
        public string BankBranchName { get; set; }

        [Required]
        [StringLength(16)]
        public string BankBranchCode { get; set; }

        [Required]
        [StringLength(64)]
        public string Currency { get; set; }

        [Required]
        [StringLength(64)]
        public string BankAccountName { get; set; }

        [Required]
        [StringLength(32)]
        public string BankAccountNo { get; set; }

        [StringLength(32)]
        public string CompanyCity { get; set; }

        [StringLength(128)]
        public string BankAddress { get; set; }

        [StringLength(32)]
        public string SwiftCode { get; set; }

        [StringLength(32)]
        public string NDANo { get; set; }

        [StringLength(1)]
        public string Contract { get; set; }

        [StringLength(32)]
        public string IsSign1 { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SignDate1 { get; set; }

        [StringLength(32)]
        public string IsSign2 { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SignDate2 { get; set; }

        [StringLength(32)]
        public string STQAApplication { get; set; }

        [StringLength(1)]
        public string KeySupplier { get; set; }

        public int Version { get; set; }

        [StringLength(1)]
        public string RevisionType { get; set; }

        [StringLength(1)]
        public string IsLastVersion { get; set; }

        [StringLength(32)]
        public string ApproveStatus { get; set; }

        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        public string ModifyUser { get; set; }

        public DateTime ModifyDate { get; set; }
    }
}
