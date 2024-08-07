using MimeKit;
using MailKit.Security;
using MailKit.Net.Smtp;



namespace Services.EmailService
{
    public class SmtpClientWrapper : Core.Interfaces.ISmtpClient
    {
        private readonly SmtpClient _smtpClient;

        public SmtpClientWrapper(SmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
        }

        public Task ConnectAsync(string host, int port, SecureSocketOptions options) =>
            _smtpClient.ConnectAsync(host, port, options);

        public Task AuthenticateAsync(string userName, string password) =>
            _smtpClient.AuthenticateAsync(userName, password);

        public Task SendAsync(MimeMessage message) =>
            _smtpClient.SendAsync(message);

        public Task DisconnectAsync(bool quit) =>
            _smtpClient.DisconnectAsync(quit);
    }
}