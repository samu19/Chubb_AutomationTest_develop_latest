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

namespace QuotePageAutomation
{
    public class QuoteDates : IFillable
    {
        public DateTime departDate;
        public DateTime returnDate;

        public void Fill()
        {
            // To pass in dates into appropriate date fields
            string departDay = departDate.Day.ToString();
            string returnDay = returnDate.Day.ToString();
            Driver.Instance.FindElement(By.Id("mat-input-2")).Click();
            //IJavaScriptExecutor js = (IJavaScriptExecutor)Driver.Instance;
            //js.ExecuteScript("document.getElementByXpath('//*[@id='mat-datepicker-0']/div[2]/mat-month-view/table/tbody').removeAttribute('readonly',0);");
            //js.ExecuteScript("document.getElementById('mat-input-2').value='"+formattedDate+"'");
            ReadOnlyCollection<IWebElement> departDays = Driver.Instance.FindElements(By.ClassName("mat-calendar-body-cell-content"));
            departDays.FirstOrDefault(a => a.Text == departDay).Click();

            Driver.Instance.FindElement(By.Id("mat-input-3")).Click();
            ReadOnlyCollection<IWebElement> returnDays = Driver.Instance.FindElements(By.ClassName("mat-calendar-body-cell-content"));
            returnDays.FirstOrDefault(a => a.Text == returnDay).Click();

            //Driver.Instance.FindElement(By.XPath("//*[@id='form-datepicker-container']/div[1]/mat-form-field/div/div[1]/div[1]/input")).SendKeys(formattedDate);
        }
    }
}
