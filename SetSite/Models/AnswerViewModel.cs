using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SetSite.Models
{
    public class AnswerViewModel
    {
        public List<Card> set { get; set; }
        public int stateId { get; set; }
        public int oppId { get; set; }
    }
}