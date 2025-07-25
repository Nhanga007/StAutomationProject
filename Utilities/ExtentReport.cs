using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using StAutomationProject.Utilities;
using System;
using System.IO;

namespace StAutomationProject.Utilities
{
    public static class ExtentReport
    {
        private static ExtentReports _extent;
        private static string _reportPath;

        public static void InitReport()
        {
            var projectDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."));
            var reportsDir = Path.Combine(projectDir, "Reports");
            if (!Directory.Exists(reportsDir))
                Directory.CreateDirectory(reportsDir);

            var fileName = $"TestReport_{DateTime.Now:yyyyMMdd_HHmmss}.html";
            _reportPath = Path.Combine(reportsDir, fileName);

            var htmlReporter = new ExtentHtmlReporter(_reportPath);

            htmlReporter.Config.DocumentTitle = "Test Automation Report";
            htmlReporter.Config.ReportName = "Test Results";
            htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Standard;

            _extent = new ExtentReports();
            _extent.AttachReporter(htmlReporter);
            _extent.AddSystemInfo("Environment", "QA");
            _extent.AddSystemInfo("URL", ConfigReader.GetBaseUrl());
        }

        public static ExtentTest CreateTest(string testName, string description = "")
        {
            return _extent.CreateTest(testName, description);
        }

        public static void LogTestResult(ExtentTest test, TestContext context, IWebDriver driver = null)
        {
            if (context.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                test.Fail(context.Result.Message);
                if (driver != null)
                {
                    var screenshotPath = CaptureScreenshot(driver, context.Test.Name);
                    test.AddScreenCaptureFromPath(screenshotPath);
                }
            }
            else if (context.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Passed)
            {
                test.Pass("Test passed");
            }
        }

        public static void FlushReport()
        {
            _extent.Flush();
            RenameReportFile();
        }

        private static void RenameReportFile()
        {
            try
            {
                var directory = Path.GetDirectoryName(_reportPath);
                var indexPath = Path.Combine(directory, "index.html");

                if (File.Exists(indexPath) && !File.Exists(_reportPath))
                {
                    File.Move(indexPath, _reportPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error renaming report file: {ex.Message}");
            }
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