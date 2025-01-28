namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserLoginRecord
    {
        [Key]
        public int SeqNo { get; set; }

        public Guid? UserID { get; set; }

        [StringLength(200)]
        public string Account { get; set; }

        [Required]
        [StringLength(50)]
        public string UserIP { get; set; }

        public bool IsSuccess { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
