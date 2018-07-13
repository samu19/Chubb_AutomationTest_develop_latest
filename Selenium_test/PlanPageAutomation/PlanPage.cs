using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumAutomation;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanPageAutomation
{
    public class PlanPage
    {
        public static void SelectPlan(int i)
        {
            string applyPlanElement = "/html/body/app-root/plan/div/div/div/div[1]/div[1]/div[3]/div[" + i + "]/div/custom-button[1]/button";
            Driver.Instance.FindElement(By.XPath(applyPlanElement)).Click();
            //new WebDriverWait(Driver.Instance, System.TimeSpan.FromSeconds(20)).Until(ExpectedConditions.UrlContains("terms"));
            ///html/body/chubb-dbs-app/app-terms-conditions/div[2]/div/div/div[2]/button

            string planPageProceedElement = "//*[@id='mat-dialog-0']/custom-dialog/div/div[2]/div/div[2]/button";
            Driver.GetWait().Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath(planPageProceedElement)));
            Driver.Instance.FindElement(By.XPath(planPageProceedElement)).Click();
            new WebDriverWait(Driver.Instance, System.TimeSpan.FromSeconds(20)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("apply/application-details"));
        }

        public static string GetCurrentURLSlug()
        {
            string[] url = Driver.Instance.Url.Split('/');
            int len = url.Length;
            return url[len - 2] + "/" + url[len - 1];
        }
    }
}
