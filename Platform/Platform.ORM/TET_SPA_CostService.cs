namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TET_SPA_CostService
    {
        public Guid ID { get; set; }

        [Required]
        [StringLength(16)]
        public string Period { get; set; }

        [Required]
        [StringLength(64)]
        public string Filler { get; set; }

        [StringLength(16)]
        public string ApproveStatus { get; set; }

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
