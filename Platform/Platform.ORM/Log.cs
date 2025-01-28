namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Log
    {
        [Key]
        public int SeqNo { get; set; }

        [StringLength(200)]
        public string ReferenceID { get; set; }

        public byte LogLevel { get; set; }

        [Required]
        public string Name { get; set; }

        public string Message { get; set; }

        public string Exceptions { get; set; }

        public Guid? UserID { get; set; }

        public Guid? SiteID { get; set; }

        public Guid? ModuleID { get; set; }

        public Guid? PageID { get; set; }

        public Guid? CreateUser { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
