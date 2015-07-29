using SetSite.Models;
using SetSite.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetSite.Tests.Mocks
{
    public class MockSetRepository : ISetRepository
    {
        public List<Game> Games { get; set; }
        public List<ApplicationUser> Users { get; set; }

        public MockSetRepository()
        {
            Games = new List<Game>();
            Users = new List<ApplicationUser>();
        }

        public int CreateGame(Game game)
        {
            game.Id = Games.Count + 1;
            Games.Add(game);
            return game.Id;
        }

        public Game FindGame(int gameId)
        {
            return Games.FirstOrDefault(g => g.Id == gameId);
        }

        public ApplicationUser FindPlayer(int playerId)
        {
            return Users.FirstOrDefault(u => u.Id == playerId);
        }

        public void UpdateGame(Game game)
        {
            var dbGame = Games.FirstOrDefault(g => g.Id == game.Id);

            if(dbGame != null)
            {
                dbGame = game;
            }
            else
            {
                CreateGame(game);
            }
        }
    }
}
