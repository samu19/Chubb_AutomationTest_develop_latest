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

        public void Fill()
        {
            // to pass in adult age
            Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("input[id*='ddlLetterTemplate_Input']")));
            Driver.ClickWithRetry(By.CssSelector("input[id*='ddlLetterTemplate_Input']"));
        }
    }
}