using OpenQA.Selenium;
using SeleniumAutomation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentPageAutomation
{
    public class VerifyDetailsPage
    {
        public static void ProceedWithPayment(string testId, string testName)
        {
            //click checkbox
            Driver.Instance.FindElement(By.XPath("/html/body/app-root/apply/div[2]/div/div/div[2]/div/verify-details/div/div[1]/div/div[1]/custom-checkbox/div/mat-checkbox/label/div")).Click();
            string submitElement = "//*[@id='verify-details-button-submit']";

            //Submit
            Driver.GetWait().Until(OpenQA.Selenium.Support.UI.ExpectedConditions.ElementExists(By.XPath(submitElement)));
            Driver.Instance.FindElement(By.XPath(submitElement)).Click();

            new OpenQA.Selenium.Support.UI.WebDriverWait(Driver.Instance, System.TimeSpan.FromSeconds(200)).Until(OpenQA.Selenium.Support.UI.ExpectedConditions.UrlContains("completion"));
            Helper.WriteToCSV("Final Page", "Policy number shown", true, null, testId, testName);
            Thread.Sleep(10000);
        }
    }
}
