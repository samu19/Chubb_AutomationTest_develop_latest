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
    public class CoverType : IFillable
    {
        public string coverType;

        public void Fill()
        {
            // To pass in Cover Type into appropriate field
            if(coverType != "Individual")
            {
                Driver.Instance.FindElement(By.XPath("//*[@id='mat-select-0']/div/div[1]")).Click();
                ReadOnlyCollection<IWebElement> coverTypeOptions = Driver.Instance.FindElements(By.ClassName("mat-option-text"));
                coverTypeOptions.FirstOrDefault(a => a.Text == coverType).Click();
                //Thread.Sleep(10000);
                
            }

        }
    }
}