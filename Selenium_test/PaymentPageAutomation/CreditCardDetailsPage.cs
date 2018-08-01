using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumAutomation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentPageAutomation
{
    public class CreditCardDetailsPage
    {
        public static CreditCardDetailsCommand FillSection(IFillable sectionInfo)
        {
            return new CreditCardDetailsCommand(sectionInfo);
        }
    }

    public class CreditCardDetailsCommand
    {
        private List<IFillable> sectionsToFill;

        public CreditCardDetailsCommand(IFillable sectionInfo)
        {
            sectionsToFill = new List<IFillable>();
            sectionsToFill.Add(sectionInfo);
        }

        public CreditCardDetailsCommand FillSection(IFillable sectionInfo)
        {
            this.sectionsToFill.Add(sectionInfo);
            return this;
        }

        public void Proceed(FullElementSelector fullElementSelector, string testId, string testName)
        {
            //bool success = false;
            //while (!success)
            //{
                foreach (IFillable section in this.sectionsToFill)
                {
                    section.Fill(fullElementSelector, testId, testName);

                }

            // Trigger Pay Button Here
            Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/app-root/apply/div[2]/div/div/div[2]/div/payment-details/div/div[2]/custom-button[2]/button")));
            Driver.Instance.FindElement(By.XPath("/html/body/app-root/apply/div[2]/div/div/div[2]/div/payment-details/div/div[2]/custom-button[2]/button")).Click();
            new OpenQA.Selenium.Support.UI.WebDriverWait(Driver.Instance, System.TimeSpan.FromSeconds(20)).Until(OpenQA.Selenium.Support.UI.ExpectedConditions.UrlContains("verify-details"));

            //ReadOnlyCollection<IWebElement> unexpectedPopUps = Driver.Instance.FindElements(By.CssSelector(".alert-wrapper.ng-star-inserted"));

            //if (unexpectedPopUps.Count() == 1)//
            //{
            //    if (Driver.Instance.FindElement(By.XPath("//*[@id='mat-dialog-0']/dia-log/div/mat-toolbar/span[2]")).Text.Contains("Error"))
            //    {
            //        Driver.Instance.FindElement(By.XPath("//*[@id='mat-dialog-0']/dia-log/div/button")).Click();
            //        PaymentSelectionPage.SelectPaymentTypeAndProceed("CC");
            //    }//

            //}
            //else
            //{
            //    new WebDriverWait(Driver.Instance, System.TimeSpan.FromSeconds(50)).Until(ExpectedConditions.UrlContains("success"));
            //    success = true;
            //}
            //}
        }


    }
}
