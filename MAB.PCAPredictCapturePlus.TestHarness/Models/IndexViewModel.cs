using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MAB.PCAPredictCapturePlus.TestHarness.Models
{
    public class IndexViewModel
    {
        public string Company { get; set; }
        public string BuildingName { get; set; }
        [Required]
        public string Street { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string Line4 { get; set; }
        public string Line5 { get; set; }
        [Required]
        public string City { get; set; }
        public string County { get; set; }
        [Required]
        public string Postcode { get; set; }
    }
}