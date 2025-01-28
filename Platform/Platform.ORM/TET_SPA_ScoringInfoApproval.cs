namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TET_SPA_ScoringInfoApproval
    {
        [Key]
        public Guid ID { get; set; }

        public Guid SIID { get; set; }

        [StringLength(64)]
        public string Type { get; set; }

        [StringLength(256)]
        public string Description { get; set; }

        [StringLength(32)]
        public string Level { get; set; }

        [StringLength(64)]
        public string Approver { get; set; }

        [StringLength(16)]
        public string Result { get; set; }

        [StringLength(256)]
        public string Comment { get; set; }

        [StringLength(64)]
        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        [StringLength(64)]
        public string ModifyUser { get; set; }

        public DateTime ModifyDate { get; set; }
    }
}
