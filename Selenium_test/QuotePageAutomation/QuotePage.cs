using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumAutomation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuotePageAutomation
{
    public class QuotePage
    {
        private static string quotePage = @"xxx";

        public static string CompanyName
        {
            get
            {
                Driver.GetWait().Until(ExpectedConditions.ElementExists(By.CssSelector("span[id*='lblCompany']")));
                return Driver.Instance.FindElement(By.CssSelector("span[id*='lblCompany']")).GetAttribute("textContent");
            }
        }

        public static void GotoQuotePage()
        {
            Driver.Instance.Navigate().GoToUrl(quotePage);
        }


        public static QuoteCommand FillSection(IFillable sectionInfo)
        {
            return new QuoteCommand(sectionInfo);
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

        public void GetQuote()
        {
            foreach (IFillable section in this.sectionsToFill)
            {
                section.Fill();

            }

            // Trigger Get Quote Button Here
            Driver.ClickWithRetry(By.CssSelector("input[id*='btnSendForEndorsement']"));
            Driver.GetWait().Until(ExpectedConditions.UrlToBe("https://10.229.85.23/Dashboard/UserDashboard.aspx"));
        }

        
    }
}
