using AventStack.ExtentReports;
using OpenQA.Selenium;
using StAutomationProject.Utilities;
using NUnit.Framework;

namespace StAutomationProject.Tests
{
    public abstract class TestBase
    {
        protected IWebDriver Driver;
        protected ExtentTest Test;
        private readonly string _browser;

        protected TestBase(string browser)
        {
            _browser = browser;
        }

        [OneTimeSetUp]
        public static void OneTimeSetUp()
        {
            Console.WriteLine("Initializing ExtentReports...");
            ExtentReport.InitReport();
            Console.WriteLine("ExtentReports initialized.");
        }

        [OneTimeTearDown]
        public static void OneTimeTearDown()
        {
            Console.WriteLine("Flushing ExtentReports...");
            ExtentReport.FlushReport();
            Console.WriteLine("ExtentReports flushed.");
        }

        [SetUp]
        public void Setup()
        {
            Driver = DriverFactory.InitDriver(_browser);
            Driver.Navigate().GoToUrl(ConfigReader.GetBaseUrl());
            Test = ExtentReport.CreateTest($"{TestContext.CurrentContext.Test.Name} ({_browser})");
        }
        [OneTimeSetUp]
        public void GlobalSetup()
        {
            ExtentReport.InitReport(); // ← bạn cần có dòng gọi này
        }

        [OneTimeTearDown]
        public void GlobalTeardown()
        {
            ExtentReport.FlushReport(); // ← nếu thiếu dòng này thì sẽ không có file report
        }
        [TearDown]
        public void TearDown()
        {
            try
            {
                Console.WriteLine("Logging test result...");
                ExtentReport.LogTestResult(Test, TestContext.CurrentContext, Driver);
                Driver?.Quit();
                Console.WriteLine("Test completed.");
            }
            finally
            {
                Driver?.Dispose();
                Driver = null;
            }
        }
    }
}