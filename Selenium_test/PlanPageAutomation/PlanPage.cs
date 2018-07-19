using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumAutomation;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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

        public static List<double> VerifyPlanAmount()
        {
            string packagePriceElement = "package-price";
            ReadOnlyCollection<IWebElement> packagePricesFinal = Driver.Instance.FindElements(By.ClassName(packagePriceElement));
            List<double> convertedPrice = new List<double>();
            string priceInString;
            foreach(IWebElement i in packagePricesFinal)
            {
                priceInString = i.Text; 
                priceInString = priceInString.Replace(",", ""); // remove comma if it exists
                convertedPrice.Add(Convert.ToDouble(priceInString));

            }
            return convertedPrice;
        }

        public static string GetCurrentURLSlug()
        {
            string[] url = Driver.Instance.Url.Split('/');
            int len = url.Length;
            return url[len - 2] + "/" + url[len - 1];
        }
        //public static void TravelDetailPageFunctionalityTest(FullElementSelector fullElementSelector)
        //{
        //    string applyPlanElement = "/html/body/app-root/plan/div/div/div/div[1]/div[1]/div[3]/div[" + 1 + "]/div/custom-button[1]/button";
        //    Driver.Instance.FindElement(By.XPath(applyPlanElement)).Click();
        //    //new WebDriverWait(Driver.Instance, System.TimeSpan.FromSeconds(20)).Until(ExpectedConditions.UrlContains("terms"));
        //    ///html/body/chubb-dbs-app/app-terms-conditions/div[2]/div/div/div[2]/button
        //    string planPageBackElement = "//*[@id='mat-dialog-0']/custom-dialog/div/div[2]/div/div[1]/button";
        //    Driver.GetWait().Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath(planPageBackElement)));
        //    Driver.Instance.FindElement(By.XPath(planPageBackElement)).Click();
        //    new WebDriverWait(Driver.Instance, System.TimeSpan.FromSeconds(20)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("plan"));
        //    Thread.Sleep(1000);
        //    Driver.Instance.FindElement(By.XPath(applyPlanElement)).Click();
        //    string planPageProceedElement = "//*[@id='mat-dialog-1']/custom-dialog/div/div[2]/div/div[2]/button";
        //    Driver.GetWait().Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath(planPageProceedElement)));
        //    Driver.Instance.FindElement(By.XPath(planPageProceedElement)).Click();
        //    new WebDriverWait(Driver.Instance, System.TimeSpan.FromSeconds(20)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("apply/application-details"));

        //    /* Status bar should show 1/3 of the bar being filled in yellow */
        //    string statusBarElement = "/html/body/app-root/apply/mat-progress-bar/div[2]";
        //    Driver.GetWait().Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath(statusBarElement)));
        //    var statusBar = Driver.Instance.FindElement(By.XPath(statusBarElement)); //transform: scaleX(0.33);

        //    string progressBarAttribute = statusBar.GetAttribute("style"); //transform: scaleX(0.33);
        //    if (progressBarAttribute != "transform: scaleX(0.33);")
        //        Console.WriteLine("FAIL: " + "Status bar should show 1/3 of the bar being filled in yellow");

        //    string nextButtonElement = "/html/body/app-root/apply/div/div/div/div[2]/div/application-details/div/div[2]/custom-button[2]/button";
        //    var nextButton = Driver.Instance.FindElement(By.XPath(nextButtonElement));
        //    if (nextButton.Enabled == true)
        //        Console.WriteLine("FAIL: " + "Get Quote button is disabled until all mandatory fields are filled correctly");
        //    else
        //        Console.WriteLine("Get Quote button is disabled until all mandatory fields are filled correctly");
        ////}

    }
}
