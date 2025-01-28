namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Site
    {
        public Guid ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public Guid? MediaFileID { get; set; }

        [Required]
        [StringLength(300)]
        public string HeaderText { get; set; }

        [Required]
        [StringLength(300)]
        public string FooterText { get; set; }
    }
}
