using Newtonsoft.Json;

namespace Prodemos.Api.Errors;

public class CodeErrorResponse
{
    [JsonProperty(PropertyName = "statusCode")]
    public int StatusCode { get; set; }

    [JsonProperty(PropertyName = "message")]
    public string[]? Message { get; set; }

    public CodeErrorResponse(int statusCode, string[]? message = null)
    {
        StatusCode = statusCode;
        if (message is null)
        {
            Message = new string[0];
            var defaultMessage = GetDefaultMessageStatusCode(statusCode);
            Message[0] = defaultMessage;
        }
        else { 
            Message = message;
        }

    }

    private string GetDefaultMessageStatusCode(int statusCode)
    {
        return statusCode switch
        {
            400 => "The web server could not understand or process the request sent",
            401 => "Unaunthorizated",
            404 => "Not Found",
            500 => "Errors ocurred on the server",
            _ => string.Empty,
        };
    }
}
