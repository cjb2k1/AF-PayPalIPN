using System;

namespace Lighthouse.AF_PayPalIPN.Model
{
    public class VerificationResult
    {
        public bool IsVerified { get; set; }
        public PayPalTransaction Transaction { get; set; }
        public Exception Exception { get; set; }
    }
}