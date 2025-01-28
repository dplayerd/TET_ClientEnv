namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TET_SPA_ScoringInfoModule2
    {
        public Guid ID { get; set; }

        public Guid SIID { get; set; }

        [StringLength(64)]
        public string ServiceFor { get; set; }

        [StringLength(64)]
        public string WorkItem { get; set; }

        [Required]
        [StringLength(64)]
        public string MachineName { get; set; }

        [Required]
        [StringLength(64)]
        public string MachineNo { get; set; }

        [Required]
        [StringLength(16)]
        public string OnTime { get; set; }

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
