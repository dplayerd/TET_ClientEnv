namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TET_SPA_Evaluation
    {
        public Guid ID { get; set; }

        [Required]
        [StringLength(16)]
        public string Period { get; set; }

        [Required]
        [StringLength(64)]
        public string BU { get; set; }

        [Required]
        [StringLength(64)]
        public string ServiceItem { get; set; }

        [Required]
        [StringLength(64)]
        public string ServiceFor { get; set; }

        [Required]
        [StringLength(128)]
        public string BelongTo { get; set; }

        [Required]
        [StringLength(16)]
        public string POSource { get; set; }

        [Required]
        [StringLength(16)]
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

        [Required]
        [StringLength(16)]
        public string TScore1 { get; set; }

        [Required]
        [StringLength(16)]
        public string TScore2 { get; set; }

        [Required]
        [StringLength(16)]
        public string DScore1 { get; set; }

        [Required]
        [StringLength(16)]
        public string DScore2 { get; set; }

        [Required]
        [StringLength(16)]
        public string QScore1 { get; set; }

        [Required]
        [StringLength(16)]
        public string QScore2 { get; set; }

        [Required]
        [StringLength(16)]
        public string CScore1 { get; set; }

        [Required]
        [StringLength(16)]
        public string CScore2 { get; set; }

        [Required]
        [StringLength(16)]
        public string SScore1 { get; set; }

        [Required]
        [StringLength(16)]
        public string SScore2 { get; set; }

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
