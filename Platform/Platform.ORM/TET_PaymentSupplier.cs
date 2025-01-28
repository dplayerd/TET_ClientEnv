namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TET_PaymentSupplier
    {
        [Key]
        public Guid ID { get; set; }

        [StringLength(256)]
        public string CoSignApprover { get; set; }

        [StringLength(512)]
        public string ApplyReason { get; set; }

        [StringLength(32)]
        public string VenderCode { get; set; }

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

        [StringLength(16)]
        public string IdNo { get; set; }

        [Required]
        [StringLength(128)]
        public string Address { get; set; }

        [Required]
        [StringLength(32)]
        public string OfficeTel { get; set; }

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
        [StringLength(64)]
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

        public int Version { get; set; }

        [StringLength(1)]
        public string IsLastVersion { get; set; }

        [StringLength(32)]
        public string ApproveStatus { get; set; }

        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        public string ModifyUser { get; set; }

        public DateTime ModifyDate { get; set; }

        [StringLength(10)]
        public string IsActive { get; set; }
    }
}
