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
        
        public void Fill(FullElementSelector fullElementSelector)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver.Instance;

            Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='lbl-applicant-mobile']/div/div[1]/div/input")));
            var mobile = Driver.Instance.FindElement(By.XPath("//*[@id='lbl-applicant-mobile']/div/div[1]/div/input"));
            js.ExecuteScript("arguments[0].scrollIntoView();", mobile);
            mobile.Click();
            mobile.Clear();
            mobile.SendKeys(Keys.Backspace + Keys.Backspace + Keys.Backspace + Keys.Backspace + Keys.Backspace + Keys.Backspace + Keys.Backspace + Keys.Backspace);
            mobile.SendKeys(aMobile);

            Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='lbl-applicant-email']/div/div[1]/div/input")));
            var email = Driver.Instance.FindElement(By.XPath("//*[@id='lbl-applicant-email']/div/div[1]/div/input"));
            js.ExecuteScript("arguments[0].scrollIntoView();", email);

            email.Click();
            while(!string.IsNullOrWhiteSpace(email.GetAttribute("ng-reflect-value")))
            {
                email.SendKeys(Keys.Backspace);

            }
            email.SendKeys(aEmail);           
        }
    }
}
