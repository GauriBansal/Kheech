using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kheech.Web.Models
{
    public class Moment
    {
        public int Id { get; set; }

        public int KheechEventId { get; set; }
        public virtual KheechEvent KheechEvent { get; set; }

        [Display(Name = "Upload Image")]
        public string Capture { get; set; }
        
        public string Description {get; set;}
        
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public DateTime InsertDate { get; set; }
    }
}
