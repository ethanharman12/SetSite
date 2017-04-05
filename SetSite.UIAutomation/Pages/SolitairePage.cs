using OpenQA.Selenium;
using SetSite.UIAutomation.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SetSite.UIAutomation.Pages
{
    public class SolitairePage
    {
        public static int DisplayedSets
        {
            get
            {
                var cards = Driver.Instance.FindElements(By.ClassName("foundRow"));
                return cards.Count;
            }
        }
        public static int FoundSetNumber
        {
            get
            {
                var count = Driver.Instance.FindElement(By.Id("setCount"));
                return int.Parse(count.Text);
            }
        }
        public static bool HasCards
        {
            get
            {
                var cards = Driver.Instance.FindElements(By.ClassName("card"));
                return cards.Count > 0;
            }
        }
        public static bool IsAt
        {
            get
            {
                var header = Driver.Instance.FindElement(By.TagName("h2"));
                return header != null && header.Text == "Solitaire";
            }
        }
        public static int PossibleNumberOfSets
        {
            get
            {
                var count = Driver.Instance.FindElement(By.Id("possibleSetsCount"));
                return int.Parse(count.Text);
            }
        }
        public static TimeSpan PreviousTime { get; set; }
        

        public static void CaptureTime()
        {
            Thread.Sleep(1100);
            PreviousTime = GetCurrentTime();
        }

        public static TimeSpan GetCurrentTime()
        {
            return TimeSpan.Parse(Driver.Instance.FindElement(By.Id("timePlayed")).Text);
        }

        public static void GoTo()
        {
            Driver.Instance.FindElement(By.Id("solitaire-link")).Click();
        }

        public static bool IsGameType(SolitaireTypeEnum gameType)
        {
            var hardStyles = new List<string> { "rgb(255, 255, 255)", "url(\"#Gradientred\")", "url(\"#Gradientgreen\")", "url(\"#Gradientblue\")" };
            var cards = Driver.Instance.FindElements(By.ClassName("card"));

            var hasHardCards = cards.Any(card => hardStyles.Contains(card.FindElement(By.XPath("child::*")).GetCssValue("fill")));

            return (gameType == SolitaireTypeEnum.Easy && !hasHardCards)
                || (gameType == SolitaireTypeEnum.Hard && hasHardCards);
        }
        
        public static void PauseGame()
        {
            Driver.Instance.FindElement(By.Id("pauseButton")).Click();
        }

        public static void ResumeGame()
        {
            Driver.Instance.FindElement(By.Id("startButton")).Click();
            Thread.Sleep(1100);
        }

        public static void SelectPossibleSet()
        {
            Driver.Instance.FindElement(By.Id("showSolButton")).Click();
            var set = Driver.Instance.FindElements(By.ClassName("preview"));

            foreach(var card in set)
            {
                card.Click();
            }
        }

        public static void ShowPossibleSet()
        {
            Driver.Instance.FindElement(By.Id("showSolButton")).Click();
        }

        public static void StartGame(SolitaireTypeEnum gameType)
        {
            if (gameType == SolitaireTypeEnum.Easy)
            {
                Driver.Instance.FindElement(By.Id("easyButton")).Click();
            }
            else
            {
                Driver.Instance.FindElement(By.Id("hardButton")).Click();
            }
        }

        public static void ToggleShowNumberOfPossibleSets()
        {
            Driver.Instance.FindElement(By.Id("possibleSetsCheck")).Click();
        }
    }
}
