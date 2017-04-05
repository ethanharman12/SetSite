using Microsoft.VisualStudio.TestTools.UnitTesting;
using SetSite.UIAutomation.Pages;

namespace SetSite.UITests
{
    [TestClass]
    public class SmokeTests : BaseTest
    {
        [TestMethod]
        public void Navigate_To_Bots()
        {
            DashboardPage.GoTo();
            BotsPage.GoTo();

            Assert.IsTrue(BotsPage.IsAt, "Did not navigate to Bots Page");
        }

        [TestMethod]
        public void Navigate_To_Multiplayer()
        {
            DashboardPage.GoTo();
            MultiplayerPage.GoTo();

            Assert.IsTrue(LoginPage.IsAt, "Did not navigate to Login Page");

            LoginPage.Login();

            Assert.IsTrue(MultiplayerPage.IsAt, "Did not navigate to Multiplayer Page");
        }

        [TestMethod]
        public void Navigate_To_Rules()
        {
            DashboardPage.GoTo();
            RulesPage.GoTo();

            Assert.IsTrue(RulesPage.IsAt, "Did not navigate to Rules Page");
        }

        [TestMethod]
        public void Navigate_To_Solitaire()
        {
            DashboardPage.GoTo();
            SolitairePage.GoTo();

            Assert.IsTrue(SolitairePage.IsAt, "Did not navigate to Solitaire Page");
        }
    }
}
