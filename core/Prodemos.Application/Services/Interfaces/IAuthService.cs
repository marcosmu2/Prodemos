using Prodemos.Domain;

namespace Prodemos.Application.Services.Interfaces;
public interface IAuthService
{
    string GetSessionUserEmail();
    bool IsAdmin();
    string CreateToken(User user, IList<string>? roles);
}
