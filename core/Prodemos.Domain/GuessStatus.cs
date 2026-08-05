using System.Text.Json.Serialization;

namespace Prodemos.Domain;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GuessStatus
{
    Pending,
    Sealed,
    Finished
}
