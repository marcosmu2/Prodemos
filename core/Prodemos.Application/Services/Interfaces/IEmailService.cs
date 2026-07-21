using Prodemos.Application.Models.Email;

namespace Prodemos.Application.Services.Interfaces;
public interface IEmailService
{
    Task<bool> SendEmailAsync(EmailMessage emailMessage, string token);
}
