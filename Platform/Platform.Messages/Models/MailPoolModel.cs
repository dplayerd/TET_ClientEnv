using Platform.Messages.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Messages.Models
{
    public class MailPoolModel
    {
        public long ID { get; set; }

        public string SenderName { get; set; }

        public string SenderEmail { get; set; }

        public string RecipientName { get; set; }

        public string RecipientEmail { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public MailPriorityEnum Priority { get; set; }

        public MailStatusEnum Status { get; set; }

        public DateTime? SendDateTime { get; set; }

        public bool IsSent { get; set; }

        public string ErrorMessage { get; set; }

        public int RetryCount { get; set; }

        public DateTime? LastRetryDateTime { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreateUser { get; set; }

    }
}
