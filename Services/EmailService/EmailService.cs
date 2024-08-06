
using Core.Interfaces;
using Core.Models;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using MailKit.Security;

namespace Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly MailSettingsModel _mailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<MailSettingsModel> mailSettings, ILogger<EmailService> logger)
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;
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

            _logger.LogInformation("Sending email:");
            _logger.LogInformation("From: {SenderName} <{SenderAddress}>", _mailSettings.Name, _mailSettings.UserName);
            _logger.LogInformation("To: {RecipientName} <{RecipientAddress}>", email.RecipientName, email.RecipientAddress);
            _logger.LogInformation("Subject: {Subject}", email.Subject);
            _logger.LogInformation("Body: {Body}", email.Body);

            using var clinet = new SmtpClient();

            try
            {
                clinet.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                clinet.Authenticate(_mailSettings.UserName, _mailSettings.Password);
                clinet.Send(message);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "An error occurred while sending the email.");
            }

            clinet.Disconnect(true);
        }


    }
}
