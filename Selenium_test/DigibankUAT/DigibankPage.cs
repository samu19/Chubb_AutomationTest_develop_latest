using OpenQA.Selenium;
using SeleniumAutomation;
using System;

namespace DigibankUAT
{
    public class DigibankPage
    {
        public void Login()
        {
            string userIDElement = "//*[@id='UID']";
            string PINElement = "//*[@id='PIN']";
            string loginButtonElement = "/html/body/form[1]/div/div[7]/button[1]";
            Driver.Instance.FindElement(By.XPath(userIDElement)).SendKeys("CULUAT53");
            Driver.Instance.FindElement(By.XPath(PINElement)).SendKeys("723629");

            Driver.Instance.FindElement(By.XPath(loginButtonElement)).Click();

        }
    }
}
