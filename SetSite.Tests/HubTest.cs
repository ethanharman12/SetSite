using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SetSite.Models;
using SetSite.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SetSite.Tests
{
    [TestClass]
    public class HubTest
    {
        [TestCleanup]
        public void CleanUp()
        {
            MultiplayerGameHub.games = new Dictionary<string, SetGame>();
        }

        [TestMethod]
        public void CreateGame()
        {
            var clientGameId = 0;

            // Arrange
            var mockRepo = new MockSetRepository();
            MultiplayerGameHub hub = new MultiplayerGameHub(mockRepo);
            var mockClients = new Mock<IHubCallerConnectionContext<dynamic>>();
            hub.Clients = mockClients.Object;
            dynamic all = new ExpandoObject();
            all.gameAdded = new Action<int>((id) =>
            {
                clientGameId = id;
            });
            mockClients.Setup(m => m.All).Returns((ExpandoObject)all);

            // Act
            var result = hub.CreateGame();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(mockRepo.FindGame(result));
            Assert.AreEqual(1, clientGameId);
        }

        [TestMethod]
        public void GetCurrentGames()
        {
            // Arrange
            var mockRepo = new MockSetRepository();
            MultiplayerGameHub hub = new MultiplayerGameHub(mockRepo);
            MultiplayerGameHub.games.Add("Game5", new SetGame());

            // Act
            var result = hub.GetCurrentGames();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(5, result[0]);
        }

        [TestMethod]
        public void JoinGame_New()
        {
            bool playerJoined = false;
            double timeSet = -1;

            // Arrange
            var mockRepo = new MockSetRepository();
            mockRepo.Users.Add(new ApplicationUser() { Id = 2, DisplayName = "TestPlayer" });
            MultiplayerGameHub hub = new MultiplayerGameHub(mockRepo);
            var setGame = new SetGame();
            MultiplayerGameHub.games.Add("Game6", setGame);

            var mockGroupManager = new Mock<IGroupManager>();
            hub.Groups = mockGroupManager.Object;

            var claim = new Claim("test", "2");
            var mockIdentity =
                Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);
            var mockContext = Mock.Of<HubCallerContext>(cc => cc.User.Identity == mockIdentity && cc.ConnectionId == "1");
            hub.Context = mockContext;

            var mockClients = new Mock<IHubCallerConnectionContext<dynamic>>();
            hub.Clients = mockClients.Object;
            dynamic all = new ExpandoObject();
            all.playerJoin = new Action<PlayerViewModel>((pvm) =>
            {
                playerJoined = true;
            });
            all.setTime = new Action<double>((sec) =>
            {
                timeSet = sec;
            });
            mockClients.Setup(m => m.All).Returns((ExpandoObject)all);
            mockClients.Setup(m => m.Caller).Returns((ExpandoObject)all);
            mockClients.Setup(m => m.Others).Returns((ExpandoObject)all);
            mockClients.Setup(m => m.OthersInGroup(It.IsAny<string>())).Returns((ExpandoObject)all);

            // Act
            hub.JoinGame(6).Wait();

            // Assert
            Assert.IsTrue(playerJoined);
            Assert.IsTrue(setGame.Players.Count == 1);
            Assert.AreEqual(0, timeSet);
        }

        [TestMethod]
        public void JoinGame_RejoinGame()
        {
            GameState currentState = new GameState();
            bool isPaused = false;
            bool playerJoined = false;
            bool playerRejoined = false;
            double timeSet = -1;
            List<List<Card>> playerSets = null;            

            // Arrange
            var mockRepo = new MockSetRepository();
            mockRepo.Users.Add(new ApplicationUser() { Id = 2, DisplayName = "TestPlayer" });
            MultiplayerGameHub hub = new MultiplayerGameHub(mockRepo);
            var setGame = new SetGame();
            setGame.Players.Add(new PlayerViewModel() { Id = 2, Name = "TestPlayer", Sets = new List<List<Card>>() });
            setGame.IsPaused = true;
            setGame.StateId = 1;
            MultiplayerGameHub.games.Add("Game6", setGame);

            var mockGroupManager = new Mock<IGroupManager>();
            hub.Groups = mockGroupManager.Object;

            var claim = new Claim("test", "2");
            var mockIdentity =
                Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);
            var mockContext = Mock.Of<HubCallerContext>(cc => cc.User.Identity == mockIdentity && cc.ConnectionId == "1");
            hub.Context = mockContext;

            var mockClients = new Mock<IHubCallerConnectionContext<dynamic>>();
            hub.Clients = mockClients.Object;
            dynamic all = new ExpandoObject();
            all.playerJoin = new Action<PlayerViewModel>((pvm) =>
            {
                playerJoined = true;
            });
            all.playerReconnected = new Action<int>((id) =>
            {
                playerRejoined = true;
            });
            all.setTime = new Action<double>((sec) =>
            {
                timeSet = sec;
            });
            all.sendSets = new Action<List<List<Card>>>((sets) =>
            {
                playerSets = sets;
            });
            all.startNextState = new Action<GameState>((state) =>
            {
                currentState = state;
            });
            all.pauseGame = new Action(() =>
            {
                isPaused = true;
            });

            mockClients.Setup(m => m.All).Returns((ExpandoObject)all);
            mockClients.Setup(m => m.Caller).Returns((ExpandoObject)all);
            mockClients.Setup(m => m.Others).Returns((ExpandoObject)all);
            mockClients.Setup(m => m.OthersInGroup(It.IsAny<string>())).Returns((ExpandoObject)all);

            // Act
            hub.JoinGame(6).Wait();

            // Assert
            Assert.IsFalse(playerJoined);
            Assert.IsTrue(playerRejoined);
            Assert.IsTrue(isPaused);
            Assert.AreEqual(1, currentState.id);
            Assert.IsNotNull(playerSets);
            Assert.IsTrue(setGame.Players.Count == 1);
            Assert.AreEqual(0, timeSet);
        }

        [TestMethod]
        public void JoinGame_SpectateGame()
        {
            GameState currentState = new GameState();
            bool isPaused = false;
            bool playerJoined = false;
            bool playerRejoined = false;
            double timeSet = -1;
            List<List<Card>> playerSets = null;

            // Arrange
            var mockRepo = new MockSetRepository();
            mockRepo.Users.Add(new ApplicationUser() { Id = 2, DisplayName = "TestPlayer" });
            MultiplayerGameHub hub = new MultiplayerGameHub(mockRepo);
            var setGame = new SetGame();
            setGame.Players.Add(new PlayerViewModel() { Id = 3, Name = "TestPlayer2", Sets = new List<List<Card>>() });
            setGame.IsPaused = true;
            setGame.StateId = 1;
            MultiplayerGameHub.games.Add("Game6", setGame);

            var mockGroupManager = new Mock<IGroupManager>();
            hub.Groups = mockGroupManager.Object;

            var claim = new Claim("test", "2");
            var mockIdentity =
                Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);
            var mockContext = Mock.Of<HubCallerContext>(cc => cc.User.Identity == mockIdentity && cc.ConnectionId == "1");
            hub.Context = mockContext;

            var mockClients = new Mock<IHubCallerConnectionContext<dynamic>>();
            hub.Clients = mockClients.Object;
            dynamic all = new ExpandoObject();
            all.playerJoin = new Action<PlayerViewModel>((pvm) =>
            {
                playerJoined = true;
            });
            all.playerReconnected = new Action<int>((id) =>
            {
                playerRejoined = true;
            });
            all.setTime = new Action<double>((sec) =>
            {
                timeSet = sec;
            });
            all.sendSets = new Action<List<List<Card>>>((sets) =>
            {
                playerSets = sets;
            });
            all.startNextState = new Action<GameState>((state) =>
            {
                currentState = state;
            });
            all.spectate = new Action<GameState>((state) =>
            {
                currentState = state;
            });
            all.pauseGame = new Action(() =>
            {
                isPaused = true;
            });

            mockClients.Setup(m => m.All).Returns((ExpandoObject)all);
            mockClients.Setup(m => m.Caller).Returns((ExpandoObject)all);
            mockClients.Setup(m => m.Others).Returns((ExpandoObject)all);
            mockClients.Setup(m => m.OthersInGroup(It.IsAny<string>())).Returns((ExpandoObject)all);

            // Act
            hub.JoinGame(6).Wait();

            // Assert
            Assert.IsTrue(playerJoined);
            Assert.IsFalse(playerRejoined);
            Assert.IsTrue(isPaused);
            Assert.AreEqual(1, currentState.id);
            Assert.IsNull(playerSets);
            Assert.IsTrue(setGame.Players.Count == 1);
            Assert.AreEqual(0, timeSet);
        }

        [TestMethod]
        public void JoinGame_ShowFinishedScore()
        {
            double timeSet = -1;
            List<List<Card>> playerSets = null;

            // Arrange
            var mockRepo = new MockSetRepository();
            mockRepo.Users.Add(new ApplicationUser() { Id = 2, DisplayName = "TestPlayer" });

            var game = new Game();
            game.Id = 6;
            game.TotalSeconds = 25;
            game.PlayerGames = new List<PlayerGame>();
            game.PlayerGames.Add(new PlayerGame() { 
                Id = 1,
                GameId = 6, 
                PlayerId = 2, 
                Score = 5, 
                Player = new ApplicationUser() { DisplayName = "Player2" } 
            });

            mockRepo.Games.Add(game);
            MultiplayerGameHub hub = new MultiplayerGameHub(mockRepo);

            var mockGroupManager = new Mock<IGroupManager>();
            hub.Groups = mockGroupManager.Object;

            var claim = new Claim("test", "2");
            var mockIdentity =
                Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);
            var mockContext = Mock.Of<HubCallerContext>(cc => cc.User.Identity == mockIdentity && cc.ConnectionId == "1");
            hub.Context = mockContext;

            var mockClients = new Mock<IHubCallerConnectionContext<dynamic>>();
            hub.Clients = mockClients.Object;
            dynamic all = new ExpandoObject();
            all.displayFinishedGame = new Action<List<PlayerViewModel>, double?>((players, time) =>
            {
                playerSets = players[0].Sets;
                timeSet = time.Value;
            });

            mockClients.Setup(m => m.Caller).Returns((ExpandoObject)all);

            // Act
            hub.JoinGame(6).Wait();

            // Assert
            Assert.IsNotNull(playerSets);
            Assert.AreEqual(5, playerSets.Count);
            Assert.AreEqual(25, timeSet);
        }
    }
}
