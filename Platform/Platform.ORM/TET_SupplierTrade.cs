namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TET_SupplierTrade
    {
        [Key]
        [StringLength(64)]
        public string SubpoenaNo { get; set; }

        public DateTime SubpoenaDate { get; set; }

        [Required]
        [StringLength(32)]
        public string VenderCode { get; set; }

        [Required]
        [StringLength(64)]
        public string Currency { get; set; }

        public decimal Amount { get; set; }

        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        public string ModifyUser { get; set; }

        public DateTime ModifyDate { get; set; }
    }
}
