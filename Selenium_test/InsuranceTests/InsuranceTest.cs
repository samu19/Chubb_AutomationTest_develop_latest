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
using OpenQA.Selenium.Support.UI;
using PlanPageAutomation;
using TravellerDetailsPageAutomation;
using PaymentPageAutomation;

namespace InsuranceTests
{
    [TestFixture]
    public class InsuranceTest : PageTestBase
    {

        [Test, TestCaseSource("QUOTEDATA"), Order(1)]
        public void Test_001_GetQuote(bool _isSingleTrip, string _countries, DateTime _departDate, DateTime _returnDate, string _coverType, string _adultAge, string _childAge)
        {
            UITest("GetQuote", () =>
            {
                GetQuote(_isSingleTrip, _countries, _departDate, _returnDate, _coverType, _adultAge, _childAge);

            });
        }
        static object[] QUOTEDATA = {
            new object[] {
                bool.Parse(ConfigurationManager.AppSettings["tripType_1"].ToString()),
                ConfigurationManager.AppSettings["countries_1"].ToString(),
                DateTime.Parse(ConfigurationManager.AppSettings["departDate_1"]),
                DateTime.Parse(ConfigurationManager.AppSettings["returnDate_1"]),
                ConfigurationManager.AppSettings["coverType_1"].ToString(),
                ConfigurationManager.AppSettings["adultAge_1"].ToString(),
                ConfigurationManager.AppSettings["childAge_1"].ToString()
            },
            new object[] {
                bool.Parse(ConfigurationManager.AppSettings["tripType_2"].ToString()),
                ConfigurationManager.AppSettings["countries_2"].ToString(),
                DateTime.Parse(ConfigurationManager.AppSettings["departDate_2"]),
                DateTime.Parse(ConfigurationManager.AppSettings["returnDate_2"]),
                ConfigurationManager.AppSettings["coverType_2"].ToString(),
                ConfigurationManager.AppSettings["adultAge_2"].ToString(),
                ConfigurationManager.AppSettings["childAge_2"].ToString()
            }
        };

        public void GetQuote(bool _isSingleTrip, string _countries, DateTime _departDate, DateTime _returnDate, string _coverType, string _adultAge, string _childAge)
        {
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

            var adultAge = new AdultAge
            {
                adultAge = _adultAge,
                coverType = _coverType
            };

            var childAge = new ChildAge
            {
                childAge = _childAge
            };


            try
            {
                QuotePage.GotoQuotePage();
                //QuotePage.FillSection(countries).FillSection(dates).FillSection(adultAge).GetQuote();
                QuotePage.FillSection(countries).FillSection(dates).FillSection(coverType).FillSection(adultAge).FillSection(childAge).GetQuote();


                Assert.IsTrue(QuotePage.GetCurrentURLSlug() == "quote", "Quote Summary Page not reached");
                //Console.WriteLine(_isSingleTrip + ", " + _countries + ", " + _departDate + ", " + _returnDate + ", " + _coverType);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                Assert.Fail();
            }

            Console.WriteLine("GetQuote passed.");


        }

        [Test, TestCaseSource("PLANDATA_1"), Order(2)]
        public void Test_002_SelectPlan(bool _isSingleTrip, string _countries, DateTime _departDate, DateTime _returnDate, string _coverType, string _adultAge, string _childAge,
            int _planNo)
        {
            UITest("SelectPlan", () =>
            {
                Test_001_GetQuote(_isSingleTrip, _countries, _departDate, _returnDate, _coverType, _adultAge, _childAge);
                SelectPlan(_planNo);
            });
        }

        public void SelectPlan(int _planNo)
        {
            try
            {

                PlanPage.SelectPlan(_planNo);
                Assert.IsTrue(PlanPage.GetCurrentURLSlug() == "summary/edit", "Enter Travel Details Page not reached.");
                //Thread.Sleep(100000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                Assert.Fail();
            }



            Console.WriteLine("Select Plan passed.");

        }

        static object[] PLANDATA_1 = {
            new int[] {
                Convert.ToInt32(ConfigurationManager.AppSettings["planNo_1"])
            }
        };

        [Test, Order(3)]
        public void Test_003_FillTravelDetails(bool _isSingleTrip, string _countries, DateTime _departDate, DateTime _returnDate, string _coverType, string _adultAge, string _childAge,
            int _planNo, ApplicantDetail _a)
        {
            UITest("FillTravelDetails", () =>
            {
                Test_002_SelectPlan(_isSingleTrip, _countries, _departDate, _returnDate, _coverType, _adultAge, _childAge, _planNo);
                FillTravelDetails(_a);
            });
        }

        [Test, Order(4)]
        public void Test_004_FillPaymentDetails(bool _isSingleTrip, string _countries, DateTime _departDate, DateTime _returnDate, string _coverType, string _adultAge, string _childAge,
            int _planNo, ApplicantDetail _a, CreditCardInfo _CCInfo)
        {
            UITest("FillPaymentDetails", () =>
            {
                Test_003_FillTravelDetails(_isSingleTrip, _countries, _departDate, _returnDate, _coverType, _adultAge, _childAge, _planNo, _a);
                FillPaymentDetails(_CCInfo);
            });
        }

