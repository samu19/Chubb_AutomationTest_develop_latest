using OpenQA.Selenium;
using SeleniumAutomation;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellerDetailsPageAutomation
{
    public class PromoCode : IFillable
    {
        public string promoCode; // This is for Cybersmart

        public void Fill(FullElementSelector fullElementSelector, string testId, string testName)
        {
            string promoCodeElement = "//*[@id='application-promo-code']/mat-form-field/div/div[1]/div/input";
            string promoCodeApplyElement = "/html/body/app-root/apply/div[2]/div/div/div[2]/div/application-details/promo-code/form/mat-card/mat-card-content/div/custom-button/button";

            Driver.Instance.FindElement(By.XPath(promoCodeElement)).SendKeys(promoCode);

            Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath(promoCodeApplyElement)));
            Driver.Instance.FindElement(By.XPath(promoCodeApplyElement)).Click();
        }
    }
}
