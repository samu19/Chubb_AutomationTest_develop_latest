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
    public class QuoteDates : IFillable
    {
        public DateTime departDate;
        public DateTime returnDate;

        public void Fill()
        {
            // To pass in dates into appropriate date fields
            // string formattedDate = YourDate.ToString("dd MMM yyyy");
            Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("input[id*='ddlLetterTemplate_Input']")));
            Driver.ClickWithRetry(By.CssSelector("input[id*='ddlLetterTemplate_Input']"));
        }
    }
}
