using System;
using System.Text;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using System.Threading;
using SeleniumAutomation;
using LandingPageAutomation;
using NUnit.Framework;
using System.Configuration;

namespace SampleTest
{
    /// <summary>
    /// Summary description for MySeleniumTests
    /// </summary>
    [TestFixture]
    public class GeneralTest : PageTestBase
    {
        //private TestContext testContextInstance;

        //public TestContext GetTestContext()
        //{ return testContextInstance; }

        //public void SetTestContext(TestContext value)
        //{ testContextInstance = value; }
  

        [Test, TestCaseSource("URL")]
        public void HasFooter(string url)
        {
            //ConfigurationManager.AppSettings["url"].ToString()
            UITest("HasFooter", () =>
            {
                LandingPage.Navigate(url);

                try
                {
                    Assert.IsTrue(LandingPage.HasAllFooter, "At least one of the Footer link checks returned false");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                    Assert.Fail();
                }
                //Assert.IsTrue(LandingPage.HasAboutUs, "Footer does not contain a clickable About Us");
                //Assert.IsTrue(LandingPage.HasPrivacyPolicy, "Footer does not contain a clickable Privacy Policy");
                //Assert.IsTrue(LandingPage.HasTermsofUse, "Footer does not contain a clickable Terms of Use");
                //Assert.IsTrue(LandingPage.HasHelp, "Footer does not contain a clickable Help");
                //Assert.IsTrue(LandingPage.HasClaims, "Footer does not contain a clickable Claims");

                //Assert.IsTrue(LandingPage.HasPolicyWording, "Footer does not contain a clickable Policy Wording");


                Console.WriteLine("HasFooter passed.");

                Driver.Close();
            });
        }

        static object[] URL = {
            new string[] {ConfigurationManager.AppSettings["url"].ToString() }
        };



        [SetUp()]
        public void SetupTest()
        {
            Driver.Initialize(ConfigurationManager.AppSettings["browser"]);

        }

        [TearDown()]
        public void MyTestCleanup()
        {
            Driver.Close();
        }
    }
}