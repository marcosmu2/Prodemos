using Prodemos.Domain;

namespace Prodemos.Application.Dtos.Matches;
public class MatchResponseDto
{
    public Guid TeamAId { get; set; }
    public Guid TeamBId { get; set; }
    public int ScoreTeamA { get; set; }
    public int ScoreTeamB { get; set; }
    public MatchStatus MatchStatus { get; set; }
    public string TeamAName { get; set; } = string.Empty;
    public string TeamBName { get; set; } = string.Empty;
}
