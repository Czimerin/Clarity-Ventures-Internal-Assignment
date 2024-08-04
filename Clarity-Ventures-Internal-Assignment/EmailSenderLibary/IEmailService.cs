

using Clarity_Ventures_Internal_Assignment.EmailSenderLibary.Models;

namespace Clarity_Ventures_Internal_Assignment.EmailSenderLibary
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailModel email);
    }
}
