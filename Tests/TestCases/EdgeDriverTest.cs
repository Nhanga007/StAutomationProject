using OpenQA.Selenium;
using StAutomationProject.Utilities;
using NUnit.Framework;
using System;

namespace StAutomationProject.Tests
{
    [TestFixture]
    public class EdgeDriverTest
    {
        [Test]
        public void TestEdgeDriver()
        {
            Console.WriteLine("Starting EdgeDriverTest...");
            try
            {
                Console.WriteLine("Initializing Edge driver...");
                IWebDriver driver = DriverFactory.InitDriver("edge");
                Console.WriteLine("Navigating to Google...");
                driver.Navigate().GoToUrl("https://www.google.com");
                Assert.That(driver.Title.Contains("Google"), "Edge driver failed to navigate to Google.");
                Console.WriteLine("Test passed, closing driver...");
                driver.Quit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}