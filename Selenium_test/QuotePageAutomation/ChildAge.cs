using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumAutomation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotePageAutomation
{
    public class ChildAge : IFillable
    {
        public string childAge;

        public void Fill()
        {
            if (String.IsNullOrWhiteSpace(childAge))
                return;

            // To pass in child age
            Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='mat-input-6']")));
            Driver.Instance.FindElement(By.XPath("//*[@id='mat-input-6']")).SendKeys(childAge);
        }
    }
}