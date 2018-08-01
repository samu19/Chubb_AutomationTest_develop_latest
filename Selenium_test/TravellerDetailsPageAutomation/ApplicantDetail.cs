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
        
        public void Fill(FullElementSelector fullElementSelector, string testId, string testName)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver.Instance;

            //if (!applicantIsTraveller)
            //{//toggle off (only for individual)
            //    string toggleElement = "/html/body/app-root/apply/div/div/div/div[2]/div/application-details/applicant-detail/form/mat-card/mat-card-header/div/mat-card-title/span";
            //    var applicantIsTravellerToggle = Driver.Instance.FindElement(By.XPath(toggleElement));
            //    js.ExecuteScript("arguments[0].scrollIntoView();", applicantIsTravellerToggle);

            //    applicantIsTravellerToggle.Click();
            //    Thread.Sleep(1000);
            //}
            Thread.Sleep(3000);
            string applicantMobileElement = "//*[@id='lbl-applicant-mobile-input']";
            Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath(applicantMobileElement)));
            var mobile = Driver.Instance.FindElement(By.XPath(applicantMobileElement));
            //js.ExecuteScript("arguments[0].scrollIntoView();", mobile);
            mobile.Click();
            mobile.Clear();
            mobile.SendKeys(Keys.Backspace + Keys.Backspace + Keys.Backspace + Keys.Backspace + Keys.Backspace + Keys.Backspace + Keys.Backspace + Keys.Backspace);
            mobile.SendKeys(aMobile);
            Helper.WriteToCSV("Applicant Details Page", "Applicant mobile number updated", true, null, testId, testName);

            string applicantEmailElement = "//*[@id='lbl-applicant-email-input']";
            Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath(applicantEmailElement)));
            var email = Driver.Instance.FindElement(By.XPath(applicantEmailElement));
            js.ExecuteScript("arguments[0].scrollIntoView();", mobile);

            //email.Click();
            while(!string.IsNullOrWhiteSpace(email.GetAttribute("ng-reflect-value")))
            {
                email.SendKeys(Keys.Backspace);

            }
            email.SendKeys(aEmail);
            Helper.WriteToCSV("Applicant Details Page", "Applicant email updated", true, null, testId, testName);

        }
    }
}
