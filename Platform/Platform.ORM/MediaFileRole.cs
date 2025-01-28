namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MediaFileRole
    {
        public Guid ID { get; set; }

        public Guid MediaFileID { get; set; }

        public Guid RoleID { get; set; }

        public Guid CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        public Guid? ModifyUser { get; set; }

        public DateTime? ModifyDate { get; set; }

        public Guid? DeleteUser { get; set; }

        public DateTime? DeleteDate { get; set; }
    }
}
