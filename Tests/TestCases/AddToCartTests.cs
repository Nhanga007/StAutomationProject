using AventStack.ExtentReports;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using PageObjects;
using StAutomationProject.PageObjects.Pages;

namespace StAutomationProject.Tests.TestCases
{
    [TestFixture]
    public class AddToCartTests : TestBase
    {
        private LoginPage _loginPage;
        private HomePage _homePage;
        private ProductPage _productPage;

        [SetUp]
        public void TestSetup()
        {
            _loginPage = new LoginPage(Driver);
            _homePage = new HomePage(Driver);
            _productPage = new ProductPage(Driver);
        }

        [Test]
        public void AddToCart_AfterLogin_ShouldAddProductSuccessfully()
        {
            _loginPage.NavigateTo();
            _loginPage.EnterCredentials("nhantaolao@gmail.com", "Xuannhan$$311");
            _loginPage.ClickLogin();
            Assert.That(Driver.Url.Contains("customer/account"), "Login failed: Not redirected to account page");
            Test.Log(Status.Info, "Login successful");

            _homePage.NavigateTo();
            _homePage.SearchAndSelectSecondSuggestion("jacket");
            Test.Log(Status.Info, "Selected second suggestion from search results");

            _productPage = _homePage.SelectSecondProduct();
            Test.Log(Status.Info, "Selected second product from search results");

            _productPage.SetQuantity(2); 
            _productPage.AddToCart();
            _productPage.IsAddToCartSuccessMessageDisplayed();
            Assert.That(_productPage.IsAddToCartSuccessMessageDisplayed(), Is.True, "Add to cart success message not displayed");
            Test.Log(Status.Info, "Product added to cart: " + _productPage.GetSuccessMessage());
        }

        [Test]
        public void AddToCart_OutOfStockProduct()
        {
            _loginPage.NavigateTo();
            _loginPage.EnterCredentials("nhantaolao@gmail.com", "Xuannhan$$311");
            _loginPage.ClickLogin();
            Assert.That(Driver.Url.Contains("customer/account"), "Login failed: Not redirected to account page");
            Test.Log(Status.Info, "Login successful");

            _homePage.NavigateTo();
            _homePage.SearchAndSelectSecondSuggestion("jacket");
            Test.Log(Status.Info, "Selected second suggestion from search results");

            _productPage = _homePage.SelectSecondProduct();
            Test.Log(Status.Info, "Selected second product from search results");

            _productPage.SetQuantity(9999);
            _productPage.AddToCart();
            _productPage.IsOutOfStockErrorDisplayed();
            Assert.That(_productPage.IsOutOfStockErrorDisplayed(), Is.True, "Out of stock error message not displayed");
            Test.Log(Status.Info, "Out of stock error displayed: " + _productPage.GetOutOfStockErrorMessage());
        }
    }
}