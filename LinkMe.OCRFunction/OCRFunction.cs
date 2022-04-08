using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LinkMe.OCRFunction;

public static class OCRFunction
{
    [FunctionName("OCRFunction")]
    public static async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
    {
        try
        {
            var formdata = await req.ReadFormAsync();
            var file = req.Form.Files["file"];
            return new OkObjectResult(file.FileName + " - " + file.Length.ToString());
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex);
        }
    }
}