namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TET_SPA_ScoringInfo
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

        [StringLength(16)]
        public string ApproveStatus { get; set; }

        [StringLength(16)]
        public string MOCount { get; set; }

        [StringLength(16)]
        public string TELLoss { get; set; }

        [StringLength(16)]
        public string CustomerLoss { get; set; }

        [StringLength(16)]
        public string Accident { get; set; }

        public int? WorkerCount { get; set; }

        [StringLength(16)]
        public string Correctness { get; set; }

        [StringLength(16)]
        public string Contribution { get; set; }

        [StringLength(64)]
        public string SelfTraining { get; set; }

        [StringLength(1000)]
        public string SelfTrainingRemark { get; set; }

        [StringLength(16)]
        public string Cooperation { get; set; }

        [StringLength(64)]
        public string Complain { get; set; }

        [StringLength(1000)]
        public string Advantage { get; set; }

        [StringLength(1000)]
        public string Improved { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; }

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
