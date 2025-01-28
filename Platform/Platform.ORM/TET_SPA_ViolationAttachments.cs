namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TET_SPA_ViolationAttachments
    {
        [Key]
        public Guid ID { get; set; }

        public Guid VDetailID { get; set; }

        [Required]
        public string FilePath { get; set; }

        [Required]
        [StringLength(128)]
        public string FileName { get; set; }

        [Required]
        [StringLength(128)]
        public string OrgFileName { get; set; }

        [Required]
        [StringLength(32)]
        public string FileExtension { get; set; }

        public int FileSize { get; set; }

        [Required]
        [StringLength(64)]
        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        [Required]
        [StringLength(64)]
        public string ModifyUser { get; set; }

        public DateTime ModifyDate { get; set; }
    }
}
