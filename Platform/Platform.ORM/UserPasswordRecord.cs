namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserPasswordRecord
    {
        [Key]
        public int SeqNo { get; set; }

        public Guid UserID { get; set; }

        [Required]
        [StringLength(128)]
        public string Password { get; set; }

        [Required]
        [StringLength(128)]
        public string Salt { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
