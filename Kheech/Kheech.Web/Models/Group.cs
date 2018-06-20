using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kheech.Web.Models
{
    public class Group
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string GroupImage { get; set; }

        /// <summary>
        /// User who creates the group
        /// </summary>
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}
