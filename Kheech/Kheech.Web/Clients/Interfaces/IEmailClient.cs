using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace Kheech.Web.Clients.Interfaces
{
    public interface IEmailClient
    {
        Task SendEmailAsync(string toEmailAddress, string subject, string htmlBody);
        Task SendActionEmailAsync(string toEmailAddress, string toName, string subject, string messageText, string actionUrl, string actionText);
    }
}
