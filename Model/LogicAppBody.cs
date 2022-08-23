namespace Lighthouse.AF_PayPalIPN.Model
{
        public class LogicAppBody
        {
            public decimal Fee { get; set; }
            public string PaymentDate { get; set; }
            public string BuyerEmail { get; set; }
        }
}