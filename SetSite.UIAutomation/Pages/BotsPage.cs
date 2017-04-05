using OpenQA.Selenium;

namespace SetSite.UIAutomation.Pages
{
    public class BotsPage
    {
        public static bool IsAt
        {
            get
            {
                var header = Driver.Instance.FindElement(By.TagName("h2"));
                return header != null && header.Text == "Bots";
            }
        }

        public static void GoTo()
        {
            Driver.Instance.FindElement(By.Id("bots-link")).Click();
        }
    }
}
