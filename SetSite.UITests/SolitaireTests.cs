using Microsoft.VisualStudio.TestTools.UnitTesting;
using SetSite.UIAutomation.Enums;
using SetSite.UIAutomation.Pages;

namespace SetSite.UITests
{
    [TestClass]
    public class SolitaireTests : BaseTest
    {
        [TestMethod]
        public void Solitaire_FindSet_Easy()
        {
            DashboardPage.GoTo();
            SolitairePage.GoTo();

            SolitairePage.StartGame(SolitaireTypeEnum.Easy);

            Assert.IsTrue(SolitairePage.HasCards, "Cards not generated on start.");

            SolitairePage.ToggleShowNumberOfPossibleSets();

            Assert.IsTrue(SolitairePage.PossibleNumberOfSets > 0, "Number of possible sets not shown.");

            SolitairePage.ShowPossibleSet();
            SolitairePage.SelectPossibleSet();

            Assert.IsTrue(SolitairePage.FoundSetNumber > 0, "Found Set Count not updated.");
            Assert.IsTrue(SolitairePage.DisplayedSets > 0, "Found Set not showing in side panel.");

            Assert.IsTrue(SolitairePage.IsGameType(SolitaireTypeEnum.Easy), "Solitaire game is not Easy.");
        }

        [TestMethod]
        public void Solitaire_PauseGame_Hard()
        {
            DashboardPage.GoTo();
            SolitairePage.GoTo();

            SolitairePage.StartGame(SolitaireTypeEnum.Hard);

            Assert.IsTrue(SolitairePage.HasCards, "Cards not generated on start.");

            SolitairePage.CaptureTime();
            SolitairePage.PauseGame();

            Assert.IsFalse(SolitairePage.HasCards, "Cards not hidden after pause.");

            SolitairePage.ResumeGame();

            Assert.IsTrue(SolitairePage.PreviousTime < SolitairePage.GetCurrentTime(), "Timer has not progressed.");
            Assert.IsTrue(SolitairePage.IsGameType(SolitaireTypeEnum.Hard), "Solitaire game is not Hard.");
        }
    }
}
