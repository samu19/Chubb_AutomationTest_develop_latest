using PaymentPageAutomation;
using QuotePageAutomation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravellerDetailsPageAutomation;

namespace InsuranceTests
{
    public class InputData
    {
        public QuoteData quoteData { get; set; }
        public List<double> expectedRates { get; set; }
        public int planNo { get; set; }
        public ApplicantDetail applicantDetail { get; set; }
        public TravellerDetails travellerDetails { get; set; }
        public CreditCardInfo creditCardInfo { get; set; }
        public string testid { get; set; }
        public string testName { get; set; }
        public bool runQuotePageFunctionality { get; set; }
        public bool runPlanPageFunctionality { get; set; }

    }
}
