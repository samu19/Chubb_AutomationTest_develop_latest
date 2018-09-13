using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceTests
{
    public class LandLordShieldInput
    {
        public string testId { get; set; }
        public string testName { get; set; }
        public string grossFloorArea { get; set; }
        public string sumInsured { get; set; }
        public DateTime effectiveStartDate { get; set; }
        public string promoCode { get; set; }
        public int plan { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string gender { get; set; }
        public DateTime DOB { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string HKID { get; set; }
        public string corrAddress1 { get; set; }
        public string corrAddress2 { get; set; }
        public string corrAddress3 { get; set; }
        public bool sameAddress { get; set; }
        public string insAddress1 { get; set; }
        public string insAddress2 { get; set; }
        public string insAddress3 { get; set; }

        public string cardNo { get; set; }
        public string cardHolderName { get; set; }
        public string expiryDate { get; set; }
        public string cvv { get; set; }

    }
}
