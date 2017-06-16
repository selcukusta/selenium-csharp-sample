using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSeleniumSample.Framework
{
    public class Page
    {
        public static void CloseInterstitialAd(IWebDriver driver)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
            var adCloseButtonSelector = By.XPath("//*[@id=\"Medyanet_Ad_Models_Interstitial_CloseButton\"]/img");

            IWebElement adCloseButton = null;
            try
            {
                adCloseButton = wait.Until(ExpectedConditions.ElementToBeClickable(adCloseButtonSelector));
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Interstitial kapatma butonu yok ya da tiklanabilir degil!");
            }

            if (adCloseButton != null)
            {
                adCloseButton.Click();
            }
        }

        public static bool WaitForJsToLoad(IWebDriver driver)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            return wait.Until(JQueryLoad) && wait.Until(JavascriptLoad);
        }

        public static bool JQueryLoad(IWebDriver driver)
        {
            try
            {
                return (bool)((IJavaScriptExecutor)driver).ExecuteScript("return JQuery.active === 0");
            }
            catch
            {
                return true;
            }
        }

        public static bool JavascriptLoad(IWebDriver driver)
        {
            return (bool)((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState === \"complete\"");
        }
    }
}
