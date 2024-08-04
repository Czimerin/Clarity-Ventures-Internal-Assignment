
using Core.Interfaces;
using Core.Models;
using MimeKit;
using MailKit.Net.Smtp;

namespace Services.EmailService
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(EmailModel email)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(email.SenderName, email.SenderAddress));
            message.To.Add(new MailboxAddress(email.RecipientName, email.RecipientAddress));

            if (email.Subject == null)
                email.Subject = "(no subject)";

            message.Subject = email.Subject;

            if (email.Body == null)
                email.Body = "";

            message.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = email.Body };

            using(var clinet = new SmtpClient())
            {
                await clinet.ConnectAsync("smtp.example.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await clinet.AuthenticateAsync("username", "password");
                await clinet.SendAsync(message);
                await clinet.DisconnectAsync(true);
            }

            
        }
    }
}
