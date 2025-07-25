using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Remote;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using System;

namespace StAutomationProject.Utilities
{
    public static class DriverFactory
    {
        private static readonly string UBlockExtensionChromePath = @"D:\uBlock\uBlockChrome";
        private static readonly string UBlockExtensionEdgePath = @"D:\uBlock\uBlockEdge";
        private static readonly string SeleniumHubUrl = "http://localhost:4444/wd/hub"; 

        public static IWebDriver InitDriver(string browserName, bool useSeleniumHub = false)
        {
            IWebDriver driver;

            if (useSeleniumHub)
            {
                switch (browserName.ToLower())
                {
                    case "chrome":
                        try
                        {
                            Console.WriteLine("Setting up RemoteWebDriver for Chrome...");
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

                            driver = new RemoteWebDriver(new Uri(SeleniumHubUrl), chromeOptions);
                            Console.WriteLine("RemoteWebDriver for Chrome initialized successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error initializing RemoteWebDriver for Chrome: {ex.Message}");
                            throw;
                        }
                        break;

                    case "edge":
                        try
                        {
                            Console.WriteLine("Setting up RemoteWebDriver for Edge...");
                            var edgeOptions = new EdgeOptions();
                            edgeOptions.AddArgument("--disable-notifications");
                            edgeOptions.AddArgument("--disable-popup-blocking");
                            edgeOptions.AddArgument("--disable-blink-features=AutomationControlled");
                            edgeOptions.AddArgument("--headless");
                            edgeOptions.AddArgument("--no-sandbox");
                            edgeOptions.AddArgument("--disable-dev-shm-usage");

                            driver = new RemoteWebDriver(new Uri(SeleniumHubUrl), edgeOptions);
                            Console.WriteLine("RemoteWebDriver for Edge initialized successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error initializing RemoteWebDriver for Edge: {ex.Message}");
                            throw;
                        }
                        break;

                    default:
                        throw new ArgumentException($"Browser '{browserName}' is not supported.");
                }
            }
            else
            {
                switch (browserName.ToLower())
                {
                    case "chrome":
                        try
                        {
                            Console.WriteLine("Setting up Chrome driver...");
                            new DriverManager().SetUpDriver(new ChromeConfig());

                            var chromeOptions = new ChromeOptions();
                            chromeOptions.AddArgument("--disable-notifications");
                            chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
                            chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
                            chromeOptions.AddArgument("--disable-popup-blocking");
                            chromeOptions.AddArgument("--disable-infobars");
                            chromeOptions.AddUserProfilePreference("autofill.profile_enabled", false);
                            chromeOptions.AddUserProfilePreference("autofill.credit_card_enabled", false);
                            chromeOptions.AddArgument("--disable-blink-features=AutomationControlled");
                            chromeOptions.AddExcludedArgument("enable-automation");
                            chromeOptions.AddAdditionalOption("useAutomationExtension", false);
                            chromeOptions.AddArgument($"--load-extension={UBlockExtensionChromePath}");

                            driver = new ChromeDriver(chromeOptions);
                            Console.WriteLine("Chrome driver initialized successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error initializing Chrome driver: {ex.Message}");
                            throw;
                        }
                        break;

                    case "edge":
                        try
                        {
                            Console.WriteLine("Setting up Edge driver...");
                            new DriverManager().SetUpDriver(new EdgeConfig());
                            var edgeOptions = new EdgeOptions();
                            edgeOptions.AddArgument("--disable-notifications");
                            edgeOptions.AddArgument("--disable-popup-blocking");
                            edgeOptions.AddArgument("--disable-blink-features=AutomationControlled");
                            edgeOptions.AddArgument($"--load-extension={UBlockExtensionEdgePath}");

                            driver = new EdgeDriver(edgeOptions);
                            Console.WriteLine("Edge driver initialized successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error initializing Edge driver: {ex.Message}");
                            throw;
                        }
                        break;

                    default:
                        throw new ArgumentException($"Browser '{browserName}' is not supported.");
                }
            }

            try
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(ConfigReader.GetImplicitWait());
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(ConfigReader.GetPageLoadTimeout());

                if (ConfigReader.GetWindowSize().ToLower() == "maximize")
                {
                    driver.Manage().Window.Maximize();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error configuring driver timeouts or window size: {ex.Message}");
                driver?.Quit();
                throw;
            }

            return driver;
        }
    }
}