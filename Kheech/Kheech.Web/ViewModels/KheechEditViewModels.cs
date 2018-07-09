using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Kheech.Web.Models;

namespace Kheech.Web.ViewModels
{
    public class KheechEditViewModels
    {

        public KheechEditViewModels()
        {
            Friends = new List<FriendViewModel>();
        }

        public List<FriendViewModel> Friends { get; set; }

        public KheechEvent KheechEvent { get; set; }

        [Display(Name = "Who do you want to invite?")]
        public string WhoToMeet { get; set; }

        [Display(Name = "Location for event")]
        public string WhereToMeet { get; set; }

    }
}