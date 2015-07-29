using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SetSite;
using SetSite.Controllers;

namespace SetSite.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Solitaire()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Solitaire() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Bots()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Bots() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Rules()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Rules() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void MultiplayerLobby()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.MultiplayerLobby() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Multiplayer()
        {
            // Arrange
            HomeController controller = new HomeController();

            int id = 2;

            // Act
            ViewResult result = controller.Multiplayer(id) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.ViewBag.gameId);
        }
    }
}
