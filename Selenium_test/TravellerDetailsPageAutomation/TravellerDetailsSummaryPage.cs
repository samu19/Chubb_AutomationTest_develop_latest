using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumAutomation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellerDetailsPageAutomation
{
    public class TravellerDetailsSummaryPage
    {
        public static void ProceedToPaymentSelection()
        {
            /* i accept and buy now */
            Driver.Instance.FindElement(By.XPath("//*[@id='mat-checkbox-2']/label/div")).Click();
            Driver.Instance.FindElement(By.Id("buyNow")).Click();
            new WebDriverWait(Driver.Instance, System.TimeSpan.FromSeconds(20)).Until(ExpectedConditions.UrlContains("select"));

        }

    }
}
