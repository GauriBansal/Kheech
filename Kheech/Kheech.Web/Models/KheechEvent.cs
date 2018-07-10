using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Display(Name = "Name of Kheech")]
        public string EventName { get; set; }

        public int LocationId { get; set; }
        public virtual Location Location { get; set; }

        [Display(Name = "Event start time")]
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Display(Name = "Group to be invited")]
        public int? GroupId { get; set; }
        public virtual Group Group { get; set; }

        public bool IsGroupEvent { get; set; }
        
        public virtual ICollection<KheechUser> KheechUsers {get; set;}
        
        public virtual ICollection<KheechComment> KheechComments {get; set;}

        public virtual ICollection<Moment> KheechMoments { get; set; }

        public DateTime InsertDate { get; set; }
    }
}
