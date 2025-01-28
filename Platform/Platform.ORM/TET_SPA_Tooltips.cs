namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TET_SPA_Tooltips
    {
        public Guid ID { get; set; }

        [Required]
        [StringLength(64)]
        public string ModuleName { get; set; }

        [Required]
        [StringLength(64)]
        public string FieldName { get; set; }

        [Required]
        [StringLength(512)]
        public string Description { get; set; }
    }
}
