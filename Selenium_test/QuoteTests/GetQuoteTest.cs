using System;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using SeleniumAutomation;
using LandingPageAutomation;
using QuotePageAutomation;
using System.Configuration;
using NUnit.Framework;

namespace QuoteTests
{
    [TestFixture]
    public class GetQuoteTest : PageTestBase
    {
        //private TestContext testContextInstance;

        //public TestContext GetTestContext()
        //{ return testContextInstance; }

        //public void SetTestContext(TestContext value)
        //{ testContextInstance = value; }

        [Test, TestCaseSource("QUOTEDATA_1")]
        public void GetQuote(bool _isSingleTrip, string _countries, DateTime _departDate, DateTime _returnDate, string _coverType)
        {
            UITest("GetQuote", () =>
            {
                //QuotePage.GotoQuotePage();

                var tripType = new TripType
                {
                    isSingleTrip = _isSingleTrip
                };

                var countries = new Countries
                {
                    countries = _countries
                };

                var dates = new QuoteDates
                {
                    departDate = _departDate,
                    returnDate = _returnDate
                };

                var coverType = new CoverType
                {
                    coverType = _coverType
                };


                try
                {
                    //QuotePage.FillSection(tripType).FillSection(countries).FillSection(dates).FillSection(coverType).GetQuote();
                    //Assert.IsTrue(LandingPage.HasAllFooter, "At least one of the Footer link checks returned false");
                    Console.WriteLine(_isSingleTrip + ", " + _countries + ", " + _departDate + ", " + _returnDate + ", " + _coverType);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                    Assert.Fail();
                }



                Console.WriteLine("GetQuote passed.");

                Driver.Close();
            });
        }

        static object[] QUOTEDATA_1 = {
            new object[] {
                bool.Parse(ConfigurationManager.AppSettings["tripType_1"].ToString()),
                ConfigurationManager.AppSettings["countries_1"].ToString(),
                DateTime.Parse(ConfigurationManager.AppSettings["departDate_1"]),
                DateTime.Parse(ConfigurationManager.AppSettings["returnDate_1"]),
                ConfigurationManager.AppSettings["coverType_1"].ToString()
            }
        };

        //public TestContext TestContext
        //{
        //    get
        //    {
        //        return testContextInstance;
        //    }
        //    set
        //    {
        //        testContextInstance = value;
        //    }
        //}


        [SetUp()]
        public void SetupTest()
        {
            Driver.Initialize();


        }

        [TearDown()]
        public void MyTestCleanup()
        {
            Driver.Close();
        }
    }
}
