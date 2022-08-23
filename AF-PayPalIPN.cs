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
using System.Net.Http;
using System.Text;


namespace Lighthouse.AF_PayPalIPN
{
    public static class AF_PayPalIPN
    {
        private static HttpClient httpClient = new HttpClient();

        private static string logicAppUri = "https://shorturl.at/kQUXZ";

        [FunctionName("AF_PayPalIPN")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log)
        {   
            var result = await req.VerifyPayPalTransactionAsync(PayPalEnvironment.Sandbox,log);
            
            if(result.IsVerified)
            {
                log.LogInformation($"Buyer {result.Transaction.PayerEmail} paid {result.Transaction.Gross} with fee {result.Transaction.Fee}");
                var fee = result.Transaction.Fee;

                await Main(fee, log);
            }

            return new OkResult();
        }

        private static async Task Main(decimal fee, ILogger log)
        {
            var jsonfee = JsonConvert.SerializeObject(fee);
            var data = new StringContent(jsonfee, Encoding.UTF8, "application/json");
            
            var response = await httpClient.PostAsync(logicAppUri,data);

            string result = response.Content.ReadAsStringAsync().Result;

            log.LogInformation(result);

            return;
        }
    }
}
