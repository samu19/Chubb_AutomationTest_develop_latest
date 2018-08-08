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
        public static void ViewPDF(string testId, string testName)
        {
            try
            {

                Driver.Instance.FindElement(By.XPath("/html/body/app-root/complete/div[2]/div/div/div/div[4]/custom-label[1]/a")).Click();
                Helper.WriteToCSV("Final Page", "PDF clicked", true, null, testId, testName);

                Thread.Sleep(2500);
            }
            catch
            {
                Helper.WriteToCSV("Final Page", "PDF clicked", false, null, testId, testName);

            }
        }
    }
}
