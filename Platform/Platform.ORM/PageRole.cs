namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TET_Supplier_RoleMenu")]
    public partial class PageRole
    {
        public Guid ID { get; set; }

        public Guid MenuID { get; set; }

        public Guid RoleID { get; set; }

        public byte AllowActs { get; set; }

        [StringLength(64)]
        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        [StringLength(64)]
        public string ModifyUser { get; set; }

        public DateTime? ModifyDate { get; set; }
    }
}
