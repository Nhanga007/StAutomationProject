using Coypu;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PageObjects;
using System;

namespace StAutomationProject.PageObjects.Pages
{
    public class LoginPage : BasePage
    {
        private By _emailInput = By.Id("email");
        private By _passwordInput = By.Id("pass");
        private By _loginButton = By.Id("send2");
        private By _errorMessage = By.CssSelector("div.message-error"); 
        private By _emailError = By.Id("email-error");
        private By _passError = By.Id("pass-error");

        public LoginPage(IWebDriver driver) : base(driver) { }

        public void NavigateTo()
        {
            Driver.Navigate().GoToUrl("https://magento.softwaretestingboard.com/customer/account/login/");
        }

        public void EnterCredentials(string email, string password)
        {
            FindElement(_emailInput).Clear(); 
            FindElement(_emailInput).SendKeys(email);
            FindElement(_passwordInput).Clear(); 
            FindElement(_passwordInput).SendKeys(password);
        }

        public void ClickLogin()
        {
            FindElement(_loginButton).Click();
        }

        public bool IsGeneralErrorDisplayed()
        {
            try
            {
                return FindElement(_errorMessage).Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("General error message not found.");
                return false;
            }
        }

        public string GetGeneralErrorMessage()
        {
            try
            {
                return FindElement(_errorMessage).Text;
            }
            catch (WebDriverTimeoutException)
            {
                return string.Empty;
            }
        }

        public bool IsEmailErrorDisplayed()
        {
            try
            {
                return FindElement(_emailError).Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public string GetEmailErrorMessage()
        {
            try
            {
                return FindElement(_emailError).Text;
            }
            catch (WebDriverTimeoutException)
            {
                return string.Empty;
            }
        }

        public bool IsPasswordErrorDisplayed()
        {
            try
            {
                return FindElement(_passError).Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public string GetPasswordErrorMessage()
        {
            try
            {
                return FindElement(_passError).Text;
            }
            catch (WebDriverTimeoutException)
            {
                return string.Empty;
            }
        }
    }
}