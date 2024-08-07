using Core.Interfaces;
using Core.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Services.EmailService
{
    public class EmailSender : IEmailSender
    {
        private readonly MailSettingsModel _mailSettings;
        private readonly ILogger<EmailSender> _logger;
        private int MaxRetries = 3;
        private const int RetryDelayMilliseconds = 2000;

        public EmailSender(IOptions<MailSettingsModel> mailSettings, ILogger<EmailSender> logger)
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(MimeMessage message)
        {
            using var client = new SmtpClient();

            for (int attempt = 1; attempt <= MaxRetries; attempt++)
            {
                try
                {
                    await client.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);

                    _logger.LogInformation("Email sent successfully");
                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Attempt {Attempt}: Failed to send email.", attempt);

                    if (attempt == MaxRetries)
                    {
                        _logger.LogError("Max({Attempt}) attempts reached.", attempt);
                        throw;
                    }

                    await Task.Delay(RetryDelayMilliseconds);
                }
            }
        }
    }
}
