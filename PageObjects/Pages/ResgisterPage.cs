using OpenQA.Selenium;
using PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StAutomationProject.PageObjects.Pages
{
    public class RegisterPage : BasePage
    {
        private By _firstNameInput = By.Id("firstname");
        private By _lastNameInput = By.Id("lastname");
        private By _emailInput = By.Id("email_address");
        private By _passwordInput = By.Id("password");
        private By _confirmPasswordInput = By.Id("password-confirmation");
        private By _createAccountButton = By.CssSelector("button[title='Create an Account']");
        private By _errorMessage = By.Id("password-confirmation-error");

        public RegisterPage(IWebDriver driver) : base(driver) { }

        public void NavigateTo()
        {
            Driver.Navigate().GoToUrl("https://magento.softwaretestingboard.com/customer/account/create/");
        }

        public void EnterRegistrationDetails(string firstName, string lastName, string email, string password, string confirmPassword)
        {
            FindElement(_firstNameInput).SendKeys(firstName);
            FindElement(_lastNameInput).SendKeys(lastName);
            FindElement(_emailInput).SendKeys(email);
            FindElement(_passwordInput).SendKeys(password);
            FindElement(_confirmPasswordInput).SendKeys(confirmPassword);
        }

        public void ClickCreateAccount()
        {
            FindElement(_createAccountButton).Click();
        }

        public bool IsErrorMessageDisplayed()
        {
            try
            {
                return FindElement(_errorMessage).Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public string GetErrorMessage()
        {
            return FindElement(_errorMessage).Text;
        }
    }
}
