using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using TestSeleniumSample.Framework;

namespace TestSeleniumSample
{
    class Program
    {
        public static IWebDriver chromeDriver = null;

        [TestCase("Demirel", "Demirel - Güncel Demirel Haberi")]
        [TestCase("Galatasaray", "Galatasaray Haberleri")]
        public static void CheckSearchFunction(string input, string expected)
        {
            using (chromeDriver = new ChromeDriver())
            {
                chromeDriver.Navigate().GoToUrl("http://www.hurriyet.com.tr");
                chromeDriver.FindElement(By.ClassName("main-sub-menu-trigger")).Click();
                chromeDriver.FindElement(By.Id("search-box-wrapper-alt")).Click();
                chromeDriver.FindElement(By.ClassName("search-box-text-input")).SendKeys(input);
                chromeDriver.FindElement(By.ClassName("invisible-search-button")).Click();
                Assert.AreEqual(chromeDriver.Title, expected);
            }
        }

        [Test]
        public static void CheckSponsoredBoxViaNavigation()
        {
            using (chromeDriver = new ChromeDriver())
            {
                chromeDriver.Navigate().GoToUrl("http://www.hurriyet.com.tr");
                IWebElement element = chromeDriver.FindElement(By.XPath("/html/body/main/div/div/div[2]/div[1]/div/a"));
                element.Click();
                chromeDriver.SwitchTo().Window(chromeDriver.WindowHandles.ElementAtOrDefault(1));
                Page.CloseInterstitialAd(chromeDriver);
                var plusNewsSelector = By.XPath("//*[@id=\"plusNewsListRightBlock\"]/div/ul/li[1]/a");
                IWebElement plusNews = chromeDriver.FindElement(plusNewsSelector);
                plusNews.Click();
                chromeDriver.SwitchTo().Window(chromeDriver.WindowHandles.ElementAtOrDefault(2));
                IWebElement sponsorDivision = chromeDriver.FindElement(By.CssSelector("div.sponsor"));
                var element3Text = sponsorDivision.Text;
                Assert.AreEqual(element3Text.ToLowerInvariant(), "sponsor");
                chromeDriver.Close();
            }
        }

        [Test]
        public static void CheckWeatherWidget()
        {
            using (chromeDriver = new ChromeDriver())
            {
                chromeDriver.Navigate().GoToUrl("http://www.hurriyet.com.tr");
                IWebElement cityName = chromeDriver.FindElement(By.CssSelector(".city-name"));
                ((IJavaScriptExecutor)chromeDriver).ExecuteScript("arguments[0].click();", cityName);
                IWebElement searchBox = chromeDriver.FindElement(By.XPath("/html/body/main/div/div/div[4]/div[2]/div/div[3]"));
                var displayValue = searchBox.GetCssValue("display");
                Assert.AreEqual(displayValue, "block");
                IWebElement searchText = chromeDriver.FindElement(By.XPath("//*[@id=\"hur-city-search\"]"));
                searchText.SendKeys("bur");
                var citySelectionList = chromeDriver.FindElements(By.CssSelector("ul.city-list-content li"));
                int listSize = citySelectionList.Count(x => x.Displayed);
                Assert.AreEqual(listSize, 3);
                IWebElement selectedListItem = chromeDriver.FindElement(By.XPath("//*[@id=\"15\"]/a"));
                selectedListItem.Click();
                var wait = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(3));
                var result = wait.Until(ExpectedConditions.TextToBePresentInElement(chromeDriver.FindElement(By.CssSelector(".city-name")), "Burdur"));
                Assert.IsTrue(result);
            }
        }

        [TearDown]
        public static void AfterAllTestsAreCompleted()
        {
            chromeDriver.Quit();
        }

        [OneTimeTearDown]
        public static void AfterTestFunctionIsEnded()
        {
            chromeDriver.Close();
        }

        static void Main()
        {

        }
    }
}
