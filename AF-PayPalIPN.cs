using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PayPalHelper.Core.Extensions;


namespace Lighthouse.AF_PayPalIPN
{
    public static class AF_PayPalIPN
    {
        [FunctionName("AF_PayPalIPN")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log)
        { 
            var result = await req.VerifyPayPalTransactionAsync(PayPalEnvironment.Sandbox,log);
            
            if(result.IsVerified)
            {
                log.LogInformation($"Buyer {result.Transaction.PayerEmail} paid {result.Transaction.Gross} with fee {result.Transaction.Fee}");
            }

            return new OkResult();
        }
    }
}
