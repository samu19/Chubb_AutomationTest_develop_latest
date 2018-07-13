using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumAutomation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TravellerDetailsPageAutomation
{
    public class ApplicantDetail : IFillable
    {
        public string aNRIC;
        public string aFullName;
        public string aDOB;
        public string aNationality;
        public string aMobile;
        public string aEmail;
        public bool applicantIsTraveller = true;
        
        public void Fill(FullElementSelector fullElementSelector)
        {
            /* applicant info */
            Driver.Instance.FindElement(By.Id("aNric")).SendKeys(aNRIC); 
            Driver.Instance.FindElement(By.Id("aFullName")).SendKeys(aFullName); 
            Driver.Instance.FindElement(By.Id("aDob")).SendKeys(aDOB); 
            Driver.Instance.FindElement(By.XPath("//*[@id='applicant-Nationality']/div/div[1]/div/input")).SendKeys(aNationality);
            Thread.Sleep(1500);

            ReadOnlyCollection<IWebElement> autocompletePopUps = Driver.Instance.FindElements(By.CssSelector(".autocomplete-popup.show"));
            if (autocompletePopUps.Count() == 0) //If autocomplete somehow does not trigger
            {
                Driver.Instance.FindElement(By.XPath("//*[@id='applicant-Nationality']/div/div[1]/div/span[1]")).Click(); //Clicks the dropdown arrow
                Thread.Sleep(500);
            }
            Driver.Instance.FindElement(By.XPath("//*[@id='applicant-Nationality']/div/div[1]/div/div/div/div")).Click(); // Selects the autocomplete popup

            Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='applicant-Number']/div/div[1]/div/input")));
            Driver.Instance.FindElement(By.XPath("//*[@id='applicant-Number']/div/div[1]/div/input")).SendKeys(aMobile);

            Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='applicant-Email']/div/div[1]/div/input")));
            Driver.Instance.FindElement(By.XPath("//*[@id='applicant-Email']/div/div[1]/div/input")).SendKeys(aEmail);

            if (!applicantIsTraveller)
                ;//toggle off (only for individual)
        }
    }
}
