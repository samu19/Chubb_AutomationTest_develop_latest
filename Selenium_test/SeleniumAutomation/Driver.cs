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
//using OpenQA.Selenium.Chrome;

namespace SeleniumAutomation
{
    public class Driver
    {
        private static int timeout = 3;
        private const int ATTEMPT = 3;

        public static IWebDriver Instance { get; set; }

        public static void Initialize(int Timeout)
        {
            ChromeOptions options = new ChromeOptions();

            options.EnableMobileEmulation("iPhone 6/7/8");
            //options.AddArgument("start-maximized");
            Instance = new ChromeDriver();

            Instance.Manage().Timeouts().ImplicitWait = System.TimeSpan.FromSeconds(timeout);
        }

        public static void Initialize()
        {
            ChromeOptions options = new ChromeOptions();

            options.EnableMobileEmulation("Galaxy S5");
            options.AddArgument("start-maximized");
            //string driverFolder = Path.GetFullPath(@"..\..\..\..\Drivers");

            Instance = new ChromeDriver(options);
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
