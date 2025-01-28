namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MediaFile
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SeqNo { get; set; }

        public Guid ID { get; set; }

        [Required]
        [StringLength(50)]
        public string ModuleName { get; set; }

        [Required]
        [StringLength(200)]
        public string ModuleID { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(50)]
        public string Purpose { get; set; }

        [Required]
        [StringLength(200)]
        public string FilePath { get; set; }

        [Required]
        [StringLength(300)]
        public string OrgFileName { get; set; }

        [Required]
        [StringLength(300)]
        public string OutputFileName { get; set; }

        [Required]
        [StringLength(100)]
        public string MimeType { get; set; }

        public bool RequireAuth { get; set; }

        public bool IsEnable { get; set; }

        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        public string ModifyUser { get; set; }

        public DateTime? ModifyDate { get; set; }

        public string DeleteUser { get; set; }

        public DateTime? DeleteDate { get; set; }
    }
}
