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
        public static void SelectPaymentTypeAndProceed(string paymentType, FullElementSelector fullElementSelector, string testId, string testName)
        {
            Thread.Sleep(5000);

            if (paymentType == "CC")
            {
                
                Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath(fullElementSelector.creditCardSelectorElement)));
                Driver.Instance.FindElement(By.XPath(fullElementSelector.creditCardSelectorElement)).Click();
            }
            else
            {
                string CASAArrowElement2 = "/html/body/app-root/apply/div[2]/div/div/div[2]/div/payment-details/payment-method/mat-card/div/form/mat-radio-group/div/div[1]/div/custom-select/div/mat-form-field/div/div[1]/div/input";
                string CASAArrowElement = "/html/body/app-root/apply/div[2]/div/div/div[2]/div/payment-details/payment-method/mat-card/div/form/mat-radio-group/div/div[1]/div/custom-select/div/mat-form-field/div/div[1]/div/span[1]";
                Driver.Instance.FindElement(By.XPath(CASAArrowElement)).Click();

                string CASAListElement = ".select-list-dropdown-option.with-border.select-singleline-option.ng-star-inserted";
                ReadOnlyCollection<IWebElement> CASAList = Driver.Instance.FindElements(By.CssSelector(CASAListElement));
                Helper.WriteToCSV("Payment Details Page", "CASA filled", true, CASAList[0].Text, testId, testName);

                CASAList[0].Click();

            }

        }
    }
}
