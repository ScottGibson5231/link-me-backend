namespace LinkMe.OCRFunction;

public class OCRResponse
{
    public ParsedResults[] ParsedResults { get; set; }
    public int OCRExitCode { get; set; }
    public bool IsErroredOnProcessing { get; set; }
    public string ErrorMessage { get; set; }
    public string ErrorDetails { get; set; }
}