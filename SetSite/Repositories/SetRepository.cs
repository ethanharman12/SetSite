using SetSite.Data;
using SetSite.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SetSite.Repositories
{
    public class SetRepository : ISetRepository
    {
        private GameContext context = new GameContext();

        public int CreateGame(Game game)
        {
            context.Games.Add(game);
            context.SaveChanges();

            return game.Id;
        }

        public Game FindGame(int gameId)
        {
            return context.Games.Find(gameId);
        }

        public ApplicationUser FindPlayer(int playerId)
        {
            return context.Users.Find(playerId);
        }

        public void UpdateGame(Game game)
        {
            context.Games.Attach(game);
            var entry = context.Entry(game);
            entry.State = EntityState.Modified;

            context.SaveChanges();
        }
    }
}