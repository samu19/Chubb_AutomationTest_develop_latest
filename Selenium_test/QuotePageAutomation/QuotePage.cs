using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumAutomation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuotePageAutomation
{
    public class QuotePage
    {
        private static string quotePage = ConfigurationManager.AppSettings["url"].ToString();



        public static void GotoQuotePage()
        {
            //Driver.Instance.Manage().Window.Maximize();
            Driver.Instance.Navigate().GoToUrl(quotePage);
        }


        public static string GetCurrentURLSlug()
        {
            return Driver.Instance.Url.Split('/').Last();
        }


        public static QuoteCommand FillSection(IFillable sectionInfo)
        {
            return new QuoteCommand(sectionInfo);
        }

        public static void QuoteFunctionalityTest(FullElementSelector fullElementSelector, bool isSingleTrip = true)
        {
            if (isSingleTrip)
            {
                /* Single Trip radio button is defaulted and selected */
                string tripTypeRadioElement = "//mat-radio-button[@class='mat-radio-button mat-accent mat-radio-checked']";
                var tripTypeRadio = Driver.Instance.FindElement(By.XPath(tripTypeRadioElement));
                if (tripTypeRadio.Text != "Single Trip")
                {
                    Console.WriteLine("FAIL: " + "Single Trip radio button is defaulted and selected");
                    Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") +"Quote Functionality", "Single Trip radio button is defaulted and selected", false);

                }
                else
                {
                    Console.WriteLine("Single Trip radio button is defaulted and selected");
                    Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") +"Quote Functionality", "Single Trip radio button is defaulted and selected", true);

                }
            }
            else
            {
                string tripTypeElement = fullElementSelector.tripTypeElement;
                //To change - choose Annual Trip radio button
                Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.Id(tripTypeElement)));
                Driver.Instance.FindElement(By.Id(tripTypeElement)).Click();
                //Driver.ClickWithRetry(By.Id(tripTypeElement));
            }


            /* Get Quote button is disabled until all mandatory fields are filled correctly */
            string quoteButtonElement = fullElementSelector.quoteButtonElement;
            var quoteButton = Driver.Instance.FindElement(By.XPath(quoteButtonElement));
            if (quoteButton.Enabled == true)
            {
                Console.WriteLine("FAIL: " + "Get Quote button is disabled until all mandatory fields are filled correctly");
                Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") +"Quote Functionality", "Get Quote button is disabled until all mandatory fields are filled correctly", false);
            }
            else
            {
                Console.WriteLine("Get Quote button is disabled until all mandatory fields are filled correctly");
                Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") +"Quote Functionality", "Get Quote button is disabled until all mandatory fields are filled correctly", true);
            }

            if (isSingleTrip)
            {
                /* Depart date is defaulted to today's date */
                var departDate = Driver.Instance.FindElement(By.XPath(fullElementSelector.departDateElement));
                string selectedDepartDate = departDate.GetAttribute("value");
                DateTime _selectedDepartDate = DateTime.ParseExact(selectedDepartDate, "dd MMM yyyy", null);

                DateTime CurrentDate = DateTime.Today;
                if (CurrentDate.Date != _selectedDepartDate.Date)
                {
                    Console.WriteLine("FAIL: " + "Depart date is defaulted to today's date");
                    Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") +"Quote Functionality", "Depart date is defaulted to today's date", false);

                }
                else
                {
                    Console.WriteLine("Depart date is defaulted to today's date");
                    Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") +"Quote Functionality", "Depart date is defaulted to today's date", true);

                }
                /*
                departDate.Click();
                DateTime _Date = DateTime.Today;
                string Day = _Date.Day.ToString();
                string formattedDate = _Date.ToString("MMM yyyy");
                string departMonthYearElement = fullElementSelector.departDateMonthElement;
                Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath(departMonthYearElement)));
                string selectedDepartDay;
                if (formattedDate.ToUpper() == Driver.Instance.FindElement(By.XPath(departMonthYearElement)).Text)
                    selectedDepartDay = Driver.Instance.FindElement(By.XPath("//div[@class='mat-calendar-body-cell-content mat-calendar-body-selected mat-calendar-body-today']")).Text;
                else
                    throw new Exception("Error!");

                if (selectedDepartDay != Day)
                    throw new Exception("Error!");

                Driver.Instance.FindElement(By.XPath("//div[@class='mat-calendar-body-cell-content mat-calendar-body-selected mat-calendar-body-today']")).Click();
                */

                /* Return date is defaulted to 5 days after depart date */
                var returnDate = Driver.Instance.FindElement(By.XPath(fullElementSelector.returnDateElement));
                string selectedReturnDate = returnDate.GetAttribute("value");
                DateTime _selectedReturnDate = DateTime.ParseExact(selectedReturnDate, "dd MMM yyyy", null);

                double defaultDaysDiff = (_selectedReturnDate - CurrentDate).TotalDays;
                if (defaultDaysDiff != 4)
                {
                    Console.WriteLine("FAIL: " + "Return date is defaulted to 5 days after depart date");
                    Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") +"Quote Functionality", "Return date is defaulted to 5 days after depart date", false);
                }
                else
                {
                    Console.WriteLine("Return date is defaulted to 5 days after depart date");
                    Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") +"Quote Functionality", "Return date is defaulted to 5 days after depart date", true);

                }
                /*
                returnDate.Click();
                string returnMonthYearElement = fullElementSelector.returnDateMonthElement;
                Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath(returnMonthYearElement)));
                string selectedReturnDay;
                string selectedReturnMonthYear = Driver.Instance.FindElement(By.XPath(returnMonthYearElement)).Text;
                selectedReturnDay = Driver.Instance.FindElement(By.XPath("//div[@class='mat-calendar-body-cell-content mat-calendar-body-selected']")).Text;

                string combinedReturnDate = selectedReturnDay + " " + selectedReturnMonthYear;
                DateTime selectedReturnDate = DateTime.ParseExact(combinedReturnDate, "dd MMM yyyy", null);

                double answer = (selectedReturnDate - _Date).TotalDays;
                if (answer != 4)
                    throw new Exception("Error!");

                Driver.Instance.FindElement(By.XPath("//div[@class='mat-calendar-body-cell-content mat-calendar-body-selected']")).Click();
                */


                /* Clicking the text box will bring up the calendar picker. Depart Date cannot be selected more than 180 days away from today */
                string maxdepartDate = departDate.GetAttribute("ng-reflect-max");

                DateTime maxDepartDate = DateTime.ParseExact(maxdepartDate.Substring(4, 11), "MMM dd yyyy", null);
                double departDiff = (maxDepartDate - CurrentDate).TotalDays;
                if (departDiff != 179)
                    Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") +"Quote Functionality", "Max Depart Date is not 180 days later", false);

                QuoteData.DateHandler(maxDepartDate.AddDays(1), fullElementSelector.departDateElement, fullElementSelector.departDateMonthElement, fullElementSelector.departDateDecreaseElement, fullElementSelector.departDateIncreaseElement, fullElementSelector.dateDayElement);
                QuoteData.DateHandler(maxDepartDate.AddDays(1), fullElementSelector.departDateElement, fullElementSelector.departDateMonthElement, fullElementSelector.departDateDecreaseElement, fullElementSelector.departDateIncreaseElement, fullElementSelector.dateDayElement);
                string checkSelectedDepartDate = departDate.GetAttribute("value");
                DateTime _checkSelectedDepartDate = DateTime.ParseExact(checkSelectedDepartDate, "dd MMM yyyy", null);
                double doubleCheckDepartDiff = (_checkSelectedDepartDate - CurrentDate).TotalDays;
                if (doubleCheckDepartDiff > 179)
                {
                    Console.WriteLine("FAIL: " + "Depart Date cannot be selected more than 180 days away from today");
                    Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") +"Quote Functionality", "Depart Date cannot be selected more than 180 days away from today", false);

                }
                else
                {
                    Console.WriteLine("Depart Date cannot be selected more than 180 days away from today");
                    Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") +"Quote Functionality", "Depart Date cannot be selected more than 180 days away from today", true);
                }

                /* Clicking the text box will bring up the calendar picker. Return Date cannot be selected more than 180 days away from depart date */
                string maxreturnDate = returnDate.GetAttribute("ng-reflect-max");
                //string newSelectedDepartDate = departDate.GetAttribute("value");
                //DateTime _newSelectedDepartDate = DateTime.ParseExact(newSelectedDepartDate, "dd MMM yyyy", null);
                DateTime maxReturnDate = DateTime.ParseExact(maxreturnDate.Substring(4, 11), "MMM dd yyyy", null);
                double returnDiff = (maxReturnDate - _checkSelectedDepartDate).TotalDays;
                if (returnDiff != 179)
                    Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") +"Quote Functionality", "Max Return Date is not 180 days later", false);
                QuoteData.DateHandler(maxReturnDate.AddDays(1), fullElementSelector.returnDateElement, fullElementSelector.returnDateMonthElement, fullElementSelector.returnDateDecreaseElement, fullElementSelector.returnDateIncreaseElement, fullElementSelector.dateDayElement);
                QuoteData.DateHandler(maxReturnDate.AddDays(1), fullElementSelector.returnDateElement, fullElementSelector.returnDateMonthElement, fullElementSelector.returnDateDecreaseElement, fullElementSelector.returnDateIncreaseElement, fullElementSelector.dateDayElement);
                string checkSelectedReturnDate = returnDate.GetAttribute("value");
                DateTime _checkSelectedReturnDate = DateTime.ParseExact(checkSelectedReturnDate, "dd MMM yyyy", null);
                double doubleCheckReturnDiff = (_checkSelectedReturnDate - _checkSelectedDepartDate).TotalDays;
                if (doubleCheckReturnDiff > 179)
                {
                    Console.WriteLine("FAIL: " + "Return Date cannot be selected more than 180 days away from depart date");
                    Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") +"Quote Functionality", "Return Date cannot be selected more than 180 days away from depart date", false);
                }
                else
                {
                    Console.WriteLine("Return Date cannot be selected more than 180 days away from depart date");
                    Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") +"Quote Functionality", "Return Date cannot be selected more than 180 days away from depart date", true);
                }

                /* Able to type and filter for countries based on the 1st letter typed in */

                string countriesElement = fullElementSelector.countriesElement;
                Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath(countriesElement)));
                var destination = Driver.Instance.FindElement(By.XPath(countriesElement));

                char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWYZ".ToCharArray();
                string currentLetter = "A";
                foreach (char a in alpha)
                {
                    currentLetter = a.ToString();
                    destination.SendKeys(a.ToString());

                    string autocompletePopUpElement = fullElementSelector.autocompletePopupElement;
                    ReadOnlyCollection<IWebElement> autocompletePopUps = Driver.Instance.FindElements(By.CssSelector(autocompletePopUpElement));
                    if (autocompletePopUps.Count == 0)
                    {
                        Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") +"Quote Functionality", "Able to type and filter for countries based on the 1st letter typed in", false);
                        break;
                    }
                    //Driver.GetWait().Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".autocomplete-popup.show")));
                    destination.SendKeys(Keys.Backspace);
                }
                if (currentLetter == "Z")
                    Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") +"Quote Functionality", "Able to type and filter for countries based on the 1st letter typed in", true);


                /* Able to select up to 5 different countries and not duplicated */
                for (int i = 0; i < 5; i++)
                {
                    destination.SendKeys("A");
                    //Thread.Sleep(1000); // 
                    string autocompletePopUpElement = fullElementSelector.autocompletePopupElement;
                    string popupCountryElement = fullElementSelector.popupCountryElement;
                    Driver.Instance.FindElement(By.XPath(popupCountryElement)).Click();
                }
                //chip ng-star-inserted
                ReadOnlyCollection<IWebElement> insertedCountries = Driver.Instance.FindElements(By.CssSelector(".chip.ng-star-inserted"));
                if (insertedCountries.Count == 5)
                    Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") +"Quote Functionality", "Able to select up to 5 different countries and not duplicated", true);
                else
                    Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") +"Quote Functionality", "Able to select up to 5 different countries and not duplicated", false);

            }
            else // annual trip for region + date
            {
                string regionElement = fullElementSelector.annualRegionElement;
                Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath(regionElement)));
                var region = Driver.Instance.FindElement(By.XPath(regionElement));
                region.Click();
                string regionListElement = fullElementSelector.annualRegionListElement;
                var regionList = Driver.Instance.FindElement(By.CssSelector(regionListElement));
                ReadOnlyCollection<IWebElement> _regionList = regionList.FindElements(By.XPath("./div/div"));

                foreach(IWebElement ii in _regionList)
                {
                    ii.Click();
                    region.Click();
                }
                region.Click();
                Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") +"Quote Functionality", "Able to select either of the 3 regions in the dropdown menu", true);

                var departDate = Driver.Instance.FindElement(By.XPath(fullElementSelector.annualDateElement));
                string selectedDepartDate = departDate.GetAttribute("value");
                DateTime _selectedDepartDate = DateTime.ParseExact(selectedDepartDate, "dd MMM yyyy", null);

                DateTime CurrentDate = DateTime.Today;
                if (CurrentDate.Date != _selectedDepartDate.Date)
                {
                    Console.WriteLine("FAIL: " + "Depart date is defaulted to today's date");
                    Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") +"Quote Functionality", "Depart date is defaulted to today's date", false);

                }
                else
                {
                    Console.WriteLine("Depart date is defaulted to today's date");
                    Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") +"Quote Functionality", "Depart date is defaulted to today's date", true);

                }

                string maxdepartDate = departDate.GetAttribute("ng-reflect-max");

                DateTime maxDepartDate = DateTime.ParseExact(maxdepartDate.Substring(4, 11), "MMM dd yyyy", null);
                double departDiff = (maxDepartDate - CurrentDate).TotalDays;
                if (departDiff != 179)
                    Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") +"Quote Functionality", "Max Depart Date is not 180 days later", false);

                QuoteData.DateHandler(maxDepartDate.AddDays(1), fullElementSelector.annualDateElement, fullElementSelector.annualDateMonthElement, fullElementSelector.annualDateDecreaseElement, fullElementSelector.annualDateIncreaseElement, fullElementSelector.dateDayElement);
                QuoteData.DateHandler(maxDepartDate.AddDays(1), fullElementSelector.annualDateElement, fullElementSelector.annualDateMonthElement, fullElementSelector.annualDateDecreaseElement, fullElementSelector.annualDateIncreaseElement, fullElementSelector.dateDayElement);
                string checkSelectedDepartDate = departDate.GetAttribute("value");
                DateTime _checkSelectedDepartDate = DateTime.ParseExact(checkSelectedDepartDate, "dd MMM yyyy", null);
                double doubleCheckDepartDiff = (_checkSelectedDepartDate - CurrentDate).TotalDays;
                if (doubleCheckDepartDiff > 179)
                {
                    Console.WriteLine("FAIL: " + "Depart Date cannot be selected more than 180 days away from today");
                    Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") +"Quote Functionality", "Departure date can only be selected up to 180 days ahead of today", false);

                }
                else
                {
                    Console.WriteLine("Depart Date cannot be selected more than 180 days away from today");
                    Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") +"Quote Functionality", "Departure date can only be selected up to 180 days ahead of today", true);
                }

            }

            /* Individual: Can only key in one age */
            string adultAgeElement = fullElementSelector.adultAgeElement;


            Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath(adultAgeElement)));
            var adultAge = Driver.Instance.FindElement(By.XPath(adultAgeElement));
            adultAge.SendKeys("23,23");
            List<string> ages = adultAge.GetAttribute("value").Split(',').ToList<string>();
            if (ages.Count() != 1)
            {
                Console.WriteLine("FAIL: " + "Individual: Can only key in one age");
                Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") +"Quote Functionality", "Individual: Can only key in one age", false, "DISPLAYED: " + adultAge.GetAttribute("value"));
            }
            else
            {
                Console.WriteLine("Individual: Can only key in one age");
                Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") +"Quote Functionality", "Individual: Can only key in one age", true);
            }

                /* Dropdown menu with 4 cover types: Individual, Couple, Family, Group */


            string coverTypeElement = fullElementSelector.coverTypeElement;
            var coverType = Driver.Instance.FindElement(By.XPath(coverTypeElement));
            coverType.Click();

            //Driver.Instance.FindElement(By.XPath(coverTypeElement)).Click(); ////div[@class='select-list-dropdown-option with-border select-singleline-option ng-star-inserted']/div
            string coverTypeDropDownElement = fullElementSelector.coverTypeDropDownElement;
            ReadOnlyCollection<IWebElement> coverTypeOptions = Driver.Instance.FindElements(By.XPath(coverTypeDropDownElement));
            string[] expectedCoverType = { "Individual", "Couple", "Family", "Group" };
            int coverTypeLimit = coverTypeOptions.Count();
            if (isSingleTrip)
            {
                if(coverTypeLimit != 4)
                    Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") + "Quote Functionality", "Dropdown menu with 4 cover types: Individual, Couple, Family, Group", false);

            }
            else
            {
                if (coverTypeLimit != 3)
                    Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") + "Quote Functionality", "Dropdown menu with 3 cover types: Individual, Couple, Family", false);

            }
            for (int j = 0; j < coverTypeLimit; j++)
            {
                if (coverTypeOptions[j].Text != expectedCoverType[j])
                    Console.WriteLine("FAIL: " + "Dropdown menu with 4 cover types: Individual, Couple, Family, Group");
            }

            /* Couple: Can only key in up to two ages, separated by commas */
            coverTypeOptions[1].Click();
            adultAge.SendKeys("23,23,23");
            ages = adultAge.GetAttribute("value").Split(',').ToList<string>();
            if (ages.Count() != 2)
            {
                Console.WriteLine("FAIL: " + "Couple: Can only key in up to two ages, separated by commas");
                Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") +"Quote Functionality", "Couple: Can only key in up to two ages, separated by commas", false, "DISPLAYED: " + adultAge.GetAttribute("value"));
            }

                

            if (isSingleTrip)
            {
                coverType.Click();
                /* Group: Can only key in up to ten ages, separated by commas */
                coverTypeOptions[3].Click();
                adultAge.SendKeys("21,22,23,24,25,26,27,28,29,30,31");
                ages = adultAge.GetAttribute("value").Split(',').ToList<string>();
                if (ages.Count() != 10)
                {
                    Console.WriteLine("FAIL: " + "Group: Can only key in up to ten ages, separated by commas");
                    Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") +"Quote Functionality", "Group: Can only key in up to ten ages, separated by commas", false, "DISPLAYED: " + adultAge.GetAttribute("value"));

                }
            }
            /* Family: Can only key in up to two ages, separated by commas */
            coverType.Click();
            coverTypeOptions[2].Click();
            adultAge.SendKeys("21,22,23");
            ages = adultAge.GetAttribute("value").Split(',').ToList<string>();
            if (ages.Count() != 2)
            {
                Console.WriteLine("FAIL: " + "Family: Can only key in up to two ages, separated by commas");
                Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") +"Quote Functionality", "Family: Can only key in up to two ages, separated by commas", false, "DISPLAYED: " + adultAge.GetAttribute("value"));
            }

                /* Family: Can only key in up to five ages, separated by commas */
                string childAgeElement = fullElementSelector.childAgeElement;
            Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath(childAgeElement)));
            var childAge = Driver.Instance.FindElement(By.XPath(childAgeElement));
            childAge.SendKeys("11,12,13,14,15,16");
            ages = childAge.GetAttribute("ng-reflect-value").Split(',').ToList<string>();
            if (ages.Count() != 5)
            {
                Console.WriteLine("FAIL: " + "Family: Can only key in up to five ages, separated by commas");
                Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") +"Quote Functionality", "Family: Can only key in up to five ages, separated by commas", false, "DISPLAYED: " + childAge.GetAttribute("value"));
            }


        }

        public static void QuoteToolTipTest(FullElementSelector fullElementSelector, JObject toolTipMsg, bool isSingleTrip = true)
        {
            if (!isSingleTrip) // If Annual Trip
            {
                string tripTypeElement = fullElementSelector.tripTypeElement;
                //To change - choose Annual Trip radio button
                Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.Id(tripTypeElement)));
                Driver.Instance.FindElement(By.Id(tripTypeElement)).Click();
                //Driver.ClickWithRetry(By.Id(tripTypeElement));
            }

            /* Destination Tool Tip */
            if (isSingleTrip)
            {
                string countriesElement = fullElementSelector.countriesElement;
                Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath(countriesElement)));
                var destination = Driver.Instance.FindElement(By.XPath(countriesElement));
                destination.SendKeys("ugan");

                string autocompletePopUpElement = fullElementSelector.autocompletePopupElement;
                ReadOnlyCollection<IWebElement> autocompletePopUps = Driver.Instance.FindElements(By.CssSelector(autocompletePopUpElement));
                if (autocompletePopUps.Count() == 0) // If autocomplete somehow does not popup
                {
                    string popupTriggerElement = fullElementSelector.popupTriggerElement;
                    Driver.Instance.FindElement(By.XPath(popupTriggerElement)).Click(); // trigger dropdown arrow
                    Thread.Sleep(500);
                }

                string popupCountryElement = fullElementSelector.popupCountryElement;
                Driver.Instance.FindElement(By.XPath(popupCountryElement)).Click();

                ///html/body/app-root/quote/div/div[2]/div/div[2]/div/form/custom-autocomplete/div/mat-form-field/div/div[1]/div/span/label/mat-label/button
                string destinationToolTipElement = "//*[@id='quote-autocomplete-countries']/div/div[1]/div/span/label/mat-label/button";
                Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath(destinationToolTipElement)));
                Driver.Instance.FindElement(By.XPath(destinationToolTipElement)).Click();
                string destinationToolTipMessage = Driver.Instance.FindElement(By.XPath("//*[@id='mat-dialog-0']/custom-dialog/div/div/div/span[2]")).Text;

                var destinationToolTipTitle = Driver.Instance.FindElement(By.XPath("//*[@id='mat-dialog-0']/custom-dialog/div/mat-toolbar"));
                string destinationTitleToolTipMessage = destinationToolTipTitle.FindElement(By.XPath("./span")).Text;

                Console.WriteLine(destinationTitleToolTipMessage + Environment.NewLine + "--------------" + Environment.NewLine + destinationToolTipMessage);

                //if (!((string)toolTipMsg["single tooltip"][destinationTitleToolTipMessage]).Equals(destinationToolTipMessage))
                //    Console.WriteLine("FAIL: " + destinationTitleToolTipMessage);

                CheckToolTip(isSingleTrip, destinationTitleToolTipMessage, destinationToolTipMessage, toolTipMsg);

                Driver.Instance.FindElement(By.XPath("//*[@id='mat-dialog-0']/custom-dialog/div/div/button")).Click();

                
            }
            else
            {///html/body/app-root/quote/div/div[2]/div/div[2]/div/form/custom-select/div/mat-form-field/div/div[1]/div/span[2]/label/mat-label/button
                string regionToolTipElement = "//*[@id='quote-select-region']/div/div[1]/div/span[2]/label/mat-label/button";
                Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath(regionToolTipElement)));
                Driver.Instance.FindElement(By.XPath(regionToolTipElement)).Click();

                var regionToolTipMessage = Driver.Instance.FindElement(By.XPath("//*[@id='mat-dialog-0']/custom-dialog/div/div"));

                var regionToolTipTitle = Driver.Instance.FindElement(By.XPath("//*[@id='mat-dialog-0']/custom-dialog/div/mat-toolbar"));
                string regionTitleToolTipMessage = regionToolTipTitle.FindElement(By.XPath("./span")).Text;
                Console.WriteLine(regionTitleToolTipMessage + Environment.NewLine + "--------------");
                //foreach (var i in coverTypeToolTipMessage)
                //{
                //    Console.WriteLine(i.FindElement(By.XPath(".//span[1]")).Text + ": " + i.FindElement(By.XPath(".//span[2]")).Text);
                //}
                ReadOnlyCollection<IWebElement> regionToolTipAll = regionToolTipMessage.FindElements(By.XPath("./div"));
                string tempRegionToolTip = null;
                foreach (IWebElement i in regionToolTipAll)
                {
                    tempRegionToolTip += ("\n" + i.Text);
                    //Console.WriteLine(i.Text + Environment.NewLine);
                }
                Console.WriteLine(tempRegionToolTip);

                //if (!((string)toolTipMsg["annual tooltip"][regionTitleToolTipMessage]).Equals(tempRegionToolTip))
                //    Console.WriteLine("FAIL: " + regionTitleToolTipMessage);

                CheckToolTip(isSingleTrip, regionTitleToolTipMessage, tempRegionToolTip, toolTipMsg);


                Driver.Instance.FindElement(By.XPath("//*[@id='mat-dialog-0']/custom-dialog/div/div/button")).Click();

            }

            Thread.Sleep(500);
            Console.WriteLine(Environment.NewLine);

            string departDateToolTipElement;

            if (isSingleTrip)
                departDateToolTipElement = "//*[@id='quote-date-picker-single']/div[1]/mat-form-field/div/div[1]/div[1]/span/label/mat-label/button";
            else
                departDateToolTipElement = "//*[@id='quote-date-picker-multi']/div/div[1]/div[1]/span/label/mat-label/button";

            Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath(departDateToolTipElement)));
            Driver.Instance.FindElement(By.XPath(departDateToolTipElement)).Click();
            string departDateToolTipMessage = Driver.Instance.FindElement(By.XPath("//*[@id='mat-dialog-1']/custom-dialog/div/div/div/span[2]")).Text;

            var departDateToolTipTitle = Driver.Instance.FindElement(By.XPath("//*[@id='mat-dialog-1']/custom-dialog/div/mat-toolbar"));
            string departDateTitleToolTipMessage = departDateToolTipTitle.FindElement(By.XPath("./span")).Text;

            Console.WriteLine(departDateTitleToolTipMessage + Environment.NewLine + "--------------" + Environment.NewLine +   departDateToolTipMessage);
            Driver.Instance.FindElement(By.XPath("//*[@id='mat-dialog-1']/custom-dialog/div/div/button")).Click();

            CheckToolTip(isSingleTrip, departDateTitleToolTipMessage, departDateToolTipMessage, toolTipMsg);


            //if (isSingleTrip)
            //{
            //    if (!((string)toolTipMsg["single tooltip"][departDateTitleToolTipMessage]).Equals(departDateToolTipMessage))
            //        Console.WriteLine("FAIL: " + departDateTitleToolTipMessage);
            //}
            //else
            //{
            //    if (!((string)toolTipMsg["annual tooltip"][departDateTitleToolTipMessage]).Equals(departDateToolTipMessage))
            //        Console.WriteLine("FAIL: " + departDateTitleToolTipMessage);
            //}
            Thread.Sleep(500);
            Console.WriteLine(Environment.NewLine);

            if (isSingleTrip)
            {
                string returnDateToolTipElement = "//*[@id='quote-date-picker-single']/div[2]/mat-form-field/div/div[1]/div[1]/span/label/mat-label/button";
                Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath(returnDateToolTipElement)));
                Driver.Instance.FindElement(By.XPath(returnDateToolTipElement)).Click();
                string returnDateToolTipMessage = Driver.Instance.FindElement(By.XPath("//*[@id='mat-dialog-2']/custom-dialog/div/div/div/span[2]")).Text;

                var returnDateToolTipTitle = Driver.Instance.FindElement(By.XPath("//*[@id='mat-dialog-2']/custom-dialog/div/mat-toolbar"));
                string returnDateTitleToolTipMessage = returnDateToolTipTitle.FindElement(By.XPath("./span")).Text;

                Console.WriteLine(returnDateTitleToolTipMessage + Environment.NewLine + "--------------" + Environment.NewLine + returnDateToolTipMessage);
                Driver.Instance.FindElement(By.XPath("//*[@id='mat-dialog-2']/custom-dialog/div/div/button")).Click();


                //if (!((string)toolTipMsg["single tooltip"][returnDateTitleToolTipMessage]).Equals(returnDateToolTipMessage))
                //    Console.WriteLine("FAIL: " + returnDateTitleToolTipMessage);

                CheckToolTip(isSingleTrip, returnDateTitleToolTipMessage, returnDateToolTipMessage, toolTipMsg);

                //

                Thread.Sleep(500);
                Console.WriteLine(Environment.NewLine);
            }
            
            string coverTypeToolTipElement = "//*[@id='quote-date-select-cover']/div/div[1]/div/span[2]/label/mat-label/button";
            Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath(coverTypeToolTipElement)));
            Driver.Instance.FindElement(By.XPath(coverTypeToolTipElement)).Click();

            string coverTypeMatDialog;
            if (isSingleTrip)
                coverTypeMatDialog = "mat-dialog-3";
            else
                coverTypeMatDialog = "mat-dialog-2";

            var coverTypeToolTipMessage = Driver.Instance.FindElement(By.XPath("//*[@id='" + coverTypeMatDialog + "']/custom-dialog/div/div"));

            var coverTypeToolTipTitle = Driver.Instance.FindElement(By.XPath("//*[@id='" + coverTypeMatDialog + "']/custom-dialog/div/mat-toolbar"));
            string coverTypeTitleToolTipMessage = coverTypeToolTipTitle.FindElement(By.XPath("./span")).Text;
            Console.WriteLine(coverTypeTitleToolTipMessage + Environment.NewLine + "--------------");
            //foreach (var i in coverTypeToolTipMessage)
            //{
            //    Console.WriteLine(i.FindElement(By.XPath(".//span[1]")).Text + ": " + i.FindElement(By.XPath(".//span[2]")).Text);
            //}
            ReadOnlyCollection<IWebElement> coverTypeToolTipAll = coverTypeToolTipMessage.FindElements(By.XPath("./div"));
            string tempCoverType = null;

           
            foreach (IWebElement i in coverTypeToolTipAll)
            {
                tempCoverType += ("\n" + i.Text);
                //Console.WriteLine(i.Text + Environment.NewLine);
            }

            Console.WriteLine(tempCoverType);

            //if (isSingleTrip)
            //{
            //    if (!((string)toolTipMsg["single tooltip"][coverTypeTitleToolTipMessage]).Equals(tempCoverType))
            //        Console.WriteLine("FAIL: " + coverTypeTitleToolTipMessage);
            //}
            //else
            //{
            //    if (!((string)toolTipMsg["annual tooltip"][coverTypeTitleToolTipMessage]).Equals(tempCoverType))
            //        Console.WriteLine("FAIL: " + coverTypeTitleToolTipMessage);
            //}

            CheckToolTip(isSingleTrip, coverTypeTitleToolTipMessage, tempCoverType, toolTipMsg);


            Driver.Instance.FindElement(By.XPath("//*[@id='" + coverTypeMatDialog + "']/custom-dialog/div/div/button")).Click();

            //if (!((string)toolTipMsg["single tooltip"][coverTypeTitleToolTipMessage]).Equals(tempCoverType))
            //    Console.WriteLine("FAIL: " + coverTypeTitleToolTipMessage);

            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver.Instance;
            

            Thread.Sleep(500);
            Console.WriteLine(Environment.NewLine);


            
            string adultAgeElement = fullElementSelector.adultAgeElement;
            var travellerAge = Driver.Instance.FindElement(By.XPath(adultAgeElement));

            

            string travellerAgeToolTipElement = "//*[@id='quote-input-adult-age']/div/div[1]/div/span/label/mat-label/button";

            string coverTypeElement = fullElementSelector.coverTypeElement;
            var coverType = Driver.Instance.FindElement(By.XPath(coverTypeElement));
            js.ExecuteScript("arguments[0].scrollIntoView();", coverType);
            coverType.Click();

            //Driver.Instance.FindElement(By.XPath(coverTypeElement)).Click(); ////div[@class='select-list-dropdown-option with-border select-singleline-option ng-star-inserted']/div
            string coverTypeDropDownElement = fullElementSelector.coverTypeDropDownElement;
            string currentCoverType;

            int j;

            if (isSingleTrip)
                j = 4;
            else
                j = 3;
            ReadOnlyCollection<IWebElement> coverTypeOptions = Driver.Instance.FindElements(By.XPath(coverTypeDropDownElement));
            for(int i=0; i<j; i++)
            {
                currentCoverType = coverTypeOptions[i].Text;
                coverTypeOptions[i].Click();
 
                Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath(adultAgeElement)));
                
                travellerAge.SendKeys("22,22"); ////*[@id="mat-dialog-5"]/custom-dialog/div/div/div/span[2]
                Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath(travellerAgeToolTipElement)));
                Driver.Instance.FindElement(By.XPath(travellerAgeToolTipElement)).Click();
                
                string travellerAgePath = "//*[@id='mat-dialog-" + (i+j).ToString() + "']/custom-dialog/div/div";
                string travellerAgeToolTipMessage = Driver.Instance.FindElement(By.XPath(travellerAgePath + "/div")).Text;

                var travellerAgeToolTipTitle = Driver.Instance.FindElement(By.XPath("//*[@id='mat-dialog-" + (i+j).ToString() + "']/custom-dialog/div/mat-toolbar"));
                string travellerAgeTitleToolTipMessage = travellerAgeToolTipTitle.FindElement(By.XPath("./span")).Text;

                Driver.Instance.FindElement(By.XPath(travellerAgePath + "/button")).Click();
                Console.WriteLine(travellerAgeTitleToolTipMessage + Environment.NewLine + "--------------" + Environment.NewLine +   travellerAgeToolTipMessage);

                //if (isSingleTrip)
                //{
                //    if (!((string)toolTipMsg["single tooltip"][travellerAgeTitleToolTipMessage]).Equals(travellerAgeToolTipMessage))
                //        Console.WriteLine("FAIL: " + travellerAgeTitleToolTipMessage);
                //}
                //else
                //{
                //    if (!((string)toolTipMsg["annual tooltip"][travellerAgeTitleToolTipMessage]).Equals(travellerAgeToolTipMessage))
                //        Console.WriteLine("FAIL: " + travellerAgeTitleToolTipMessage);
                //}
                //

                CheckToolTip(isSingleTrip, currentCoverType+": "+travellerAgeTitleToolTipMessage, travellerAgeToolTipMessage, toolTipMsg);


                Thread.Sleep(500);
                Console.WriteLine(Environment.NewLine);

                coverType.Click();
            }

            // Child ToolTip
            string childMatInput;
            string childMatDialog;

            if (isSingleTrip)
            {
                childMatInput = "mat-input-7";
                childMatDialog = "mat-dialog-8";
            }
            else
            {
                childMatInput = "mat-input-8";
                childMatDialog = "mat-dialog-6";
            }

            coverTypeOptions.FirstOrDefault(aa => aa.Text == "Family").Click();
            var childAge = Driver.Instance.FindElement(By.XPath(fullElementSelector.childAgeElement));
            childAge.SendKeys("12,13");
            Thread.Sleep(500);
            string childAgeToolTipElement = "//*[@id='quote-input-child-age']/div/div[1]/div/span/label/mat-label/button";
            //Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath(childAgeToolTipElement)));
            Driver.Instance.FindElement(By.XPath(childAgeToolTipElement)).Click();

            string childAgePath = "//*[@id='"+ childMatDialog + "']/custom-dialog/div/div";
            string childAgeToolTipMessage = Driver.Instance.FindElement(By.XPath(childAgePath + "/div")).Text;

            var childAgeToolTipTitle = Driver.Instance.FindElement(By.XPath("//*[@id='"+ childMatDialog + "']/custom-dialog/div/mat-toolbar"));
            string childAgeTitleToolTipMessage = childAgeToolTipTitle.FindElement(By.XPath("./span")).Text;

            Driver.Instance.FindElement(By.XPath(childAgePath + "/button")).Click();
            Console.WriteLine(childAgeTitleToolTipMessage + Environment.NewLine + "--------------" + Environment.NewLine +   childAgeToolTipMessage);

            //if (isSingleTrip)
            //{
            //    if (!((string)toolTipMsg["single tooltip"][childAgeTitleToolTipMessage]).Equals(childAgeToolTipMessage))
            //        Console.WriteLine("FAIL: " + childAgeTitleToolTipMessage);
            //}
            //else
            //{
            //    if (!((string)toolTipMsg["annual tooltip"][childAgeTitleToolTipMessage]).Equals(childAgeToolTipMessage))
            //        Console.WriteLine("FAIL: " + childAgeTitleToolTipMessage);
            //}

            CheckToolTip(isSingleTrip, "Family: " + childAgeTitleToolTipMessage, childAgeToolTipMessage, toolTipMsg);

        }

        public static void QuoteErrorMessageTest(FullElementSelector fullElementSelector, List<QuoteErrors> quoteErrors)
        {
            List<string> errorTitles = new List<string>();
            errorTitles.Add("0 If no Destination for Single Trip is filled or no Countries selected");
            errorTitles.Add("1 For Individual cover type, if Traveller Age is not filled");
            errorTitles.Add("2 If more than 5 countries selected");
            errorTitles.Add("3 For Couple cover type, if Adults Age is not filled");
            errorTitles.Add("4 If under Family Cover Type,  Adult(s) Age is not filled");
            errorTitles.Add("5 If under Family Cover Type,  Child(ren) Age is not filled");
            errorTitles.Add("6 If under Group Cover Type,  Traveller(s) Age is not filled");
            errorTitles.Add("7 If under Group Cover Type, all the travellers are under 18");
            errorTitles.Add("8 When customer selects family plan type, if customer enters age below 18 in the Adult(s) Age");
            errorTitles.Add("9 When customer selects couple plan type, if customer enters age below 18 in the Adult(s) Age");
            errorTitles.Add("10 If customer tries to search an invalid string, inputs integer or inputs Special Character");
            errorTitles.Add("11 For Individual cover type, if Traveller Age is not filled");
            errorTitles.Add("12 For Couple cover type, if Adults Age is not filled");
            errorTitles.Add("13 If under Family Cover Type,  Adult(s) Age is not filled");
            errorTitles.Add("14 If under Family Cover Type,  Child(ren) Age is not filled");
            errorTitles.Add("15 When customer selects family plan type, if customer enters age below 18 in the Adult(s) Age");
            errorTitles.Add("16 When customer selects couple plan type, if customer enters age below 18 in the Adult(s) Age");

            //IJavaScriptExecutor js = (IJavaScriptExecutor)Driver.Instance;
            List<QuoteErrors> displayedQuoteErrors = new List<QuoteErrors>();

            string errorElement = ".mat-error.ng-star-inserted";
            string quoteButtonElement = fullElementSelector.quoteButtonElement;
            // Trigger Get Quote Button Here /html/body/app-root/quote/div/div[2]/div/div[2]/div/form/custom-button/button


            string countriesElement = fullElementSelector.countriesElement;
            Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath(countriesElement)));
            var destination = Driver.Instance.FindElement(By.XPath(countriesElement));
            destination.SendKeys("");
            
            Driver.ClickWithRetry(By.XPath(quoteButtonElement));
            Thread.Sleep(1000);
            //var errorMessage = Driver.Instance.FindElement(By.CssSelector(errorElement));
            //Console.WriteLine(errorMessage.Text);

            string adultAgeElement = fullElementSelector.adultAgeElement;
            var travellerAge = Driver.Instance.FindElement(By.XPath(adultAgeElement));
            travellerAge.SendKeys("");
            Driver.ClickWithRetry(By.XPath(quoteButtonElement));
            Thread.Sleep(1000);

            var errorMessage = Driver.Instance.FindElements(By.CssSelector(errorElement));
            Console.WriteLine("*** 0 If no Destination for Single Trip is filled or no Countries selected," + Environment.NewLine + "1 For Individual cover type, if Traveller Age is not filled: ***");
            foreach (IWebElement i in errorMessage)
            {
                Console.WriteLine(i.Text);
                displayedQuoteErrors.Add(new QuoteErrors{ error = i.Text });
            }

            travellerAge.SendKeys("22");

            for (int i = 0; i < 6; i++)
            {
                destination.SendKeys("A");
                //Thread.Sleep(1000); // 
                string autocompletePopUpElement = fullElementSelector.autocompletePopupElement;
                string popupCountryElement = fullElementSelector.popupCountryElement;

                ReadOnlyCollection<IWebElement> autocompletePopUps = Driver.Instance.FindElements(By.CssSelector(autocompletePopUpElement));
                if (autocompletePopUps.Count() == 0) // If autocomplete somehow does not popup
                {
                    string popupTriggerElement = fullElementSelector.popupTriggerElement;
                    Driver.Instance.FindElement(By.XPath(popupTriggerElement)).Click(); // trigger dropdown arrow
                    Thread.Sleep(500);
                }

                Driver.Instance.FindElement(By.XPath(popupCountryElement)).Click();

            }
            errorMessage = Driver.Instance.FindElements(By.CssSelector(errorElement));
            Console.WriteLine("*** 2 If more than 5 countries selected: ***");
            Console.WriteLine(errorMessage[0].Text);
            displayedQuoteErrors.Add(new QuoteErrors { error = errorMessage[0].Text });
            Thread.Sleep(3000);


            /* Destination ok, individual age ok */
            string coverTypeElement = fullElementSelector.coverTypeElement;
            var coverType = Driver.Instance.FindElement(By.XPath(coverTypeElement));
            coverType.Click();

            //Driver.Instance.FindElement(By.XPath(coverTypeElement)).Click(); ////div[@class='select-list-dropdown-option with-border select-singleline-option ng-star-inserted']/div
            string coverTypeDropDownElement = fullElementSelector.coverTypeDropDownElement;
            ReadOnlyCollection<IWebElement> coverTypeOptions = Driver.Instance.FindElements(By.XPath(coverTypeDropDownElement));
            coverTypeOptions[1].Click();

            travellerAge.SendKeys("");
            Driver.ClickWithRetry(By.XPath(quoteButtonElement));
            errorMessage = Driver.Instance.FindElements(By.CssSelector(errorElement));
            Console.WriteLine("*** 3 For Couple cover type, if Adults Age is not filled: ***");

            Console.WriteLine(errorMessage[0].Text);
            displayedQuoteErrors.Add(new QuoteErrors { error = errorMessage[0].Text });

            coverType.Click();
            coverTypeOptions[2].Click();

            travellerAge.SendKeys("");
            string childAgeElement = fullElementSelector.childAgeElement;
            Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath(childAgeElement)));
            var childAge = Driver.Instance.FindElement(By.XPath(childAgeElement));
            childAge.SendKeys("");
            Driver.ClickWithRetry(By.XPath(quoteButtonElement));
            errorMessage = Driver.Instance.FindElements(By.CssSelector(errorElement));
            Console.WriteLine("*** 4 If under Family Cover Type,  Adult(s) Age is not filled," + Environment.NewLine + "5 If under Family Cover Type,  Child(ren) Age is not filled: ***");
            foreach (IWebElement i in errorMessage)
            {
                Console.WriteLine(i.Text);
                displayedQuoteErrors.Add(new QuoteErrors { error = i.Text });
            }

            coverType.Click();
            coverTypeOptions[3].Click();

            travellerAge.SendKeys("");
            Driver.ClickWithRetry(By.XPath(quoteButtonElement));
            errorMessage = Driver.Instance.FindElements(By.CssSelector(errorElement));
            Console.WriteLine("*** 6 If under Group Cover Type,  Traveller(s) Age is not filled: ***");
            Console.WriteLine(errorMessage[0].Text);
            displayedQuoteErrors.Add(new QuoteErrors { error = errorMessage[0].Text });

            travellerAge.SendKeys("17,17,17");
            Driver.ClickWithRetry(By.XPath(quoteButtonElement));
            errorMessage = Driver.Instance.FindElements(By.CssSelector(errorElement));
            Console.WriteLine("*** 7 If under Group Cover Type, all the travellers are under 18: ***");
            Console.WriteLine(errorMessage[0].Text);
            displayedQuoteErrors.Add(new QuoteErrors { error = errorMessage[0].Text });

            coverType.Click();
            coverTypeOptions[2].Click();
            travellerAge.SendKeys("17,17");
            Driver.ClickWithRetry(By.XPath(quoteButtonElement));
            errorMessage = Driver.Instance.FindElements(By.CssSelector(errorElement));
            Console.WriteLine("*** 8 When customer selects family plan type, if customer enters age below 18 in the Adult(s) Age: ***");
            Console.WriteLine(errorMessage[0].Text);
            displayedQuoteErrors.Add(new QuoteErrors { error = errorMessage[0].Text });

            coverType.Click();
            coverTypeOptions[1].Click();
            travellerAge.SendKeys("17,17");
            Driver.ClickWithRetry(By.XPath(quoteButtonElement));
            errorMessage = Driver.Instance.FindElements(By.CssSelector(errorElement));
            Console.WriteLine("*** 9 When customer selects couple plan type, if customer enters age below 18 in the Adult(s) Age: ***");
            Console.WriteLine(errorMessage[0].Text);
            displayedQuoteErrors.Add(new QuoteErrors { error = errorMessage[0].Text });


            coverType.Click();
            coverTypeOptions[0].Click();
            destination.SendKeys("Amazing");
            errorMessage = Driver.Instance.FindElements(By.CssSelector(errorElement));
            Console.WriteLine("*** 10 If customer tries to search an invalid string, inputs integer or inputs Special Character: ***");
            Console.WriteLine(errorMessage[0].Text);
            displayedQuoteErrors.Add(new QuoteErrors { error = errorMessage[0].Text });

            GotoQuotePage();
            IAlert alert = Driver.Instance.SwitchTo().Alert();
            alert.Accept();

            /* change to annual trip */
            string tripTypeElement = fullElementSelector.tripTypeElement;
            Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.Id(tripTypeElement)));
            Driver.Instance.FindElement(By.Id(tripTypeElement)).Click();

            travellerAge = Driver.Instance.FindElement(By.XPath(adultAgeElement));
            travellerAge.SendKeys("");
            Driver.ClickWithRetry(By.XPath(quoteButtonElement));
            errorMessage = Driver.Instance.FindElements(By.CssSelector(errorElement));
            Console.WriteLine("*** 11 For Individual cover type, if Traveller Age is not filled: ***");
            Console.WriteLine(errorMessage[0].Text);
            displayedQuoteErrors.Add(new QuoteErrors { error = errorMessage[0].Text });

            coverType = Driver.Instance.FindElement(By.XPath(coverTypeElement));
            coverType.Click();

            //Driver.Instance.FindElement(By.XPath(coverTypeElement)).Click(); ////div[@class='select-list-dropdown-option with-border select-singleline-option ng-star-inserted']/div
            coverTypeOptions = Driver.Instance.FindElements(By.XPath(coverTypeDropDownElement));
            coverTypeOptions[1].Click();

            travellerAge.SendKeys("");
            Driver.ClickWithRetry(By.XPath(quoteButtonElement));
            errorMessage = Driver.Instance.FindElements(By.CssSelector(errorElement));
            Console.WriteLine("*** 12 For Couple cover type, if Adults Age is not filled: ***");

            Console.WriteLine(errorMessage[0].Text);
            displayedQuoteErrors.Add(new QuoteErrors { error = errorMessage[0].Text });

            coverType.Click();
            coverTypeOptions[2].Click();

            travellerAge.SendKeys("");
            childAgeElement = fullElementSelector.annualChildAgeElement;
            Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath(childAgeElement)));
            childAge = Driver.Instance.FindElement(By.XPath(childAgeElement));
            childAge.SendKeys("");
            Driver.ClickWithRetry(By.XPath(quoteButtonElement));
            errorMessage = Driver.Instance.FindElements(By.CssSelector(errorElement));
            Console.WriteLine("*** 13 If under Family Cover Type,  Adult(s) Age is not filled," + Environment.NewLine + "14 If under Family Cover Type,  Child(ren) Age is not filled: ***");
            foreach (IWebElement i in errorMessage)
            {
                Console.WriteLine(i.Text);
                displayedQuoteErrors.Add(new QuoteErrors { error = i.Text });
            }

            travellerAge.SendKeys("17,17");
            Driver.ClickWithRetry(By.XPath(quoteButtonElement));
            errorMessage = Driver.Instance.FindElements(By.CssSelector(errorElement));
            Console.WriteLine("*** 15 When customer selects family plan type, if customer enters age below 18 in the Adult(s) Age: ***");
            Console.WriteLine(errorMessage[0].Text);
            displayedQuoteErrors.Add(new QuoteErrors { error = errorMessage[0].Text });

            coverType.Click();
            coverTypeOptions[1].Click();
            travellerAge.SendKeys("17,17");
            Driver.ClickWithRetry(By.XPath(quoteButtonElement));
            errorMessage = Driver.Instance.FindElements(By.CssSelector(errorElement));
            Console.WriteLine("*** 16 When customer selects couple plan type, if customer enters age below 18 in the Adult(s) Age: ***");
            Console.WriteLine(errorMessage[0].Text);
            displayedQuoteErrors.Add(new QuoteErrors { error = errorMessage[0].Text });

            int count = quoteErrors.Count;
            string tripTypeToDisplay = "Single Trip: ";
            for (int j = 0; j < count; j++)
            {
                if (j == 11)
                    tripTypeToDisplay = "Annual Trip: ";

                if (!quoteErrors[j].error.Equals(displayedQuoteErrors[j].error))
                {
                    Console.WriteLine(j.ToString() + " FAIL" + Environment.NewLine + quoteErrors[j].error + Environment.NewLine + displayedQuoteErrors[j].error + Environment.NewLine);
                    Helper.WriteToCSV(tripTypeToDisplay + "Quote Error", errorTitles[j], false, "DISPLAYED: " + displayedQuoteErrors[j].error + ", EXPECTED: " + quoteErrors[j].error);
                }
                else
                    Helper.WriteToCSV(tripTypeToDisplay + "Quote Error", errorTitles[j], true, "DISPLAYED: " + displayedQuoteErrors[j].error);
            }
        }

        public static void PlanPageFunctionalityTest(FullElementSelector fullElementSelector)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver.Instance;

            string accordionCollapsedElement = "mat-expansion-panel";
            ReadOnlyCollection<IWebElement> planCollapsed = Driver.Instance.FindElements(By.XPath("//mat-expansion-panel[contains(@class, '" + accordionCollapsedElement + "')]"));
            planCollapsed[0].Click();
            js.ExecuteScript("arguments[0].scrollIntoView();", planCollapsed[1]);
            planCollapsed[1].Click();
            js.ExecuteScript("arguments[0].scrollIntoView();", planCollapsed[2]);
            planCollapsed[2].Click();
            js.ExecuteScript("arguments[0].scrollIntoView();", planCollapsed[3]);
            planCollapsed[3].Click();
            js.ExecuteScript("arguments[0].scrollIntoView();", planCollapsed[4]);
            planCollapsed[4].Click();

            Thread.Sleep(1500);

            string accordionExpandedElement = "mat-expansion-panel-spacing";
            ReadOnlyCollection<IWebElement> planExpanded = Driver.Instance.FindElements(By.XPath("//mat-expansion-panel[contains(@class, '" + accordionExpandedElement + "')]"));
            if (planExpanded.Count != 5)
            {
                Console.WriteLine("FAIL: " + "Each individual key benefit can be expanded by clicking the  dropdown arrow which shows more in-depth coverage for each key benefit");
            }

            //Collapse all
            if (Driver.Instance.FindElement(By.XPath("/html/body/app-root/plan/div/div/div/div[1]/div/div[1]/custom-label[2]/span")).Text.ToUpper() != "COLLAPSE ALL")
                Console.WriteLine("FAIL: Collapse All button is incorrectly named.");
            js.ExecuteScript("arguments[0].scrollIntoView();", Driver.Instance.FindElement(By.XPath("/html/body/app-root/plan/div/div/div/div[1]/div/div[1]/custom-label[2]/span")));
            Driver.Instance.FindElement(By.XPath("/html/body/app-root/plan/div/div/div/div[1]/div/div[1]/custom-label[2]/span")).Click();
            planExpanded = Driver.Instance.FindElements(By.XPath("//mat-expansion-panel[contains(@class, '" + accordionExpandedElement + "')]"));
            if (planExpanded.Count > 0)
            {
                Console.WriteLine("FAIL: " + "Clicking on collapse all button collapses all the key benefits");
            }
            //Expand all
            if (Driver.Instance.FindElement(By.XPath("/html/body/app-root/plan/div/div/div/div[1]/div/div[1]/custom-label[2]/span")).Text.ToUpper() != "EXPAND ALL")
                Console.WriteLine("FAIL: Expand All button is incorrectly named.");
            js.ExecuteScript("arguments[0].scrollIntoView();", Driver.Instance.FindElement(By.XPath("/html/body/app-root/plan/div/div/div/div[1]/div/div[1]/custom-label[2]/span")));
            Driver.Instance.FindElement(By.XPath("/html/body/app-root/plan/div/div/div/div[1]/div/div[1]/custom-label[2]/span")).Click();
            planExpanded = Driver.Instance.FindElements(By.XPath("//mat-expansion-panel[contains(@class, '" + accordionExpandedElement + "')]"));
            ReadOnlyCollection<IWebElement> planCarets = Driver.Instance.FindElements(By.XPath("//div[contains(@class, 'accordion-caret')]"));
            if (planExpanded.Count != 5)
            {
                Console.WriteLine("FAIL: " + "Clicking on expand all button opens all the key benefits");
            }
            js.ExecuteScript("arguments[0].scrollIntoView();", planCarets[0]);
            planCarets[0].Click();
            //js.ExecuteScript("arguments[0].scrollIntoView();", planExpanded[1]);


            planCarets[1].Click();
            //js.ExecuteScript("arguments[0].scrollIntoView();", planExpanded[2]);

            planCarets[2].Click();
            //js.ExecuteScript("arguments[0].scrollIntoView();", planExpanded[3]);

            planCarets[3].Click();
            //js.ExecuteScript("arguments[0].scrollIntoView();", planExpanded[4]);

            planCarets[4].Click();
        }

        public static void CheckToolTip(bool isSingleTrip, string elementTitleToCheck, string messageToCheck, JObject toolTipMsg)
        {
            string toolTipType = null;
            if (isSingleTrip)
                toolTipType = "single tooltip";
            else
                toolTipType = "annual tooltip";

            if (!((string)toolTipMsg[toolTipType][elementTitleToCheck]).Equals(messageToCheck))
            {
                Console.WriteLine("FAIL: " + elementTitleToCheck);
                Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") + "ToolTip", elementTitleToCheck, false, "DISPLAYED: " + messageToCheck.Replace("\r", "").Replace("\n", "<newline>") + ", EXPECTED: " + (toolTipMsg[toolTipType][elementTitleToCheck]).ToString().Replace("\r", "").Replace("\n", "<newline>"));
            }
            else
                Helper.WriteToCSV((isSingleTrip ? "Single Trip: " : "Annual Trip: ") + "ToolTip", elementTitleToCheck, true, "DISPLAYED: " + messageToCheck.Replace("\r", "").Replace("\n", "<newline>"));
        }

    }

    public class QuoteCommand
    {   
        private List<IFillable> sectionsToFill;

        public QuoteCommand(IFillable sectionInfo)
        {
            sectionsToFill = new List<IFillable>();
            sectionsToFill.Add(sectionInfo);
        }

        public QuoteCommand FillSection(IFillable sectionInfo)
        {
            this.sectionsToFill.Add(sectionInfo);
            return this;
        }

        public void GetQuote(FullElementSelector fullElementSelector, string testId, string testName)
        {
            foreach (IFillable section in this.sectionsToFill)
            {
                if (section != null)
                    section.Fill(fullElementSelector, testId, testName);

            }

            Thread.Sleep(500);
            string quoteButtonElement = fullElementSelector.quoteButtonElement;
            // Trigger Get Quote Button Here /html/body/app-root/quote/div/div[2]/div/div[2]/div/form/custom-button/button
            Driver.ClickWithRetry(By.XPath(quoteButtonElement));
            new WebDriverWait(Driver.Instance, System.TimeSpan.FromSeconds(20)).Until(ExpectedConditions.UrlContains("plan"));
            Helper.WriteToCSV("Quote Page", "Got Quote", true, null, testId, testName);

        }


    }
}
