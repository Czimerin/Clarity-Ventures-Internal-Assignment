using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class EmailModel
    {
        public List<EmailRecipient>? ToRecipients { get; set; } = new List<EmailRecipient>();
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public List<EmailRecipient>? CcRecipients { get; set; } = new List<EmailRecipient>();
        public List<EmailAttachment>? Attachments { get; set; } = new List<EmailAttachment>();
    }

    public class EmailRecipient
    {
        public string RecipientName { get; set; }
        public string RecipientAddress { get; set; }
    }

    public class EmailAttachment
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
    }
}
