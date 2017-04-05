using OpenQA.Selenium;

namespace SetSite.UIAutomation.Pages
{
    public class RulesPage
    {
        public static bool IsAt
        {
            get
            {
                var header = Driver.Instance.FindElement(By.TagName("h2"));
                return header != null && header.Text == "Rules";
            }
        }

        public static void GoTo()
        {
            Driver.Instance.FindElement(By.Id("rules-link")).Click();
        }
    }
}
