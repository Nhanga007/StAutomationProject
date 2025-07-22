using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using System;
using OpenQA.Selenium.Remote;

namespace StAutomationProject.Utilities
{
    public static class DriverFactory
    {
        public static IWebDriver InitDriver(string browserName)
        {
            IWebDriver driver;

            switch (browserName.ToLower())
            {
                case "chrome":
                    new DriverManager().SetUpDriver(new ChromeConfig());
                    var chromeOptions = new ChromeOptions();
                    // Tắt thông báo đẩy
                    chromeOptions.AddArgument("--disable-notifications");
                    // Tắt gợi ý lưu mật khẩu và tự động điền
                    chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
                    chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
                    // Tắt các popup và gợi ý khác
                    chromeOptions.AddArgument("--disable-popup-blocking");
                    chromeOptions.AddArgument("--disable-infobars");
                    chromeOptions.AddArgument("--disable-extensions");
                    // Tắt lưu địa chỉ
                    chromeOptions.AddUserProfilePreference("autofill.profile_enabled", false);
                    chromeOptions.AddUserProfilePreference("autofill.credit_card_enabled", false);
                    // Tắt dấu hiệu automation
                    chromeOptions.AddArgument("--disable-blink-features=AutomationControlled");
                    chromeOptions.AddExcludedArgument("enable-automation");
                    chromeOptions.AddAdditionalOption("useAutomationExtension", false);
                    chromeOptions.AddArgument("--headless"); // Chạy không giao diện (tốt cho CI)
                    chromeOptions.AddArgument("--no-sandbox"); // Cần cho Docker
                    chromeOptions.AddArgument("--disable-dev-shm-usage"); // Tối ưu hóa bộ nhớ trong container
                    driver = new RemoteWebDriver(new Uri(seleniumHubUrl), chromeOptions);
                    driver = new ChromeDriver(chromeOptions);
                    break;

                case "firefox":
                    new DriverManager().SetUpDriver(new FirefoxConfig());
                    var firefoxOptions = new FirefoxOptions();
                    driver = new FirefoxDriver(firefoxOptions);
                    break;

                case "edge":
                    new DriverManager().SetUpDriver(new EdgeConfig());
                    var edgeOptions = new EdgeOptions();
                    driver = new EdgeDriver(edgeOptions);
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