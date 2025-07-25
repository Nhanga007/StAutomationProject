using AventStack.ExtentReports;
using NUnit.Framework;
using StAutomationProject.PageObjects.Pages;

namespace StAutomationProject.Tests.TestCases
{
    [TestFixture("Chrome")]
    [TestFixture("Edge")]
    [Parallelizable(ParallelScope.Fixtures)]
    public class LoginTests : TestBase
    {
        private LoginPage _loginPage;

        public LoginTests(string browser) : base(browser)
        {
        }

        [SetUp]
        public void TestSetup()
        {
            _loginPage = new LoginPage(Driver);
            _loginPage.NavigateTo();
        }

        [Test]
        //Tài khoản mật khẩu hợp lý
        public void Login_WithValidCredentials_ShouldLoginSuccessfully()
        {
            _loginPage.EnterCredentials("nhantaolao@gmail.com", "Xuannhan$$311");
            _loginPage.ClickLogin();
            Assert.That(Driver.Url.Contains("customer/account"), "Login failed: Not redirected to account page");
            Test.Log(Status.Info, "Login successful");
        }

        [Test]
        //Sai pass 
        public void Login_WithInvalidCredentials_ShouldShowError()
        {
            _loginPage.EnterCredentials("invalid@example.com", "wrongpassword");
            _loginPage.ClickLogin();
            Assert.That(_loginPage.IsGeneralErrorDisplayed(), Is.True, "General error message not displayed");
            Assert.That(_loginPage.GetGeneralErrorMessage(), Does.Contain("The account sign-in was incorrect or your account is disabled temporarily"), "Incorrect error message");
            Test.Log(Status.Info, "Error message displayed: " + _loginPage.GetGeneralErrorMessage());
        }

        [Test]
        //Email để trống
        public void Login_WithoutEmail_ShouldShowEmailRequiredError()
        {
            _loginPage.EnterCredentials("", "password");
            _loginPage.ClickLogin();
            Assert.That(_loginPage.IsEmailErrorDisplayed(), Is.True, "Email error message not displayed");
            Assert.That(_loginPage.GetEmailErrorMessage(), Is.EqualTo("This is a required field."), "Incorrect email error message");
            Test.Log(Status.Info, "Email required validation triggered: " + _loginPage.GetEmailErrorMessage());
        }

        [Test]
        //bỏ trống pass
        public void Login_WithoutPassword_ShouldShowPasswordRequiredError()
        {
            _loginPage.EnterCredentials("nhantaolao@gmail.com", "");
            _loginPage.ClickLogin();
            Assert.That(_loginPage.IsPasswordErrorDisplayed(), Is.True, "Password error message not displayed");
            Assert.That(_loginPage.GetPasswordErrorMessage(), Is.EqualTo("This is a required field."), "Incorrect password error message");
            Test.Log(Status.Info, "Password required validation triggered: " + _loginPage.GetPasswordErrorMessage());
        }

        [Test]
        //Để trống cả email & pass
        public void Login_WithoutEmailAndPassword_ShouldShowEmailRequiredError()
        {
            _loginPage.EnterCredentials("", "");
            _loginPage.ClickLogin();
            Assert.That(_loginPage.IsEmailErrorDisplayed(), Is.True, "Email error message not displayed");
            Assert.That(_loginPage.GetEmailErrorMessage(), Is.EqualTo("This is a required field."), "Incorrect email error message");
            Test.Log(Status.Info, "Email required validation triggered: " + _loginPage.GetEmailErrorMessage());
        }

        [Test]
        //Tài khoản không đúng định dạng
        public void Login_WithInvalidEmailFormat_ShouldShowInvalidEmailError()
        {
            _loginPage.EnterCredentials("aaaaaaa", "password");
            _loginPage.ClickLogin();
            Assert.That(_loginPage.IsEmailErrorDisplayed(), Is.True, "Email error message not displayed");
            Assert.That(_loginPage.GetEmailErrorMessage(), Is.EqualTo("Please enter a valid email address (Ex: johndoe@domain.com)."), "Incorrect email error message");
            Test.Log(Status.Info, "Invalid email format validation triggered: " + _loginPage.GetEmailErrorMessage());
        }

        [Test]
        //Password với định dạng sai (Sai pass)
        public void Login_WithSpecialCharactersPassword_ShouldShowError()
        {
            _loginPage.EnterCredentials("nhantaolao@gmail.com", "!@#$%^&*()");
            _loginPage.ClickLogin();
            Assert.That(_loginPage.IsGeneralErrorDisplayed(), Is.True, "General error message not displayed");
            Assert.That(_loginPage.GetGeneralErrorMessage(), Does.Contain("The account sign-in was incorrect or your account is disabled temporarily"), "Incorrect error message");
            Test.Log(Status.Info, "Special characters password validation triggered: " + _loginPage.GetGeneralErrorMessage());
        }

        [Test]
        //Tài khoản chưa được tạo (Sai tk hoặc mk)
        public void Login_WithInactiveAccount_ShouldShowError()
        {
            _loginPage.EnterCredentials("inactive@example.com", "Password123");
            _loginPage.ClickLogin();
            Assert.That(_loginPage.IsGeneralErrorDisplayed(), Is.True, "General error message not displayed");
            Assert.That(_loginPage.GetGeneralErrorMessage(), Does.Contain("The account sign-in was incorrect or your account is disabled temporarily"), "Incorrect error message");
            Test.Log(Status.Info, "Inactive account validation triggered: " + _loginPage.GetGeneralErrorMessage());
        }
    }
}