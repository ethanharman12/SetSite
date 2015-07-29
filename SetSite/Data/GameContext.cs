using SetSite.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SetSite.Data
{
    public class GameContext : ApplicationDbContext
    {
        public GameContext()
            //: base("DefaultConnection")
        {
        }
        
        public DbSet<Game> Games { get; set; }
        public DbSet<PlayerGame> PlayerGames { get; set; }
    }
}