using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kheech.Web.Models
{
    public class KheechEvent
    {
        public int Id { get; set; }

        /// <summary>
        /// This is the User who creates the Kheech event and invite people over
        /// </summary>
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public string EventName { get; set; }

        public int LocationId { get; set; }
        public virtual Location Location { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int GroupId { get; set; }
        public virtual Group Group { get; set; }

        public bool IsGroupEvent { get; set; }
        
        public virtual ICollection<KheechUser> KheechUsers {get; set;}
        
        public virtual ICollection<KheechComment> KheechComments {get; set;}

    }
}
