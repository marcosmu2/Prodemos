namespace Prodemos.Domain;
public class UserGuest
{
    public Guid UserPlayId { get; set; }
    public Guid MatchId { get; set; }
    public int ScoreTeamAGuessed { get; set; }
    public int ScoreTeamBGuessed { get; set; }
    public GuessStatus GuessStatus { get; set; }
    public virtual Match? Match { get; set; }
    public virtual UserPlay? UserPlay { get; set; }
}
