using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clarity_Ventures_Internal_Assignment.EmailSenderLibary.Models
{
    public class EmailModel
    {


        public string Sender { get; set; }
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string? Body { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }       
    }
}
