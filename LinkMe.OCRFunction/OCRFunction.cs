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

        
        HttpResponseMessage ocrresponse = await OCRCallout.SendAsync(imageString);
    
        FunctionResponse response = new FunctionResponse();

        if (ocrresponse.IsSuccessStatusCode == false)
        {
            //return an error.
            response.error = "the OCR API was unsuccessful.";
            return new BadRequestObjectResult(response);
        }
        else
        {
            //process the successful response.

            OCRResponse convertedImage = FormatConverter.ConvertOCRReponseToObject(ocrresponse);

            var exitCode = convertedImage.ParsedResults[0].FileParseExitCode;

            if (exitCode != "1")
            {
                //handle the problem.
                response.error = "the image was not processed successfully. Please take a new photo and try again.";
                return (ActionResult) new BadRequestObjectResult(response);
            }
            else
            {
                //we have a successful parse
                //strip any weird returns from the text.
                string url = StringValidation.RemoveEscapes(convertedImage.ParsedResults[0].ParsedText);

                //check if the url is valid
                if (StringValidation.ValidateUrl(url) == true)
                {
                    response.url = url;
                    return (ActionResult) new OkObjectResult(response);
                }
                else
                {
                    response.error = "the image does not contain a valid URL.";
                    return (ActionResult) new BadRequestObjectResult(response);
                }
            }
        }
    }
}