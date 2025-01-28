namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TET_SPA_ScoringRatio
    {
        [Key]
        public Guid ID { get; set; }

        public Guid ServiceItemID { get; set; }

        [StringLength(16)]
        public string POSource { get; set; }

        public decimal TRatio1 { get; set; }

        public decimal TRatio2 { get; set; }

        public decimal DRatio1 { get; set; }

        public decimal DRatio2 { get; set; }

        public decimal QRatio1 { get; set; }

        public decimal QRatio2 { get; set; }

        public decimal CRatio1 { get; set; }

        public decimal CRatio2 { get; set; }

        public decimal SRatio1 { get; set; }

        public decimal SRatio2 { get; set; }

        [Required]
        [StringLength(64)]
        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        [Required]
        [StringLength(64)]
        public string ModifyUser { get; set; }

        public DateTime ModifyDate { get; set; }
    }
}
