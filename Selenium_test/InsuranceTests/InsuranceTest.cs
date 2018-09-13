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
using System.Linq;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using DigibankUAT;

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
            UITest(TestName, () =>
            {
                try
                {
                    if(input.runQuotePageFunctionality)
                        QuoteFunctionality();
                    GetQuote(input.quoteData, fullElementSelector, input.testid, input.testName);
                    if(input.runPlanPageFunctionality)
                        PlanPage.PlanPageFunctionalityTest(fullElementSelector);

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


        //[Test]
        //[TestCaseSource("checkRatesData")]
        ////[TestCaseSource("NewData2")]
        //public void Test_005_CheckRates(InputData input)
        //{


        //    FullElementSelector fullElementSelector = LoadElementSelectors();
        //    string TestName = "CHECK_RATE_" + input.testid + "-" + input.testName;
        //    UITest(TestName, () =>
        //    {

        //        //before your loop

        //        //in your loop



        //        //after your loop

        //        //input.quoteData.isSingleTrip,input.quoteData.countries,input.quoteData.regionNo,input.quoteData.departDate,input.quoteData.returnDate,input.quoteData.coverType,input.quoteData.adultAge,input.quoteData.childAge,input.quoteData.promoCode

        //        try
        //        {
        //            GetQuote(input.quoteData, fullElementSelector, input.testid, input.testName);
        //            //QuotePage.PlanPageFunctionalityTest(fullElementSelector);
        //            List<double> displayedRates = new List<double>();
        //            displayedRates = PlanPage.RetrievePlanAmount();
        //            LogRatesChecking(input, displayedRates);
        //        }
        //        catch (Exception ex)
        //        {
        //            string message_ = "Exception for " + TestName + " : " + ex.Message;
        //            Helper.redirectConsoleLog(TestName, message_);
        //            Assert.Fail();
        //        }



        //        string message = TestName + " passed.";

        //        string path = ConfigurationManager.AppSettings["testFolder"].ToString();

        //        SaveScreenshot(path, "Success", TestName);
        //        Helper.redirectConsoleLog(TestName, message);

        //    });
        //}

        //[Test]
        //public void Test_005_CheckRates_AllInOne()
        //{
        //    string TestName = "Check_Rates_AllinOne";
        //    List<InputData> ratesInput = LoadJsonInput(ConfigurationManager.AppSettings["CheckRatesSource"].ToString());
        //    FullElementSelector fullElementSelector = LoadElementSelectors();
        //    string path = ConfigurationManager.AppSettings["testFolder"].ToString();
        //    UITest(TestName, () =>
        //    {




        //        foreach (InputData i in ratesInput)
        //        {
        //            try
        //            {

        //                string TestName_ = "CHECK_RATE_" + i.testid + "-" + i.testName;
        //                GetQuote(i.quoteData, fullElementSelector, i.testid, i.testName);
        //                List<double> displayedRates = new List<double>();
        //                displayedRates = PlanPage.RetrievePlanAmount();
        //                LogRatesChecking(i, displayedRates);
        //                SaveScreenshot(path, "Success", TestName_);
        //            }
        //            //After the loop
        //            //QuotePage.PlanPageFunctionalityTest(fullElementSelector);


        //            catch (Exception ex)
        //            {
        //                string message_ = i.testid + " # " + "Exception for " + "-" + i.testName + "___" + TestName + " : " + ex.Message;
        //                Helper.redirectConsoleLog(TestName, message_);
        //            }
        //        }


        //        string message = TestName + " passed.";

        //        //string path = ConfigurationManager.AppSettings["testFolder"].ToString();
        //        //CalculateTestPassRates();
                
        //        Helper.redirectConsoleLog(TestName, message);

        //    });
        //}



        public void CalculateTestPassRates()
        {
            string fileName = ConfigurationManager.AppSettings["checkRatesFileName"];

            string[] lines = System.IO.File.ReadAllLines(Path.Combine(ConfigurationManager.AppSettings["testFolder"] + fileName + ".csv"));
            IEnumerable<string> strs = lines;

            //var columnQuery =
            //    from line in strs
            //    let elements = line.Split(',')
            //    select Convert.ToInt32(elements[4]);

            IEnumerable<IEnumerable<int>> multiColQuery =
                from line in strs
                let elements = line.Split(',')
                let scores = elements.Skip(4)
                select (from str in scores
                        select Convert.ToInt32(str));

            var results = multiColQuery.ToList();
            int columnCount = results[0].Count();

            for (int column = 0; column < columnCount; column++)
            {
                var results2 = from row in results
                               select row.ElementAt(column);

                double average = results2.Average() * 100;
                int passed = results2.Sum();
                int failed = results2.Count() - passed;

                // Add one to column because the first exam is Exam #1,  
                // not Exam #0.  
                Console.WriteLine("Pass percentage: {0}%   Total Passed: {1}   Total Failed: {2}", average, passed, failed);

            }
        }


        [Test]
        public void Test_009_CalculateTestPassRates()
        {
            string TestName = "CalculateTestPassRates";
            try
            {
                CalculateTestPassRates();

            }
            catch (Exception ex)
            {
                string message_ = "Exception for " + TestName + " : " + ex.Message;
                Helper.redirectConsoleLog(TestName, message_);
                Assert.Fail();
            }

        }

        public static void GetQuote(QuoteData quoteData, FullElementSelector fullElementSelector, string testId, string testName)
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
                if (ConfigurationManager.AppSettings["paymentMode"] == "CC")
                {
                    QuotePage.GotoQuotePage();
                }
                else
                {
                    Driver.Instance.SwitchTo().ParentFrame();
                }


                //if (quoteData.coverType != "Family")
                //{
                if (Helper.isAlertPresent())
                {
                    IAlert alert = Driver.Instance.SwitchTo().Alert();
                    alert.Accept();
                }
                //}
                //QuotePage.FillSection(countries).FillSection(dates).FillSection(adultAge).GetQuote();
                QuotePage.FillSection(_quoteData).GetQuote(fullElementSelector, testId, testName);


                //Assert.IsTrue(QuotePage.GetCurrentURLSlug() == fullElementSelector.planSummarySlug, "Plan Summary Page not reached");
                //Console.WriteLine(_isSingleTrip + ", " + _countries + ", " + _departDate + ", " + _returnDate + ", " + _coverType);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                Helper.WriteToCSV("Quote Page", "Plan Summary Page reached", false, ex.Message, testId, testName);

                Assert.Fail();
            }

            Console.WriteLine("GetQuote passed.");
            Helper.WriteToCSV("Quote Page", "Plan Summary Page reached", true, null, testId, testName);



        }

        [Test, TestCaseSource("NewData"), Order(1)]
        public void Test_002_FillTravelDetails(InputData input)
        {
            FullElementSelector fullElementSelector = LoadElementSelectors();
            string TestName = input.testid + "-" + input.testName;
            UITest(TestName, () =>
            {
                if (input.runQuotePageFunctionality)
                    QuoteFunctionality();
                GetQuote(input.quoteData, fullElementSelector, input.testid, input.testName);
                //EditTravellerDetailsPage.TravelDetailPageFunctionalityTest(fullElementSelector);

                //if (input.runPlanPageFunctionality)
                //    PlanPage.PlanPageFunctionalityTest(fullElementSelector);

                //EditTravellerDetailsPage.TravelDetailPageFunctionalityTest(fullElementSelector);

                SelectPlan(input.planNo, fullElementSelector, input.testid, input.testName);
                ReadOnlyCollection<IWebElement> travelDetailsBox = EditTravellerDetailsPage.RetrieveTravelDetails();

                //CheckTravelDetails(travelDetailsBox, input);
                

                FillTravelDetails(input.applicantDetail, input.travellerDetails, input.testid, input.testName);

            });
        }



        //[Test, TestCaseSource("NewData"), Order(1)]
        public void Test_000_VerifyPlanCosts(InputData input)
        {
            FullElementSelector fullElementSelector = LoadElementSelectors();
            string TestName = input.testid + "-" + input.testName;
            UITest("VerifyPlanCosts", () =>
            {

                GetQuote(input.quoteData, fullElementSelector, input.testid, input.testName);
                SelectPlan(input.planNo, fullElementSelector, input.testid, input.testName);


            });
        }

        public string[] SelectPlan(int _planNo, FullElementSelector fullElementSelector, string testId, string testName)
        {
            string[] planAmount = null;
            try
            {
                planAmount = PlanPage.SelectPlan(_planNo, fullElementSelector, testId, testName);
                //Assert.IsTrue(PlanPage.GetCurrentURLSlug() == "apply/application-details", "Enter Travel Details Page not reached.");
                
                //Thread.Sleep(100000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                Helper.WriteToCSV("Plan Page", "Enter Travel Details Page reached", false, ex.Message, testId, testName);

                Assert.Fail();
            }


            Console.WriteLine("Select Plan passed.");
            Helper.WriteToCSV("Plan Page", "Enter Travel Details Page reached", true, null, testId, testName);

            return planAmount;

        }


        //public void VerifyPlanAmount()
        //{
        //    try
        //    {
        //        PlanPage.VerifyPlanAmount();

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception: " + ex.Message);
        //        Assert.Fail();
        //    }



        //    Console.WriteLine("Select Plan passed.");

        //}

        static object[] PLANDATA_1 = {
            new int[] {
                Convert.ToInt32(ConfigurationManager.AppSettings["planNo_1"])
            }
        };

        [Test, TestCaseSource("NewData")]
        public void Test_003_FillPaymentDetails(InputData input)
        {
            FullElementSelector fullElementSelector = LoadElementSelectors();
            string TestName = input.testid + "-" + input.testName;
            UITest(TestName, () =>
            {
                if (input.runQuotePageFunctionality)
                    QuoteFunctionality();
                GetQuote(input.quoteData, fullElementSelector, input.testid, input.testName);
                //EditTravellerDetailsPage.TravelDetailPageFunctionalityTest(fullElementSelector);

                //if (input.runPlanPageFunctionality)
                //    PlanPage.PlanPageFunctionalityTest(fullElementSelector);

                //EditTravellerDetailsPage.TravelDetailPageFunctionalityTest(fullElementSelector);

                SelectPlan(input.planNo, fullElementSelector, input.testid, input.testName);
                ReadOnlyCollection<IWebElement> travelDetailsBox = EditTravellerDetailsPage.RetrieveTravelDetails();

                //CheckTravelDetails(travelDetailsBox, input);


                FillTravelDetails(input.applicantDetail, input.travellerDetails, input.testid, input.testName);
                FillPaymentDetails(ConfigurationManager.AppSettings["paymentMode"], input.creditCardInfo, fullElementSelector, input.testid, input.testName);

            });
        }

        [Test, TestCaseSource("NewData")]
        public void Test_004_EndToEnd(InputData input)
        {
            FullElementSelector fullElementSelector = LoadElementSelectors();
            string TestName = input.testid + "-" + input.testName;
            UITest(TestName, () =>
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Starting test for test id: " + input.testid + " , test case: " + input.testName);
                Console.ForegroundColor = ConsoleColor.White;
                if (input.runQuotePageFunctionality)
                    QuoteFunctionality();

                if (ConfigurationManager.AppSettings["paymentMode"] != "CC")
                {
                    DigibankPage.Login(input.userName, input.pin);
                    //DigibankPage.Login("RRCIN002", "173734");

                    DigibankPage.GoToChubb();
                }
                GetQuote(input.quoteData, fullElementSelector, input.testid, input.testName);
                //EditTravellerDetailsPage.TravelDetailPageFunctionalityTest(fullElementSelector);

                //if (input.runPlanPageFunctionality)
                //    PlanPage.PlanPageFunctionalityTest(fullElementSelector);

                //EditTravellerDetailsPage.TravelDetailPageFunctionalityTest(fullElementSelector);

                string[] planAmount = SelectPlan(input.planNo, fullElementSelector, input.testid, input.testName);
                //ReadOnlyCollection<IWebElement> travelDetailsBox = EditTravellerDetailsPage.RetrieveTravelDetails();
                //CheckTravelDetails(travelDetailsBox, input, Convert.ToDouble(planAmount[1]));
                //CheckTravelDetails(travelDetailsBox, input);


                FillTravelDetails(input.applicantDetail, input.travellerDetails, input.testid, input.testName);
                FillPaymentDetails(ConfigurationManager.AppSettings["paymentMode"], input.creditCardInfo, fullElementSelector, input.testid, input.testName);

                VerifyDetailsPage.ProceedWithPayment(fullElementSelector, input.testid, input.testName);

                PaymentSuccessPage.ViewPDF(input.testid, input.testName);

            });
        }

        public void FillTravelDetails(ApplicantDetail _a, TravellerDetails _t, string testId, string testName)
        {
            ApplicantDetail applicantDetail = _a;

            FullElementSelector fullElementSelector = LoadElementSelectors();
            try
            {
                //EditTravellerDetailsPage.TravelDetailPageFunctionalityTest(fullElementSelector);
                EditTravellerDetailsPage.FillSection(applicantDetail).FillSection(_t).Proceed(fullElementSelector, testId, testName);


                /* traveller info */
                // if needed

                //Thread.Sleep(5000);

                //TravellerDetailsSummaryPage.ProceedToPaymentSelection();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                Helper.WriteToCSV(null, ex.Message, false);
                Assert.Fail();
            }

            Helper.WriteToCSV("Payment Details Page", "Payment details page reached", true, null, testId, testName);


            Console.WriteLine("Fill traveller details passed.");

        }

        public void FillPaymentDetails(string paymentType, CreditCardInfo creditCardInfo, FullElementSelector fullElementSelector, string testId, string testName)
        {
            try
            {
                PaymentSelectionPage.SelectPaymentTypeAndProceed(paymentType, fullElementSelector, testId, testName);

                Helper.WriteToCSV("Payment Details Page", "payment option selected", true, null, testId, testName);

                //trigger payment
                if(paymentType == "CC")
                    CreditCardDetailsPage.FillSection(creditCardInfo).Proceed(fullElementSelector, testId, testName);
                else
                    CreditCardDetailsPage.FillSection(null).Proceed(fullElementSelector, testId, testName);




            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                Assert.Fail();
            }


            Helper.WriteToCSV("Verify Details Page", "Verify details page reached", true, null, testId, testName);

            Console.WriteLine("Payment Details input successfully.");
        }


        [Test]
        public void QuoteFunctionality()
        {
            FullElementSelector fullElementSelector = LoadElementSelectors();
            List<QuoteErrors> quoteErrors = LoadQuoteErrors();
            //string TestName = input.testid + "-" + input.testName;
            string TestName = "Quote Functionality Test";
            ////For header
            //var csv = new StringBuilder();
            //var newLine = string.Format("{0}|{1}|{2}|{3}|{4}", "Date/Time", "Category", "Description", "Status", "More Info");
            //csv.AppendLine(newLine);
            //File.AppendAllText(ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + ".csv", csv.ToString());

            UITest(TestName, () =>
            {
                try
                {
                    if (Helper.isAlertPresent())
                    {
                        IAlert alert = Driver.Instance.SwitchTo().Alert();
                        alert.Accept();
                    }
                    QuotePage.GotoQuotePage();

                    //QuotePage.QuoteErrorMessageTest(fullElementSelector, quoteErrors);

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
                    QuotePage.GotoQuotePage();
                    if (Helper.isAlertPresent())
                    {
                        IAlert alert = Driver.Instance.SwitchTo().Alert();
                        alert.Accept();
                    }

                    //QuotePage.GotoQuotePage();
                    //if (Helper.isAlertPresent())
                    //{
                    //    IAlert alert = Driver.Instance.SwitchTo().Alert();
                    //    alert.Accept();
                    //}
                    ////QuotePage.FillSection(null).GetQuote(fullElementSelector);
                    //////QuotePage.PlanPageFunctionalityTest(fullElementSelector);
                    //QuotePage.QuoteFunctionalityTest(fullElementSelector);
                    QuotePage.GotoQuotePage();
                    if (Helper.isAlertPresent())
                    {
                        IAlert alert = Driver.Instance.SwitchTo().Alert();
                        alert.Accept();
                    }
                    QuotePage.QuoteFunctionalityTest(fullElementSelector, false);

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
        //[Test, TestCaseSource("NewData"), Order(7)]
        public void Test_007_EndToEnd(InputData input)
        {
            FullElementSelector fullElementSelector = LoadElementSelectors();
            string TestName = input.testid + "-" + input.testName;
            UITest(TestName, () =>
            {
                try
                {
                    GetQuote(input.quoteData, fullElementSelector, input.testid, input.testName);
                    SelectPlan(input.planNo, fullElementSelector, input.testid, input.testName);
                    //FillTravelDetails(input.applicantDetail);
                    FillPaymentDetails(ConfigurationManager.AppSettings["paymentMode"], input.creditCardInfo, fullElementSelector, input.testid, input.testName);

                    //Assert.IsTrue(QuotePage.GetCurrentURLSlug() == "quote", "Quote Summary Page not reached");
                    //Test_004_FillPaymentDetails(_isSingleTrip, _countries, _departDate, _returnDate, _coverType, _adultAge, _childAge, _planNo, _a, _CCInfo);



                    PaymentSuccessPage.ViewPDF(input.testid, input.testName);
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

        public static void CheckTravelDetails(ReadOnlyCollection<IWebElement> travelDetailsBox, InputData input, double chosenPlanAmount)
        {
            string expectedTripType;
            //policy Type Check
            if (input.quoteData.isSingleTrip)
                expectedTripType = "Single Trip";
            else
                expectedTripType = "Annual Multi-Trip";

            if (expectedTripType != travelDetailsBox[0].FindElement(By.XPath("./div/div")).Text)
                Helper.WriteToCSV("Enter Travel Details Page", "Trip Type Check", false, null, input.testid, input.testName);
            else
                Helper.WriteToCSV("Enter Travel Details Page", "Trip Type Check", true, null, input.testid, input.testName);

            //Destination check Regex.Replace(countries, @"\s+", "");
            string inputCountry = null;
            if(!String.IsNullOrWhiteSpace(input.quoteData.countries))
                inputCountry = Regex.Replace(input.quoteData.countries, @"\s+", "").ToLower();

            string displayedCountry = Regex.Replace(travelDetailsBox[1].FindElement(By.XPath("./div/div")).Text, @"\s+", "").ToLower();
            if (input.quoteData.isSingleTrip)
            {
                if (inputCountry != displayedCountry)
                    Helper.WriteToCSV("Enter Travel Details Page", "Destination Check", false, "Displayed: " + displayedCountry, input.testid, input.testName);
                else
                    Helper.WriteToCSV("Enter Travel Details Page", "Destination Check", true, null, input.testid, input.testName);
            }
            else
            {
                if (("region" + input.quoteData.regionNo != displayedCountry))
                    Helper.WriteToCSV("Enter Travel Details Page", "Destination Check", false, "Displayed: " + displayedCountry, input.testid, input.testName);
                else
                    Helper.WriteToCSV("Enter Travel Details Page", "Destination Check", true, null, input.testid, input.testName);
            }

            //Dates Regex.Replace(travelDetailsBox[1].Text, @"\s+", "")
            string[] displayedDates = (Regex.Replace(travelDetailsBox[2].FindElement(By.XPath("./div/div")).Text, @"\s+", "")).Split('-');
            DateTime displayedDepartDate = DateTime.ParseExact(displayedDates[0], "ddMMMyyyy", null);
            DateTime displayedReturnDate = DateTime.ParseExact(displayedDates[1], "ddMMMyyyy", null);

            if (input.quoteData.isSingleTrip)
            {
                if (displayedDepartDate.Date != input.quoteData.departDate.Date || displayedReturnDate.Date != input.quoteData.returnDate.Date)
                    Helper.WriteToCSV("Enter Travel Details Page", "Depart and Return Date Check", false, null, input.testid, input.testName);
                else
                    Helper.WriteToCSV("Enter Travel Details Page", "Depart and Return Date Check", true, null, input.testid, input.testName);

            }
            else
            {
                if (displayedDepartDate.Date != input.quoteData.departDate.Date)
                    Helper.WriteToCSV("Enter Travel Details Page", "Depart Date Check", false, null, input.testid, input.testName);
                else
                    Helper.WriteToCSV("Enter Travel Details Page", "Depart Date Check", true, null, input.testid, input.testName);

            }

            //Covertype
            if (input.quoteData.coverType.ToLower() != travelDetailsBox[3].FindElement(By.XPath("./div/div")).Text.ToLower())
                Helper.WriteToCSV("Enter Travel Details Page", "Cover Type Check", false, "Displayed: " + travelDetailsBox[3].FindElement(By.XPath("./div/div")).Text, input.testid, input.testName);
            else
                Helper.WriteToCSV("Enter Travel Details Page", "Cover Type Check", true, null, input.testid, input.testName);

            //plan
            bool planCorrect;
            switch(input.planNo)
            {
                case 1:
                    planCorrect = (travelDetailsBox[4].FindElement(By.XPath("./div/div")).Text.Contains("Classic")) ? true : false;

                    break;

                case 2:
                    planCorrect = (travelDetailsBox[4].FindElement(By.XPath("./div/div")).Text.Contains("Premier")) ? true : false;

                    break;

                case 3:
                    planCorrect = (travelDetailsBox[4].FindElement(By.XPath("./div/div")).Text.Contains("Platinum")) ? true : false;

                
                    break;
                default:
                    planCorrect = false;
                    break;
                    


            }
            if(planCorrect)
                Helper.WriteToCSV("Enter Travel Details Page", "Selected Plan Check", true, null, input.testid, input.testName);
            else
                Helper.WriteToCSV("Enter Travel Details Page", "Selected Plan Check", false, "Displayed: " + travelDetailsBox[4].FindElement(By.XPath("./div/div")).Text, input.testid, input.testName);



            //Total premium
            string displayedPremium = (travelDetailsBox[5].FindElement(By.XPath("./div/div")).Text);
            if (chosenPlanAmount != Convert.ToDouble(displayedPremium.Substring(3)))
                Helper.WriteToCSV("Enter Travel Details Page", "Displayed Premium Check", false, "Displayed: " + displayedPremium, input.testid, input.testName);
            else
                Helper.WriteToCSV("Enter Travel Details Page", "Displayed Premium Check", true, null, input.testid, input.testName);

        }



        static object[] EndToEndData= AddConfigData2();

        static object[] NewData = AddConfigData3(Convert.ToInt16(ConfigurationManager.AppSettings["endToEndStart"]));


        static object[] checkRatesData = AddConfigData4(Convert.ToInt16(ConfigurationManager.AppSettings["checkRatesStart"]));
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
            List<InputData> fromJson = LoadJsonInput("config"); // fromJson will contain all the information from config.json
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


        public static object[] AddConfigData3(int start)
        {
            List<object> full = new List<object>();
            List<InputData> fromJson = LoadJsonInput(ConfigurationManager.AppSettings["endToEndFileName"]); // fromJson will contain all the information from config.json
            int listLen = fromJson.Count;
            for (int i = start-1; i < listLen; i++)
            {
                List<object> singleSet = new List<object>();
                singleSet.Add(fromJson[i]);
                full.Add(singleSet.ToArray());
            }
            return full.ToArray();

        }

        public static object[] AddConfigData4(int start)
        {
            List<object> full = new List<object>();
            List<InputData> fromJson = LoadJsonInput("checkRatesTesting"); // fromJson will contain all the information from config.json
            int listLen = fromJson.Count;
            for (int i = start-1; i < listLen; i++)
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
        public static List<InputData> LoadJsonInput(string fileName)
        {
            string jsonConfigFolder = Path.GetFullPath(ConfigurationManager.AppSettings["testFolder"]);
            //string jsonConfigFolder = Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\");
            using (StreamReader r = new StreamReader(jsonConfigFolder + fileName + ".json"))
            {
                string json = r.ReadToEnd();
                List<InputData> foo = JsonConvert.DeserializeObject<List<InputData>>(json);
                return foo;
            }
        }

        public static List<InputData> LoadJsonInput2()
        {
            string jsonConfigFolder = Path.GetFullPath(ConfigurationManager.AppSettings["testFolder"]);
            using (StreamReader r = new StreamReader(jsonConfigFolder + "checkRatesTesting2.json"))
            {
                string json = r.ReadToEnd();
                List<InputData> foo = JsonConvert.DeserializeObject<List<InputData>>(json);
                return foo;
            }
        }

        public static FullElementSelector LoadElementSelectors()
        {
            string jsonConfigFolder = Path.GetFullPath(ConfigurationManager.AppSettings["testFolder"]);
            //
            using (StreamReader r = new StreamReader(jsonConfigFolder + ConfigurationManager.AppSettings["elementConfigFileName"] + ".json"))
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


        public static void LogRatesChecking(InputData input, List<double> displayedRates)
        {
            var csv = new StringBuilder();
            int classicOutcome = Convert.ToInt16(displayedRates[0].Equals(input.expectedRates[0]));
            int premierOutcome = Convert.ToInt16(displayedRates[1].Equals(input.expectedRates[1]));
            int platinumOutcome = Convert.ToInt16(displayedRates[2].Equals(input.expectedRates[2]));
            var newLine = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}|{17}|{18}|{19}|{20}|{21}|{22}|{23}", input.testid, (input.quoteData.returnDate - input.quoteData.departDate).Days + 1, (input.quoteData.isSingleTrip?"Single Trip":"Annual Trip"), (input.quoteData.isSingleTrip ? input.quoteData.countries : null), (!input.quoteData.isSingleTrip ? input.quoteData.regionNo.ToString() : null), input.quoteData.departDate.ToString("dd MMM yyyy"), (input.quoteData.isSingleTrip ? input.quoteData.returnDate.ToString("dd MMM yyyy") : null), input.quoteData.coverType, input.quoteData.adultAge, input.quoteData.childAge, input.quoteData.promoCode, displayedRates[0].ToString(), input.expectedRates[0], classicOutcome, displayedRates[1].ToString(), input.expectedRates[1], premierOutcome, displayedRates[2].ToString(), input.expectedRates[2], platinumOutcome, (classicOutcome+premierOutcome+platinumOutcome == 3? 1 : 0), displayedRates[3], displayedRates[4], displayedRates[5]); //Convert.ToInt16(displayedRates[0].Equals(input.expectedRates[0])), Convert.ToInt16(displayedRates[1].Equals(input.expectedRates[1])), Convert.ToInt16(displayedRates[2].Equals(input.expectedRates[2])));
            csv.AppendLine(newLine);
            File.AppendAllText(ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["checkRatesFileName"] + ".csv", csv.ToString());
        }


        [OneTimeSetUp()]
        public void SetupTest()
        {
            //Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\");
            //Driver.Initialize();
            //For header
            var csv = new StringBuilder();
            var newLine = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}", "Date/Time", "Test ID", "Scenario", "Category", "Description", "Status", "More Info");
            csv.AppendLine(newLine);
            File.AppendAllText(ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + ".csv", csv.ToString());
            //LoadToolTip();

            //var csv2 = new StringBuilder();

            //var newLine2 = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}|{17}|{18}|{19}|{20}|{21}{22}|{23}|{24}", "Id", "TripDuration", "TripType", "Countries", "RegionNo", "departDate", "returnDate", "coverType", "adultAge", "childAge", "promoCode", "Displayed Classic", "Expected Classic", "Classic Outcome", "Displayed Premier", "Expected Premier", "Premier Outcome", "Displayed Platinum", "Expected Platinum", "Platinum Outcome", "Overall Outcome", "=\"Pass Percentage: \"&AVERAGE(T:T)*100&\"%\"", "", "=\"# Passed: \"&SUM(U:U)", "=\"# Failed: \"&SUM(U:U)/AVERAGE(U:U)-SUM(U:U)");
            //csv2.AppendLine(newLine2);
            //File.AppendAllText(ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["checkRatesFileName"] + ".csv", csv2.ToString());
            ////LoadToolTip();
        }

        [OneTimeTearDown()]
        public void MyTestCleanup()
        {
            Driver.Close();
            //string oldfilePath = ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + ".csv";
            //string newfilePath = ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + "_" + DateTime.Now.ToString("ddMMMyyyy_hhmmss") + ".csv";
            System.IO.File.Move(ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + ".csv", ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + "_" + DateTime.Now.ToString("ddMMMyyyy_hhmmss") + ".csv");
            //System.IO.File.Move(ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["checkRatesFileName"] + ".csv", ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["checkRatesFileName"] + "_" + DateTime.Now.ToString("ddMMMyyyy_hhmmss") + ".csv");

        }


        [SetUp()]
        public void SetupTest2()
        {
            //Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\");
            Driver.Initialize(ConfigurationManager.AppSettings["browser"]);

        }
        [TearDown()]
        public void MyTestCleanup2()
        {
            Driver.Close();
            //string oldfilePath = ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + ".csv";
            //string newfilePath = ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + "_" + DateTime.Now.ToString("ddMMMyyyy_hhmmss") + ".csv";
            //System.IO.File.Move(ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + ".csv", ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + "_" + DateTime.Now.ToString("ddMMMyyyy_hhmmss") + ".csv");
            //System.IO.File.Move(ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["checkRatesFileName"] + ".csv", ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["checkRatesFileName"] + "_" + DateTime.Now.ToString("ddMMMyyyy_hhmmss") + ".csv");

        }

        //[TearDown()]
        //public void EachTestCleanup()
        //{
        //    IAlert alert = Driver.Instance.SwitchTo().Alert();
        //    alert.Accept();
        //}
    }



    [TestFixture]
    public class RatesChecking : PageTestBase
    {
        [Test]
        public void Test_005_CheckRates_AllInOne()
        {
            string TestName = "Check_Rates_AllinOne";
            List<InputData> ratesInput = InsuranceTest.LoadJsonInput(ConfigurationManager.AppSettings["CheckRatesSource"].ToString());
            Console.WriteLine("Reading from: " + ConfigurationManager.AppSettings["CheckRatesSource"].ToString());
            FullElementSelector fullElementSelector = InsuranceTest.LoadElementSelectors();
            string path = ConfigurationManager.AppSettings["testFolder"].ToString();
            UITest(TestName, () =>
            {




                foreach (InputData i in ratesInput)
                {
                    try
                    {

                        string TestName_ = "CHECK_RATE_" + i.testid + "-" + i.testName;
                        InsuranceTest.GetQuote(i.quoteData, fullElementSelector, i.testid, i.testName);
                        List<double> displayedRates = new List<double>();
                        displayedRates = PlanPage.RetrievePlanAmount();
                        InsuranceTest.LogRatesChecking(i, displayedRates);
                        SaveScreenshot(path, "Success", TestName_);
                    }
                    //After the loop
                    //QuotePage.PlanPageFunctionalityTest(fullElementSelector);


                    catch (Exception ex)
                    {
                        string message_ = i.testid + " # " + "Exception for " + "-" + i.testName + "___" + TestName + " : " + ex.Message;
                        Helper.redirectConsoleLog(TestName, message_);
                    }
                }


                string message = TestName + " passed.";

                //string path = ConfigurationManager.AppSettings["testFolder"].ToString();
                //CalculateTestPassRates();

                Helper.redirectConsoleLog(TestName, message);

            });
        }


        [OneTimeSetUp()]
        public void SetupTest()
        {
            //Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\");
            //Driver.Initialize();
            //For header
            var csv = new StringBuilder();
            var newLine = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}", "Date/Time", "Test ID", "Scenario", "Category", "Description", "Status", "More Info");
            csv.AppendLine(newLine);
            File.AppendAllText(ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + ".csv", csv.ToString());
            //LoadToolTip();

            var csv2 = new StringBuilder();

            var newLine2 = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}|{17}|{18}|{19}|{20}|{21}{22}|{23}|{24}", "Id", "TripDuration", "TripType", "Countries", "RegionNo", "departDate", "returnDate", "coverType", "adultAge", "childAge", "promoCode", "Displayed Classic", "Expected Classic", "Classic Outcome", "Displayed Premier", "Expected Premier", "Premier Outcome", "Displayed Platinum", "Expected Platinum", "Platinum Outcome", "Overall Outcome", "=\"Pass Percentage: \"&AVERAGE(T:T)*100&\"%\"", "", "=\"# Passed: \"&SUM(U:U)", "=\"# Failed: \"&SUM(U:U)/AVERAGE(U:U)-SUM(U:U)");
            csv2.AppendLine(newLine2);
            File.AppendAllText(ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["checkRatesFileName"] + ".csv", csv2.ToString());
            //LoadToolTip();
        }

        [OneTimeTearDown()]
        public void MyTestCleanup()
        {
            Driver.Close();
            //string oldfilePath = ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + ".csv";
            //string newfilePath = ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + "_" + DateTime.Now.ToString("ddMMMyyyy_hhmmss") + ".csv";
            System.IO.File.Move(ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + ".csv", ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + "_" + DateTime.Now.ToString("ddMMMyyyy_hhmmss") + ".csv");
            System.IO.File.Move(ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["checkRatesFileName"] + ".csv", ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["checkRatesFileName"] + "_" + DateTime.Now.ToString("ddMMMyyyy_hhmmss") + ".csv");

        }


        [SetUp()]
        public void SetupTest2()
        {
            //Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\");
            Driver.Initialize(ConfigurationManager.AppSettings["browser"]);

        }
        [TearDown()]
        public void MyTestCleanup2()
        {
            Driver.Close();
            //string oldfilePath = ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + ".csv";
            //string newfilePath = ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + "_" + DateTime.Now.ToString("ddMMMyyyy_hhmmss") + ".csv";
            //System.IO.File.Move(ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + ".csv", ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + "_" + DateTime.Now.ToString("ddMMMyyyy_hhmmss") + ".csv");
            //System.IO.File.Move(ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["checkRatesFileName"] + ".csv", ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["checkRatesFileName"] + "_" + DateTime.Now.ToString("ddMMMyyyy_hhmmss") + ".csv");

        }
    }



    [TestFixture]
    public class Testing : PageTestBase
    {
        [Test, TestCaseSource("NewData")]
        public void LandLordShield(LandLordShieldInput input)
        {
            string url = @"https://chubbstg.prod.acquia-sites.com/hk/dbs/landlordshield-insurance";
            //List<InputData> ratesInput = InsuranceTest.LoadJsonInput(ConfigurationManager.AppSettings["CheckRatesSource"].ToString());
            //FullElementSelector fullElementSelector = InsuranceTest.LoadElementSelectors();
            //string path = ConfigurationManager.AppSettings["testFolder"].ToString();
            string floorAreaElement = "//*[@id='edit_additional_data_insured_gross_area_chosen']/a/span";
            string floorAreaDropDownElements = "//*[@id='edit_additional_data_insured_gross_area_chosen']/div/ul/li";
            string buildingSumInsured = "//*[@id='edit-additional-data-building-sum-insured-optional']";
            string nextButtonElement = "//*[@id='edit-section-01']/div[2]/input";
            
            string dateElement = "//*[@id='edit-agreements-0-effective-date']";
            string monthElement = "//*[@id='ui-datepicker-div']/div/div/select[1]/option";
            string yearElement = "//*[@id='ui-datepicker-div']/div/div/select[2]/option";

            string dayRowElement = "//*[@id='ui-datepicker-div']/table/tbody/tr";
            string indivDayElement = "./td";

            string promoCodeElement = "//*[@id='edit-agreements-0-voucher-voucher-number']";

            string getQuoteButton = "//*[@id='edit-section-02']/div[2]/input";

            UITest("Testing", () =>
            {

                Driver.Instance.Navigate().GoToUrl(url);

                Thread.Sleep(2500);

                Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath(floorAreaElement)));
                Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath(floorAreaElement)));

                Driver.Instance.FindElement(By.XPath(floorAreaElement)).Click();
                ReadOnlyCollection<IWebElement> floorAreaDropDowns = Driver.Instance.FindElements(By.XPath(floorAreaDropDownElements));
                floorAreaDropDowns.FirstOrDefault(a => a.Text == input.grossFloorArea).Click();


                Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath(buildingSumInsured)));
                Driver.Instance.FindElement(By.XPath(buildingSumInsured)).SendKeys(input.sumInsured);

                Driver.Instance.FindElement(By.XPath(nextButtonElement)).Click();

                DateHandler(input.effectiveStartDate, dateElement, monthElement, yearElement, dayRowElement, indivDayElement);



                Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath(getQuoteButton)));
                Driver.Instance.FindElement(By.XPath(getQuoteButton)).Click();

                /* Select Plan */
                Thread.Sleep(2500);

                int plan = input.plan; 
                string discountedLeftElement = "//*[@id='edit-product-ids']/div[";
                string discountedRightElement = "]/div[2]/div/span[1]";

                string discountedPrice = Driver.Instance.FindElement(By.XPath(discountedLeftElement + plan + discountedRightElement)).Text;

                Console.WriteLine("Discounted Price = " + discountedPrice);

                Helper.WriteToCSV("Plan Page", "Discounted Price", true, discountedPrice, input.testId, input.testName);


                string originalLeftElement = "//*[@id='edit-product-ids']/div[";
                string originalRightElement = "]/div[2]/div/span[3]";

                string originalPrice = Driver.Instance.FindElement(By.XPath(originalLeftElement + plan + originalRightElement)).Text;

                Console.WriteLine("Original Price = " + originalPrice);
                Helper.WriteToCSV("Plan Page", "Original Price", true, originalPrice, input.testId, input.testName);


                string planLeftElement = "//*[@id='edit-product-ids']/div[";
                string planRightElement = "]/label";
                Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath(planLeftElement + plan + planRightElement)));
                Driver.Instance.FindElement(By.XPath(planLeftElement + plan + planRightElement)).Click();

                /* Applicant Details */
                Thread.Sleep(1000);

                string firstName = "//*[@id='edit-contract-holder-first-name']";
                string lastName = "//*[@id='edit-contract-holder-last-name']";

                string genderLeftElement = "//*[@id='edit-contract-holder-gender']/div[";
                string genderRightElement = "]/label";

                string dOBElement = "//*[@id='edit-contract-holder-date-of-birth']";


                string emailElement = "//*[@id='edit-contract-holder-contact-detail-list-0-detail']";
                string mobileElement ="//*[@id='edit-contract-holder-contact-detail-list-1-detail']";
                string HKIDElement = "//*[@id='edit-contract-holder-id-list-0-detail']";


                string corrBuildingAddressElement = "//*[@id='edit-contract-holder-address-list-0-lines-0']";
                string corrEstateAddressElement = "//*[@id='edit-contract-holder-address-list-0-lines-1']";
                string corrEstateDistrictElement = "//*[@id='edit-contract-holder-address-list-0-lines-2']";

                string insAddressSameLeftElement = "//*[@id='edit-additional-data-contract-holder-correspondence-address']/div[";
                string insAddressSameRightElement = "]/label";

                string insBuildingAddressElement = "//*[@id='edit-contract-holder-address-list-1-lines-0']";
                string insEstateAddressElement = "//*[@id='edit-contract-holder-address-list-1-lines-1']";
                string insEstateDistrictElement = "//*[@id='edit-contract-holder-address-list-1-lines-2']";


                string detailsNextElement = "//*[@id='edit-insured-address']/div[2]/input";


                Driver.Instance.FindElement(By.XPath(firstName)).SendKeys(input.firstName);
                Driver.Instance.FindElement(By.XPath(lastName)).SendKeys(input.lastName);


                string gender = input.gender;
                string fullGenderElement = null;
                if (gender == "M")
                    fullGenderElement = genderLeftElement + "1" + genderRightElement;
                else
                    fullGenderElement = genderLeftElement + "2" + genderRightElement;

                Driver.Instance.FindElement(By.XPath(fullGenderElement)).Click();


                DateHandler(input.DOB, dOBElement, monthElement, yearElement, dayRowElement, indivDayElement);

                Driver.Instance.FindElement(By.XPath(emailElement)).SendKeys(input.email);
                Driver.Instance.FindElement(By.XPath(mobileElement)).SendKeys(input.mobile);

                //"X1234566"
                Driver.Instance.FindElement(By.XPath(HKIDElement)).SendKeys(input.HKID);

                Driver.Instance.FindElement(By.XPath(corrBuildingAddressElement)).SendKeys(input.corrAddress1);
                Driver.Instance.FindElement(By.XPath(corrEstateAddressElement)).SendKeys(input.corrAddress2);
                Driver.Instance.FindElement(By.XPath(corrEstateDistrictElement)).SendKeys(input.corrAddress3);

                bool sameAddress = input.sameAddress;

                if(!sameAddress)
                {
                    Driver.Instance.FindElement(By.XPath(insAddressSameLeftElement + "2" + insAddressSameRightElement)).Click();

                    Driver.Instance.FindElement(By.XPath(insBuildingAddressElement)).SendKeys(input.insAddress1);
                    Driver.Instance.FindElement(By.XPath(insEstateAddressElement)).SendKeys(input.insAddress2);
                    Driver.Instance.FindElement(By.XPath(insEstateDistrictElement)).SendKeys(input.insAddress3);

                }
                else
                {
                    Driver.Instance.FindElement(By.XPath(insAddressSameLeftElement + "1" + insAddressSameRightElement)).Click();

                }


                Driver.Instance.FindElement(By.XPath(detailsNextElement)).Click();


                /* Review and Pay */ 
                string reviewPremiumElement = "//*[@id='edit-levy-markup']/table/tbody/tr/td/div/div[";
                string reviewPremiumSubElement = "]/div[2]/strong";

                string reviewPremiumAmount = null;
                //string scrollToElement = "//*[@id='edit-levy-markup']/h2";
                //var scrollTo = Driver.Instance.FindElement(By.XPath(scrollToElement));

                //IJavaScriptExecutor js = (IJavaScriptExecutor)Driver.Instance;
                //js.ExecuteScript("arguments[0].scrollIntoView();", scrollTo);

                //ReadOnlyCollection<IWebElement> reviewPremiums = Driver.Instance.FindElements(By.XPath(reviewPremiumElement));
                //foreach(IWebElement reviewPremium in reviewPremiums)
                //{
                //    Thread.Sleep(250);
                //    reviewPremiumAmount = reviewPremium.FindElement(By.XPath(reviewPremiumSubElement)).Text;
                //    Console.WriteLine(reviewPremiumAmount);
                //    Helper.WriteToCSV("Review Page", "Review Premiums", true, reviewPremiumAmount, input.testId, input.testName);

                //}

                for (int p = 1; p <= 3; p++)
                {
                    Thread.Sleep(250);
                    var reviewPremiums = Driver.Instance.FindElement(By.XPath(reviewPremiumElement + p + reviewPremiumSubElement));

                    reviewPremiumAmount = reviewPremiums.Text;
                    Console.WriteLine(reviewPremiumAmount);
                    Helper.WriteToCSV("Review Page", "Review Premiums", true, reviewPremiumAmount, input.testId, input.testName);

                }



                string confirmElement = "//*[@id='edit-4-section-1']/div[2]/input";
                Driver.Instance.FindElement(By.XPath(confirmElement)).Click();



                //string toolBarPremiumElement = "/html/body/div[1]/nav/div[2]/div/div/div[3]/div";

                string firstCheckBoxElement = "//*[@id='edit-4-section-2']/div[1]/div[2]/label";
                string secondCheckBoxElement  = "//*[@id='edit-4-section-2']/div[1]/div[3]/label";

                Driver.Instance.FindElement(By.XPath(firstCheckBoxElement)).Click();
                Driver.Instance.FindElement(By.XPath(secondCheckBoxElement)).Click();

                string reviewConfirmElement = "//*[@id='edit-4-section-2']/div[2]/input";
                Driver.Instance.FindElement(By.XPath(reviewConfirmElement)).Click();

                /* Payment */
                string cardHolderNameElement = "//*[@id='edit-agreements-0-payment-information-detail-card-holder-name']";
                string expiryDateElement = "//*[@id='edit-agreements-0-payment-information-detail-card-expiry-m-m-y-y']";
                string cvv = "//*[@id='edit-agreements-0-payment-information-detail-card-cvv']";

                string payNowElement = "//*[@id='edit-4-section-3']/div[2]/input";

                string tokenExiFrameElement = "//*[@id='tokenExIframe']";

                Driver.Instance.FindElement(By.XPath(cardHolderNameElement)).SendKeys(input.cardHolderName);


                Driver.GetWait().Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.XPath(tokenExiFrameElement)));
                Driver.Instance.FindElement(By.Id("pan")).SendKeys(input.cardNo); //
                Driver.Instance.SwitchTo().DefaultContent();

                Driver.Instance.FindElement(By.XPath(expiryDateElement)).SendKeys(input.expiryDate);
                Driver.Instance.FindElement(By.XPath(cvv)).SendKeys(input.cvv);


                Driver.Instance.FindElement(By.XPath(payNowElement)).Click();


            });

        }


        public static void DateHandler(DateTime date, string dateElement, string monthElement, string yearElement, string dayRowElement, string indivDayElement)
        {
            string Day = date.Day.ToString();
            string Month = date.ToString("MMM");
            string Year = date.Year.ToString();

            Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath(dateElement)));
            Driver.Instance.FindElement(By.XPath(dateElement)).Click();

            ReadOnlyCollection<IWebElement> months = Driver.Instance.FindElements(By.XPath(monthElement));
            months.FirstOrDefault(b => b.Text == Month).Click();

            ReadOnlyCollection<IWebElement> years = Driver.Instance.FindElements(By.XPath(yearElement));
            years.FirstOrDefault(c => c.Text == Year).Click();

            ReadOnlyCollection<IWebElement> dayRows = Driver.Instance.FindElements(By.XPath(dayRowElement));

            foreach (IWebElement dayRow in dayRows)
            {
                ReadOnlyCollection<IWebElement> days = dayRow.FindElements(By.XPath(indivDayElement));
                if (days.FirstOrDefault(i => i.Text == Day) != null)
                {
                    Thread.Sleep(700);
                    days.FirstOrDefault(i => i.Text == Day).Click();
                    break;
                }
            }
        }

        public static List<LandLordShieldInput> LoadJsonInput(string fileName)
        {
            string jsonConfigFolder = Path.GetFullPath(ConfigurationManager.AppSettings["testFolder"]);
            //string jsonConfigFolder = Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\");
            using (StreamReader r = new StreamReader(jsonConfigFolder + fileName + ".json"))
            {
                string json = r.ReadToEnd();
                List<LandLordShieldInput> foo = JsonConvert.DeserializeObject<List<LandLordShieldInput>>(json);
                return foo;
            }
        }


        static object[] NewData = AddLandLordData(1);

        public static object[] AddLandLordData(int start)
        {
            List<object> full = new List<object>();
            List<LandLordShieldInput> fromJson = LoadJsonInput("landLordTest"); // fromJson will contain all the information from config.json
            int listLen = fromJson.Count;
            for (int i = start - 1; i < listLen; i++)
            {
                List<object> singleSet = new List<object>();
                singleSet.Add(fromJson[i]);
                full.Add(singleSet.ToArray());
            }
            return full.ToArray();

        }


        [OneTimeSetUp()]
        public void SetupTest()
        {
            //Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\");
            //Driver.Initialize();
            //For header
            var csv = new StringBuilder();
            var newLine = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}", "Date/Time", "Test ID", "Scenario", "Category", "Description", "Status", "More Info");
            csv.AppendLine(newLine);
            File.AppendAllText(ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + ".csv", csv.ToString());
            //LoadToolTip();

            //var csv2 = new StringBuilder();

            //var newLine2 = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}|{17}|{18}|{19}|{20}|{21}{22}|{23}|{24}", "Id", "TripDuration", "TripType", "Countries", "RegionNo", "departDate", "returnDate", "coverType", "adultAge", "childAge", "promoCode", "Displayed Classic", "Expected Classic", "Classic Outcome", "Displayed Premier", "Expected Premier", "Premier Outcome", "Displayed Platinum", "Expected Platinum", "Platinum Outcome", "Overall Outcome", "=\"Pass Percentage: \"&AVERAGE(T:T)*100&\"%\"", "", "=\"# Passed: \"&SUM(U:U)", "=\"# Failed: \"&SUM(U:U)/AVERAGE(U:U)-SUM(U:U)");
            //csv2.AppendLine(newLine2);
            //File.AppendAllText(ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["checkRatesFileName"] + ".csv", csv2.ToString());
            ////LoadToolTip();
        }

        [OneTimeTearDown()]
        public void MyTestCleanup()
        {
            Driver.Close();
            //string oldfilePath = ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + ".csv";
            //string newfilePath = ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + "_" + DateTime.Now.ToString("ddMMMyyyy_hhmmss") + ".csv";
            System.IO.File.Move(ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + ".csv", ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + "_" + DateTime.Now.ToString("ddMMMyyyy_hhmmss") + ".csv");
            //System.IO.File.Move(ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["checkRatesFileName"] + ".csv", ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["checkRatesFileName"] + "_" + DateTime.Now.ToString("ddMMMyyyy_hhmmss") + ".csv");

        }


        [SetUp()]
        public void SetupTest2()
        {
            //Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\");
            Driver.Initialize(ConfigurationManager.AppSettings["browser"]);

        }
        [TearDown()]
        public void MyTestCleanup2()
        {
            Driver.Close();
            //string oldfilePath = ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + ".csv";
            //string newfilePath = ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + "_" + DateTime.Now.ToString("ddMMMyyyy_hhmmss") + ".csv";
            //System.IO.File.Move(ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + ".csv", ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + "_" + DateTime.Now.ToString("ddMMMyyyy_hhmmss") + ".csv");
            //System.IO.File.Move(ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["checkRatesFileName"] + ".csv", ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["checkRatesFileName"] + "_" + DateTime.Now.ToString("ddMMMyyyy_hhmmss") + ".csv");

        }
    }


    [TestFixture]
    public class CyberSmart : PageTestBase
    {
        [Test, TestCaseSource("NewData")]
        public void Test_CyberSmart(CyberSmartInput input)
        {
            FullElementSelector fullElementSelector = InsuranceTest.LoadElementSelectors();
            string TestName = input.testid + "-" + input.testName;
            UITest(TestName, () =>
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Starting test for test id: " + input.testid + " , test case: " + input.testName);
                Console.ForegroundColor = ConsoleColor.White;

                CyberSmartPlanPage.GotoPlanPage();
                CyberSmartPlanPage.SelectPlan(input.planNo, input.isFamilyPlan, fullElementSelector, input.testid, input.testName);
                EditTravellerDetailsPage.FillSection(input.promoCode).FillSection(input.applicantDetail).Proceed(fullElementSelector, input.testid, input.testName);

                if (input.isMonthlyPayment)
                {
                    Thread.Sleep(3000);
                    string paymentSelection = "/html/body/app-root/apply/div[2]/div/div/div[2]/div/payment-details/payment-type/mat-card/mat-radio-group/div/div[2]/mat-radio-button/label/div[2]";
                    Driver.GetWait().Until(ExpectedConditions.ElementExists(By.XPath(paymentSelection)));
                    Driver.GetWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath(paymentSelection)));
                    Driver.Instance.FindElement(By.XPath(paymentSelection)).Click();

                }

                CreditCardDetailsPage.FillSection(input.creditCardInfo).Proceed(fullElementSelector, input.testid, input.testName);
                //VerifyDetailsPage.ProceedWithPayment(fullElementSelector, input.testid, input.testName);

                //EditTravellerDetailsPage.TravelDetailPageFunctionalityTest(fullElementSelector);

                //if (input.runPlanPageFunctionality)
                //    PlanPage.PlanPageFunctionalityTest(fullElementSelector);

                //EditTravellerDetailsPage.TravelDetailPageFunctionalityTest(fullElementSelector);

                //string[] planAmount = SelectPlan(input.planNo, fullElementSelector, input.testid, input.testName);
                ////ReadOnlyCollection<IWebElement> travelDetailsBox = EditTravellerDetailsPage.RetrieveTravelDetails();
                ////CheckTravelDetails(travelDetailsBox, input, Convert.ToDouble(planAmount[1]));
                ////CheckTravelDetails(travelDetailsBox, input);


                //FillTravelDetails(input.applicantDetail, input.travellerDetails, input.testid, input.testName);
                //FillPaymentDetails(ConfigurationManager.AppSettings["paymentMode"], input.creditCardInfo, fullElementSelector, input.testid, input.testName);

                //VerifyDetailsPage.ProceedWithPayment(input.testid, input.testName);

                //PaymentSuccessPage.ViewPDF(input.testid, input.testName);

            });
        }

        static object[] NewData = AddCyberSmartData(1);

        public static List<CyberSmartInput> LoadJsonInput(string fileName)
        {
            string jsonConfigFolder = Path.GetFullPath(ConfigurationManager.AppSettings["testFolder"]);
            //string jsonConfigFolder = Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\");
            using (StreamReader r = new StreamReader(jsonConfigFolder + fileName + ".json"))
            {
                string json = r.ReadToEnd();
                List<CyberSmartInput> foo = JsonConvert.DeserializeObject<List<CyberSmartInput>>(json);
                return foo;
            }
        }

        public static object[] AddCyberSmartData(int start)
        {
            List<object> full = new List<object>();
            List<CyberSmartInput> fromJson = LoadJsonInput("cyberSmartInput"); // fromJson will contain all the information from config.json
            int listLen = fromJson.Count;
            for (int i = start - 1; i < listLen; i++)
            {
                List<object> singleSet = new List<object>();
                singleSet.Add(fromJson[i]);
                full.Add(singleSet.ToArray());
            }
            return full.ToArray();

        }

        [OneTimeSetUp()]
        public void SetupTest()
        {
            //Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\");
            //Driver.Initialize();
            //For header
            var csv = new StringBuilder();
            var newLine = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}", "Date/Time", "Test ID", "Scenario", "Category", "Description", "Status", "More Info");
            csv.AppendLine(newLine);
            File.AppendAllText(ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + ".csv", csv.ToString());
            //LoadToolTip();

            //var csv2 = new StringBuilder();

            //var newLine2 = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}|{17}|{18}|{19}|{20}|{21}{22}|{23}|{24}", "Id", "TripDuration", "TripType", "Countries", "RegionNo", "departDate", "returnDate", "coverType", "adultAge", "childAge", "promoCode", "Displayed Classic", "Expected Classic", "Classic Outcome", "Displayed Premier", "Expected Premier", "Premier Outcome", "Displayed Platinum", "Expected Platinum", "Platinum Outcome", "Overall Outcome", "=\"Pass Percentage: \"&AVERAGE(T:T)*100&\"%\"", "", "=\"# Passed: \"&SUM(U:U)", "=\"# Failed: \"&SUM(U:U)/AVERAGE(U:U)-SUM(U:U)");
            //csv2.AppendLine(newLine2);
            //File.AppendAllText(ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["checkRatesFileName"] + ".csv", csv2.ToString());
            //LoadToolTip();
        }

        [OneTimeTearDown()]
        public void MyTestCleanup()
        {
            Driver.Close();
            //string oldfilePath = ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + ".csv";
            //string newfilePath = ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + "_" + DateTime.Now.ToString("ddMMMyyyy_hhmmss") + ".csv";
            System.IO.File.Move(ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + ".csv", ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + "_" + DateTime.Now.ToString("ddMMMyyyy_hhmmss") + ".csv");
            //System.IO.File.Move(ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["checkRatesFileName"] + ".csv", ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["checkRatesFileName"] + "_" + DateTime.Now.ToString("ddMMMyyyy_hhmmss") + ".csv");

        }


        [SetUp()]
        public void SetupTest2()
        {
            //Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\");
            Driver.Initialize(ConfigurationManager.AppSettings["browser"]);

        }
        [TearDown()]
        public void MyTestCleanup2()
        {
            Driver.Close();
            //string oldfilePath = ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + ".csv";
            //string newfilePath = ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + "_" + DateTime.Now.ToString("ddMMMyyyy_hhmmss") + ".csv";
            //System.IO.File.Move(ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + ".csv", ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["UnitTestLogFileName"] + "_" + DateTime.Now.ToString("ddMMMyyyy_hhmmss") + ".csv");
            //System.IO.File.Move(ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["checkRatesFileName"] + ".csv", ConfigurationManager.AppSettings["testFolder"] + ConfigurationManager.AppSettings["checkRatesFileName"] + "_" + DateTime.Now.ToString("ddMMMyyyy_hhmmss") + ".csv");

        }
    }

}
