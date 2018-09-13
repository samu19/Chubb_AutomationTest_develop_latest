using OpenQA.Selenium;
using SeleniumAutomation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanPageAutomation
{
    public class CyberSmartPlanPage
    {

        private static string planPage = ConfigurationManager.AppSettings["url"].ToString();



        public static void GotoPlanPage()
        {
            //Driver.Instance.Manage().Window.Maximize();
            Driver.Instance.Navigate().GoToUrl(planPage);
        }
        public static void SelectPlan(int i, bool familyPlan, FullElementSelector fullElementSelector, string testId, string testName)
        {
            string applyPlanPartialElement = "//*[@id='plan-button-apply";
                                      // ']
            string planToggleElement = "//*[@id='plan-type-1']/label/div[2]";

            if (familyPlan) // toggle to family plan
                Driver.Instance.FindElement(By.XPath(planToggleElement)).Click();


            string applyPlanElement = applyPlanPartialElement + (i - 1).ToString() + "']";
            Driver.Instance.FindElement(By.XPath(applyPlanElement)).Click();

        }
    }
}
