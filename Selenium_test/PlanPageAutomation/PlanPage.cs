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
        public static string[] SelectPlan(int i, string testId, string testName)
        {
            string applyPlanElement = "/html/body/app-root/plan/div/div/div/div[1]/div[1]/div[3]/div[" + i + "]/div/custom-button[1]/button";
            string planAmountElement = "/html/body/app-root/plan/div/div/div/div[1]/div[1]/div[3]/div[" + i + "]/div/div[2]";
            string originalAmountElement = "/html/body/app-root/plan/div/div/div/div[1]/div[1]/div[3]/div[" + i + "]/div/div[3]";
            string planAmount = Driver.Instance.FindElement(By.XPath(planAmountElement)).Text;
            string originalAmount = Driver.Instance.FindElement(By.XPath(originalAmountElement)).Text;
            Helper.WriteToCSV("Plan Page", "Retrieved Premiums", true, "Original: " + originalAmount + ", Final: " +  planAmount, testId, testName);

            Driver.Instance.FindElement(By.XPath(applyPlanElement)).Click();
            //new WebDriverWait(Driver.Instance, System.TimeSpan.FromSeconds(20)).Until(ExpectedConditions.UrlContains("terms"));
            ///html/body/chubb-dbs-app/app-terms-conditions/div[2]/div/div/div[2]/button

            //string planPageProceedElement = "//*[@id='mat-dialog-0']/custom-dialog/div/div[2]/div/div[2]/button";
            //Driver.GetWait().Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath(planPageProceedElement)));
            //Helper.WriteToCSV("Quote Page", "Selected Plan", true, null, testId, testName);


            //Driver.Instance.FindElement(By.XPath(planPageProceedElement)).Click();
            new WebDriverWait(Driver.Instance, System.TimeSpan.FromSeconds(20)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("apply/application-details"));
            Helper.WriteToCSV("Plan Page", "Proceeded to Application Details Page", true, null, testId, testName);
            string[] combinedAmount = { originalAmount, planAmount };
            return combinedAmount;
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

        public static void PlanPageFunctionalityTest(FullElementSelector fullElementSelector)
        {
            List<double> planAmounts = VerifyPlanAmount();

            if(planAmounts.Count == 3)
                Helper.WriteToCSV("Plan Page Functionality", "Pricing of the 3 plans, classic, premier and platinum is displayed", true);
            else
                Helper.WriteToCSV("Plan Page Functionality", "Pricing of the 3 plans, classic, premier and platinum is displayed", false);

            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver.Instance;

            string accordionCollapsedElement = "mat-expansion-panel";
            ReadOnlyCollection<IWebElement> planCollapsed = Driver.Instance.FindElements(By.XPath("//mat-expansion-panel[contains(@class, '" + accordionCollapsedElement + "')]"));
            bool coverageFlag = true;

            if (planCollapsed.Count != 5)
                coverageFlag = false;

            foreach (IWebElement benefit in planCollapsed)
            {
                List<string> coverageAmount = benefit.FindElement(By.XPath("./mat-expansion-panel-header/span/div/div[3]")).Text.Replace("\r\n", "|").Split('|').ToList<string>();
                if (coverageAmount.Count != 3)
                    coverageFlag = false;
            }

            if (coverageFlag)
                Helper.WriteToCSV("Plan Page Functionality", "5 key benefits are shown, all collapsed with coverage showing", true);
            else
                Helper.WriteToCSV("Plan Page Functionality", "5 key benefits are shown, all collapsed with coverage showing", false);


            foreach (IWebElement benefit in planCollapsed)
            {
                benefit.Click();
                ReadOnlyCollection<IWebElement> planItems = benefit.FindElements(By.XPath("./div/div/div"));
                js.ExecuteScript("arguments[0].scrollIntoView();", planItems[planItems.Count - 1]);
            }



            Thread.Sleep(1500);

            string accordionExpandedElement = "mat-expansion-panel-spacing";
            ReadOnlyCollection<IWebElement> planExpanded = Driver.Instance.FindElements(By.XPath("//mat-expansion-panel[contains(@class, '" + accordionExpandedElement + "')]"));
            if (planExpanded.Count == 5)
            {
                Helper.WriteToCSV("Plan Page Functionality", "Each individual key benefit can be expanded by clicking the  dropdown arrow which shows more in-depth coverage for each key benefit", true);

            }
            else
            {
                Console.WriteLine("FAIL: " + "Each individual key benefit can be expanded by clicking the  dropdown arrow which shows more in-depth coverage for each key benefit");
                Helper.WriteToCSV("Plan Page Functionality", "Each individual key benefit can be expanded by clicking the  dropdown arrow which shows more in-depth coverage for each key benefit", false);

            }

            //Collapse all
            if (Driver.Instance.FindElement(By.XPath("/html/body/app-root/plan/div/div/div/div[1]/div/div[1]/custom-label[2]/span")).Text.ToUpper() != "COLLAPSE ALL")
                Console.WriteLine("FAIL: Collapse All button is incorrectly named.");
            //js.ExecuteScript("arguments[0].scrollIntoView();", Driver.Instance.FindElement(By.XPath("/html/body/app-root/plan/div/div/div/div[1]/div/div[1]/custom-label[2]/span")));
            js.ExecuteScript("window.scrollBy(0,-2000)");

            Driver.Instance.FindElement(By.XPath("/html/body/app-root/plan/div/div/div/div[1]/div/div[1]/custom-label[2]/span")).Click();
            planExpanded = Driver.Instance.FindElements(By.XPath("//mat-expansion-panel[contains(@class, '" + accordionExpandedElement + "')]"));
            if (planExpanded.Count > 0)
            {
                Console.WriteLine("FAIL: " + "Clicking on collapse all button collapses all the key benefits");
                Helper.WriteToCSV("Plan Page Functionality", "Clicking on collapse all button collapses all the key benefits", false);

            }
            else
            {
                Helper.WriteToCSV("Plan Page Functionality", "Clicking on collapse all button collapses all the key benefits", true);

            }
            //Expand all
            if (Driver.Instance.FindElement(By.XPath("/html/body/app-root/plan/div/div/div/div[1]/div/div[1]/custom-label[2]/span")).Text.ToUpper() != "EXPAND ALL")
                Console.WriteLine("FAIL: Expand All button is incorrectly named.");
            //js.ExecuteScript("arguments[0].scrollIntoView();", Driver.Instance.FindElement(By.XPath("/html/body/app-root/plan/div/div/div/div[1]/div/div[1]/custom-label[2]/span")));
            Driver.Instance.FindElement(By.XPath("/html/body/app-root/plan/div/div/div/div[1]/div/div[1]/custom-label[2]/span")).Click();
            planExpanded = Driver.Instance.FindElements(By.XPath("//mat-expansion-panel[contains(@class, '" + accordionExpandedElement + "')]"));
            ReadOnlyCollection<IWebElement> planCarets = Driver.Instance.FindElements(By.XPath("//div[contains(@class, 'accordion-caret')]"));
            if (planExpanded.Count != 5)
            {
                Console.WriteLine("FAIL: " + "Clicking on expand all button opens all the key benefits");
                Helper.WriteToCSV("Plan Page Functionality", "Clicking on expand all button opens all the key benefits", false);

            }
            else
                Helper.WriteToCSV("Plan Page Functionality", "Clicking on expand all button opens all the key benefits", true);

            //js.ExecuteScript("arguments[0].scrollIntoView();", planCarets[0]);
            planCarets[0].Click();
            js.ExecuteScript("arguments[0].scrollIntoView();", planCarets[0]);
            Thread.Sleep(500);

            planCarets[1].Click();
            js.ExecuteScript("arguments[0].scrollIntoView();", planCarets[1]);
            Thread.Sleep(500);

            planCarets[2].Click();
            js.ExecuteScript("arguments[0].scrollIntoView();", planCarets[2]);
            Thread.Sleep(500);

            planCarets[3].Click();
            js.ExecuteScript("arguments[0].scrollIntoView();", planCarets[3]);
            Thread.Sleep(500);

            planCarets[4].Click();

            js.ExecuteScript("window.scrollBy(0,-2000)");
            Thread.Sleep(500);

            if (Driver.Instance.FindElement(By.XPath("/html/body/app-root/plan/div/div/div/div[1]/div/div[1]/custom-label[2]/span")).Text.ToUpper() != "EXPAND ALL")
            {
                Console.WriteLine("FAIL: Expand All button is incorrectly named.");
                Helper.WriteToCSV("Plan Page Functionality", "Each individual key benefit can be collapsed by clicking the  dropdown arrow to show only the 5 key benefits and their coverages", false);

            }
            else
            {
                Helper.WriteToCSV("Plan Page Functionality", "Each individual key benefit can be collapsed by clicking the  dropdown arrow to show only the 5 key benefits and their coverages", true);

            }

            var policyWording = Driver.Instance.FindElement(By.XPath("//*[@id='plan-label-policy-wording-2']"));
            policyWording.Click();
            Driver.Instance.SwitchTo().Window(Driver.Instance.WindowHandles.Last());

            new WebDriverWait(Driver.Instance, System.TimeSpan.FromSeconds(20)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("PolicyWording.pdf"));
            Helper.WriteToCSV("Plan Page Functionality", "The policy wording link will download the policy wording document in PDF and open it as a new tab in the browser for iB", true);

            Driver.Instance.Close();
            Driver.Instance.SwitchTo().Window(Driver.Instance.WindowHandles.First());


        }

    }
}
