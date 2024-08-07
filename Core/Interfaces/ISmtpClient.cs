using MailKit.Security;
using MimeKit;

namespace Core.Interfaces
{
    public interface ISmtpClient
    {
        Task ConnectAsync(string host, int port, SecureSocketOptions options);
        Task AuthenticateAsync(string userName, string password);
        Task SendAsync(MimeMessage message);
        Task DisconnectAsync(bool quit);
    }
}