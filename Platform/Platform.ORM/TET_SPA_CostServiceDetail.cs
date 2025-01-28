namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TET_SPA_CostServiceDetail
    {
        public Guid ID { get; set; }

        public Guid CSID { get; set; }

        [Required]
        [StringLength(16)]
        public string Source { get; set; }

        [Required]
        [StringLength(16)]
        public string IsEvaluate { get; set; }

        [Required]
        [StringLength(64)]
        public string BU { get; set; }

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
        [StringLength(64)]
        public string AssessmentItem { get; set; }

        [Required]
        [StringLength(64)]
        public string PriceDeflator { get; set; }

        [Required]
        [StringLength(64)]
        public string PaymentTerm { get; set; }

        [Required]
        [StringLength(64)]
        public string Cooperation { get; set; }

        [StringLength(1000)]
        public string Advantage { get; set; }

        [StringLength(1000)]
        public string Improved { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; }

        [StringLength(1000)]
        public string Remark { get; set; }

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
