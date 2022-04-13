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


    public static OCRResponse ConvertOCRReponseToObject(HttpResponseMessage response)
    {
        string strContent = response.Content.ToString();
        
        Console.WriteLine(strContent);

        OCRResponse respObj = JsonConvert.DeserializeObject<OCRResponse>(strContent);
        
        
        return respObj;
    }
    
}