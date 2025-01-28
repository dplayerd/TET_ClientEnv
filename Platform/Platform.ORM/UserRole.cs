namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TET_Supplier_UserRoles")]
    public partial class UserRole
    {
        public Guid ID { get; set; }

        public string UserID { get; set; }

        public Guid RoleID { get; set; }

        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        public string ModifyUser { get; set; }

        public DateTime? ModifyDate { get; set; }
    }
}
