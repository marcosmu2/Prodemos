using System.Runtime.Serialization;

namespace Prodemos.Domain;
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
