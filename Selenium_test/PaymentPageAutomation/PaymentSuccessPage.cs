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
    public class PaymentSuccessPage
    {
        public static void ViewPDF()
        {
            Driver.Instance.FindElement(By.XPath("/html/body/chubb-dbs-app/app-pay/payment-success/div/div/div/div/div[4]/a")).Click();
            Thread.Sleep(10000);
        }
    }
}
