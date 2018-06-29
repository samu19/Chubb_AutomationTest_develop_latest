using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumAutomation;
using System;
using System.Collections.Generic;
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
            Driver.ClickWithRetry(By.XPath("/html/body/chubb-dbs-app/app-trip/div/form/div[2]/button"));
            new WebDriverWait(Driver.Instance, System.TimeSpan.FromSeconds(20)).Until(ExpectedConditions.UrlContains("quote"));
            //Driver.GetWait().Until(ExpectedConditions.UrlToBe("https://10.229.85.23/Dashboard/UserDashboard.aspx"));
        }

        
    }
}
