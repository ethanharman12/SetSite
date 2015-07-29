using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SetSite.Models
{
    public class PlayerGame
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int GameId { get; set; }
        public int Score { get; set; }

        public virtual Game Game { get; set; }
        public virtual ApplicationUser Player { get; set; }
    }
}