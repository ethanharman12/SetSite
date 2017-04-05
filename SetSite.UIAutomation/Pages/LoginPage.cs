using System;
using OpenQA.Selenium;

namespace SetSite.UIAutomation.Pages
{
    public class LoginPage
    {
        public static bool IsAt
        {
            get
            {
                var header = Driver.Instance.FindElement(By.TagName("h2"));
                return header != null && header.Text == "Log in.";
            }
        }

        public static void GoTo()
        {
            Driver.Instance.FindElement(By.Id("loginlink")).Click();
        }

        public static void Login()
        {
            Driver.Instance.FindElement(By.Id("Email")).SendKeys("automated@test.com");
            Driver.Instance.FindElement(By.Id("Password")).SendKeys("password");
            Driver.Instance.FindElement(By.Id("LoginSubmit")).Click();
        }
    }
}
