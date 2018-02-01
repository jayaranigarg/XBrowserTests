using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System;

namespace SeleniumSample
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [DeploymentItem(@"..\..\Drivers\chromedriver.exe")]
        public void ChromeTest()
        {
            var chromeDriver = new ChromeDriver();
            chromeDriver.Navigate().GoToUrl("http://www.google.com");
            chromeDriver.FindElement(By.Id("lst-ib")).SendKeys("Hello");
            var filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()) + ".png";
            var screenshot = chromeDriver.GetScreenshot();
            screenshot.SaveAsFile(filePath);
            TestContext.AddResultFile(filePath);
            chromeDriver.FindElement(By.Id("lst-ib")).SendKeys(Keys.Enter);
            chromeDriver.Quit();
        }

        [TestMethod]
        [DeploymentItem(@"..\..\Drivers\geckodriver.exe")]
        public void FirefoxTest()
        {
            var firefoxDriver = new FirefoxDriver();
            firefoxDriver.Navigate().GoToUrl("http://www.google.com");
            firefoxDriver.FindElement(By.Id("lst-ib")).SendKeys("Hello");
            var filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()) + ".png";
            Console.WriteLine(filePath);
            var screenshot = firefoxDriver.GetScreenshot();
            screenshot.SaveAsFile(filePath, ScreenshotImageFormat.Png);
            TestContext.AddResultFile(filePath);
            firefoxDriver.FindElement(By.Id("lst-ib")).SendKeys(Keys.Enter);
            firefoxDriver.Quit();
        }

        [TestMethod]
        [DeploymentItem(@"..\..\Drivers\IEDriverServer.exe")]
        public void IETest()
        {
            var iedriver = new InternetExplorerDriver();
            iedriver.Navigate().GoToUrl("http://www.google.com");
            iedriver.FindElement(By.Id("lst-ib")).SendKeys("Hello");
            var filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()) + ".png";
            var screenshot = iedriver.GetScreenshot();
            screenshot.SaveAsFile(filePath);
            TestContext.AddResultFile(filePath);
            iedriver.FindElement(By.Id("lst-ib")).SendKeys(Keys.Enter);
            iedriver.Quit();
        }
        public TestContext TestContext { get; set; }
    }
}
