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
        public static void ProceedWithPayment(FullElementSelector fullElementSelector, string testId, string testName)
        {
            try
            {
                //click checkbox //                
                Driver.Instance.FindElement(By.XPath(fullElementSelector.iAcceptCheckBoxElement)).Click();
                string submitElement = fullElementSelector.submitElement;

                //Submit
                Driver.GetWait().Until(OpenQA.Selenium.Support.UI.ExpectedConditions.ElementExists(By.XPath(submitElement)));
                Driver.Instance.FindElement(By.XPath(submitElement)).Click();
                //new OpenQA.Selenium.Support.UI.WebDriverWait(Driver.Instance, System.TimeSpan.FromSeconds(60)).Until(OpenQA.Selenium.Support.UI.ExpectedConditions.UrlContains("complete"));
                Console.WriteLine("Submitting Payment...");
                string policyNoElement = fullElementSelector.policyNoElement;
                

                new OpenQA.Selenium.Support.UI.WebDriverWait(Driver.Instance, System.TimeSpan.FromSeconds(60)).Until(OpenQA.Selenium.Support.UI.ExpectedConditions.ElementExists(By.XPath(policyNoElement)));

                var policyNo_ = Driver.Instance.FindElement(By.XPath(policyNoElement));
                string policyNo = policyNo_.Text;

                Helper.WriteToCSV("Final Page", "Policy number shown", true, policyNo, testId, testName);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Payment successful, Policy Number is: " + policyNo + Environment.NewLine);
                Console.ForegroundColor = ConsoleColor.White;
                Thread.Sleep(2500);
            }
            catch
            {
                Helper.WriteToCSV("Final Page", "Policy number shown", false, null, testId, testName);

            }
        }
    }
}
