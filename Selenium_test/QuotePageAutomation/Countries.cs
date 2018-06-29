using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumAutomation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuotePageAutomation
{
    public class Countries : IFillable
    {
        public string countries;

        public void Fill()
        {
            // To pass in countries
            ////*[@id="Destination"]/div/div[1]/div/div[1]/div/input
            Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='Destination']/div/div[1]/div/div[1]/div/input")));
            var destination = Driver.Instance.FindElement(By.XPath("//*[@id='Destination']/div/div[1]/div/div[1]/div/input"));
            destination.SendKeys(countries);
            Thread.Sleep(1500);
            ReadOnlyCollection<IWebElement> autocompletePopUps = Driver.Instance.FindElements(By.CssSelector(".autocomplete-popup.show"));
            if (autocompletePopUps.Count() == 0) // If autocomplete somehow does not popup
            {
                Driver.Instance.FindElement(By.XPath("//*[@id='Destination']/div/div[1]/div/div[1]/div/span")).Click(); // trigger dropdown arrow
                Thread.Sleep(500);
            }

            Driver.Instance.FindElement(By.XPath("//*[@id='Destination']/div/div[1]/div/div[2]/div/div")).Click();
        }
    }
}