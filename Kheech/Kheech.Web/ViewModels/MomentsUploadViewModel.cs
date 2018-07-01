using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kheech.Web.ViewModels
{
    public class MomentsUploadViewModel
    {
        public int KheechId { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        [Display(Name = "Upload Image")]
        public HttpPostedFileBase File { get; set; }
    }
}