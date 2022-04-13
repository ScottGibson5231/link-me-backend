using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinkMe.OCRFunction;

public class OCRCallout
{

    public static async Task<HttpResponseMessage> SendAsync(string base64Image)
    {
        HttpClient httpClient = new HttpClient();
            httpClient.Timeout = new TimeSpan(0, 2, 0);


        MultipartFormDataContent form = new MultipartFormDataContent();
        form.Add(new StringContent(Environment.GetEnvironmentVariable("OCR_API_KEY")), "apikey");
        form.Add(new StringContent(base64Image), "base64Image");
        form.Add(new StringContent("eng"), "language");
        form.Add(new StringContent("1"), "ocrengine");
        form.Add(new StringContent("true"), "scale");
        
        HttpResponseMessage response = await httpClient.PostAsync("https://api.ocr.space/Parse/Image", form);

        return response;
        
    }
}