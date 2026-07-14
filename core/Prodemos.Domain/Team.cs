namespace Prodemos.Domain;
public class Team : BaseDomainModel
{
    public string Name { get; set; } = string.Empty;

    public string FlagUrl { get; set; } = string.Empty;

    public virtual ICollection<Match>? Matches { get; set; }

}
