namespace Prodemos.Application.Dtos.Team;
public class TeamResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string FlagUrl { get; set; } = string.Empty;
}
