namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TET_Supplier_Menu")]
    public partial class Page
    {
        public Guid ID { get; set; }

        public Guid SiteID { get; set; }

        public Guid? ParentID { get; set; }

        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        public byte MenuType { get; set; }

        [StringLength(128)]
        public string Linkurl { get; set; }

        public Guid? ModuleID { get; set; }

        [StringLength(128)]
        public string PageIcon { get; set; }

        public int SortNo { get; set; }

        public bool IsEnable { get; set; }

        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        public string ModifyUser { get; set; }

        public DateTime? ModifyDate { get; set; }
    }
}
