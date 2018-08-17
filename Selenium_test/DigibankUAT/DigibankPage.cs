using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumAutomation;
using System;
using System.Threading;

namespace DigibankUAT
{
    public class DigibankPage
    {
        public static void Login(string userID, string PIN)
        {
            Driver.Instance.Navigate().GoToUrl(@"https://sib8.dbs.com/IB/Welcome?FROM_IB=TRUE");

            if (Helper.isAlertPresent())
            {
                IAlert alert = Driver.Instance.SwitchTo().Alert();
                alert.Accept();
            }

            string userIDElement = "//*[@id='UID']";
            string PINElement = "//*[@id='PIN']";
            string loginButtonElement = "/html/body/form[1]/div/div[7]/button[1]";
            Driver.Instance.FindElement(By.XPath(userIDElement)).SendKeys(userID);
            Driver.Instance.FindElement(By.XPath(PINElement)).SendKeys(PIN);

            Driver.Instance.FindElement(By.XPath(loginButtonElement)).Click();
            Thread.Sleep(10000);
        }

        public static void GoToChubb()
        {
            string applyElement = "//*[@id='topnav6']/div[1]/h4";
            string travelInsuranceElement = "//*[@id='topnav6']/div[2]/a[6]";
            string getOTPElement = "//*[@id='btnLabel']";
            //Thread.Sleep(10000);
            string OTPElement = "//*[@id='SMSLoginPin']";
            string getQuoteElement = "/html/body/div/section/div[2]/div[2]/div/div[2]/button";
            string loginElement = "//*[@id='submitButton']";

            string iframeElement = "/html/frameset/frame[2]";

            Driver.GetWait().Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.XPath(iframeElement)));
            //string popUpSkipElement = "//*[@id='skipButton']";
            //var popupSkip = Driver.Instance.FindElement(By.XPath(popUpSkipElement));
            //popupSkip.Click();

            

            var applyMenu = Driver.Instance.FindElement(By.XPath(applyElement));

            //Actions action = new Actions(Driver.Instance);
            //action.MoveToElement(applyMenu).Perform();
            applyMenu.Click();
            Thread.Sleep(1000);
            Driver.Instance.FindElement(By.XPath(travelInsuranceElement)).Click();


            string iframeElement2 = "//*[@id='iframe1']";
            Driver.GetWait().Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.XPath(iframeElement2)));


            Driver.Instance.FindElement(By.XPath(getOTPElement)).Click();

            Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath(OTPElement)));
            Driver.Instance.FindElement(By.XPath(OTPElement)).SendKeys("111111");
            Driver.Instance.FindElement(By.XPath(loginElement)).Click();
            Thread.Sleep(3000);

            //Driver.GetWait().Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.XPath(iframeElement)));

            //Driver.GetWait().Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.XPath(iframeElement2)));


            Driver.Instance.FindElement(By.XPath(getQuoteElement)).Click();



        }
    }
}
