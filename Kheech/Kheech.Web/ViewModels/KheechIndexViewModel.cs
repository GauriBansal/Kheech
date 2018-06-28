using Kheech.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kheech.Web.ViewModels
{
    public class KheechIndexViewModel
    {
        public List<KheechEvent> ActiveKheechEvents { get; set; }

        public List<KheechEvent> RecentSchedules { get; set; }

        public List<Moment> RecentMoments { get; set; }

        public List<KheechUser> RecentFriends { get; set; }
    }
}