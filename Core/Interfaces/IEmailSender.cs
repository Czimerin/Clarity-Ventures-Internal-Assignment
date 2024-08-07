using MimeKit;

namespace Core.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(MimeMessage message);
    }
}