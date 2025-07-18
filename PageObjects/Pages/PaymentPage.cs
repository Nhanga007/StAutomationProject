using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PageObjects;
using System;

namespace StAutomationProject.PageObjects.Pages
{
    public class PaymentPage : BasePage
    {
        private By _proceedToCheckoutButton = By.CssSelector("button[data-role='proceed-to-checkout']");
        private By _newAddressButton = By.CssSelector("button.action-show-popup[data-bind*='showFormPopUp']");
        private By _companyInput = By.XPath("//div[@class='field']//div[@class='control']/input[@name='company']");
        private By _streetAddress1 = By.XPath("//input[@name='street[0]']");
        private By _streetAddress2 = By.XPath("//input[@name='street[1]']");
        private By _streetAddress3 = By.XPath("//input[@name='street[2]']");
        private By _cityInput = By.XPath("//input[@name='city']");
        private By _stateProvinceDropdown = By.XPath("//select[@name='region_id']");
        private By _zipPostalCodeInput = By.XPath("//input[@name='postcode']");
        private By _countryDropdown = By.XPath("//select[@name='country_id']");
        private By _phoneNumberInput = By.XPath("//input[@name='telephone']");
        private By _shipHereButton = By.XPath("//button[contains(@class, 'action-save-address')]/span[normalize-space()='Ship here']/..");
        private By _nextButton = By.CssSelector("button[data-role='opc-continue']");
        private By _placeOrderButton = By.CssSelector("button.action.primary.checkout[data-bind*='placeOrder']");
        private By _shipHereSelectButton = By.CssSelector("button.action-select-shipping-item[data-bind*='selectAddress']"); 
        private By _changeButton = By.CssSelector("button.action.switch[data-action='customer-menu-toggle']"); 
        private By _myAccountLink = By.XPath("//div[@aria-hidden='false']//a[normalize-space()='My Account']");
        private By _myOrdersLink = By.XPath("//a[normalize-space()='My Orders']"); 

        public PaymentPage(IWebDriver driver) : base(driver) { }

        public void ProceedToCheckout()
        {
            FindElement(_proceedToCheckoutButton).Click();
            Wait.Until(d => Driver.Url.Contains("/checkout"));
        }

        public void AddNewAddress(string company, string street1, string street2, string street3, string city, string state, string zip, string country, string phone)
        {
            ProceedToCheckout();
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            js.ExecuteScript("arguments[0].click();", FindElement(_newAddressButton));
            Wait.Until(d => Driver.FindElements(By.CssSelector(".modal-popup._show")).Count > 0);

            if (!string.IsNullOrEmpty(company)) FindElement(_companyInput).SendKeys(company);
            if (!string.IsNullOrEmpty(street1)) FindElement(_streetAddress1).SendKeys(street1);
            if (!string.IsNullOrEmpty(street2)) FindElement(_streetAddress2).SendKeys(street2);
            if (!string.IsNullOrEmpty(street3)) FindElement(_streetAddress3).SendKeys(street3);
            if (!string.IsNullOrEmpty(city)) FindElement(_cityInput).SendKeys(city);

            var stateDropdown = new SelectElement(FindElement(_stateProvinceDropdown));
            stateDropdown.SelectByText(state);

            if (!string.IsNullOrEmpty(zip)) FindElement(_zipPostalCodeInput).SendKeys(zip);

            var countryDropdown = new SelectElement(FindElement(_countryDropdown));
            countryDropdown.SelectByText(country);

            if (!string.IsNullOrEmpty(phone)) FindElement(_phoneNumberInput).SendKeys(phone);

            js.ExecuteScript("arguments[0].click();", FindElement(_shipHereButton));
            Wait.Until(d => !Driver.FindElements(By.CssSelector(".modal-popup._show")).Any());

            Thread.Sleep(2000);
        }

        public void ProceedToNext()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            js.ExecuteScript("arguments[0].click();", FindElement(_nextButton));
            Wait.Until(d => Driver.FindElements(_placeOrderButton).Count > 0);
        }

        public void PlaceOrder()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            js.ExecuteScript("arguments[0].click();", FindElement(_placeOrderButton));
            Wait.Until(d => Driver.Url.Contains("/checkout/onepage/success") || Driver.FindElements(By.CssSelector(".checkout-success")).Count > 0);
        }

        public void NavigateToMyAccount()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            js.ExecuteScript("arguments[0].click();", FindElement(_changeButton));
            Wait.Until(d => Driver.FindElements(_myAccountLink).Count > 0);
            FindElement(_myAccountLink).Click();
            Wait.Until(d => Driver.Url.Contains("/customer/account"));
        }

        public void ViewMyOrders()
        {
            Wait.Until(d => Driver.Url.Contains("/customer/account"));
            FindElement(_myOrdersLink).Click();
            Wait.Until(d => Driver.Url.Contains("/sales/order/history/"));
        }
    }
}