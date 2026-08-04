using Prodemos.Domain;

namespace Prodemos.Application.Dtos.UserPlays;
public class UserGuestUserPlayDto
{
    public Guid Id { get; set; }
    public string TeamAName { get; set; } = string.Empty;
    public string TeamBName { get; set; } = string.Empty;
    public int ScoreTeamAGuessed { get; set; }
    public int ScoreTeamBGuessed { get; set; }
    public GuessStatus GuessStatus { get; set; }
}
