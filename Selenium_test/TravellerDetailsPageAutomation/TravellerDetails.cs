using OpenQA.Selenium;
using SeleniumAutomation;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TravellerDetailsPageAutomation
{
    public class TravellerDetails : IFillable
    {
        public bool applicantIsTraveller = true;
        public List<TravellerDetail> travellerDetailsList;

        public void Fill(FullElementSelector fullElementSelector, string testId, string testName)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver.Instance;
            Thread.Sleep(3000);
            if (!applicantIsTraveller)
            {//toggle off (only for individual)
                string toggleElement = "/html/body/app-root/apply/div/div/div/div[2]/div/application-details/applicant-detail/form/mat-card/mat-card-header/div/mat-card-title/span";
                var applicantIsTravellerToggle = Driver.Instance.FindElement(By.XPath(toggleElement));

                var scrollForToggle = Driver.Instance.FindElement(By.XPath("/html/body/app-root/apply/div[2]/div/div/div[2]/div/application-details/view-details-box/mat-card/mat-card-header"));
                js.ExecuteScript("arguments[0].scrollIntoView();", scrollForToggle);
                Thread.Sleep(1000);

                applicantIsTravellerToggle.Click();
                Thread.Sleep(2000);
            }

            int totalCount = travellerDetailsList.Count;
            if (totalCount == 0 && !applicantIsTraveller)
            {
                Console.WriteLine("Please provide traveller detail");
                return; // if nothing, skip
            }

            ReadOnlyCollection<IWebElement> travellerList = Driver.Instance.FindElements(By.CssSelector("mat-expansion-panel[formarrayname='details']"));

            int displayedTravellerCount = travellerList.Count();

            if (!applicantIsTraveller) // individual, one traveller detail, start from traveller one
            {
                FillTravellerDetails(0, fullElementSelector, travellerList, js);
                Helper.WriteToCSV("Applicant Details Page", "Traveller 1 filled", true, null, testId, testName);

                //var indivTraveller = travellerList[0].FindElement(By.XPath("./div/div/div[2]"));
                //indivTraveller.FindElement(By.XPath("./custom-input[1]/div/mat-form-field/div/div[1]/div/input")).SendKeys(travellerDetailsList[0].tNRIC);
                //indivTraveller.FindElement(By.XPath("./custom-input[2]/div/mat-form-field/div/div[1]/div/input")).SendKeys(travellerDetailsList[0].tFullName);

                //indivTraveller.FindElement(By.XPath("./div/custom-input/div/mat-form-field/div/div[1]/div/input")).SendKeys(travellerDetailsList[0].tDOB);
                //indivTraveller.FindElement(By.XPath("./div/custom-autocomplete/div/mat-form-field/div/div[1]/div/input")).SendKeys(travellerDetailsList[0].tNationality);

                //string autocompletePopUpElement = fullElementSelector.autocompletePopupElement;
                //ReadOnlyCollection<IWebElement> autocompletePopUps = Driver.Instance.FindElements(By.CssSelector(autocompletePopUpElement));
                //if (autocompletePopUps.Count() == 0) // If autocomplete somehow does not popup
                //{
                //    string popupTriggerElement = fullElementSelector.popupTriggerElement;
                //    Driver.Instance.FindElement(By.XPath(popupTriggerElement)).Click(); // trigger dropdown arrow
                //    Thread.Sleep(500);
                //}

                //string popupCountryElement = fullElementSelector.popupCountryElement;
                //Driver.Instance.FindElement(By.XPath(popupCountryElement)).Click();

                //Thread.Sleep(1000);
            }
            else // start from traveller 2
            {
                for (int i = 1; i <= displayedTravellerCount-1; i++)
                {
                    FillTravellerDetails(i, fullElementSelector, travellerList, js);
                    Helper.WriteToCSV("Applicant Details Page", "Traveller" + (i+1) + "filled", true, null, testId, testName);

                    //    foreach (TravellerDetail t in travellerDetailsList)
                    //    {
                    //        /* traveller info */
                    //        Driver.Instance.FindElement(By.Id("aNric")).SendKeys(t.tNRIC);
                    //        Driver.Instance.FindElement(By.Id("aFullName")).SendKeys(t.tFullName);
                    //        Driver.Instance.FindElement(By.Id("aDob")).SendKeys(t.tDOB);
                    //        Driver.Instance.FindElement(By.XPath("//*[@id='applicant-Nationality']/div/div[1]/div/input")).SendKeys(t.tNationality);
                    //        Thread.Sleep(1500);

                    //        ReadOnlyCollection<IWebElement> autocompletePopUps = Driver.Instance.FindElements(By.CssSelector(".autocomplete-popup.show"));
                    //        if (autocompletePopUps.Count() == 0) //If autocomplete somehow does not trigger
                    //        {
                    //            Driver.Instance.FindElement(By.XPath("//*[@id='applicant-Nationality']/div/div[1]/div/span[1]")).Click(); //Clicks the dropdown arrow
                    //            Thread.Sleep(500);
                    //        }
                    //        Driver.Instance.FindElement(By.XPath("//*[@id='applicant-Nationality']/div/div[1]/div/div/div/div")).Click(); // Selects the autocomplete popup
                    //        travellerCount++;
                    //    }
                }

            }
            


        }

        public void FillTravellerDetails(int travellerIndex, FullElementSelector fullElementSelector, ReadOnlyCollection<IWebElement> travellerList, IJavaScriptExecutor js)
        {
            var indivTraveller = travellerList[travellerIndex].FindElement(By.XPath("./div/div/div[2]"));
            int retrieveIndex;

            if (travellerIndex == 0)
                retrieveIndex = 0;
            else
                retrieveIndex = travellerIndex - 1;

            js.ExecuteScript("arguments[0].scrollIntoView();", travellerList[travellerIndex]);

            indivTraveller.FindElement(By.XPath("./custom-input[1]/div/mat-form-field/div/div[1]/div/input")).SendKeys(travellerDetailsList[retrieveIndex].tNRIC);
            indivTraveller.FindElement(By.XPath("./custom-input[2]/div/mat-form-field/div/div[1]/div/input")).SendKeys(travellerDetailsList[retrieveIndex].tFullName);

            indivTraveller.FindElement(By.XPath("./div/custom-input/div/mat-form-field/div/div[1]/div/input")).SendKeys(travellerDetailsList[retrieveIndex].tDOB);
            indivTraveller.FindElement(By.XPath("./div/custom-autocomplete/div/mat-form-field/div/div[1]/div/input")).SendKeys(travellerDetailsList[retrieveIndex].tNationality);

            string autocompletePopUpElement = fullElementSelector.autocompletePopupElement;
            ReadOnlyCollection<IWebElement> autocompletePopUps = Driver.Instance.FindElements(By.CssSelector(autocompletePopUpElement));
            if (autocompletePopUps.Count() == 0) // If autocomplete somehow does not popup
            {
                string popupTriggerElement = fullElementSelector.popupTriggerElement;
                Driver.Instance.FindElement(By.XPath(popupTriggerElement)).Click(); // trigger dropdown arrow
                Thread.Sleep(500);
            }

            string popupCountryElement = "./div/custom-autocomplete/div/mat-form-field/div/div[1]/div/div/div/div";
            indivTraveller.FindElement(By.XPath(popupCountryElement)).Click();

            Thread.Sleep(1000);
        }
    }
}
