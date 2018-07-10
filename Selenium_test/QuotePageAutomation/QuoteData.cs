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

namespace QuotePageAutomation
{
    public class QuoteData : IFillable
    {
        public bool isSingleTrip { get; set; }
        public string countries { get; set; }
        public DateTime departDate { get; set; }
        public DateTime returnDate { get; set; }
        public string coverType { get; set; }
        public string adultAge { get; set; }
        public string childAge { get; set; }
        public string promoCode { get; set; }



        public void Fill(FullElementSelector fullElementSelector)
        {
            //QuoteFunctionalityTest(fullElementSelector);
            if (!isSingleTrip) // If Annual Trip
            {
                string tripTypeElement = fullElementSelector.tripTypeElement;
                //To change - choose Annual Trip radio button
                Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.Id(tripTypeElement)));
                Driver.ClickWithRetry(By.Id(tripTypeElement));
            }

            // To pass in countries IBMB: 
            ////*[@id="Destination"]/div/div[1]/div/div[1]/div/input
            string countriesElement = fullElementSelector.countriesElement;
            Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath(countriesElement)));
            var destination = Driver.Instance.FindElement(By.XPath(countriesElement));

            //char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            //foreach(char a in alpha)
            //{
            //    destination.SendKeys(a.ToString());
            //    destination.SendKeys(Keys.Backspace);
            //}

            destination.SendKeys(countries);
            //Thread.Sleep(1500); // 
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


            //Dates

            DateHandler(departDate, fullElementSelector.departDateElement, fullElementSelector.departDateMonthElement, fullElementSelector.departDateDecreaseElement, fullElementSelector.departDateIncreaseElement, fullElementSelector.dateDayElement);
            DateHandler(returnDate, fullElementSelector.returnDateElement, fullElementSelector.returnDateMonthElement, fullElementSelector.returnDateDecreaseElement, fullElementSelector.returnDateIncreaseElement, fullElementSelector.dateDayElement);
            

       
            // To pass in Cover Type into appropriate field
            if (coverType != "Individual")
            {
                string coverTypeElement = fullElementSelector.coverTypeElement;
                Driver.Instance.FindElement(By.XPath(coverTypeElement)).Click(); ////div[@class='select-list-dropdown-option with-border select-singleline-option ng-star-inserted']/div
                string coverTypeDropDownElement = fullElementSelector.coverTypeDropDownElement;
                ReadOnlyCollection<IWebElement> coverTypeOptions = Driver.Instance.FindElements(By.XPath(coverTypeDropDownElement));
                coverTypeOptions.FirstOrDefault(a => a.Text == coverType).Click();
                //Thread.Sleep(10000);

            }

            //adult age

            string adultAgeElement = fullElementSelector.adultAgeElement;


            Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath(adultAgeElement)));
            Driver.Instance.FindElement(By.XPath(adultAgeElement)).SendKeys(adultAge);

            //child age
            if (!String.IsNullOrWhiteSpace(childAge))
            {
                string childAgeElement = fullElementSelector.childAgeElement;
                // To pass in child age
                Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath(childAgeElement)));
                Driver.Instance.FindElement(By.XPath(childAgeElement)).SendKeys(childAge);
            }

            //promo code
            string promoCodeElement = fullElementSelector.promoCodeElement;
            Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath(promoCodeElement)));
            Driver.Instance.FindElement(By.XPath(promoCodeElement)).SendKeys(promoCode);
        }

        

        public static void DateHandler(DateTime _Date, string _DateElement, string _MonthYearElement, string _prevMonthElement, string _nextMonthElement, string _daysElement)
        {                       
            Driver.Instance.FindElement(By.XPath(_DateElement)).Click();

            string Day = _Date.Day.ToString();
            string formattedDate = _Date.ToString("MMM yyyy");
            string MonthYearElement = _MonthYearElement;
            Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath(MonthYearElement)));

            while (formattedDate.ToUpper() != Driver.Instance.FindElement(By.XPath(MonthYearElement)).Text)
            {
                DateTime expectedDate = DateTime.ParseExact(formattedDate, "MMM yyyy", null);
                DateTime displayedDate = DateTime.ParseExact(Driver.Instance.FindElement(By.XPath(MonthYearElement)).Text, "MMM yyyy", null);

                if (expectedDate < displayedDate) // If expected date is less than displayed date
                    Driver.Instance.FindElement(By.XPath(_prevMonthElement)).Click(); // go back one month
                else
                    Driver.Instance.FindElement(By.XPath(_nextMonthElement)).Click(); // go forward one month
            }
            ReadOnlyCollection<IWebElement> CalendarDays = Driver.Instance.FindElements(By.ClassName(_daysElement));
            CalendarDays.FirstOrDefault(a => a.Text == Day).Click();
            DateTime aa = DateTime.ParseExact(Driver.Instance.FindElement(By.XPath(_DateElement)).GetAttribute("value"), "dd MMM yyyy", null).Date;
            /* If attempted selected date did not register, it means not clickable. */
            if (DateTime.ParseExact(Driver.Instance.FindElement(By.XPath(_DateElement)).GetAttribute("value"), "dd MMM yyyy", null).Date != _Date.Date)
                CalendarDays.FirstOrDefault(b => b.Text == (Convert.ToInt32(Day)-1).ToString()).Click();// have to close calendar by clicking max date

            //IJavaScriptExecutor js = (IJavaScriptExecutor)Driver.Instance;
            //js.ExecuteScript("document.getElementByXpath('//*[@id='mat-datepicker-0']/div[2]/mat-month-view/table/tbody').removeAttribute('readonly',0);");
            //js.ExecuteScript("document.getElementById('mat-input-2').value='"+formattedDate+"'");
        }
    }
}
