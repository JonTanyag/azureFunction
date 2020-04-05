using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace TimeTriggeredFunction
{
    public static class GetCovidResults
    {
        [FunctionName("GetCovidResults")]
        public static async void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var result = await CovidResultGetter.GetResults();

            log.LogInformation($"Displaying results: " + result);
        }
    }
}
