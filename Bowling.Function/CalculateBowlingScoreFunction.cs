using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Bowling.Function.Logic;

namespace Bowling.Function
{
    public static class CalculateBowlingScoreFunction
    {
        private static BowlingScoreCalculator bowlingScoreCalculator = new BowlingScoreCalculator();
        [FunctionName("CalculateBowlingScore")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "CalculateBowlingScore/{rollScores}")] HttpRequest req, string rollScores, ILogger log)
        {
            try
            {
                var result = bowlingScoreCalculator.CalculateScore(rollScores);
                if (result.Error == null)
                {
                    return new OkObjectResult(result);
                }
                else
                {
                    return new BadRequestObjectResult(result);
                }
            }
            catch (Exception ex)
            {
                log.LogInformation(ex.ToString());
                return new StatusCodeResult(500);
            }
        }
    }
}
