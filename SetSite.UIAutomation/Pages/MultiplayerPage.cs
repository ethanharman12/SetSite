using OpenQA.Selenium;

namespace SetSite.UIAutomation.Pages
{
    public class MultiplayerPage
    {
        public static bool IsAt
        {
            get
            {
                var header = Driver.Instance.FindElement(By.TagName("h2"));
                return header != null && header.Text == "Multiplayer Lobby";
            }
        }

        public static void GoTo()
        {
            Driver.Instance.FindElement(By.Id("multiplayer-link")).Click();
        }
    }
}
