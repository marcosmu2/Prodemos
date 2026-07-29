namespace Prodemos.Application.Dtos.Championship;
public class ChampionshipResponseDto
{
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<ChampionshipMatchDto>? Matches { get; set; }
}
