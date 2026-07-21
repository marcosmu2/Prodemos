using Newtonsoft.Json;

namespace Prodemos.Api.Errors;

public class CodeErrorException : CodeErrorResponse
{
    [JsonProperty(PropertyName = "details")]
    public string Details { get; set; } = string.Empty;

    public CodeErrorException(int statusCode, string[]? message = null, string details = "") : base(statusCode, message)
    {
        Details = details;
    }
}
