using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SetSite.Models
{
    public class PlayerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<List<Card>> Sets { get; set; }
    }
}