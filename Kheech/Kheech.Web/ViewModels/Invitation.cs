using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kheech.Web.Models;
using Microsoft.AspNet.Identity;

namespace Kheech.Web.ViewModels
{
    public class Invitation
    {
        public string To { get; set; }
        public string Subject => $"You are Invited to join Kheech";
        public string Body
        {
            get
            {
                return "you are invited to join the best app to achedule a meeting with friends. Please click the link below: #";
            }
        }

        public string Email => "reach2gauri@gmail.com";
        public string Password => "Sunday@02";
    }
}