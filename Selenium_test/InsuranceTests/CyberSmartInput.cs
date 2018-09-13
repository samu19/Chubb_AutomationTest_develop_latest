using PaymentPageAutomation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravellerDetailsPageAutomation;

namespace InsuranceTests
{
    public class CyberSmartInput
    {
        public string testid { get; set; }
        public string testName { get; set; }
        public int planNo { get; set; }
        public bool isFamilyPlan { get; set; }
        public ApplicantDetail applicantDetail { get; set; }
        public PromoCode promoCode { get; set; }
        public bool isMonthlyPayment { get; set; }
        public CreditCardInfo creditCardInfo { get; set; }

    }
}
