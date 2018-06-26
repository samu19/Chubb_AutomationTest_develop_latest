using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System.IO;
using OpenQA.Selenium.Chrome;

namespace SeleniumAutomation
{
    public class Driver
    {
        private static int timeout = 10;
        private const int ATTEMPT = 4;

        public static IWebDriver Instance { get; set; }

        public static void Initialize(int Timeout)
        {

            Instance = new ChromeDriver();

            Instance.Manage().Timeouts().ImplicitWait = System.TimeSpan.FromSeconds(timeout);
        }

        public static void Initialize()
        {
            //string driverFolder = Path.GetFullPath(@"..\..\..\..\Drivers");

            Instance = new ChromeDriver();
            Instance.Manage().Timeouts().ImplicitWait = System.TimeSpan.FromSeconds(10);
        }

        public static void Close()
        {
            try
            {
                Instance.Close();
                Instance.Quit();
            }
            catch
            { }
        }

        public static WebDriverWait GetWait()
        {
            return GetWait(true);
        }

        public static WebDriverWait GetWait(bool IgnoreStaleElementException)
        {
            var wait = new WebDriverWait(Instance, TimeSpan.FromSeconds(timeout));

            if (IgnoreStaleElementException)
            {
                wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            }
            return wait;
        }

        public static void ClickWithRetry(By by)
        {
            int attempt = 0;

            while (attempt < ATTEMPT)
            {
                try
                {
                    Instance.FindElement(by).Click();
                    break;
                }
                catch (StaleElementReferenceException)
                {
                    if (attempt < ATTEMPT)
                    {
                        attempt++;
                        continue;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

    }
}
