using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MailPool")]
    public partial class MailPool
    {
        public long ID { get; set; }

        [Required]
        [StringLength(100)]
        public string SenderName { get; set; }

        [Required]
        [StringLength(255)]
        public string SenderEmail { get; set; }

        [StringLength(100)]
        public string RecipientName { get; set; }

        [Required]
        [StringLength(255)]
        public string RecipientEmail { get; set; }

        [Required]
        [StringLength(255)]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

        public byte Priority { get; set; }

        public byte Status { get; set; }

        public DateTime? SendDateTime { get; set; }

        public bool IsSent { get; set; }

        public string ErrorMessage { get; set; }

        public int RetryCount { get; set; }

        public DateTime? LastRetryDateTime { get; set; }

        public DateTime CreateDate { get; set; }

        [Required]
        [StringLength(64)]
        public string CreateUser { get; set; }
    }
}
