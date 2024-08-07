using Xunit;
using Moq;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using MimeKit;
using Services.EmailService;
using Core.Models;
using Core.Interfaces;
using Tests.Utilities;

namespace Tests.Services.Tests
{
    public class EmailServiceTests
    {
        private readonly Mock<IEmailSender> _mockEmailSender;
        private readonly Mock<ILogger<EmailService>> _mockLogger;
        private readonly EmailService _emailService;
        private readonly MailSettingsModel _mailSettings;

        public EmailServiceTests()
        {
            _mockEmailSender = new Mock<IEmailSender>();
            _mockLogger = new Mock<ILogger<EmailService>>();
            _mailSettings = new MailSettingsModel
            {
                Name = "Test Name",
                UserName = "test@example.com",
                Host = "smtp.example.com",
                Port = 587,
                Password = "password"
            };
            var options = Options.Create(_mailSettings);
            _emailService = new EmailService(_mockEmailSender.Object, _mockLogger.Object, options);
        }

        [Fact]
        public async Task SendEmailAsync_CreatesAndSendsEmail()
        {
            // Arrange
            var emailModel = new EmailModel
            {
                Subject = "Test Subject",
                Body = "Test Body",
                ToRecipients = new List<EmailRecipient>
            {
                new EmailRecipient { RecipientName = "Recipient Name", RecipientAddress = "recipient@example.com" }
            }
            };

            // Act
            await _emailService.SendEmailAsync(emailModel);

            // Assert
            _mockEmailSender.Verify(sender => sender.SendEmailAsync(It.IsAny<MimeMessage>()), Times.Once);
            
        }

        [Fact]
        public async Task SendEmailAsync_LogsSuccess()
        {
            // Arrange
            var emailModel = new EmailModel
            {
                Subject = "Test Subject",
                Body = "Test Body",
                ToRecipients = new List<EmailRecipient>
            {
                new EmailRecipient { RecipientName = "Recipient Name", RecipientAddress = "recipient@example.com" }
            }
            };

            var testLogger = new TestLogger();
            var options = Options.Create(_mailSettings);
            _mockEmailSender.Setup(sender => sender.SendEmailAsync(It.IsAny<MimeMessage>())).Returns(Task.CompletedTask);

            var emailService = new EmailService(_mockEmailSender.Object, testLogger, options);
            

            // Act
            await emailService.SendEmailAsync(emailModel);

            // Assert
            Assert.Contains(testLogger.LogMessages, msg => msg.Contains("Email log:"));
        }

        [Fact]
        public async Task SendEmailAsync_LogsFailureOnException()
        {
            // Arrange
            var emailModel = new EmailModel
            {
                Subject = "Test Subject",
                Body = "Test Body",
                ToRecipients = new List<EmailRecipient>
        {
            new EmailRecipient { RecipientName = "Recipient Name", RecipientAddress = "recipient@example.com" }
        }
            };

            // Set up the mock email sender to throw an exception
            _mockEmailSender.Setup(sender => sender.SendEmailAsync(It.IsAny<MimeMessage>())).Throws(new Exception("Test Exception"));

            var testLogger = new TestLogger();
            var options = Options.Create(new MailSettingsModel
            {
                Name = "Test Name",
                UserName = "test@example.com",
                Host = "smtp.example.com",
                Port = 587,
                Password = "password"
            });

            var emailService = new EmailService(_mockEmailSender.Object, testLogger, options);

            // Act
            await Assert.ThrowsAsync<Exception>(() => emailService.SendEmailAsync(emailModel));

            // Assert
            Assert.Contains(testLogger.LogMessages, msg => msg.Contains("Error sending email"));
        }
    }
}
