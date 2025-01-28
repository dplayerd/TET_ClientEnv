namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SystemUser
    {
        public string ID { get; set; }

        [Required]
        [StringLength(200)]
        public string Account { get; set; }

        [Required]
        [StringLength(128)]
        public string Password { get; set; }

        [Required]
        [StringLength(128)]
        public string HashKey { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(200)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public Guid? MediaFileID { get; set; }

        public bool IsEnable { get; set; }

        public bool IsDeleted { get; set; }

        public Guid CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        public Guid? ModifyUser { get; set; }

        public DateTime? ModifyDate { get; set; }

        public Guid? DeleteUser { get; set; }

        public DateTime? DeleteDate { get; set; }
    }
}
