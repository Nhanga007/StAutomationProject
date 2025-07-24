using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using System;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace StAutomationProject.Utilities
{
    public static class DriverFactory
    {
        private static readonly string seleniumHubUrl = "http://localhost:4444/wd/hub"; 

        public static IWebDriver InitDriver(string browserName)
        {
            IWebDriver driver;

            switch (browserName.ToLower())
            {
                case "chrome":
                    new DriverManager().SetUpDriver(new ChromeConfig());
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArgument("--disable-notifications");
                    chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
                    chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
                    chromeOptions.AddArgument("--disable-popup-blocking");
                    chromeOptions.AddArgument("--disable-infobars");
                    chromeOptions.AddArgument("--disable-extensions");
                    chromeOptions.AddUserProfilePreference("autofill.profile_enabled", false);
                    chromeOptions.AddUserProfilePreference("autofill.credit_card_enabled", false);
                    chromeOptions.AddArgument("--disable-blink-features=AutomationControlled");
                    chromeOptions.AddExcludedArgument("enable-automation");
                    chromeOptions.AddAdditionalOption("useAutomationExtension", false);
                    chromeOptions.AddArgument("--headless"); 
                    chromeOptions.AddArgument("--no-sandbox"); 
                    chromeOptions.AddArgument("--disable-dev-shm-usage"); 
                    driver = new RemoteWebDriver(new Uri(seleniumHubUrl), chromeOptions);
                    break;
                case "firefox":
                    new DriverManager().SetUpDriver(new FirefoxConfig());
                    var firefoxOptions = new FirefoxOptions();
                    driver = new RemoteWebDriver(new Uri(seleniumHubUrl), firefoxOptions); 
                    break;

                case "edge":
                    new DriverManager().SetUpDriver(new EdgeConfig());
                    var edgeOptions = new EdgeOptions();
                    driver = new RemoteWebDriver(new Uri(seleniumHubUrl), edgeOptions); 
                    break;
                default:
                    throw new ArgumentException($"Browser '{browserName}' is not supported.");
            }

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
            driver.Manage().Window.Maximize();

            return driver;
        }
    }
}