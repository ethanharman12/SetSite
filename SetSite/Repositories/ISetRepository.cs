using SetSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetSite.Repositories
{
    public interface ISetRepository
    {
        int CreateGame(Game game);
        Game FindGame(int gameId);
        ApplicationUser FindPlayer(int playerId);
        void UpdateGame(Game game);
    }
}
