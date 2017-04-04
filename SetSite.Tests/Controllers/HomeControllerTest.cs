using Microsoft.VisualStudio.TestTools.UnitTesting;
using SetSite.Controllers;
using System.Web.Mvc;

namespace SetSite.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            HomeController controller = new HomeController();
            
            ViewResult result = controller.Index() as ViewResult;
            
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Solitaire()
        {
            HomeController controller = new HomeController();
            
            ViewResult result = controller.Solitaire() as ViewResult;
            
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Bots()
        {
            HomeController controller = new HomeController();
            
            ViewResult result = controller.Bots() as ViewResult;
            
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Rules()
        {
            HomeController controller = new HomeController();
            
            ViewResult result = controller.Rules() as ViewResult;
            
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void MultiplayerLobby()
        {
            HomeController controller = new HomeController();
            
            ViewResult result = controller.MultiplayerLobby() as ViewResult;
            
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Multiplayer()
        {
            HomeController controller = new HomeController();

            int id = 2;
            
            ViewResult result = controller.Multiplayer(id) as ViewResult;
            
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.ViewBag.gameId);
        }
    }
}
