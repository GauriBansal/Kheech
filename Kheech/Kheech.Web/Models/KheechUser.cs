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
        public KheechEvent KheechEvent { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }
}