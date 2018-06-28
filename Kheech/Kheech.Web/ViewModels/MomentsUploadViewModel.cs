using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kheech.Web.ViewModels
{
    public class MomentsUploadViewModel
    {
        public int KheechId { get; set; }

        public HttpPostedFileBase File { get; set; }
    }
}