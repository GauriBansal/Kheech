using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Kheech.Web.Clients.Interfaces;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.IO;

namespace Kheech.Web.Clients
{
    public class SendGridEmailClient : IEmailClient
    {
        private readonly string _apiKey;

        private readonly string _genericEmail = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Content/Emails/GenericEmail.html"));

        public SendGridEmailClient()
        {
            _apiKey = ConfigurationManager.AppSettings["SendGridApiKey"];
        }

        public async Task SendEmailAsync(string toEmailAddress, string subject, string htmlBody)
        {
            var message = _genericEmail.Replace("{{EmailBody}}", htmlBody);
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress("noreply@kheech.com", "Kheech");
            var to = new EmailAddress(toEmailAddress);
            var email = MailHelper.CreateSingleEmail(from, to, subject, "", htmlBody);

            await client.SendEmailAsync(email);
        }
    }
}