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

            HttpResponseMessage resp = await OCRCallout.Send(file);

            string text = await FormatConverter.ConvertOCRReponseToString(resp);

            if (StringValidation.ValidateUrl(text))
            {
                return new OkObjectResult(text);
            }
            else
            {
                throw new Exception("bad shit");
            }
            
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex);
        }
    }
}