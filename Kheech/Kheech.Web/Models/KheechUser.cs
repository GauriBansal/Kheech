using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kheech.Web.Models
{
    public class KheechUser
    {
        public int Id { get; set; }

        public int KheechEventId { get; set; }
        public virtual KheechEvent KheechEvent { get; set; }

        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        
        public bool IsAccepted { get; set; }
         
    }
}