        public void FillTravelDetails(ApplicantDetail _a)
        {
            ApplicantDetail applicantDetail = _a;

            try
            {
                EditTravellerDetailsPage.FillSection(applicantDetail).Proceed();


                /* traveller info */
                // if needed

                Thread.Sleep(5000);

                TravellerDetailsSummaryPage.ProceedToPaymentSelection();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                Assert.Fail();
            }



            Console.WriteLine("Fill traveller details passed.");

        }

        public void FillPaymentDetails(CreditCardInfo creditCardInfo)
        {
            try
            {
                PaymentSelectionPage.SelectPaymentTypeAndProceed("CC");


                //trigger payment
                CreditCardDetailsPage.FillSection(creditCardInfo).Pay();
                
                

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                Assert.Fail();
            }



            Console.WriteLine("Payment Success.");
        }

        [Test, TestCaseSource("EndToEndData"), Order(7)]
        public void Test_007_EndToEnd(bool _isSingleTrip, string _countries, DateTime _departDate, DateTime _returnDate, string _coverType, string _adultAge, string _childAge,
            int _planNo, ApplicantDetail _a, CreditCardInfo _CCInfo)
        {
            UITest("End-To-End", () =>
            {
                try
                {
                    //QuotePage.GotoQuotePage();

                    //QuotePage.FillSection(countries).FillSection(dates).FillSection(coverType).FillSection(adultAge).FillSection(childAge).GetQuote();


                    //Assert.IsTrue(QuotePage.GetCurrentURLSlug() == "quote", "Quote Summary Page not reached");
                    Test_004_FillPaymentDetails(_isSingleTrip, _countries, _departDate, _returnDate, _coverType, _adultAge, _childAge, _planNo, _a, _CCInfo);


                    
                    PaymentSuccessPage.ViewPDF();
                    // click pdf



                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                    Assert.Fail();
                }



                Console.WriteLine("End-To-End passed.");

                //Driver.Close();
            });
        }


        static object[] EndToEndData = {
            new object[] {
                bool.Parse(ConfigurationManager.AppSettings["tripType_1"].ToString()),
                ConfigurationManager.AppSettings["countries_1"].ToString(),
                DateTime.Parse(ConfigurationManager.AppSettings["departDate_1"]),
                DateTime.Parse(ConfigurationManager.AppSettings["returnDate_1"]),
                ConfigurationManager.AppSettings["coverType_1"].ToString(),
                ConfigurationManager.AppSettings["adultAge_1"].ToString(),
                ConfigurationManager.AppSettings["childAge_1"].ToString(),
                Convert.ToInt32(ConfigurationManager.AppSettings["planNo_1"]),
                new ApplicantDetail
                {
                    aNRIC = ConfigurationManager.AppSettings["aNRIC_1"].ToString(),
                    aFullName = ConfigurationManager.AppSettings["aFullName_1"].ToString(),
                    aDOB = ConfigurationManager.AppSettings["aDOB_1"].ToString(),
                    aNationality = ConfigurationManager.AppSettings["aNationality_1"].ToString(),
                    aMobile = ConfigurationManager.AppSettings["aMobile_1"].ToString(),
                    aEmail = ConfigurationManager.AppSettings["aEmail_1"].ToString()
                },
                new CreditCardInfo
                {
                    cardNo = ConfigurationManager.AppSettings["cardNo_1"].ToString(),
                    cardHolderName = ConfigurationManager.AppSettings["cardHolderName_1"].ToString(),
                    expiryDate = ConfigurationManager.AppSettings["expiryDate_1"].ToString(),
                    cvv = ConfigurationManager.AppSettings["cvv_1"].ToString()
                }
            }
            ,
            new object[] {
                bool.Parse(ConfigurationManager.AppSettings["tripType_2"].ToString()),
                ConfigurationManager.AppSettings["countries_2"].ToString(),
                DateTime.Parse(ConfigurationManager.AppSettings["departDate_2"]),
                DateTime.Parse(ConfigurationManager.AppSettings["returnDate_2"]),
                ConfigurationManager.AppSettings["coverType_2"].ToString(),
                ConfigurationManager.AppSettings["adultAge_2"].ToString(),
                ConfigurationManager.AppSettings["childAge_2"].ToString(),
                Convert.ToInt32(ConfigurationManager.AppSettings["planNo_2"]),
                new ApplicantDetail
                {
                    aNRIC = ConfigurationManager.AppSettings["aNRIC_2"].ToString(),
                    aFullName = ConfigurationManager.AppSettings["aFullName_2"].ToString(),
                    aDOB = ConfigurationManager.AppSettings["aDOB_2"].ToString(),
                    aNationality = ConfigurationManager.AppSettings["aNationality_2"].ToString(),
                    aMobile = ConfigurationManager.AppSettings["aMobile_2"].ToString(),
                    aEmail = ConfigurationManager.AppSettings["aEmail_2"].ToString()
                },
                new CreditCardInfo
                {
                    cardNo = ConfigurationManager.AppSettings["cardNo_2"].ToString(),
                    cardHolderName = ConfigurationManager.AppSettings["cardHolderName_2"].ToString(),
                    expiryDate = ConfigurationManager.AppSettings["expiryDate_2"].ToString(),
                    cvv = ConfigurationManager.AppSettings["cvv_2"].ToString()
                }
            }
        };

        [OneTimeSetUp()]
        public void SetupTest()
        {
            Driver.Initialize();


        }

        [OneTimeTearDown()]
        public void MyTestCleanup()
        {
            Driver.Close();
        }
    }
}
