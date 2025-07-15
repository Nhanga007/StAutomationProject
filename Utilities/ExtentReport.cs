using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using StAutomationProject.Utilities;
using System;
using System.IO;

namespace Utilities
{
    public static class ExtentReport
    {
        private static ExtentReports _extent;
        private static ExtentTest _test;

        public static void InitReport()
        {
            var projectDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."));
            var reportsDir = Path.Combine(projectDir, "Reports");
            if (!Directory.Exists(reportsDir))
                Directory.CreateDirectory(reportsDir);

            var reportPath = Path.Combine(reportsDir, $"TestReport_{DateTime.Now:yyyyMMdd_HHmmss}.html");

            var htmlReporter = new ExtentHtmlReporter(reportPath);
            _extent = new ExtentReports();
            _extent.AttachReporter(htmlReporter);

            _extent.AddSystemInfo("Environment", "QA");
            _extent.AddSystemInfo("Browser", ConfigReader.GetBrowser());
            _extent.AddSystemInfo("URL", ConfigReader.GetBaseUrl());
        }

        public static ExtentTest CreateTest(string testName, string description = "")
        {
            _test = _extent.CreateTest(testName, description);
            return _test;
        }

        public static void LogTestResult(TestContext context, IWebDriver driver = null)
        {
            if (context.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                _test.Fail(context.Result.Message);
                if (driver != null)
                {
                    var screenshotPath = CaptureScreenshot(driver, context.Test.Name);
                    _test.AddScreenCaptureFromPath(screenshotPath);
                }
            }
            else if (context.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Passed)
            {
                _test.Pass("Test passed");
            }
        }

        public static void FlushReport()
        {
            _extent.Flush();
        }

        private static string CaptureScreenshot(IWebDriver driver, string testName)
        {
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            var screenshotPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", $"{testName}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            screenshot.SaveAsFile(screenshotPath);
            return screenshotPath;
        }
    }
}