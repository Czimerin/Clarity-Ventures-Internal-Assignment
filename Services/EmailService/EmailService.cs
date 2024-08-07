
using Core.Interfaces;
using Core.Models;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using MailKit.Security;
using Serilog;

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

            var logDetails = new LogDetails
            {
                Sender = $"{_mailSettings.Name} <{_mailSettings.UserName}>",
                Recipient = $"{email.RecipientName} <{email.RecipientAddress}>",
                Subject = email.Subject,
                Body = email.Body,
                SentDate = DateTime.UtcNow,
                Status = "Pending"
            };

            LogEmailDetails(logDetails);

            using var clinet = new SmtpClient();

            try
            {
                clinet.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                clinet.Authenticate(_mailSettings.UserName, _mailSettings.Password);
                clinet.Send(message);

                logDetails.Status = "Success";
                _logger.LogInformation("Email sent successfully");
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "An error occurred while sending the email.");
                logDetails.Status = "Failed";
            }

            clinet.Disconnect(true);
            Log.Information("Email log: {@logDetails}", logDetails);
        }

        private void LogEmailDetails(LogDetails logDetails)
        {
            _logger.LogInformation("Sending email:");
            _logger.LogInformation("From: {Sender}", logDetails.Sender);
            _logger.LogInformation("To: {Recipient}", logDetails.Recipient);
            _logger.LogInformation("Subject: {Subject}", logDetails.Subject);
            _logger.LogInformation("Body: {Body}", logDetails.Body);
        }


    }
}
