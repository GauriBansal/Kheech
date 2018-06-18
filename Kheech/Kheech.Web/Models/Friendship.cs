using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kheech.Web.Models
{
    public class Friendship
    {
        public int Id { get; set; }

        public string InitiatorId { get; set; }
        public virtual ApplicationUser Initiator { get; set; }

        public string RecipientId { get; set; }
        public virtual ApplicationUser Recipient { get; set; }

        public int FriendshipStatusId { get; set; }
        public FriendshipStatus FriendshipStatus { get; set; }

        //TODO: Set this to DateTime.UtcNow in SaveChanges() method on DbContext
        //public DateTime InsertDate => DateTime.UtcNow;
    }
}