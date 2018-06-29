using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumAutomation;
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
            Driver.Instance.FindElement(By.XPath("/html/body/chubb-dbs-app/app-quote/div/div[1]/div/div/div["+ i +"]/div/div/div[5]/button")).Click();
            new WebDriverWait(Driver.Instance, System.TimeSpan.FromSeconds(20)).Until(ExpectedConditions.UrlContains("terms"));
            ///html/body/chubb-dbs-app/app-terms-conditions/div[2]/div/div/div[2]/button
            Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath("/html/body/chubb-dbs-app/app-terms-conditions/div[2]/div/div/div[2]/button")));
            Driver.Instance.FindElement(By.XPath("/html/body/chubb-dbs-app/app-terms-conditions/div[2]/div/div/div[2]/button")).Click();
            new WebDriverWait(Driver.Instance, System.TimeSpan.FromSeconds(20)).Until(ExpectedConditions.UrlContains("summary/edit"));
        }

        public static string GetCurrentURLSlug()
        {
            string[] url = Driver.Instance.Url.Split('/');
            int len = url.Length;
            return url[len - 2] + "/" + url[len - 1];
        }
    }
}
