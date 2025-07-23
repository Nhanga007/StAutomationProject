using StAutomationProject.PageObjects.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace StAutomationProject.Tests.TestCases
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    public class RegisterTests : TestBase
    {
        private RegisterPage _registerPage;

        [SetUp]
        public void TestSetup()
        {
            _registerPage = new RegisterPage(Driver);
            _registerPage.NavigateTo();
        }

        [Test]
        public void Register_WithValidDetails_ShouldRegisterSuccessfully()
        {
            string email = $"test{DateTime.Now.Ticks}@gmail.com";
            _registerPage.EnterRegistrationDetails("Nguyen", "XuanNhan", $"test{DateTime.Now.Ticks}@example.com", "Password123", "Password123");
            _registerPage.ClickCreateAccount();
            Assert.That(Driver.Url.Contains("customer/account"), "Registration failed: Not redirected to account page");
            Test.Log(AventStack.ExtentReports.Status.Info,$"Register with email:  + {email}");
        }

        [Test]
        public void Register_WithMismatchedPasswords_ShouldShowError()
        {
            _registerPage.EnterRegistrationDetails("Nguyen", "XuanNhan", "test@example.com", "Password123", "DifferentPassword");
            _registerPage.ClickCreateAccount();
            Assert.That(_registerPage.IsErrorMessageDisplayed(), "Please enter the same value again.");
            Test.Log(AventStack.ExtentReports.Status.Info, "Error message displayed: " + _registerPage.GetErrorMessage());
        }
    }
}
