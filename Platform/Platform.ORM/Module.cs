namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Module
    {
        public Guid ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Controller { get; set; }

        [Required]
        [StringLength(100)]
        public string Action { get; set; }

        [StringLength(100)]
        public string AdminController { get; set; }

        [StringLength(100)]
        public string AdminAction { get; set; }

        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        public string ModifyUser { get; set; }

        public DateTime? ModifyDate { get; set; }

        public string DeleteUser { get; set; }

        public DateTime? DeleteDate { get; set; }
    }
}
