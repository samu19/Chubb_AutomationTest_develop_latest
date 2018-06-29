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
    public class AdultAge : IFillable
    {
        public string adultAge;
        public string coverType;
        public void Fill()
        {
            string xPath = "/html/body/chubb-dbs-app/app-trip/div/form/div[1]/div/chubb-traveler-age/div/mat-form-field/div/div[1]/div/input";

            if (coverType == "Family")
                xPath = "//*[@id='mat-input-5']";
            // to pass in adult age
            Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath(xPath)));
            Driver.Instance.FindElement(By.XPath(xPath)).SendKeys(adultAge);
        }
    }
}