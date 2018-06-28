using System.Collections.Generic;
using Kheech.Web.Models;
using Kheech.Web.ViewModels;

namespace Kheech.Web.ViewModels
{
    public class FriendsDetailViewModel
    {
        public FriendViewModel FriendInformation { get; set; }

        public List<KheechEvent> FriendActivity { get; set; }

        public List<KheechUser> CommonActivity { get; set; }
    }
}