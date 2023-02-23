using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Abby.Utility
{
    public class EmailSender : IEmailSender
    {
        public string SendGridSecret { get; set; }
        public EmailSender(IConfiguration config)
        {
            SendGridSecret = config.GetValue<string>("SendGrid:SecretKey");
        }

        Task IEmailSender.SendEmailAsync(string email, string subject, string htmlMessage)
        {


            var client = new SendGridClient(SendGridSecret);
            var from = new EmailAddress("hybase@outlook.hu");
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from,to,subject,htmlMessage,htmlMessage);



            return client.SendEmailAsync(msg);
        }
    }
}
