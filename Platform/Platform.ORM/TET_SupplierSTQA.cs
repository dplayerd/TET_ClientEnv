namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TET_SupplierSTQA
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        [StringLength(128)]
        public string BelongTo { get; set; }

        [Required]
        [StringLength(64)]
        public string Purpose { get; set; }

        [Required]
        [StringLength(64)]
        public string BusinessTerm { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(64)]
        public string Type { get; set; }

        [Required]
        [StringLength(64)]
        public string UnitALevel { get; set; }

        [Required]
        [StringLength(64)]
        public string UnitCLevel { get; set; }

        [Required]
        [StringLength(64)]
        public string UnitDLevel { get; set; }

        [StringLength(512)]
        public string Comment { get; set; }

        [StringLength(32)]
        public string ApproveStatus { get; set; }

        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        public string ModifyUser { get; set; }

        public DateTime ModifyDate { get; set; }
    }
}
