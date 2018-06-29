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
    public class PaymentSelectionPage
    {
        public static void SelectPaymentTypeAndProceed(string paymentType)
        {
            bool pass = false;
            while (!pass)
            {
                switch (paymentType)
                {
                    case "CC":
                        Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath("/html/body/chubb-dbs-app/app-pay/payment/chubb-radio-group/mat-radio-group/div[2]/div/div/mat-radio-button/label/div[1]/div[1]")));
                        Driver.Instance.FindElement(By.XPath("/html/body/chubb-dbs-app/app-pay/payment/chubb-radio-group/mat-radio-group/div[2]/div/div/mat-radio-button/label/div[1]/div[1]")).Click();

                        Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath("/html/body/chubb-dbs-app/app-pay/payment/div[4]/div/button")));

                        Driver.Instance.FindElement(By.XPath("/html/body/chubb-dbs-app/app-pay/payment/div[4]/div/button")).Click();
                        new WebDriverWait(Driver.Instance, System.TimeSpan.FromSeconds(20)).Until(ExpectedConditions.UrlContains("paydbs"));
                        pass = true;
                        break;

                    case "PL":
                        Driver.Instance.FindElement(By.XPath("/html/body/chubb-dbs-app/app-pay/payment/div/div[4]/div/button")).Click();
                        Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='mat-dialog-0']/dia-log/div/button")));
                        Driver.Instance.FindElement(By.XPath("//*[@id='mat-dialog-0']/dia-log/div/button")).Click();
                        Thread.Sleep(3000);
                        ReadOnlyCollection<IWebElement> unexpectedPopUps = Driver.Instance.FindElements(By.CssSelector(".alert-wrapper.ng-star-inserted"));

                        if (unexpectedPopUps.Count() != 0)
                        {
                            Driver.Instance.FindElement(By.XPath("//*[@id='mat-dialog-1']/dia-log/div/button")).Click();
                            paymentType = "CC";
                        }
                        break;
                }
            }

        }
    }
}
