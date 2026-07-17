using FluentEmail.Core;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Prodemos.Application.Models.Email;
using Prodemos.Application.Services.Interfaces;

namespace Prodemos.Infrastructure.Services;
public class EmailService : IEmailService
{
    private readonly IFluentEmail _fluentEmail;
    private readonly EmailFluentSettings _settings;

    public EmailService(IFluentEmail fluentEmail, IOptions<EmailFluentSettings> settings)
    {
        _fluentEmail = fluentEmail;
        _settings = settings.Value;
    }

    public async Task<bool> SendEmailAsync(EmailMessage emailMessage, string token)
    {
        var htmlBody = $"{emailMessage.Body} {_settings.BaseUrlClient}/password/reset/{token}";
        var emailReponse = await _fluentEmail.To(emailMessage.To)
                    .Subject(emailMessage.Subject)
                    .Body(htmlBody)
                    .SendAsync();
        return emailReponse.Successful;
    }
}
