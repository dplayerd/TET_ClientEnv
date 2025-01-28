namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TET_SPA_ScoringInfoModule4
    {
        public Guid ID { get; set; }

        public Guid SIID { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(64)]
        public string Location { get; set; }

        [Required]
        [StringLength(16)]
        public string IsDamage { get; set; }

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
