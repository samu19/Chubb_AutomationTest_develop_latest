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

namespace PaymentPageAutomation
{
    public class CreditCardInfo : IFillable
    {
        public string cardNo;
        public string cardHolderName;
        public string expiryDate;
        public string cvv;

        public void Fill(FullElementSelector fullElementSelector, string testId, string testName)
        {
            /* credit card number */
            Driver.GetWait().Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.XPath("//*[@id='tokenexIframe']")));
            Driver.Instance.FindElement(By.Id("pan")).SendKeys(cardNo); //
            Driver.Instance.SwitchTo().DefaultContent();
            Helper.WriteToCSV("Payment Details Page", "Card number filled", true, null, testId, testName);

            Thread.Sleep(1000);

            /* name */
            Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='undefined-input']")));
            ReadOnlyCollection<IWebElement> creditCardInfo = Driver.Instance.FindElements(By.XPath("//*[@id='undefined-input']"));
            creditCardInfo[0].SendKeys(cardHolderName);
            Helper.WriteToCSV("Payment Details Page", "Cardholder name filled", true, null, testId, testName);

            /* Expiry */
            //Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath("/html/body/chubb-dbs-app/app-pay/payment-dbs/form/div[1]/div[3]/div[2]/div[1]/mat-form-field/div/div[1]/div/input")));
            //var expiry = Driver.Instance.FindElement(By.XPath("/html/body/chubb-dbs-app/app-pay/payment-dbs/form/div[1]/div[3]/div[2]/div[1]/mat-form-field/div/div[1]/div/input"));
            creditCardInfo[1].SendKeys(expiryDate);
            Helper.WriteToCSV("Payment Details Page", "Expiry Date filled", true, null, testId, testName);

            /* cvv */
            //Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath("/html/body/chubb-dbs-app/app-pay/payment-dbs/form/div[1]/div[3]/div[2]/div[2]/mat-form-field/div/div[1]/div/input")));
            //Driver.Instance.FindElement(By.XPath("/html/body/chubb-dbs-app/app-pay/payment-dbs/form/div[1]/div[3]/div[2]/div[2]/mat-form-field/div/div[1]/div/input")).SendKeys(cvv);
            creditCardInfo[2].SendKeys(cvv);
            Helper.WriteToCSV("Payment Details Page", "cvv filled", true, null, testId, testName);

            //
        }
    }
}
