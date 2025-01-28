namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TET_PaymentSupplierContact
    {
        [Key]
        public Guid ID { get; set; }

        public Guid PSID { get; set; }

        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [StringLength(64)]
        public string Title { get; set; }

        [Required]
        [StringLength(32)]
        public string Tel { get; set; }

        [StringLength(64)]
        public string Email { get; set; }

        [StringLength(128)]
        public string Remark { get; set; }

        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        public string ModifyUser { get; set; }

        public DateTime ModifyDate { get; set; }
    }
}
