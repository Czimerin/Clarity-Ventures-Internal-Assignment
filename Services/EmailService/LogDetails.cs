using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.EmailService
{
    public class LogDetails
    {
        public string Sender { get; set; }
        public string Recipients { get; set; }
        public string CcRecipients { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime SentDate { get; set; }
        public string Status { get; set; }
    }
}
