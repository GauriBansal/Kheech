using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Kheech.Web.Models;

namespace Kheech.Web.ViewModels
{
    public class ScheduleViewModel
    {
        public ICollection<Friendship> Friends {get; set;}
        
        [Display(Name = "What do you want to do?")]
        public string EventName { get; set; }

        [Display(Name = "When do you want to meet?")]
        public DateTime WhenToMeet { get; set; }

        [Display(Name = "Who do you want to meet?")]
        public string WhoToMeet { get; set; }

        [Display(Name = "Where do you want to meet?")]
        public string WhereToMeet { get; set; }
    }
}
