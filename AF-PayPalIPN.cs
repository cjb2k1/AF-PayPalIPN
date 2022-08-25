using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Lighthouse.AF_PayPalIPN.Extensions;
using Lighthouse.AF_PayPalIPN.Model;

namespace Lighthouse.AF_PayPalIPN
{
    public static class AF_PayPalIPN
    {
        private static HttpClient httpClient = new HttpClient();

        private static Uri logicAppUri = new Uri("https://prod-59.northeurope.logic.azure.com:443/workflows/d9697466474141b3ba0018f78b5c75c9/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=h6NznOI6qVjcCG1WZ9OhEzrP0ycJQ1rr815S4DlJE-4");
        
        [FunctionName("AF_PayPalIPN")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log)
        { 
            var result = await req.VerifyPayPalTransactionAsync(PayPalEnvironment.Live,log);
            
            if(result.IsVerified)
            {
                log.LogInformation($"Buyer {result.Transaction.PayerEmail} paid {result.Transaction.Gross} with fee {result.Transaction.Fee}");

                var ipnDateParser = new IPNDateParser();

                var labody = new LogicAppBody
                {
                    Fee = result.Transaction.Fee,
                    AuthID = result.Transaction.AuthID,
                    BuyerEmail = result.Transaction.PayerEmail,
                    TxnType = result.Transaction.TransactionType,
                    TxnID = result.Transaction.TransactionId,
                    TransactionDate = ipnDateParser.parseDateTime(result.Transaction.PaymentDate)
                };

                var jsonString = JsonConvert.SerializeObject(labody);

                var data = new StringContent(jsonString, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(logicAppUri,data);

                string laresult = response.StatusCode.ToString();

                log.LogInformation(laresult);
            }

            return new OkResult();
        }
    }
}
