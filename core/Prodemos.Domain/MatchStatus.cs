using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Prodemos.Domain;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MatchStatus
{
    [EnumMember(Value = "Upcoming")]
    Upcoming,
    [EnumMember(Value = "First Time")]
    FirstTime,
    [EnumMember(Value = "Half Time")]
    HalfTime,
    [EnumMember(Value = "Second Time")]
    SecondTime,
    [EnumMember(Value = "Extra Time")]
    ExtraTime,
    [EnumMember(Value = "Finished")]
    Finished
}
