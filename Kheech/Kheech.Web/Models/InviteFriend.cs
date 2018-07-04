using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kheech.Web.Models
{
    public class InviteFriend
    {
        public int Id { get; set; }

        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "Enter your Friends Email")]
        public string Email { get; set; }

        public int FriendshipStatusId { get; set; }
        public virtual FriendshipStatus FriendshipStatus { get; set; }

        public DateTime InsertDate { get; set; }
    }
}