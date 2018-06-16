using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kheech.Web.Models
{
    public class GroupUser
    {
        public int Id { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}