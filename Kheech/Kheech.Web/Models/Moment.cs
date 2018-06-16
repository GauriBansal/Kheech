using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kheech.Web.Models
{
    public class Moment
    {
        public int Id { get; set; }

        public int KheechEventId { get; set; }
        public KheechEvent KheechEvent { get; set; }

        public string Capture { get; set; }
    }
}