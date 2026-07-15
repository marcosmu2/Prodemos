namespace Prodemos.Domain;
public class UserPlay : BaseDomainModel
{
    public Guid UserId { get; set; }
    public Guid ChampionshipId { get; set; }
    public int Points { get; set; }
    public virtual Championship? Championship { get; set; }
    public virtual ICollection<UserGuest>? UserGuests { get; set; }
}
