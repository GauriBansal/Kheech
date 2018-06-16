using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kheech.Web.Models
{
    public class Location
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string address1 { get; set; }

        public string address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }
    }
}