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
using System.Web.Hosting;

namespace Kheech.Web.Clients
{
    public class SendGridEmailClient : IEmailClient
    {
        private readonly SendGridClient _sendGridApi;

        private readonly string _genericEmail = File.ReadAllText(HostingEnvironment.MapPath("~/Content/Emails/GenericEmail.html"));
        private readonly string _actionEmail = File.ReadAllText(HostingEnvironment.MapPath("~/Content/Emails/ActionEmail.html"));

        public SendGridEmailClient(string apiKey)
        {
            _sendGridApi = new SendGridClient(apiKey);
        }

        public async Task SendEmailAsync(string toEmailAddress, string subject, string messageText)
        {
            var from = new EmailAddress("kheech@arkansascodingacademy.com", "Kheech");
            var htmlBody = _genericEmail.Replace("{{EmailBody}}", messageText);
            var to = new EmailAddress(toEmailAddress);
            var email = MailHelper.CreateSingleEmail(from, to, subject, messageText, htmlBody);

            await _sendGridApi.SendEmailAsync(email);
        }
        
        public async Task SendActionEmailAsync(string toEmailAddress, string toName, string subject, string messageText, string actionUrl, string actionText)
        {
            var from = new EmailAddress("kheech@arkansascodingacademy.com", "Kheech");
            var to = new EmailAddress(toEmailAddress);
            var htmlBody = _actionEmail
                .Replace("{{Name}}", toName ?? "Kheech user")
                .Replace("{{EmailBody}}", messageText)
                .Replace("{{ActionLinkUrl}}", actionUrl)
                .Replace("{{ActionLinkText}}", actionText);

            var email = MailHelper.CreateSingleEmail(from, to, subject, messageText, htmlBody);

            await _sendGridApi.SendEmailAsync(email);
        }
    }
}
