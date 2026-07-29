using Prodemos.Domain;

namespace Prodemos.Application.Dtos.Championship;
public class ChampionshipMatchDto
{
    public string TeamAName { get; set; } = string.Empty;
    public string TeamBName { get; set; } = string.Empty;
    public MatchStatus MatchStatus { get; set; }
}
