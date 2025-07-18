using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PageObjects;
using System;

namespace StAutomationProject.PageObjects.Pages
{
    public class ProductPage : BasePage
    {
        private By _addToCartButton = By.Id("product-addtocart-button");
        private By _successMessage = By.CssSelector("div.message-success");
        private By _errorMessage = By.CssSelector("div.message-error"); 
        private By _sizeOption = By.Id("option-label-size-143-item-169"); // Size option
        private By _colorOption = By.Id("option-label-color-93-item-56"); // Color option
        private By _quantityInput = By.Id("qty"); // Quantity input

        public ProductPage(IWebDriver driver) : base(driver) { }

        public void SelectSizeAndColor()
        {
            FindElement(_sizeOption).Click();
            FindElement(_colorOption).Click();
        }

        public void SetQuantity(int quantity)
        {
            var qtyInput = FindElement(_quantityInput);
            qtyInput.Clear();
            qtyInput.SendKeys(quantity.ToString());
        }

        public void AddToCart()
        {
            SelectSizeAndColor(); 
            FindElement(_addToCartButton).Click();
        }

        public bool IsAddToCartSuccessMessageDisplayed()
        {
            try
            {
                return FindElement(_successMessage).Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Success message not found.");
                return false;
            }
        }

        public bool GetSuccessMessage()
        {
            try
            {
                return FindElement(_successMessage).Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public bool IsOutOfStockErrorDisplayed()
        {
            try
            {
                Wait.Until(d => FindElement(_errorMessage).Displayed); 
                return FindElement(_errorMessage).Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Out of stock error message not found.");
                return false;
            }
        }

        public string GetOutOfStockErrorMessage()
        {
            try
            {
                Wait.Until(d => FindElement(_errorMessage).Displayed);
                return FindElement(_errorMessage).Text;
            }
            catch (WebDriverTimeoutException)
            {
                return string.Empty;
            }
        }
    }
}