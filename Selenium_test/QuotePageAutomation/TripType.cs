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
    public class TripType : IFillable
    {
        public bool isSingleTrip;

        public void Fill()
        {
            if (!isSingleTrip) // If Annual Trip
            {
                //To change - choose Annual Trip radio button
                Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.Id("mat-radio-3-input")));
                Driver.ClickWithRetry(By.Id("mat-radio-3-input"));
            }
        }
    }
}
