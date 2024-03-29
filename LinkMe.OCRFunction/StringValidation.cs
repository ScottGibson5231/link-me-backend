using System;

namespace LinkMe.OCRFunction;

public class StringValidation
{
    //This method is used to check if the passed in string is a valid URI. We need this to ensure that
    //the response from the OCR API actually is a usable url. 
    public static bool ValidateUrl(string url)
    {
        Uri uriResult;
        bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult) 
                      && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

        return result;
    }

    public static string RemoveEscapes(string s)
    {
        s = s.Replace("\n", "");
        s = s.Replace("\r", "");
        s = s.Replace("\t", "");
        s = s.Replace("\\", "");
        s = s.Replace("\'", "");
        s = s.Replace("\"", "");

        return s;
    }
}