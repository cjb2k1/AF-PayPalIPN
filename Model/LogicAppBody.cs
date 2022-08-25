using System;

namespace Lighthouse.AF_PayPalIPN.Model
{
        public class LogicAppBody
        {
            public decimal Fee { get; set; }
            public string AuthID { get; set; }
            public string BuyerEmail { get; set; }
            public string TxnType { get; set; }
            public string TxnID { get; set; }
            public DateTime TransactionDate { get; set; }
        }
}