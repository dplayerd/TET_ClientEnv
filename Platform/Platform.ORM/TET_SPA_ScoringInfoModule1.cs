namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TET_SPA_ScoringInfoModule1
    {
        public Guid ID { get; set; }

        public Guid SIID { get; set; }

        [Required]
        [StringLength(16)]
        public string Source { get; set; }

        [Required]
        [StringLength(16)]
        public string Type { get; set; }

        [Required]
        [StringLength(128)]
        public string Supplier { get; set; }

        [Required]
        [StringLength(64)]
        public string EmpName { get; set; }

        [StringLength(64)]
        public string MajorJob { get; set; }

        [StringLength(16)]
        public string IsIndependent { get; set; }

        [StringLength(64)]
        public string SkillLevel { get; set; }

        [StringLength(64)]
        public string EmpStatus { get; set; }

        [StringLength(4)]
        public string TELSeniorityY { get; set; }

        [StringLength(4)]
        public string TELSeniorityM { get; set; }

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
