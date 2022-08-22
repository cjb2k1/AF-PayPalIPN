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

        private static string logicAppUri = "https://prod-59.northeurope.logic.azure.com%2Fworkflows%2Fd9697466474141b3ba0018f78b5c75c9%2Ftriggers%2Fmanual%2Fpaths%2Finvoke%3Fapi-version%3D2016-10-01%26sp%3D%252Ftriggers%252Fmanual%252Frun%26sv%3D1.0%26sig%3Dh6NznOI6qVjcCG1WZ9OhEzrP0ycJQ1rr815S4DlJE-4&data=05%7C01%7Cchris.burton%40thelighthouse.org.uk%7C57f42c086b6349059e8b08da843915e1%7C43ae72d10a7c40c28788e02e0d9b3b3d%7C0%7C0%7C637967677953768409%7CUnknown%7CTWFpbGZsb3d8eyJWIjoiMC4wLjAwMDAiLCJQIjoiV2luMzIiLCJBTiI6Ik1haWwiLCJXVCI6Mn0%3D%7C3000%7C%7C%7C&sdata=qvp0%2F3lPPDO7HITxTHWBUZpCwiTP1qKh2sUgpswqYoM%3D&reserved=0";
        
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
