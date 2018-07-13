using OpenQA.Selenium;
using SeleniumAutomation;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TravellerDetailsPageAutomation
{
    public class TravellerDetails : IFillable
    {
        public List<TravellerDetail> travellerDetailsList;

        public void Fill(FullElementSelector fullElementSelector)
        {
            int travellerCount = 1;
            foreach (TravellerDetail t in travellerDetailsList)
            {
                /* traveller info */
                Driver.Instance.FindElement(By.Id("aNric")).SendKeys(t.tNRIC);
                Driver.Instance.FindElement(By.Id("aFullName")).SendKeys(t.tFullName);
                Driver.Instance.FindElement(By.Id("aDob")).SendKeys(t.tDOB);
                Driver.Instance.FindElement(By.XPath("//*[@id='applicant-Nationality']/div/div[1]/div/input")).SendKeys(t.tNationality);
                Thread.Sleep(1500);

                ReadOnlyCollection<IWebElement> autocompletePopUps = Driver.Instance.FindElements(By.CssSelector(".autocomplete-popup.show"));
                if (autocompletePopUps.Count() == 0) //If autocomplete somehow does not trigger
                {
                    Driver.Instance.FindElement(By.XPath("//*[@id='applicant-Nationality']/div/div[1]/div/span[1]")).Click(); //Clicks the dropdown arrow
                    Thread.Sleep(500);
                }
                Driver.Instance.FindElement(By.XPath("//*[@id='applicant-Nationality']/div/div[1]/div/div/div/div")).Click(); // Selects the autocomplete popup
                travellerCount++;
            }


            //Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='applicant-Number']/div/div[1]/div/input")));
            //Driver.Instance.FindElement(By.XPath("//*[@id='applicant-Number']/div/div[1]/div/input")).SendKeys(aMobile);

            //Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='applicant-Email']/div/div[1]/div/input")));
            //Driver.Instance.FindElement(By.XPath("//*[@id='applicant-Email']/div/div[1]/div/input")).SendKeys(aEmail);

            //if (!applicantIsTraveller)
            //    ;//toggle off (only for individual)
        }
    }
}
