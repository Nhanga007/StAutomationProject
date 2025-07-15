using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                // Debug: In ra đường dẫn để kiểm tra
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
                    return "https://magento.softwaretestingboard.com"; // Default value
                }
                return baseUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting BaseUrl: {ex.Message}");
                return "https://magento.softwaretestingboard.com"; // Default value
            }
        }

        public static string GetBrowser()
        {
            try
            {
                var browser = _config["TestSettings:Browser"];
                if (string.IsNullOrEmpty(browser))
                {
                    Console.WriteLine("Browser not found in config, using default");
                    return "Chrome"; // Default value
                }
                return browser;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting Browser: {ex.Message}");
                return "Chrome"; // Default value
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
                    return 30; // Default value
                }

                if (int.TryParse(timeoutString, out int timeout))
                {
                    return timeout;
                }
                else
                {
                    Console.WriteLine($"Invalid timeout value: {timeoutString}, using default");
                    return 30; // Default value
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting Timeout: {ex.Message}");
                return 30; // Default value
            }
        }

        // Helper method để debug configuration
        public static void DebugConfiguration()
        {
            Console.WriteLine("=== Configuration Debug ===");
            Console.WriteLine($"BaseUrl: {GetBaseUrl()}");
            Console.WriteLine($"Browser: {GetBrowser()}");
            Console.WriteLine($"Timeout: {GetTimeout()}");
            Console.WriteLine("=== End Debug ===");
        }
    }
}