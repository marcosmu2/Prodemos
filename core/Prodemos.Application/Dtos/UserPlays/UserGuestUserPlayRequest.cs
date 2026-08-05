namespace Prodemos.Application.Dtos.UserPlays;
public class UserGuestUserPlayRequest
{
    public Guid? MatchId { get; set; }
    public int? ScoreTeamAGuessed { get; set; }
    public int? ScoreTeamBGuessed { get; set; }
}
