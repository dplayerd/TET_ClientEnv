namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AdminLog
    {
        [Key]
        public int SeqNo { get; set; }

        [Required]
        [StringLength(200)]
        public string ReferenceID { get; set; }

        [StringLength(300)]
        public string Title { get; set; }

        public byte? AccessType { get; set; }

        [StringLength(50)]
        public string AccessName { get; set; }

        public string Message { get; set; }

        public Guid UserID { get; set; }

        public Guid? ModuleID { get; set; }

        public Guid? CreateUser { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
