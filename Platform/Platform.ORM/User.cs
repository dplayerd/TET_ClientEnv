namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        [StringLength(64)]
        public string UserID { get; set; }

        [Required]
        [StringLength(64)]
        public string EmpID { get; set; }

        [Required]
        [StringLength(64)]
        public string FirstNameEN { get; set; }

        [Required]
        [StringLength(64)]
        public string LastNameEN { get; set; }

        [Required]
        [StringLength(64)]
        public string FirstNameCH { get; set; }

        [Required]
        [StringLength(64)]
        public string LastNameCH { get; set; }

        [Required]
        [StringLength(64)]
        public string UnitCode { get; set; }

        [Required]
        [StringLength(128)]
        public string UnitName { get; set; }

        [Required]
        [StringLength(64)]
        public string LeaderID { get; set; }

        [Required]
        [StringLength(128)]
        public string EMail { get; set; }

        [Required]
        [StringLength(1)]
        public string IsEnabled { get; set; }
    }
}
