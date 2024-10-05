using SendGrid.Helpers.Mail;
using SendGrid;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Sowing_O2.Utilities
{
    public class EmailSender : IEmailSender
    {
        public string SendGridSecret { get; set; }

        public EmailSender(IConfiguration config)
        {
            SendGridSecret = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");

            if (string.IsNullOrEmpty(SendGridSecret))
            {
                throw new InvalidOperationException("La clave de API de SendGrid no está configurada.");
            }
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SendGridClient(SendGridSecret);
            var from = new EmailAddress("sowingo2@outlook.com");
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);
            return client.SendEmailAsync(msg);
        }
    }
}
