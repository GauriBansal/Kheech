using Kheech.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kheech.Web.ViewModels
{
    public class KheechIndexViewModel
    {
        public IEnumerable<KheechEvent> ActiveKheechEvents { get; set; }

        public IEnumerable<KheechEvent> RecentSchedules { get; set; }

        public IEnumerable<Moment> RecentMoments { get; set; }

        public IEnumerable<KheechUser> RecentFriends { get; set; }
    }
}