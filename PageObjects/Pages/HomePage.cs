using Coypu;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PageObjects;
using System;

namespace StAutomationProject.PageObjects.Pages
{
    public class HomePage : BasePage
    {
        private By _searchInput = By.Id("search");
        private By _searchSuggestions = By.CssSelector("[id^='qs-option']"); 
        private By _searchResults = By.CssSelector("li.product-item");

        public HomePage(IWebDriver driver) : base(driver) { }

        public void NavigateTo()
        {

            Driver.Navigate().GoToUrl("https://magento.softwaretestingboard.com/");
        }

        public void SearchAndSelectSecondSuggestion(string keyword)
        {
            var searchBox = FindElement(_searchInput);
            searchBox.Clear();
            searchBox.SendKeys(keyword); 

            Wait.Until(d => Driver.FindElements(_searchSuggestions).Count > 0 || d.FindElement(_searchInput).GetAttribute("aria-expanded") == "true");
            System.Threading.Thread.Sleep(2000); 

            var suggestions = Driver.FindElements(_searchSuggestions);
            if (suggestions.Count < 2)
                throw new Exception("Less than 2 search suggestions found.");

            suggestions[1].Click();
        }

        public ProductPage SelectSecondProduct()
        {
            var products = Driver.FindElements(_searchResults);
            if (products.Count < 2)
                throw new Exception("Less than 2 products found in search results.");
            products[1].FindElement(By.XPath("//body/div[@class='page-wrapper']/main[@id='maincontent']/div[@class='columns']/div[@class='column main']/div[@class='search results']/div[@class='products wrapper grid products-grid']/ol[@class='products list items product-items']/li[1]/div[1]")).Click(); 
            return new ProductPage(Driver);
        }
        public IList<IWebElement> GetProductItems()
        {
            Wait.Until(d => Driver.FindElements(_searchResults).Count > 0); 
            return Driver.FindElements(_searchResults);
        }
    }
}