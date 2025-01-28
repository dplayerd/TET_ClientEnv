namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TET_SPA_ApproverSetup
    {
        [Key]
        public Guid ID { get; set; }

        public Guid ServiceItemID { get; set; }

        public Guid BUID { get; set; }

        [Required]
        [StringLength(640)]
        public string InfoFill { get; set; }

        [Required]
        [StringLength(64)]
        public string InfoConfirm { get; set; }

        [Required]
        [StringLength(64)]
        public string Lv1Apprvoer { get; set; }

        [StringLength(64)]
        public string Lv2Apprvoer { get; set; }

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
