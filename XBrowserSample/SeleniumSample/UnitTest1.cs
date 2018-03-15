using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System;
using OpenQA.Selenium.Remote;

namespace SeleniumSample
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [DataRow("chrome")]
        [DataRow("firefox")]
        [DataRow("ie")]
        public void SearchPageTest(string browser)
        {
            var driver = GetDriver(browser);
            driver.Navigate().GoToUrl("http://www.google.com");
            driver.FindElement(By.Id("lst-ib")).SendKeys("Hello");
            var filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()) + ".png";
            var screenshot = driver.GetScreenshot();
            screenshot.SaveAsFile(filePath);
            TestContext.AddResultFile(filePath);
            driver.FindElement(By.Id("lst-ib")).SendKeys(Keys.Enter);
            driver.Quit();
        }
        
        #region private methods

        private RemoteWebDriver GetDriver(string browser)
        {
            switch (browser)
            {
                case "chrome":
                    return GetChromeDriver();
                case "firefox":
                    return GetFirefoxDriver();
                case "ie":
                default:
                    return GetIEDriver();
            }
        }

        private RemoteWebDriver GetChromeDriver()
        {
            var path = Environment.GetEnvironmentVariable("ChromeSeleniumDriverPath");
            if (!string.IsNullOrWhiteSpace(path))
            {
                return new ChromeDriver(path);
            }
            else
            {
                return new ChromeDriver();
            }
        }

        private RemoteWebDriver GetFirefoxDriver()
        {
            var path = Environment.GetEnvironmentVariable("GeckoSeleniumDriverPath");
            if (!string.IsNullOrWhiteSpace(path))
            {
                return new FirefoxDriver(path);
            }
            else
            {
                return new FirefoxDriver();
            }
        }

        private RemoteWebDriver GetIEDriver()
        {
            var path = Environment.GetEnvironmentVariable("IESeleniumDriverPath");
            if (!string.IsNullOrWhiteSpace(path))
            {
                return new InternetExplorerDriver(path);
            }
            else
            {
                return new InternetExplorerDriver();
            }
        }
        #endregion
        public TestContext TestContext { get; set; }
    }
}
