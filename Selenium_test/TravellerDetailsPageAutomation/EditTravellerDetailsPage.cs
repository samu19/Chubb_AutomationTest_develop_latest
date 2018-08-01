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

namespace TravellerDetailsPageAutomation
{
    public class EditTravellerDetailsPage
    {
        public static EditTravellerDetailsCommand FillSection(IFillable sectionInfo)
        {
            return new EditTravellerDetailsCommand(sectionInfo);
        }

        public static void TravelDetailPageFunctionalityTest(FullElementSelector fullElementSelector)
        {
            string applyPlanElement = "/html/body/app-root/plan/div/div/div/div[1]/div[1]/div[3]/div[" + 1 + "]/div/custom-button[1]/button";
            Driver.Instance.FindElement(By.XPath(applyPlanElement)).Click();
            ////new WebDriverWait(Driver.Instance, System.TimeSpan.FromSeconds(20)).Until(ExpectedConditions.UrlContains("terms"));
            /////html/body/chubb-dbs-app/app-terms-conditions/div[2]/div/div/div[2]/button
            //string planPageBackElement = "//*[@id='mat-dialog-0']/custom-dialog/div/div[2]/div/div[1]/button";
            //Driver.GetWait().Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath(planPageBackElement)));
            //Driver.Instance.FindElement(By.XPath(planPageBackElement)).Click();
            //new WebDriverWait(Driver.Instance, System.TimeSpan.FromSeconds(20)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("plan"));
            Thread.Sleep(1000);
            //Driver.Instance.FindElement(By.XPath(applyPlanElement)).Click();
            //string planPageProceedElement = "//*[@id='mat-dialog-1']/custom-dialog/div/div[2]/div/div[2]/button";
            //Driver.GetWait().Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath(planPageProceedElement)));
            //Driver.Instance.FindElement(By.XPath(planPageProceedElement)).Click();
            new WebDriverWait(Driver.Instance, System.TimeSpan.FromSeconds(20)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("apply/application-details"));

            /* Status bar should show 1/3 of the bar being filled in yellow */
            string statusBarElement = "/html/body/app-root/apply/div[1]/mat-progress-bar/div[2]";
            Driver.GetWait().Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath(statusBarElement)));
            var statusBar = Driver.Instance.FindElement(By.XPath(statusBarElement)); //transform: scaleX(0.33);

            


            string progressBarAttribute = statusBar.GetAttribute("style"); //transform: scaleX(0.33);
            if (progressBarAttribute != "transform: scaleX(0.33);")
                Console.WriteLine("FAIL: " + "Status bar should show 1/3 of the bar being filled in yellow");

            string nextButtonElement = "/html/body/app-root/apply/div/div/div/div[2]/div/application-details/div/div[2]/custom-button[2]/button";
            var nextButton = Driver.Instance.FindElement(By.XPath(nextButtonElement));
            if (nextButton.Enabled == true)
                Console.WriteLine("FAIL: " + "Next button is disabled until all mandatory fields are filled correctly");
            else
                Console.WriteLine("Next button is disabled until all mandatory fields are filled correctly");

            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver.Instance;

            ReadOnlyCollection<IWebElement> travellerList = Driver.Instance.FindElements(By.CssSelector("mat-expansion-panel[formarrayname='details']"));
            for(int i = 1; i <= (travellerList.Count); i++)
            {
                CheckTravellerCountry(i, fullElementSelector, travellerList, js);

            }

        }
        

        public static ReadOnlyCollection<IWebElement> RetrieveTravelDetails()
        {
            string travelDetailsBoxElement = "/html/body/app-root/apply/div[2]/div/div/div[2]/div/application-details/view-details-box/mat-card/mat-card-content/div/div";
            ReadOnlyCollection<IWebElement> travelDetailsBox = Driver.Instance.FindElements(By.XPath(travelDetailsBoxElement));
            return travelDetailsBox;
        }

        public static void CheckTravellerCountry(int travellerIndex, FullElementSelector fullElementSelector, ReadOnlyCollection<IWebElement> travellerList, IJavaScriptExecutor js)
        {
            var indivTraveller = travellerList[travellerIndex].FindElement(By.XPath("./div/div/div[2]"));
            int retrieveIndex;

            if (travellerIndex == 0)
                retrieveIndex = 0;
            else
                retrieveIndex = travellerIndex - 1;

            js.ExecuteScript("arguments[0].scrollIntoView();", indivTraveller);


            indivTraveller.FindElement(By.XPath("./div/custom-autocomplete/div/mat-form-field/div/div[1]/div/span")).Click();

            string autocompletePopUpElement = fullElementSelector.autocompletePopupElement;
            ReadOnlyCollection<IWebElement> autocompletePopUps = Driver.Instance.FindElements(By.CssSelector(autocompletePopUpElement));
            if (autocompletePopUps.Count() == 0) // If autocomplete somehow does not popup
            {
                string popupTriggerElement = fullElementSelector.popupTriggerElement;
                Driver.Instance.FindElement(By.XPath(popupTriggerElement)).Click(); // trigger dropdown arrow
                Thread.Sleep(500);
            }

            string popupCountryElement = "./div/custom-autocomplete/div/mat-form-field/div/div[1]/div/div/div/div";
            string firstCountry = indivTraveller.FindElement(By.XPath(popupCountryElement)).Text;

            Thread.Sleep(1000);
        }
    }

    public class EditTravellerDetailsCommand
    {
        private List<IFillable> sectionsToFill;

        public EditTravellerDetailsCommand(IFillable sectionInfo)
        {
            sectionsToFill = new List<IFillable>();
            sectionsToFill.Add(sectionInfo);
        }

        public EditTravellerDetailsCommand FillSection(IFillable sectionInfo)
        {
            this.sectionsToFill.Add(sectionInfo);
            return this;
        }

        public void Proceed(FullElementSelector fullElementSelector, string testId, string testName)
        {
            foreach (IFillable section in this.sectionsToFill)
            {
                if (section != null)
                    section.Fill(fullElementSelector, testId, testName);

            }

            // Trigger Proceed Button Here /html/body/chubb-dbs-app/app-summary/app-traveller-detail/form/div[4]/div/div[2]/button
            Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='application-details-button-next']")));

            Driver.Instance.FindElement(By.XPath("//*[@id='application-details-button-next']")).Click();
            new WebDriverWait(Driver.Instance, System.TimeSpan.FromSeconds(20)).Until(ExpectedConditions.UrlContains("payment-details"));
        }


    }


}
