using AventStack.ExtentReports;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using StAutomationProject.PageObjects.Pages;

namespace StAutomationProject.Tests.TestCases
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    public class CartTests : TestBase
    {
        private LoginPage _loginPage;
        private HomePage _homePage;
        private ProductPage _productPage;
        private CartPage _cartPage;

        [SetUp]
        public void TestSetup()
        {
            _loginPage = new LoginPage(Driver);
            _homePage = new HomePage(Driver);
            _productPage = new ProductPage(Driver);
            _cartPage = new CartPage(Driver);
        }

        [Test]
        public void Verify_Cart_After_Login()
        {
            _loginPage.NavigateTo();
            _loginPage.EnterCredentials("nhantaolao@gmail.com", "Xuannhan$$311");
            _loginPage.ClickLogin();
            Assert.That(Driver.Url.Contains("customer/account"), "Login failed: Not redirected to account page");
            Test.Log(Status.Info, "Login successful"); Thread.Sleep(2000);
            if (!_cartPage.EmptyCart())
            {
                Test.Log(Status.Info, "Cart is empty");
                Assert.Fail("Cart is empty");
            }
            Test.Log(Status.Info, "Cart is not empty, test completed.");
        }

        [Test]
        public void Verify_Cart_Update_Quantity()
        {
            SetupCartForUpdate();
            Test.Log(Status.Info, "Quantity updated successfully to: " + _cartPage.GetCartItemQuantity());
        }

        private void SetupCartForUpdate()
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
            Test.Log(Status.Info, "Product added to cart");

            _cartPage.CartIsNotEmpty();
            Test.Log(Status.Info, "Navigated to edit cart page");

            _cartPage.UpdateQuantity(5);
            Test.Log(Status.Info, "Updated quantity to 5");
        }

        [Test]
        public void PayTest()
        {
            SetupCartForUpdate(); 
            Test.Log(Status.Info, "Quantity updated successfully to: " + _cartPage.GetCartItemQuantity());

            var paymentPage = new PaymentPage(Driver);
            paymentPage.AddNewAddress(
                company: "Tech Corp",
                street1: "123 Main St",
                street2: "Apt 4B",
                street3: "",
                city: "Hanoi",
                state: "Ohio",
                zip: "12345",
                country: "Japan",
                phone: "0123456789"
            );
            Test.Log(Status.Info, "New address added and proceeded to checkout");
            paymentPage.ProceedToNext();
            Test.Log(Status.Info, "Proceeded to next step");

            paymentPage.PlaceOrder();
            Test.Log(Status.Info, "Order placed successfully");

            paymentPage.NavigateToMyAccount();
            Test.Log(Status.Info, "Navigated to My Account");
            paymentPage.ViewMyOrders();
            Test.Log(Status.Info, "Viewing My Orders");

            Assert.That(Driver.Url.Contains("/sales/order/history/"), "Failed to navigate to My Orders page");
        }
    }
}