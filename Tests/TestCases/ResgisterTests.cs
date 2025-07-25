using AventStack.ExtentReports;
using NUnit.Framework;
using StAutomationProject.PageObjects.Pages;

namespace StAutomationProject.Tests.TestCases
{
    [TestFixture("Chrome")]
    [TestFixture("Edge")]
    [Parallelizable(ParallelScope.Fixtures)]
    public class RegisterTests : TestBase
    {
        private RegisterPage _registerPage;

        public RegisterTests(string browser) : base(browser) { }

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
            _registerPage.EnterRegistrationDetails("Nguyen", "XuanNhan", email, "Password123", "Password123");
            _registerPage.ClickCreateAccount();
            Assert.That(Driver.Url.Contains("customer/account"), "Registration failed: Not redirected to account page");
            Test.Log(Status.Info, $"Register with email: {email}");
        }

        [Test]
        public void Register_WithMismatchedPasswords_ShouldShowError()
        {
            _registerPage.EnterRegistrationDetails("Nguyen", "XuanNhan", "test@example.com", "Password123", "DifferentPassword");
            _registerPage.ClickCreateAccount();
            Assert.That(_registerPage.IsErrorMessageDisplayed(), "Error message not displayed for mismatched passwords.");
            Test.Log(Status.Info, "Error message displayed: " + _registerPage.GetErrorMessage());
        }
    }
}