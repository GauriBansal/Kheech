using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kheech.Web.Models
{
    public class KheechComment
    {
        public int Id { get; set; }

        public string Discussion { get; set; }

        public int KheechEventId { get; set; }
        public virtual KheechEvent KheechEvent { get; set; }

        public string CreatorId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public DateTime InsertDate { get; set; }
    }
}
