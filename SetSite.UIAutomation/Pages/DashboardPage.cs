namespace SetSite.UIAutomation.Pages
{
    public static class DashboardPage
    {
        public static void GoTo()
        {
            Driver.Instance.Navigate().GoToUrl("https://localhost:44301/");
        }
    }
}
