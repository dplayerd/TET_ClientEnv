namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TET_SupplierSPA
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        [StringLength(128)]
        public string BelongTo { get; set; }

        [Required]
        [StringLength(32)]
        public string Period { get; set; }

        [Required]
        [StringLength(64)]
        public string BU { get; set; }

        [Required]
        [StringLength(64)]
        public string ServiceFor { get; set; }

        [Required]
        [StringLength(64)]
        public string AssessmentItem { get; set; }

        [Required]
        [StringLength(64)]
        public string PerformanceLevel { get; set; }

        [Required]
        [StringLength(16)]
        public string TotalScore { get; set; }

        [Required]
        [StringLength(16)]
        public string TScore { get; set; }

        [Required]
        [StringLength(16)]
        public string DScore { get; set; }

        [Required]
        [StringLength(16)]
        public string QScore { get; set; }

        [Required]
        [StringLength(16)]
        public string CScore { get; set; }

        [Required]
        [StringLength(16)]
        public string SScore { get; set; }

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
