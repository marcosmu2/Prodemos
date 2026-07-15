using System.ComponentModel.DataAnnotations.Schema;

namespace Prodemos.Domain;
public class Championship : BaseDomainModel
{
    [Column(TypeName = "NVARCHAR(200)")]
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<Match>? Matches { get; set; }
    public virtual ICollection<UserPlay>? UserPlays { get; set; }
}