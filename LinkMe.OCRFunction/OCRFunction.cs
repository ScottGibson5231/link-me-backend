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
        
        
        log.LogInformation("C# HTTP trigger function processed a request.");

        string imageString = req.Query["imageString"];

        string requestBody = String.Empty;
        using (StreamReader streamReader =  new  StreamReader(req.Body))
        {
            requestBody = await streamReader.ReadToEndAsync();
        }
        dynamic data = JsonConvert.DeserializeObject(requestBody);
        imageString = imageString ?? data?.imageString;

        
        HttpResponseMessage OCRresponse = await OCRCallout.Send(imageString);
        
        OCRResponse resp = await FormatConverter.ConvertOCRReponseToObject(OCRresponse);
        

        if (resp.ParsedResults[0].FileParseExitCode == "1")
        {
            //we have a successful parse
            //strip any weird returns from the text.
            string url = StringValidation.RemoveEscapes(resp.ParsedResults[0].ParsedText);

            //check if the url is valid
            if (StringValidation.ValidateUrl(url))
            {
                return (ActionResult)new OkObjectResult($"{url}");
            }
            
            new BadRequestObjectResult("Soemthing went wrooooong");
            
        }

        return new BadRequestObjectResult("something went wrong bad");

    }
}