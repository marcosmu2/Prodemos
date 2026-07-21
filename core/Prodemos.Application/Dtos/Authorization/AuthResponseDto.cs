namespace Prodemos.Application.Dtos.Authorization;
public class AuthResponseDto
{
    public string? Id { get; set; }
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Token { get; set; }
    public ICollection<string>? Roles { get; set; }
}
