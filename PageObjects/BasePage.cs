using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using SeleniumExtras.WaitHelpers;

namespace PageObjects
{
    public class BasePage
    {
        protected IWebDriver Driver;
        protected WebDriverWait Wait;

        public BasePage(IWebDriver driver)
        {
            Driver = driver;
            Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        protected IWebElement FindElement(By by)
        {
            return Wait.Until(drv => drv.FindElement(by));
        }
        protected void CloseAdsIfPresent()
        {
            try
            {
                var ad = Driver.FindElement(By.CssSelector(".ad_position_box"));
                if (ad.Displayed)
                {
                    var closeBtn = ad.FindElement(By.CssSelector(".close"));
                    closeBtn.Click();
                    Wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector(".ad_position_box")));
                }
            }
            catch { /* ignore nếu không có quảng cáo */ }
        }


    }
}
