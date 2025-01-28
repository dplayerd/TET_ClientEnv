namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TET_Parameters
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        [StringLength(64)]
        public string Type { get; set; }

        [Required]
        [StringLength(64)]
        public string Item { get; set; }

        public int Seq { get; set; }

        public bool IsEnable { get; set; }

        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        public string ModifyUser { get; set; }

        public DateTime ModifyDate { get; set; }
    }
}
