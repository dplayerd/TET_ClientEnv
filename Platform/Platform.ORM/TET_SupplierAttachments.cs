namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TET_SupplierAttachments
    {
        [Key]
        public Guid ID { get; set; }

        public Guid SupplierID { get; set; }

        [Required]
        [StringLength(128)]
        public string FileName { get; set; }

        [Required]
        [StringLength(128)]
        public string OrgFileName { get; set; }

        public string FilePath { get; set; }

        public string FileExtension { get; set; }

        public int FileSize { get; set; }

        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        public string ModifyUser { get; set; }

        public DateTime ModifyDate { get; set; }
    }
}
