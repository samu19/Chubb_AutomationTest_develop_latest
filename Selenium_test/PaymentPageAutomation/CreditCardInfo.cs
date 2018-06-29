using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumAutomation;
using System;
using System.Collections.Generic;
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

        public void Fill()
        {
            /* credit card number */
            Driver.GetWait().Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.XPath("//*[@id='tokenExIframeDiv']")));
            Driver.Instance.FindElement(By.Id("pan")).SendKeys(cardNo); //
            Driver.Instance.SwitchTo().DefaultContent();

            Thread.Sleep(1000);

            /* name */
            Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath("/html/body/chubb-dbs-app/app-pay/payment-dbs/form/div[1]/div[3]/div[1]/mat-form-field/div/div[1]/div/input")));
            Driver.Instance.FindElement(By.XPath("/html/body/chubb-dbs-app/app-pay/payment-dbs/form/div[1]/div[3]/div[1]/mat-form-field/div/div[1]/div/input")).SendKeys(cardHolderName);

            /* Expiry */
            Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath("/html/body/chubb-dbs-app/app-pay/payment-dbs/form/div[1]/div[3]/div[2]/div[1]/mat-form-field/div/div[1]/div/input")));
            var expiry = Driver.Instance.FindElement(By.XPath("/html/body/chubb-dbs-app/app-pay/payment-dbs/form/div[1]/div[3]/div[2]/div[1]/mat-form-field/div/div[1]/div/input"));
            expiry.SendKeys(expiryDate);

            /* cvv */
            Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath("/html/body/chubb-dbs-app/app-pay/payment-dbs/form/div[1]/div[3]/div[2]/div[2]/mat-form-field/div/div[1]/div/input")));
            Driver.Instance.FindElement(By.XPath("/html/body/chubb-dbs-app/app-pay/payment-dbs/form/div[1]/div[3]/div[2]/div[2]/mat-form-field/div/div[1]/div/input")).SendKeys(cvv);
            //
        }
    }
}
