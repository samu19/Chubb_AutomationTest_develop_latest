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
                return (HasAboutUs && HasPrivacyPolicy  && HasClaims && HasTermsofUse && HasHelp && HasPolicyWording);
            }
        }

        public static bool HasAboutUs
        {
            get
            {
                /*
                var resultset = Driver.Instance.FindElement(By.XPath("/html/body/app-root/quote/foo-ter/footer/div/div[2]/custom-label/ul"));
                var options = resultset.FindElements(By.XPath("./li/a"));

                Driver.GetWait().Until(d => d.FindElements(By.Id("main-nav")).Count > 0);
                */
                Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/app-root/quote/foo-ter/footer/div/div[2]/custom-label/ul/li[1]/a")));
                Driver.ClickWithRetry(By.XPath("/html/body/app-root/quote/foo-ter/footer/div/div[2]/custom-label/ul/li[1]/a"));

                Driver.Instance.SwitchTo().Window(Driver.Instance.WindowHandles.Last());

                Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath("/html/body/div[2]/div/div/div/div[3]/div/h3[1]")));
                var title = Driver.Instance.FindElement((By.XPath("/html/body/div[2]/div/div/div/div[3]/div/h3[1]")));
                string titleText = title.Text;
                //Driver.Instance.FindElement(By.XPath("//*[@id='mat-dialog-0']/dia-log/div/mat-toolbar/button")).Click();
                //Actions action = new Actions(Driver.Instance);

                //action.KeyDown(Keys.Control).SendKeys( "W").Build().Perform();

                Driver.Instance.SwitchTo().Window(Driver.Instance.WindowHandles.First());

                return (titleText == "About Chubb Travel Insurance?");


            }
        }

        public static bool HasPrivacyPolicy
        {
            get
            {
                //return Driver.Instance.Title.Equals("REM - User Dashboard");
                Thread.Sleep(1500);
                //Driver.GetWait().Until(d => d.FindElements(By.Id("main-nav")).Count > 0);

                Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/app-root/quote/foo-ter/footer/div/div[2]/custom-label/ul/li[2]/a")));
                Driver.ClickWithRetry(By.XPath("/html/body/app-root/quote/foo-ter/footer/div/div[2]/custom-label/ul/li[2]/a"));

                Driver.Instance.SwitchTo().Window(Driver.Instance.WindowHandles.Last());


                Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath("/html/body/div[1]/header/div/div/div/h1")));
                var title = Driver.Instance.FindElement((By.XPath("/html/body/div[1]/header/div/div/div/h1")));
                string titleText = title.Text;
                //Driver.Instance.FindElement(By.XPath("//*[@id='mat-dialog-1']/dia-log/div/mat-toolbar/button")).Click();

                Driver.Instance.SwitchTo().Window(Driver.Instance.WindowHandles.First());


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

                Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/app-root/quote/foo-ter/footer/div/div[2]/custom-label/ul/li[4]/a")));
                Driver.ClickWithRetry(By.XPath("/html/body/app-root/quote/foo-ter/footer/div/div[2]/custom-label/ul/li[4]/a"));

                Driver.Instance.SwitchTo().Window(Driver.Instance.WindowHandles.Last());

                Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath("//div[@class='article-body'][1]/h3[1]")));
                var title = Driver.Instance.FindElement((By.XPath("//div[@class='article-body'][1]/h3[1]")));
                string titleText = title.Text;
                //Driver.Instance.FindElement(By.XPath("//*[@id='mat-dialog-2']/dia-log/div/mat-toolbar/button")).Click();
                Driver.Instance.SwitchTo().Window(Driver.Instance.WindowHandles.First());

                return (titleText == "Acceptance of Terms");


            }
        }

        public static bool HasHelp
        {
            get
            {
                //return Driver.Instance.Title.Equals("REM - User Dashboard");

                //Driver.GetWait().Until(d => d.FindElements(By.Id("main-nav")).Count > 0);
                Thread.Sleep(1500);
                Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/app-root/quote/foo-ter/footer/div/div[2]/custom-label/ul/li[5]/a")));
                Driver.ClickWithRetry(By.XPath("/html/body/app-root/quote/foo-ter/footer/div/div[2]/custom-label/ul/li[5]/a"));

                Driver.Instance.SwitchTo().Window(Driver.Instance.WindowHandles.Last());

                Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath("//div[@class='article-body'][1]/div[1]/h3")));
                var title = Driver.Instance.FindElement((By.XPath("//div[@class='article-body'][1]/div[1]/h3")));
                string titleText = title.Text;
                //Driver.Instance.FindElement(By.XPath("//*[@id='mat-dialog-3']/dia-log/div/mat-toolbar/button")).Click();
                Driver.Instance.SwitchTo().Window(Driver.Instance.WindowHandles.First());

                return (titleText == "24 Hour Emergency Hotline");


            }
        }

        public static bool HasClaims
        {
            get
            {
                //return Driver.Instance.Title.Equals("REM - User Dashboard");
                Thread.Sleep(1500);
                //Driver.GetWait().Until(d => d.FindElements(By.Id("main-nav")).Count > 0);

                Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/app-root/quote/foo-ter/footer/div/div[2]/custom-label/ul/li[3]/a")));
                Driver.ClickWithRetry(By.XPath("/html/body/app-root/quote/foo-ter/footer/div/div[2]/custom-label/ul/li[3]/a"));

                Driver.Instance.SwitchTo().Window(Driver.Instance.WindowHandles.Last());

                Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='container']/div[1]/div/div[1]/h2/span[1]")));
                var title = Driver.Instance.FindElement((By.XPath("//*[@id='container']/div[1]/div/div[1]/h2/span[1]")));
                string titleText = title.Text;
                //Driver.Instance.FindElement(By.XPath("//*[@id='mat-dialog-4']/dia-log/div/mat-toolbar/button")).Click();
                Driver.Instance.SwitchTo().Window(Driver.Instance.WindowHandles.First());


                return (titleText == "Welcome to the Chubb Claim Centre");


            }
        }

        public static bool HasPolicyWording
        {
            get
            {
                //return Driver.Instance.Title.Equals("REM - User Dashboard");
                Thread.Sleep(1500);
                //Driver.GetWait().Until(d => d.FindElements(By.Id("main-nav")).Count > 0);

                Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/app-root/quote/foo-ter/footer/div/custom-label[5]/a")));
                Driver.ClickWithRetry(By.XPath("/html/body/app-root/quote/foo-ter/footer/div/custom-label[5]/a"));
                Driver.Instance.SwitchTo().Window(Driver.Instance.WindowHandles.Last());

                new WebDriverWait(Driver.Instance, System.TimeSpan.FromSeconds(10)).Until(ExpectedConditions.UrlContains("PolicyWording"));

                //Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='mat-dialog-5']/dia-log/div/mat-dialog-content/div/pdf-viewer/div/div/div[1]/div[2]/div[8]")));
                //var title = Driver.Instance.FindElement((By.XPath("//*[@id='mat-dialog-5']/dia-log/div/mat-dialog-content/div/pdf-viewer/div/div/div[1]/div[2]/div[8]")));
 
                //Driver.Instance.FindElement(By.XPath("//*[@id='mat-dialog-5']/dia-log/div/mat-toolbar/button")).Click();

                return (true);


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
