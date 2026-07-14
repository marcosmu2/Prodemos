namespace Prodemos.Domain;
public class Match : BaseDomainModel
{
    public Guid TeamAId { get; set; }
    public Guid TeamBId { get; set; }
    public int ScoreTeamA { get; set; }
    public int ScoreTeamB { get; set; }
    public MatchStatus MatchStatus { get; set; }
    public Guid ChampionshipId { get; set; }
    public virtual Team? TeamA { get; set; }
    public virtual Team? TeamB { get; set; }
    public virtual Championship? Championship { get; set;}
    public virtual ICollection<UserGuest>? UserGuests { get; set; }
}
