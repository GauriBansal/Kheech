using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kheech.Web.ViewModels
{
    public class ScheduleViewModel
    {
        public string EventName { get; set; }

        public DateTime WhenToMeet { get; set; }

        public string WhoToMeet { get; set; }

        public string WhereToMeet { get; set; }

        //public string TypeOfMeet { get; set; }
    }
}