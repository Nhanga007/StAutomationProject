using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace StAutomationProject.Utilities
{
    public static class ConfigReader
    {
        private static IConfiguration _config;

        static ConfigReader()
        {
            try
            {
                var basePath = Directory.GetCurrentDirectory();
                var configPath = Path.Combine(basePath, "Configurations", "appsettings.json");

                Console.WriteLine($"Config base path: {basePath}");
                Console.WriteLine($"Config file path: {configPath}");
                Console.WriteLine($"Config file exists: {File.Exists(configPath)}");

                _config = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile("Configurations/appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                Console.WriteLine("Configuration loaded successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading configuration: {ex.Message}");
                throw;
            }
        }

        public static string GetBaseUrl()
        {
            try
            {
                var baseUrl = _config["TestSettings:BaseUrl"];
                if (string.IsNullOrEmpty(baseUrl))
                {
                    Console.WriteLine("BaseUrl not found in config, using default");
                    return "https://magento.softwaretestingboard.com";
                }
                return baseUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting BaseUrl: {ex.Message}");
                return "https://magento.softwaretestingboard.com";
            }
        }

        public static int GetTimeout()
        {
            try
            {
                var timeoutString = _config["TestSettings:Timeout"];
                if (string.IsNullOrEmpty(timeoutString))
                {
                    Console.WriteLine("Timeout not found in config, using default");
                    return 30;
                }

                if (int.TryParse(timeoutString, out int timeout))
                {
                    return timeout;
                }
                else
                {
                    Console.WriteLine($"Invalid timeout value: {timeoutString}, using default");
                    return 30;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting Timeout: {ex.Message}");
                return 30;
            }
        }

        public static List<string> GetSupportedBrowsers()
        {
            try
            {
                var browsers = _config.GetSection("Browsers:SupportedBrowsers").Get<List<string>>();
                if (browsers == null || browsers.Count == 0)
                {
                    Console.WriteLine("SupportedBrowsers not found in config, using default [Chrome, Edge]");
                    return new List<string> { "Chrome", "Edge" };
                }
                return browsers;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting SupportedBrowsers: {ex.Message}");
                return new List<string> { "Chrome", "Edge" };
            }
        }

        public static int GetImplicitWait()
        {
            try
            {
                var waitString = _config["DriverSettings:ImplicitWait"];
                if (string.IsNullOrEmpty(waitString))
                {
                    Console.WriteLine("ImplicitWait not found in config, using default");
                    return 10;
                }

                if (int.TryParse(waitString, out int wait))
                {
                    return wait;
                }
                else
                {
                    Console.WriteLine($"Invalid ImplicitWait value: {waitString}, using default");
                    return 10;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting ImplicitWait: {ex.Message}");
                return 10;
            }
        }

        public static int GetPageLoadTimeout()
        {
            try
            {
                var timeoutString = _config["DriverSettings:PageLoadTimeout"];
                if (string.IsNullOrEmpty(timeoutString))
                {
                    Console.WriteLine("PageLoadTimeout not found in config, using default");
                    return 30;
                }

                if (int.TryParse(timeoutString, out int timeout))
                {
                    return timeout;
                }
                else
                {
                    Console.WriteLine($"Invalid PageLoadTimeout value: {timeoutString}, using default");
                    return 30;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting PageLoadTimeout: {ex.Message}");
                return 30;
            }
        }

        public static string GetWindowSize()
        {
            try
            {
                var windowSize = _config["DriverSettings:WindowSize"];
                if (string.IsNullOrEmpty(windowSize))
                {
                    Console.WriteLine("WindowSize not found in config, using default");
                    return "maximize";
                }
                return windowSize;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting WindowSize: {ex.Message}");
                return "maximize";
            }
        }
        public static string GetReportPath()
        {
            try
            {
                var path = _config["ReportSettings:ReportPath"];
                if (string.IsNullOrEmpty(path))
                {
                    Console.WriteLine("ReportPath not found in config, using default");
                    return "Reports";
                }
                return path;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting ReportPath: {ex.Message}");
                return "Reports";
            }
        }

        public static string GetReportName()
        {
            try
            {
                var name = _config["ReportSettings:ReportName"];
                if (string.IsNullOrEmpty(name))
                {
                    Console.WriteLine("ReportName not found in config, using default");
                    return "AutomationTestReport";
                }
                return name;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting ReportName: {ex.Message}");
                return "AutomationTestReport";
            }
        }
        public static void DebugConfiguration()
        {
            Console.WriteLine("=== Configuration Debug ===");
            Console.WriteLine($"BaseUrl: {GetBaseUrl()}");
            Console.WriteLine($"Timeout: {GetTimeout()}");
            Console.WriteLine($"SupportedBrowsers: {string.Join(", ", GetSupportedBrowsers())}");
            Console.WriteLine($"ImplicitWait: {GetImplicitWait()}");
            Console.WriteLine($"PageLoadTimeout: {GetPageLoadTimeout()}");
            Console.WriteLine($"WindowSize: {GetWindowSize()}");
            Console.WriteLine("=== End Debug ===");
        }
    }
}