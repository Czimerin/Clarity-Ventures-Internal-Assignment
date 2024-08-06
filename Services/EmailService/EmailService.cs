
using Core.Interfaces;
using Core.Models;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MailKit.Security;

namespace Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly MailSettingsModel _mailSettings;

        public EmailService(IOptions<MailSettingsModel> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(EmailModel email)
        {
            var message = new MimeMessage();


            message.From.Add(new MailboxAddress(_mailSettings.Name, _mailSettings.UserName));
            message.To.Add(new MailboxAddress(email.RecipientName, email.RecipientAddress));

            if (email.Subject == null)
                email.Subject = "(no subject)";

            message.Subject = email.Subject;

            if (email.Body == null)
                email.Body = "";

            message.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = email.Body };

            using var clinet = new SmtpClient();

            clinet.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            clinet.Authenticate(_mailSettings.UserName, _mailSettings.Password);
            clinet.Send(message);
            clinet.Disconnect(true);
 

            
        }
    }
}
