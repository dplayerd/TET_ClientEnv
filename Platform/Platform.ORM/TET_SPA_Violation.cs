namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TET_SPA_Violation
    {
        [Key]
        public Guid ID { get; set; }

        [StringLength(16)]
        public string Period { get; set; }

        [StringLength(16)]
        public string ApproveStatus { get; set; }

        [StringLength(64)]
        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        [StringLength(64)]
        public string ModifyUser { get; set; }

        public DateTime ModifyDate { get; set; }
    }
}
