using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kheech.Web.Models
{
    public class Friendship
    {
        public int Id { get; set; }

        public string ApplicationUserId1 { get; set; }
        public ApplicationUser ApplicationUser1 { get; set; }

        public string ApplicationUserId2 { get; set; }
        public ApplicationUser ApplicationUser2 { get; set; }

        public int FriendshipStatusId { get; set; }
        public FriendshipStatus FriendshipStatus { get; set; }

        public DateTime InsertDate => DateTime.UtcNow;
    }
}