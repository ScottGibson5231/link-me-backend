using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LinkMe.OCRFunction;

public class FormatConverter
{


    public static async Task<OCRResponse> ConvertOCRReponseToObject(HttpResponseMessage response)
    {
        string strContent = await response.Content.ReadAsStringAsync();
        
        Console.WriteLine(strContent);

        OCRResponse respObj = JsonConvert.DeserializeObject<OCRResponse>(strContent);
        
        
        return respObj;
    }
    
}