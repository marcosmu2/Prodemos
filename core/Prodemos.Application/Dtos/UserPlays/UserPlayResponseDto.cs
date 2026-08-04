using Prodemos.Domain;

namespace Prodemos.Application.Dtos.UserPlays;
public class UserPlayResponseDto
{
    public Guid Id { get; set; }
    public string ChampionshipName { get; set; } = string.Empty;
    public int Points { get; set; }
    public ICollection<UserGuestUserPlayDto> UserGuests { get; set; } = new List<UserGuestUserPlayDto>();
}
