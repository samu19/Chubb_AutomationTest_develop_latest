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
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace InsuranceTests
{
    [TestFixture]
    public class InsuranceTest : PageTestBase
    {
        /// <summary>
        /// TestCaseSource specifies the input information for this test method.
        /// Its format is an object array. If the object array has n objects,
        /// this test will run n times.
        /// </summary>
        [Test, TestCaseSource("NewData"), Order(1)]
        public void Test_001_GetQuote(InputData input)
        {
            

            FullElementSelector fullElementSelector = LoadElementSelectors();
            string TestName = input.testid + "-" + input.testName;
            UITest("GetQuote", () =>
            {
                try
                {
                    GetQuote(input.quoteData, fullElementSelector);
                    //QuotePage.PlanPageFunctionalityTest(fullElementSelector);

                }
                catch (Exception ex)
                {
                    string message_ = "Exception for " + TestName + " : " + ex.Message;
                    Helper.redirectConsoleLog(TestName, message_);
                    Assert.Fail();
                }



                string message = TestName + " passed.";

                string path = ConfigurationManager.AppSettings["testFolder"].ToString();

                SaveScreenshot(path, "Success", TestName);
                Helper.redirectConsoleLog(TestName, message);

            });
        }


        public void GetQuote(QuoteData quoteData, FullElementSelector fullElementSelector)
        {
            var _quoteData = new QuoteData
            {
                isSingleTrip = quoteData.isSingleTrip,
                countries = quoteData.countries,
                departDate = quoteData.departDate,
                returnDate = quoteData.returnDate,
                coverType = quoteData.coverType,
                adultAge = quoteData.adultAge,
                childAge = quoteData.childAge,
                promoCode = quoteData.promoCode,
                regionNo = quoteData.regionNo
            };


            try
            {
                QuotePage.GotoQuotePage();

                //if (quoteData.coverType != "Family")
                //{
                if (Helper.isAlertPresent())
                {
                    IAlert alert = Driver.Instance.SwitchTo().Alert();
                    alert.Accept();
                }
                //}
                //QuotePage.FillSection(countries).FillSection(dates).FillSection(adultAge).GetQuote();
                QuotePage.FillSection(_quoteData).GetQuote(fullElementSelector);


                Assert.IsTrue(QuotePage.GetCurrentURLSlug() == fullElementSelector.planSummarySlug, "Plan Summary Page not reached");
                //Console.WriteLine(_isSingleTrip + ", " + _countries + ", " + _departDate + ", " + _returnDate + ", " + _coverType);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                Assert.Fail();
            }

            Console.WriteLine("GetQuote passed.");


        }

        [Test, TestCaseSource("NewData"), Order(1)]
        public void Test_002_FillTravelDetails(InputData input)
        {
            FullElementSelector fullElementSelector = LoadElementSelectors();
            string TestName = input.testid + "-" + input.testName;
            UITest("FillTravelDetails", () =>
            {

                GetQuote(input.quoteData, fullElementSelector);
                SelectPlan(input.planNo);
                FillTravelDetails(input.applicantDetail, input.travellerDetails);

            });
        }



        [Test, TestCaseSource("NewData"), Order(1)]
        public void Test_000_VerifyPlanCosts(InputData input)
        {
            FullElementSelector fullElementSelector = LoadElementSelectors();
            string TestName = input.testid + "-" + input.testName;
            UITest("VerifyPlanCosts", () =>
            {

                GetQuote(input.quoteData, fullElementSelector);
                SelectPlan(input.planNo);


            });
        }

        public void SelectPlan(int _planNo)
        {
            try
            {
                PlanPage.SelectPlan(_planNo);
                Assert.IsTrue(PlanPage.GetCurrentURLSlug() == "apply/application-details", "Enter Travel Details Page not reached.");
                //Thread.Sleep(100000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                Assert.Fail();
            }



            Console.WriteLine("Select Plan passed.");

        }


        public void VerifyPlanAmount()
        {
            try
            {
                PlanPage.VerifyPlanAmount();

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

        //[Test, Order(3)]
        public void Test_003_FillTravelDetails(bool _isSingleTrip, string _countries, DateTime _departDate, DateTime _returnDate, string _coverType, string _adultAge, string _childAge,
            int _planNo, ApplicantDetail _a)
        {
            UITest("FillTravelDetails", () =>
            {
                //Test_002_SelectPlan(_isSingleTrip, _countries, _departDate, _returnDate, _coverType, _adultAge, _childAge, _planNo);
                //FillTravelDetails(_a);
            });
        }

        //[Test, Order(4)]
        public void Test_004_FillPaymentDetails(bool _isSingleTrip, string _countries, DateTime _departDate, DateTime _returnDate, string _coverType, string _adultAge, string _childAge,
            int _planNo, ApplicantDetail _a, CreditCardInfo _CCInfo)
        {
            FullElementSelector fullElementSelector = LoadElementSelectors();
            UITest("FillPaymentDetails", () =>
            {
                Test_003_FillTravelDetails(_isSingleTrip, _countries, _departDate, _returnDate, _coverType, _adultAge, _childAge, _planNo, _a);
                FillPaymentDetails(_CCInfo, fullElementSelector);
            });
        }

        public void FillTravelDetails(ApplicantDetail _a, TravellerDetails _t)
        {
            ApplicantDetail applicantDetail = _a;

            FullElementSelector fullElementSelector = LoadElementSelectors();
            try
            {
                EditTravellerDetailsPage.FillSection(applicantDetail).FillSection(_t).Proceed(fullElementSelector);


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

        public void FillPaymentDetails(CreditCardInfo creditCardInfo, FullElementSelector fullElementSelector)
        {
            try
            {
                PaymentSelectionPage.SelectPaymentTypeAndProceed("CC");


                //trigger payment
                CreditCardDetailsPage.FillSection(creditCardInfo).Pay(fullElementSelector);
                
                

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                Assert.Fail();
            }



            Console.WriteLine("Payment Success.");
        }


        [Test]
        public void QuoteFunctionality()
        {
            FullElementSelector fullElementSelector = LoadElementSelectors();
            List<QuoteErrors> quoteErrors = LoadQuoteErrors();
            //string TestName = input.testid + "-" + input.testName;
            string TestName = "Quote Functionality Test";
            UITest(TestName, () =>
            {
                try
                {
                    QuotePage.GotoQuotePage();
                    QuotePage.QuoteErrorMessageTest(fullElementSelector, quoteErrors);

                    QuotePage.GotoQuotePage();
                    if (Helper.isAlertPresent())
                    {
                        IAlert alert = Driver.Instance.SwitchTo().Alert();
                        alert.Accept();
                    }

                    Console.WriteLine("Single Trip" + Environment.NewLine);
                    QuotePage.QuoteToolTipTest(fullElementSelector, LoadToolTip());
                    
                    //Console.WriteLine(Environment.NewLine + "++++++++++++++++++++++++++++++++++++++++++++++++++++++" + Environment.NewLine);

                    QuotePage.GotoQuotePage();
                    if (Helper.isAlertPresent())
                    {
                        IAlert alert = Driver.Instance.SwitchTo().Alert();
                        alert.Accept();
                    }
                    Console.WriteLine("Annual Trip" + Environment.NewLine);
                    QuotePage.QuoteToolTipTest(fullElementSelector, LoadToolTip(), false);
                    //QuotePage.QuoteToolTipTest(fullElementSelector);
                    //Console.WriteLine(Environment.NewLine + "++++++++++++++++++++++++++++++++++++++++++++++++++++++" + Environment.NewLine);

                    //QuotePage.GotoQuotePage();
                    //if (Helper.isAlertPresent())
                    //{
                    //    IAlert alert = Driver.Instance.SwitchTo().Alert();
                    //    alert.Accept();
                    //}
                    ////QuotePage.FillSection(null).GetQuote(fullElementSelector);
                    //////QuotePage.PlanPageFunctionalityTest(fullElementSelector);
                    //QuotePage.QuoteFunctionalityTest(fullElementSelector);
                    //Console.WriteLine(Environment.NewLine + "++++++++++++++++++++++++++++++++++++++++++++++++++++++" + Environment.NewLine);
                    //EditTravellerDetailsPage.TravelDetailPageFunctionalityTest(fullElementSelector);

                    //QuotePage.QuoteErrorMessageTest(fullElementSelector);

                }
                catch (Exception ex)
                {
                    string message_ = "Exception for " + TestName + " : " + ex.Message;
                    Helper.redirectConsoleLog(TestName, message_);
                    Assert.Fail();
                }



                string message = TestName + " passed.";

                string path = ConfigurationManager.AppSettings["testFolder"].ToString();

                SaveScreenshot(path, "Success", TestName);
                Helper.redirectConsoleLog(TestName, message);

            });
        }

        /// <summary>
        /// TestCaseSource specifies the input information for this test method.
        /// Its format is an object array. If the object array has n objects,
        /// this test will run n times.
        /// </summary>
        [Test, TestCaseSource("NewData"), Order(7)]
        public void Test_007_EndToEnd(InputData input)
        {
            FullElementSelector fullElementSelector = LoadElementSelectors();
            string TestName = input.testid + "-" + input.testName;
            UITest(TestName, () =>
            {
                try
                {
                    GetQuote(input.quoteData, fullElementSelector);
                    SelectPlan(input.planNo);
                    //FillTravelDetails(input.applicantDetail);
                    FillPaymentDetails(input.creditCardInfo, fullElementSelector);

                    //Assert.IsTrue(QuotePage.GetCurrentURLSlug() == "quote", "Quote Summary Page not reached");
                    //Test_004_FillPaymentDetails(_isSingleTrip, _countries, _departDate, _returnDate, _coverType, _adultAge, _childAge, _planNo, _a, _CCInfo);



                    PaymentSuccessPage.ViewPDF();
                    // click pdf



                }
                catch (Exception ex)
                {
                    string message_ = "Exception for " + TestName + " : " + ex.Message;
                    Helper.redirectConsoleLog(TestName, message_);
                    Assert.Fail();
                }



                string message = TestName + " passed.";

                string path = ConfigurationManager.AppSettings["testFolder"].ToString();

                SaveScreenshot(path, "Success", TestName);
                Helper.redirectConsoleLog(TestName, message);


                //Driver.Close();
            });
        }



        //static object[] EndToEndData = {
        //    new object[] {
        //        bool.Parse(ConfigurationManager.AppSettings["tripType_1"].ToString()),
        //        ConfigurationManager.AppSettings["countries_1"].ToString(),
        //        DateTime.Parse(ConfigurationManager.AppSettings["departDate_1"]),
        //        DateTime.Parse(ConfigurationManager.AppSettings["returnDate_1"]),
        //        ConfigurationManager.AppSettings["coverType_1"].ToString(),
        //        ConfigurationManager.AppSettings["adultAge_1"].ToString(),
        //        ConfigurationManager.AppSettings["childAge_1"].ToString(),
        //        Convert.ToInt32(ConfigurationManager.AppSettings["planNo_1"]),
        //        new ApplicantDetail
        //        {
        //            aNRIC = ConfigurationManager.AppSettings["aNRIC_1"].ToString(),
        //            aFullName = ConfigurationManager.AppSettings["aFullName_1"].ToString(),
        //            aDOB = ConfigurationManager.AppSettings["aDOB_1"].ToString(),
        //            aNationality = ConfigurationManager.AppSettings["aNationality_1"].ToString(),
        //            aMobile = ConfigurationManager.AppSettings["aMobile_1"].ToString(),
        //            aEmail = ConfigurationManager.AppSettings["aEmail_1"].ToString()
        //        },
        //        new CreditCardInfo
        //        {
        //            cardNo = ConfigurationManager.AppSettings["cardNo_1"].ToString(),
        //            cardHolderName = ConfigurationManager.AppSettings["cardHolderName_1"].ToString(),
        //            expiryDate = ConfigurationManager.AppSettings["expiryDate_1"].ToString(),
        //            cvv = ConfigurationManager.AppSettings["cvv_1"].ToString()
        //        }
        //    }
        //    ,
        //    new object[] {
        //        bool.Parse(ConfigurationManager.AppSettings["tripType_2"].ToString()),
        //        ConfigurationManager.AppSettings["countries_2"].ToString(),
        //        DateTime.Parse(ConfigurationManager.AppSettings["departDate_2"]),
        //        DateTime.Parse(ConfigurationManager.AppSettings["returnDate_2"]),
        //        ConfigurationManager.AppSettings["coverType_2"].ToString(),
        //        ConfigurationManager.AppSettings["adultAge_2"].ToString(),
        //        ConfigurationManager.AppSettings["childAge_2"].ToString(),
        //        Convert.ToInt32(ConfigurationManager.AppSettings["planNo_2"]),
        //        new ApplicantDetail
        //        {
        //            aNRIC = ConfigurationManager.AppSettings["aNRIC_2"].ToString(),
        //            aFullName = ConfigurationManager.AppSettings["aFullName_2"].ToString(),
        //            aDOB = ConfigurationManager.AppSettings["aDOB_2"].ToString(),
        //            aNationality = ConfigurationManager.AppSettings["aNationality_2"].ToString(),
        //            aMobile = ConfigurationManager.AppSettings["aMobile_2"].ToString(),
        //            aEmail = ConfigurationManager.AppSettings["aEmail_2"].ToString()
        //        },
        //        new CreditCardInfo
        //        {
        //            cardNo = ConfigurationManager.AppSettings["cardNo_2"].ToString(),
        //            cardHolderName = ConfigurationManager.AppSettings["cardHolderName_2"].ToString(),
        //            expiryDate = ConfigurationManager.AppSettings["expiryDate_2"].ToString(),
        //            cvv = ConfigurationManager.AppSettings["cvv_2"].ToString()
        //        }
        //    }
        //};

        static object[] EndToEndData= AddConfigData2();

        static object[] NewData = AddConfigData3();
        //{
        //    addConfigData(1),
        //    addConfigData(2)
        //    addConfigData2()
        //};

        //public static object[] addConfigData(int i)
        //{
        //    return new object[] {
        //        bool.Parse(ConfigurationManager.AppSettings["tripType_" + i].ToString()),
        //        ConfigurationManager.AppSettings["countries_" + i].ToString(),
        //        DateTime.Parse(ConfigurationManager.AppSettings["departDate_" + i]),
        //        DateTime.Parse(ConfigurationManager.AppSettings["returnDate_" + i]),
        //        ConfigurationManager.AppSettings["coverType_" + i].ToString(),
        //        ConfigurationManager.AppSettings["adultAge_" + i].ToString(),
        //        ConfigurationManager.AppSettings["childAge_" + i].ToString(),
        //        Convert.ToInt32(ConfigurationManager.AppSettings["planNo_" + i]),
        //        new ApplicantDetail
        //        {
        //            aNRIC = ConfigurationManager.AppSettings["aNRIC_" + i].ToString(),
        //            aFullName = ConfigurationManager.AppSettings["aFullName_" + i].ToString(),
        //            aDOB = ConfigurationManager.AppSettings["aDOB_" + i].ToString(),
        //            aNationality = ConfigurationManager.AppSettings["aNationality_" + i].ToString(),
        //            aMobile = ConfigurationManager.AppSettings["aMobile_" + i].ToString(),
        //            aEmail = ConfigurationManager.AppSettings["aEmail_" + i].ToString()
        //        },
        //        new CreditCardInfo
        //        {
        //            cardNo = ConfigurationManager.AppSettings["cardNo_" + i].ToString(),
        //            cardHolderName = ConfigurationManager.AppSettings["cardHolderName_" + i].ToString(),
        //            expiryDate = ConfigurationManager.AppSettings["expiryDate_" + i].ToString(),
        //            cvv = ConfigurationManager.AppSettings["cvv_" + i].ToString()
        //        }
        //    };
        ////}


        /// <summary>
        /// Transfers the information from the json file into the input for the EndToEnd Test
        /// </summary>
        public static object[] AddConfigData2()
        {
            List<object> full = new List<object>();
            List<InputData> fromJson = LoadJsonInput(); // fromJson will contain all the information from config.json
            int listLen = fromJson.Count;
            for (int i = 0; i < listLen; i++)
            {
                List<object> singleSet = new List<object>();
                singleSet.Add(fromJson[i].quoteData.isSingleTrip);
                singleSet.Add(fromJson[i].quoteData.countries);
                singleSet.Add(fromJson[i].quoteData.departDate);
                singleSet.Add(fromJson[i].quoteData.returnDate);
                singleSet.Add(fromJson[i].quoteData.coverType);
                singleSet.Add(fromJson[i].quoteData.adultAge);
                singleSet.Add(fromJson[i].quoteData.childAge);
                singleSet.Add(fromJson[i].planNo);
                singleSet.Add(fromJson[i].applicantDetail);
                singleSet.Add(fromJson[i].creditCardInfo);
                singleSet.Add(fromJson[i].testid);
                singleSet.Add(fromJson[i].testName);
                full.Add(singleSet.ToArray()); 
            }
            return full.ToArray();

        }


        public static object[] AddConfigData3()
        {
            List<object> full = new List<object>();
            List<InputData> fromJson = LoadJsonInput(); // fromJson will contain all the information from config.json
            int listLen = fromJson.Count;
            for (int i = 0; i < listLen; i++)
            {
                List<object> singleSet = new List<object>();
                singleSet.Add(fromJson[i]);
                full.Add(singleSet.ToArray());
            }
            return full.ToArray();

        }

        /// <summary>
        /// Retrieves the information from config.json file,
        /// and deserialises into InputData class. Returns as a list of that class.
        /// </summary>
        public static List<InputData> LoadJsonInput()
        {
            string jsonConfigFolder = Path.GetFullPath(ConfigurationManager.AppSettings["testFolder"]);
            using (StreamReader r = new StreamReader(jsonConfigFolder + "config2.json"))
            {
                string json = r.ReadToEnd();
                List<InputData> foo = JsonConvert.DeserializeObject<List<InputData>>(json);
                return foo;
            }
        }


        public static FullElementSelector LoadElementSelectors()
        {
            string jsonConfigFolder = Path.GetFullPath(ConfigurationManager.AppSettings["testFolder"]);
            using (StreamReader r = new StreamReader(jsonConfigFolder + "elementConfig.json"))
            {
                string json = r.ReadToEnd();
                FullElementSelector foo = JsonConvert.DeserializeObject<FullElementSelector>(json);
                return foo;
            }
        }

        public static List<QuoteErrors> LoadQuoteErrors()
        {
            string jsonConfigFolder = Path.GetFullPath(ConfigurationManager.AppSettings["testFolder"]);
            using (StreamReader r = new StreamReader(jsonConfigFolder + "quoteErrors.json"))
            {
                string json = r.ReadToEnd();
                List<QuoteErrors> foo = JsonConvert.DeserializeObject<List<QuoteErrors>>(json);
                return foo;
            }
        }

        public JObject LoadToolTip()
        {
            string jsonConfigFolder = Path.GetFullPath(ConfigurationManager.AppSettings["testFolder"]);
            JObject o1 = JObject.Parse(File.ReadAllText(jsonConfigFolder + "quoteToolTip.json"));
            //string a = (string)o1["tooltip"][0]["Destination"];\
            return o1;
        }

        [OneTimeSetUp()]
        public void SetupTest()
        {
            Driver.Initialize();
            //LoadToolTip();
        }

        [OneTimeTearDown()]
        public void MyTestCleanup()
        {
            Driver.Close();
        }

        //[TearDown()]
        //public void EachTestCleanup()
        //{
        //    IAlert alert = Driver.Instance.SwitchTo().Alert();
        //    alert.Accept();
        //}
    }
}
