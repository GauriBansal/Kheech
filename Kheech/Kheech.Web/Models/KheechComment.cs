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
        public KheechEvent KheechEvent { get; set; }
    }
}