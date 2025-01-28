namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TET_SPA_ViolationDetail
    {
        [Key]
        public Guid ID { get; set; }

        public Guid ViolationID { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(128)]
        public string BelongTo { get; set; }

        [Required]
        [StringLength(64)]
        public string BU { get; set; }

        [Required]
        [StringLength(64)]
        public string AssessmentItem { get; set; }

        [Required]
        [StringLength(64)]
        public string MiddleCategory { get; set; }

        [Required]
        [StringLength(64)]
        public string SmallCategory { get; set; }

        [Required]
        [StringLength(128)]
        public string CustomerName { get; set; }

        [Required]
        [StringLength(128)]
        public string CustomerPlant { get; set; }

        
        [StringLength(128)]
        public string CustomerDetail { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

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
