using System.ComponentModel.DataAnnotations.Schema;

namespace Prodemos.Domain;
public class Team : BaseDomainModel
{
    [Column(TypeName = "NVARCHAR(100)")]
    public string Name { get; set; } = string.Empty;
    [Column(TypeName = "NVARCHAR(400)")]
    public string FlagUrl { get; set; } = string.Empty;

    public virtual ICollection<Match>? MatchesAsTeamA { get; set; }
    public virtual ICollection<Match>? MatchesAsTeamB { get; set; }

}
