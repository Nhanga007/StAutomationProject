using AventStack.ExtentReports;
using OpenQA.Selenium;
using StAutomationProject.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace StAutomationProject.Tests
{
    public abstract class TestBase
    {
        protected IWebDriver Driver;
        protected ExtentTest Test;

        [OneTimeSetUp]
        public void GlobalSetup()
        {
            Console.WriteLine("Initializing ExtentReports...");
            ExtentReport.InitReport();
            Console.WriteLine("ExtentReports initialized.");
        }
        [OneTimeSetUp]
        public static void OneTimeSetUp()
        {
            ExtentReport.Init();
        }

        [OneTimeTearDown]
        public static void OneTimeTearDown()
        {
            ExtentReport.Cleanup();
        }

        [SetUp]
        public void Setup()
        {
            Driver = DriverFactory.InitDriver("Chrome");
            Driver.Manage().Window.Maximize();
            Test = ExtentReport.CreateTest(TestContext.CurrentContext.Test.Name);
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                Console.WriteLine("Logging test result...");
                ExtentReport.LogTestResult(TestContext.CurrentContext, Driver);
                Driver?.Quit();
                Console.WriteLine("Test completed.");
            }
            finally
            {
                Driver?.Dispose();
                Driver = null;
            }
        }

        [OneTimeTearDown]
        public void GlobalTearDown()
        {
            Console.WriteLine("Flushing ExtentReports...");
            ExtentReport.FlushReport();
            Console.WriteLine("ExtentReports flushed.");
        }
    }
}