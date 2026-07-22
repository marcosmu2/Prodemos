using Prodemos.Domain;

namespace Prodemos.Application.Services.Interfaces;
public interface IAuthService
{
    string GetSessionUserEmail();
    string CreateToken(User user, IList<string>? roles); 
}
