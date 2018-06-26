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
    public class CoverType : IFillable
    {
        public string coverType;

        public void Fill()
        {
            // To pass in Cover Type into appropriate field
            switch(coverType)
            {
                case "Individual":
                    //default selection is Individual
                    break;

                case "Family":
                    Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("input[id*='ddlLetterTemplate_Input']")));
                    Driver.ClickWithRetry(By.CssSelector("input[id*='ddlLetterTemplate_Input']"));
                    break;

                case "Group":
                    Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("input[id*='ddlLetterTemplate_Input']")));
                    Driver.ClickWithRetry(By.CssSelector("input[id*='ddlLetterTemplate_Input']"));
                    break;

                case "Couple":
                    Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("input[id*='ddlLetterTemplate_Input']")));
                    Driver.ClickWithRetry(By.CssSelector("input[id*='ddlLetterTemplate_Input']"));
                    break;

            }
        }
    }
}