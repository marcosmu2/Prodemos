using Prodemos.Domain;

namespace Prodemos.Application.Services.Interfaces;
public interface IAuthService
{
    string GetSessionUser();
    string CreateToken(User user, IList<string>? roles); 
}
