using System;

namespace Lighthouse.AF_PayPalIPN.Model
{
        public class LogicAppBody
        {
            public decimal Fee { get; set; }
            public DateTime PaymentDate { get; set; }
            public string BuyerEmail { get; set; }
        }
}