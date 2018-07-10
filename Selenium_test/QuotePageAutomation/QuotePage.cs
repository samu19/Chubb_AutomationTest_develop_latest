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
            /* Single Trip radio button is defaulted and selected */
            string tripTypeRadioElement = "//mat-radio-button[@class='mat-radio-button mat-accent mat-radio-checked']";
            var tripTypeRadio = Driver.Instance.FindElement(By.XPath(tripTypeRadioElement));
            if (tripTypeRadio.Text != "Single Trip")
                Console.WriteLine("FAIL: " + "Single Trip radio button is defaulted and selected");
            else
                Console.WriteLine("Single Trip radio button is defaulted and selected");

            /* Get Quote button is disabled until all mandatory fields are filled correctly */
            string quoteButtonElement = fullElementSelector.quoteButtonElement;
            var quoteButton = Driver.Instance.FindElement(By.XPath(quoteButtonElement));
            if (quoteButton.Enabled == true)
                Console.WriteLine("FAIL: " + "Get Quote button is disabled until all mandatory fields are filled correctly");
            else
                Console.WriteLine("Get Quote button is disabled until all mandatory fields are filled correctly");
            
            /* Depart date is defaulted to today's date */
            var departDate = Driver.Instance.FindElement(By.XPath(fullElementSelector.departDateElement));
            string selectedDepartDate = departDate.GetAttribute("value");
            DateTime _selectedDepartDate = DateTime.ParseExact(selectedDepartDate, "dd MMM yyyy", null);

            DateTime CurrentDate = DateTime.Today;
            if (CurrentDate.Date != _selectedDepartDate.Date)
                Console.WriteLine("FAIL: " + "Depart date is defaulted to today's date");
            else
                Console.WriteLine("Depart date is defaulted to today's date");
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
                Console.WriteLine("FAIL: " + "Return date is defaulted to 5 days after depart date");
            else
                Console.WriteLine("Return date is defaulted to 5 days after depart date");
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
                throw new Exception("Error! Max Depart Date is not 180 days later.");
            QuoteData.DateHandler(maxDepartDate.AddDays(1), fullElementSelector.departDateElement, fullElementSelector.departDateMonthElement, fullElementSelector.departDateDecreaseElement, fullElementSelector.departDateIncreaseElement, fullElementSelector.dateDayElement);
            QuoteData.DateHandler(maxDepartDate.AddDays(100), fullElementSelector.departDateElement, fullElementSelector.departDateMonthElement, fullElementSelector.departDateDecreaseElement, fullElementSelector.departDateIncreaseElement, fullElementSelector.dateDayElement);
            string checkSelectedDepartDate = departDate.GetAttribute("value");
            DateTime _checkSelectedDepartDate = DateTime.ParseExact(checkSelectedDepartDate, "dd MMM yyyy", null);
            double doubleCheckDepartDiff = (_checkSelectedDepartDate - CurrentDate).TotalDays;
            if (doubleCheckDepartDiff > 179)
                Console.WriteLine("FAIL: " + "Depart Date cannot be selected more than 180 days away from today");
            else
                Console.WriteLine("Depart Date cannot be selected more than 180 days away from today");

            /* Clicking the text box will bring up the calendar picker. Return Date cannot be selected more than 180 days away from depart date */
            string maxreturnDate = returnDate.GetAttribute("ng-reflect-max");
            //string newSelectedDepartDate = departDate.GetAttribute("value");
            //DateTime _newSelectedDepartDate = DateTime.ParseExact(newSelectedDepartDate, "dd MMM yyyy", null);
            DateTime maxReturnDate = DateTime.ParseExact(maxreturnDate.Substring(4, 11), "MMM dd yyyy", null);
            double returnDiff = (maxReturnDate - _checkSelectedDepartDate).TotalDays;
            if (returnDiff != 179)
                throw new Exception("Error! Max Return Date is not 180 days later.");
            QuoteData.DateHandler(maxReturnDate.AddDays(1), fullElementSelector.returnDateElement, fullElementSelector.returnDateMonthElement, fullElementSelector.returnDateDecreaseElement, fullElementSelector.returnDateIncreaseElement, fullElementSelector.dateDayElement);
            QuoteData.DateHandler(maxReturnDate.AddDays(1), fullElementSelector.returnDateElement, fullElementSelector.returnDateMonthElement, fullElementSelector.returnDateDecreaseElement, fullElementSelector.returnDateIncreaseElement, fullElementSelector.dateDayElement);
            string checkSelectedReturnDate = returnDate.GetAttribute("value");
            DateTime _checkSelectedReturnDate = DateTime.ParseExact(checkSelectedReturnDate, "dd MMM yyyy", null);
            double doubleCheckReturnDiff = (_checkSelectedReturnDate - _checkSelectedDepartDate).TotalDays;
            if (doubleCheckReturnDiff > 179)
                Console.WriteLine("FAIL: " + "Return Date cannot be selected more than 180 days away from depart date");
            else
                Console.WriteLine("Return Date cannot be selected more than 180 days away from depart date");


            /* Able to type and filter for countries based on the 1st letter typed in */

            string countriesElement = fullElementSelector.countriesElement;
            Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath(countriesElement)));
            var destination = Driver.Instance.FindElement(By.XPath(countriesElement));

            char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWYZ".ToCharArray();
            foreach (char a in alpha)
            {
                destination.SendKeys(a.ToString());
                Driver.GetWait().Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".autocomplete-popup.show")));
                destination.SendKeys(Keys.Backspace);
            }

            /* Able to select up to 5 different countries and not duplicated */
            for (int i = 0; i < 5; i++)
            {
                destination.SendKeys("A");
                //Thread.Sleep(1000); // 
                string autocompletePopUpElement = fullElementSelector.autocompletePopupElement;
                string popupCountryElement = fullElementSelector.popupCountryElement;
                Driver.Instance.FindElement(By.XPath(popupCountryElement)).Click();
            }

            /* Individual: Can only key in one age */
            string adultAgeElement = fullElementSelector.adultAgeElement;


            Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath(adultAgeElement)));
            var adultAge = Driver.Instance.FindElement(By.XPath(adultAgeElement));
            adultAge.SendKeys("23,23");
            List<string> ages = adultAge.GetAttribute("value").Split(',').ToList<string>();
            if (ages.Count() != 1)
                Console.WriteLine("FAIL: " + "Individual: Can only key in one age");
            else
                Console.WriteLine("Individual: Can only key in one age");

            /* Dropdown menu with 4 cover types: Individual, Couple, Family, Group */


            string coverTypeElement = fullElementSelector.coverTypeElement;
            var coverType = Driver.Instance.FindElement(By.XPath(coverTypeElement));
            coverType.Click();

            //Driver.Instance.FindElement(By.XPath(coverTypeElement)).Click(); ////div[@class='select-list-dropdown-option with-border select-singleline-option ng-star-inserted']/div
            string coverTypeDropDownElement = fullElementSelector.coverTypeDropDownElement;
            ReadOnlyCollection<IWebElement> coverTypeOptions = Driver.Instance.FindElements(By.XPath(coverTypeDropDownElement));
            string[] expectedCoverType = { "Individual", "Couple", "Family", "Group" };
            for (int j = 0; j < 4; j++)
            {
                if (coverTypeOptions[j].Text != expectedCoverType[j])
                    Console.WriteLine("FAIL: " + "Dropdown menu with 4 cover types: Individual, Couple, Family, Group");
            }

            /* Couple: Can only key in up to two ages, separated by commas */
            coverTypeOptions[1].Click();
            adultAge.SendKeys("23,23,23");
            ages = adultAge.GetAttribute("value").Split(',').ToList<string>();
            if (ages.Count() != 2)
                Console.WriteLine("FAIL: " + "Couple: Can only key in up to two ages, separated by commas");

            coverType.Click();

            /* Group: Can only key in up to ten ages, separated by commas */
            coverTypeOptions[3].Click();
            adultAge.SendKeys("21,22,23,24,25,26,27,28,29,30,31");
            ages = adultAge.GetAttribute("value").Split(',').ToList<string>();
            if (ages.Count() != 10)
                Console.WriteLine("FAIL: " + "Group: Can only key in up to ten ages, separated by commas");
            /* Family: Can only key in up to two ages, separated by commas */
            coverType.Click();
            coverTypeOptions[2].Click();
            adultAge.SendKeys("21,22,23");
            ages = adultAge.GetAttribute("value").Split(',').ToList<string>();
            if (ages.Count() != 2)
                Console.WriteLine("FAIL: " + "Family: Can only key in up to two ages, separated by commas");

            /* Family: Can only key in up to five ages, separated by commas */
            string childAgeElement = fullElementSelector.childAgeElement;
            Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath(childAgeElement)));
            var childAge = Driver.Instance.FindElement(By.XPath(childAgeElement));
            childAge.SendKeys("11,12,13,14,15,16");
            ages = childAge.GetAttribute("ng-reflect-value").Split(',').ToList<string>();
            if (ages.Count() != 5)
                Console.WriteLine("FAIL: " + "Family: Can only key in up to five ages, separated by commas");
        }

        public static void QuoteToolTipTest(FullElementSelector fullElementSelector, bool isSingleTrip = true)
        {
            

            /* Able to type and filter for countries based on the 1st letter typed in */

            string countriesElement = fullElementSelector.countriesElement;
            Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath(countriesElement)));
            var destination = Driver.Instance.FindElement(By.XPath(countriesElement));
            destination.Click();

            string destinationToolTip = "/html/body/app-root/quote/div/div[2]/div/div[2]/div/form/custom-autocomplete/div/mat-form-field/div/div[1]/div/span/label/mat-label/button";

            
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

        public void GetQuote(FullElementSelector fullElementSelector)
        {
            foreach (IFillable section in this.sectionsToFill)
            {
                section.Fill(fullElementSelector);

            }

            string quoteButtonElement = fullElementSelector.quoteButtonElement;
            // Trigger Get Quote Button Here /html/body/app-root/quote/div/div[2]/div/div[2]/div/form/custom-button/button
            Driver.ClickWithRetry(By.XPath(quoteButtonElement));
            new WebDriverWait(Driver.Instance, System.TimeSpan.FromSeconds(20)).Until(ExpectedConditions.UrlContains("plan"));

        }

        
    }
}
