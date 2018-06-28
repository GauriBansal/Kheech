using System.Collections.Generic;

namespace Kheech.Web.ViewModels
{
    public class FriendsIndexViewModel
    {
        public int FriendsCount { get; set; }

        public List<FriendViewModel> FriendViewModels { get; set; }
    }
}