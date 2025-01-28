namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TET_SupplierSTQAApproval
    {
        [Key]
        public Guid ID { get; set; }

        public Guid STQAID { get; set; }

        [Required]
        [StringLength(64)]
        public string Type { get; set; }

        [Required]
        [StringLength(256)]
        public string Description { get; set; }

        [Required]
        [StringLength(32)]
        public string Level { get; set; }

        [Required]
        [StringLength(64)]
        public string Approver { get; set; }

        [StringLength(16)]
        public string Result { get; set; }

        [StringLength(256)]
        public string Comment { get; set; }

        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        public string ModifyUser { get; set; }

        public DateTime ModifyDate { get; set; }
    }
}
