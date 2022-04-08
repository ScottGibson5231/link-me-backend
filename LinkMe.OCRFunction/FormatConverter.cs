using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace LinkMe.OCRFunction;

public class FormatConverter
{


    public async static Task<string> ConvertOCRReponseToString(HttpResponseMessage response)
    {
        string strContent = await response.Content.ReadAsStringAsync();

        string result = "";

        Rootobject ocrResult = JsonConvert.DeserializeObject<Rootobject>(strContent);
        
        if (ocrResult.OCRExitCode == 1)
        {
            for (int i = 0; i < ocrResult.ParsedResults.Count() ; i++)
            {
                result = result + ocrResult.ParsedResults[i].ParsedText ;
            }
        }
        else
        {
            Console.WriteLine("busted in the convert OCR response");
        }

        return result;
    }

    public static string ConvertIFormToBase64(IFormFile file)
    {
        if (file.Length > 0)
        {
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                string s = Convert.ToBase64String(fileBytes);
                return s;
            }
        }

        return null;
    }

}