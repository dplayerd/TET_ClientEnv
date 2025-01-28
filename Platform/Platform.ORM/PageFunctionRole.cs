namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PageFunctionRole
    {
        public Guid ID { get; set; }

        public Guid PageID { get; set; }

        public Guid RoleID { get; set; }

        [Required]
        [StringLength(20)]
        public string FunctionCode { get; set; }

        public bool IsAllow { get; set; }

        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        public string ModifyUser { get; set; }

        public DateTime? ModifyDate { get; set; }

        public string DeleteUser { get; set; }

        public DateTime? DeleteDate { get; set; }
    }
}
