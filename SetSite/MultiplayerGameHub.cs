using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SetSite.Models;
using System.Threading.Tasks;
using SetSite.Data;
using SetSite.Repositories;

namespace SetSite
{
    public class MultiplayerGameHub : Hub
    {
        public static Dictionary<string, SetGame> games = new Dictionary<string, SetGame>();

        public ISetRepository setRepo { get; set; }

        public UserManager<ApplicationUser, int> userManager { get; set; }

        //public MultiplayerGameHub()
        //    : this(new SetRepository(), HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>())
        //{
        //}

        public MultiplayerGameHub(ISetRepository repo)//, UserManager<ApplicationUser, int> manager)
        {
            setRepo = repo;
            //userManager = manager;
        }

        public int CreateGame()
        {
            //var userId = Context.User.Identity.GetUserId<int>();

            var game = new Game() { CreateDate = DateTime.Now };

            var id = setRepo.CreateGame(game);

            games.Add("Game" + id, new SetGame());

            Clients.All.gameAdded(id);

            return id;
        }

        public List<int> GetCurrentGames()
        {
            List<int> gameIds = new List<int>();

            var currentGames = games.Where(keyVal => keyVal.Value.StateId == 0).Select(keyval => keyval.Key);

            foreach (var key in currentGames)
            {
                gameIds.Add(int.Parse(key.Remove(0, 4)));
            }

            return gameIds;
        }

        public async Task JoinGame(int gameId)
        {
            var userId = Context.User.Identity.GetUserId<int>();

            if (games.Keys.Contains("Game" + gameId))
            {
                var game = games["Game" + gameId];

                await Groups.Add(Context.ConnectionId, "Game" + gameId);

                foreach (var player in game.Players.Where(p => p.Id != userId))
                {
                    Clients.Caller.playerJoin(new PlayerViewModel() { Id = player.Id, Name = player.Name, Sets = player.Sets });
                }

                var currentPlayer = game.Players.FirstOrDefault(p => p.Id == userId);

                if (currentPlayer != null)
                {
                    Clients.OthersInGroup("Game" + gameId).playerReconnected(userId);

                    ResendGame(game, currentPlayer);
                }
                else
                {
                    if (game.StateId == 0) //only add players if the game hasn't started
                    {
                        var user = setRepo.FindPlayer(userId);
                        var pvm = new PlayerViewModel() { Id = user.Id, Name = user.DisplayName };
                        game.Players.Add(pvm);
                        Clients.OthersInGroup("Game" + gameId).playerJoin(pvm);
                    }
                    else
                    {
                        //spectate
                        Clients.Caller.spectate(new GameState() { id = game.StateId, cards = game.TotalSet });
                        if (game.IsPaused)
                        {
                            Clients.Caller.pauseGame();
                        }
                    }
                }

                var seconds = game.IsPaused
                                ? game.Timer.SecondsElapsed
                                : game.Timer.GetCurrentSecondsElapsed();

                Clients.Caller.setTime(seconds);
            }
            else
            {
                var game = setRepo.FindGame(gameId);

                //if exists, show score
                if (game != null)
                {
                    ShowScore(game);
                }
                else //show error message 
                {

                }
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var userId = Context.User.Identity.GetUserId<int>();

            //send message to others in group
            foreach (var pair in games)
            {
                if (pair.Value.Players.Any(p => p.Id == userId))
                {
                    Clients.OthersInGroup(pair.Key).playerDisconnected(userId);
                }
            }

            return base.OnDisconnected(stopCalled);
        }

        public void PauseGame(int gameId)
        {
            var game = games["Game" + gameId];

            if (game.Timer.StartTime != null)
            {
                var span = DateTime.Now.Subtract(game.Timer.StartTime.Value);
                game.Timer.SecondsElapsed += span.TotalSeconds;
                game.Timer.StartTime = null;
            }

            game.IsPaused = true;

            Clients.Group("Game" + gameId).pauseGame();
        }

        public void ResumeGame(int gameId)
        {
            var game = games["Game" + gameId];
            game.Timer.StartTime = DateTime.Now;

            game.IsPaused = false;

            Clients.Group("Game" + gameId).resumeGame();
        }

        public void SendInvite(int gameId, int friendId)
        {
            //if(connections[friendId])
        }

        public void SendSet(AnswerViewModel message, int gameId)
        {
            //will probably need to parse this to validate information
            //gameId, playerId, etc.

            var userId = Context.User.Identity.GetUserId<int>();

            var game = games["Game" + gameId];

            if (game.IsSet(message.set))
            {
                message.oppId = userId;

                //check to see if exists?
                var player = game.Players.FirstOrDefault(p => p.Id == userId);
                if (player.Sets == null)
                {
                    player.Sets = new List<List<Card>>();
                }
                player.Sets.Add(message.set);

                Clients.OthersInGroup("Game" + gameId).updateSet(message);

                var gameState = game.ProcessSet(message.set);

                Clients.Group("Game" + gameId).startNextState(gameState);

                if (gameState.id == -1)
                {
                    var span = DateTime.Now.Subtract(game.Timer.StartTime.Value);
                    game.Timer.SecondsElapsed += span.TotalSeconds;
                    game.Timer.StartTime = null;

                    SaveGame(gameId);
                }
            }
        }

        public void StartGame(int gameId)
        {
            var userId = Context.User.Identity.GetUserId<int>();
            var game = games["Game" + gameId];

            Clients.OthersInGroup("Game" + gameId).sendStartVote(userId);
            game.Vote(userId);

            if (game.Players.Count == game.StartVotes.Count)
            {
                var gameState = game.StartGame();
                game.Timer.StartTime = DateTime.Now;
                Clients.Group("Game" + gameId).startNextState(gameState);
            }
        }

        private void ResendGame(SetGame game, PlayerViewModel currentPlayer)
        {
            if (game.StateId > 0)
            {
                Clients.Caller.sendSets(currentPlayer.Sets);
                Clients.Caller.startNextState(new GameState() { id = game.StateId, cards = game.TotalSet });
                if (game.IsPaused)
                {
                    Clients.Caller.pauseGame();
                }
            }
        }

        private void SaveGame(int gameId)
        {
            var game = games["Game" + gameId];

            var dbGame = setRepo.FindGame(gameId);

            if (dbGame != null)
            {
                dbGame.TotalSeconds = game.Timer.SecondsElapsed;

                dbGame.PlayerGames = new List<PlayerGame>();
                foreach (var player in game.Players)
                {
                    dbGame.PlayerGames.Add(new PlayerGame()
                    {
                        GameId = gameId,
                        PlayerId = player.Id,
                        Score = player.Sets != null
                                    ? player.Sets.Count
                                    : 0
                    });
                }

                setRepo.UpdateGame(dbGame);
            }
        }

        private void ShowScore(Game game)
        {
            if (game != null)
            {
                var players = new List<PlayerViewModel>();

                if (game.PlayerGames != null)
                {
                    foreach (var pg in game.PlayerGames)
                    {
                        var pvm = new PlayerViewModel()
                        {
                            Id = pg.PlayerId,
                            Name = pg.Player.DisplayName,
                            Sets = new List<List<Card>>()
                        };

                        for (int i = 0; i < pg.Score; i++)
                        {
                            var cardList = new List<Card>()
                                            {
                                                new Card(),
                                                new Card(),
                                                new Card()
                                            };
                            pvm.Sets.Add(cardList);
                        }

                        players.Add(pvm);
                    }
                }

                Clients.Caller.displayFinishedGame(players, game.TotalSeconds);
            }
        }
    }
}