using Coypu;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PageObjects;
using System;

namespace StAutomationProject.PageObjects.Pages
{
    public class CartPage : BasePage
    {
        private By _cartIcon = By.CssSelector("a.action.showcart");
        private By _viewEditCartLink = By.XPath("//span[normalize-space()='View and Edit Cart']/.."); 
        private By _cartItems = By.CssSelector("div.cart-item");
        private By _quantityInput = By.CssSelector("input.qty"); 
        private By _updateButton = By.CssSelector("button.action.update"); 

        public CartPage(IWebDriver driver) : base(driver) { }

        public bool EmptyCart()
        {
            FindElement(_cartIcon).Click();
            Thread.Sleep(2000); 
            try
            {
                Wait.Until(d => FindElement(_viewEditCartLink).Displayed);
                return true; 
            }
            catch (NoSuchElementException)
            {
                return false; 
            }
        }

        public void CartIsNotEmpty()
        {
            FindElement(_cartIcon).Click();
            Wait.Until(d =>
            {
                try
                {
                    return FindElement(_viewEditCartLink).Displayed || FindElement(_cartItems).Displayed;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            }); 
            FindElement(_viewEditCartLink).Click();
            Wait.Until(d => Driver.Url.Contains("/checkout/cart") || FindElement(_cartItems).Displayed);
        }

        public bool HasCartItems()
        {
            try
            {
                return FindElement(_cartItems).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public void UpdateQuantity(int newQuantity)
        {
            var qtyInput = FindElement(_quantityInput); 
            qtyInput.Clear();
            qtyInput.SendKeys(newQuantity.ToString());
            FindElement(_updateButton).Click(); 
            Wait.Until(d => qtyInput.GetAttribute("value") == newQuantity.ToString()); 
        }

        public int GetCartItemQuantity()
        {
            Thread.Sleep(10000);
            var qtyInput = FindElement(_quantityInput); 
            return int.Parse(qtyInput.GetAttribute("value"));
        }
    }
}