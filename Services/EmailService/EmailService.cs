
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
        private readonly IEmailSender _emailSender;
        private readonly ILogger<EmailService> _logger;
        private readonly MailSettingsModel _mailSettings;

        public EmailService(IEmailSender emailSender, ILogger<EmailService> logger, IOptions<MailSettingsModel> mailSettings)
        {
            _emailSender = emailSender;
            _logger = logger;
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(EmailModel email)
        {
            var message = CreateMessage(email);

            var logDetails = new LogDetails
            {
                Sender = $"{_mailSettings.Name} <{_mailSettings.UserName}>",
                Recipients = email.ToRecipients != null
                    ? string.Join(", ", email.ToRecipients.Select(r => $"{r.RecipientName} <{r.RecipientAddress}>"))
                    : "",
                CcRecipients = email.CcRecipients != null
                    ? string.Join(", ", email.CcRecipients.Select(r => $"{r.RecipientName} <{r.RecipientAddress}>"))
                    : "",
                Subject = email.Subject,
                Body = email.Body,
                SentDate = DateTime.UtcNow,
                Status = "Pending"
            };

            LogEmailDetails(logDetails);

            try
            {
                await _emailSender.SendEmailAsync(message);
                logDetails.Status = "Success";
            }
            catch
            {
                logDetails.Status = "Failed";
            }

            _logger.LogInformation("Email log: {@logDetails}", logDetails);



        }

        private MimeMessage CreateMessage(EmailModel email)
        {
            var builder = new EmailMessageBuilder(_mailSettings);
            builder.SetFrom(_mailSettings.Name, _mailSettings.UserName);
            builder.AddTo(email.ToRecipients);
            builder.AddCc(email.CcRecipients);
            builder.SetSubject(email.Subject);
            builder.SetBody(email.Body);
            builder.AddAttachments(email.Attachments);

            return builder.Build();
        }

        private class EmailMessageBuilder
        {
            private readonly MailSettingsModel _mailSettings;
            private readonly MimeMessage _message;

            public EmailMessageBuilder(MailSettingsModel mailSettings)
            {
                _mailSettings = mailSettings;
                _message = new MimeMessage();
            }

            public void SetFrom(string name, string address)
            {
                _message.From.Add(new MailboxAddress(name, address));
            }

            public void AddTo(List<EmailRecipient> toRecipients)
            {
                if (toRecipients != null)
                {
                    foreach (var recipient in toRecipients)
                    {
                        _message.To.Add(new MailboxAddress(recipient.RecipientName, recipient.RecipientAddress));
                    }
                }
            }

            public void AddCc(List<EmailRecipient> ccRecipients)
            {
                if (ccRecipients != null)
                {
                    foreach (var ccRecipient in ccRecipients)
                    {
                        _message.Cc.Add(new MailboxAddress(ccRecipient.RecipientName, ccRecipient.RecipientAddress));
                    }
                }
            }

            public void SetSubject(string subject)
            {
                _message.Subject = subject ?? "(no subject)";
            }

            public void SetBody(string body)
            {
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = body ?? ""
                };
                _message.Body = bodyBuilder.ToMessageBody();
            }

            public void AddAttachments(List<EmailAttachment> attachments)
            {
                if (attachments != null && attachments.Any())
                {
                    var bodyBuilder = new BodyBuilder
                    {
                        HtmlBody = (_message.Body as TextPart)?.Text ?? ""
                    };

                    foreach (var attachment in attachments)
                    {
                        bodyBuilder.Attachments.Add(attachment.FileName, attachment.Content);
                    }
                    _message.Body = bodyBuilder.ToMessageBody();
                }
            }

            public MimeMessage Build()
            {
                return _message;
            }
        }


        private void LogEmailDetails(LogDetails logDetails)
        {
            _logger.LogInformation("Sending email:");
            _logger.LogInformation("From: {Sender}", logDetails.Sender);
            _logger.LogInformation("To: {Recipient}", logDetails.Recipients);
            _logger.LogInformation("Subject: {Subject}", logDetails.Subject);
            _logger.LogInformation("Body: {Body}", logDetails.Body);
        }


    }
}
