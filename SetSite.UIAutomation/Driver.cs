using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace SetSite.UIAutomation
{
    public class Driver
    {
        public static IWebDriver Instance { get; set; }

        public static void Initialize()
        {
            Instance = new ChromeDriver();
            Instance.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
        }

        public static void Close()
        {
            if(Instance != null)
            {
                Instance.Close();
            }
        }
    }
}
