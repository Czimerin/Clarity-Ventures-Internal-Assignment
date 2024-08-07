using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.EmailService
{
    public class LogDetails
    {
        public required string Sender { get; set; }
        public required string Recipients { get; set; }
        public string? CcRecipients { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public required DateTime SentDate { get; set; }
        public required string Status { get; set; }
    }
}
