using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumAutomation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace LandingPageAutomation
{
    public class LandingPage
    {     
        public static void Navigate(string url)
        {
            Driver.Instance.Navigate().GoToUrl(url);
        }
        public static bool HasAllFooter
        {
            get
            {
                return (HasAboutUs && HasPrivacyPolicy && HasTermsofUse && HasHelp && HasClaims && HasPolicyWording);
            }
        }

        public static bool HasAboutUs
        {
            get
            {
                //return Driver.Instance.Title.Equals("REM - User Dashboard");

                Driver.GetWait().Until(d => d.FindElements(By.Id("main-nav")).Count > 0);

                Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/chubb-dbs-app/foo-ter/footer/div/div/div/div/div[4]/ul[1]/li[1]")));
                Driver.ClickWithRetry(By.XPath("/html/body/chubb-dbs-app/foo-ter/footer/div/div/div/div/div[4]/ul[1]/li[1]"));


                Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='mat-dialog-0']/dia-log/div/mat-toolbar/span[1]")));
                var title = Driver.Instance.FindElement((By.XPath("//*[@id='mat-dialog-0']/dia-log/div/mat-toolbar/span[1]")));
                string titleText = title.Text;
                Driver.Instance.FindElement(By.XPath("//*[@id='mat-dialog-0']/dia-log/div/mat-toolbar/button")).Click();

                return (titleText == "About Us");


            }
        }

        public static bool HasPrivacyPolicy
        {
            get
            {
                //return Driver.Instance.Title.Equals("REM - User Dashboard");
                Thread.Sleep(1500);
                //Driver.GetWait().Until(d => d.FindElements(By.Id("main-nav")).Count > 0);

                Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/chubb-dbs-app/foo-ter/footer/div/div/div/div/div[4]/ul[1]/li[2]")));
                Driver.ClickWithRetry(By.XPath("/html/body/chubb-dbs-app/foo-ter/footer/div/div/div/div/div[4]/ul[1]/li[2]"));


                Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='mat-dialog-1']/dia-log/div/mat-toolbar/span[1]")));
                var title = Driver.Instance.FindElement((By.XPath("//*[@id='mat-dialog-1']/dia-log/div/mat-toolbar/span[1]")));
                string titleText = title.Text;
                Driver.Instance.FindElement(By.XPath("//*[@id='mat-dialog-1']/dia-log/div/mat-toolbar/button")).Click();

                return (titleText == "Privacy Policy");


            }
        }

        public static bool HasTermsofUse
        {
            get
            {
                //return Driver.Instance.Title.Equals("REM - User Dashboard");
                Thread.Sleep(1500);
                //Driver.GetWait().Until(d => d.FindElements(By.Id("main-nav")).Count > 0);

                Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/chubb-dbs-app/foo-ter/footer/div/div/div/div/div[4]/ul[1]/li[3]")));
                Driver.ClickWithRetry(By.XPath("/html/body/chubb-dbs-app/foo-ter/footer/div/div/div/div/div[4]/ul[1]/li[3]"));


                Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='mat-dialog-2']/dia-log/div/mat-toolbar/span[1]")));
                var title = Driver.Instance.FindElement((By.XPath("//*[@id='mat-dialog-2']/dia-log/div/mat-toolbar/span[1]")));
                string titleText = title.Text;
                Driver.Instance.FindElement(By.XPath("//*[@id='mat-dialog-2']/dia-log/div/mat-toolbar/button")).Click();

                return (titleText == "Terms of use");


            }
        }

        public static bool HasHelp
        {
            get
            {
                //return Driver.Instance.Title.Equals("REM - User Dashboard");

                //Driver.GetWait().Until(d => d.FindElements(By.Id("main-nav")).Count > 0);
                Thread.Sleep(1500);
                Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/chubb-dbs-app/foo-ter/footer/div/div/div/div/div[4]/ul[2]/li[1]")));
                Driver.ClickWithRetry(By.XPath("/html/body/chubb-dbs-app/foo-ter/footer/div/div/div/div/div[4]/ul[2]/li[1]"));


                Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='mat-dialog-3']/dia-log/div/mat-toolbar/span[1]")));
                var title = Driver.Instance.FindElement((By.XPath("//*[@id='mat-dialog-3']/dia-log/div/mat-toolbar/span[1]")));
                string titleText = title.Text;
                Driver.Instance.FindElement(By.XPath("//*[@id='mat-dialog-3']/dia-log/div/mat-toolbar/button")).Click();

                return (titleText == "Help");


            }
        }

        public static bool HasClaims
        {
            get
            {
                //return Driver.Instance.Title.Equals("REM - User Dashboard");
                Thread.Sleep(1500);
                //Driver.GetWait().Until(d => d.FindElements(By.Id("main-nav")).Count > 0);

                Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/chubb-dbs-app/foo-ter/footer/div/div/div/div/div[4]/ul[2]/li[2]")));
                Driver.ClickWithRetry(By.XPath("/html/body/chubb-dbs-app/foo-ter/footer/div/div/div/div/div[4]/ul[2]/li[2]"));


                Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='mat-dialog-4']/dia-log/div/mat-toolbar/span[1]")));
                var title = Driver.Instance.FindElement((By.XPath("//*[@id='mat-dialog-4']/dia-log/div/mat-toolbar/span[1]")));
                string titleText = title.Text;
                Driver.Instance.FindElement(By.XPath("//*[@id='mat-dialog-4']/dia-log/div/mat-toolbar/button")).Click();

                return (titleText == "Claims");


            }
        }

        public static bool HasPolicyWording
        {
            get
            {
                //return Driver.Instance.Title.Equals("REM - User Dashboard");
                Thread.Sleep(1500);
                //Driver.GetWait().Until(d => d.FindElements(By.Id("main-nav")).Count > 0);

                Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/chubb-dbs-app/foo-ter/footer/div/div/div/div/div[5]/small[2]/a")));
                Driver.ClickWithRetry(By.XPath("/html/body/chubb-dbs-app/foo-ter/footer/div/div/div/div/div[5]/small[2]/a"));


                Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='mat-dialog-5']/dia-log/div/mat-dialog-content/div/pdf-viewer/div/div/div[1]/div[2]/div[8]")));
                var title = Driver.Instance.FindElement((By.XPath("//*[@id='mat-dialog-5']/dia-log/div/mat-dialog-content/div/pdf-viewer/div/div/div[1]/div[2]/div[8]")));
                string titleText = title.Text;
                Driver.Instance.FindElement(By.XPath("//*[@id='mat-dialog-5']/dia-log/div/mat-toolbar/button")).Click();

                return (titleText == "Policy Wording");


            }
        }
        //public static NavigateCommand Navigate()
        //{
        //    return new NavigateCommand();
        //}
    }
    
    //public class NavigateCommand
    //{

    //    public NavigateCommand() { }

    //    public void Navigate()
    //    {
    //        string url = ConfigurationManager.AppSettings["url"].ToString();

    //        Driver.Instance.Navigate().GoToUrl(url);



    //    }
    //}
}
