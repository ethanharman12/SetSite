using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SetSite.Models
{
    public class Card
    {
        public int id { get; set; }
        public string color { get; set; }
        public int number { get; set; }
        public string fill { get; set; }
        public string shape { get; set; }
    }
}