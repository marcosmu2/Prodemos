using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prodemos.Application.Models.Email;
using Prodemos.Application.Services.Interfaces;

namespace Prodemos.Api.Controllers;
[ApiController]
[Route("api/v1/[controller]")]
public class TestController : ControllerBase
{
    private readonly IEmailService _emailService;

    public TestController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> SendEmail()
    {
        var emailMessage = new EmailMessage
        {
            Body = "Esta es una prueba del envio de Email",
            Subject = "Test",
            To = "marcosm3.14@gmail.com"
        };

        var result = await _emailService.SendEmailAsync(emailMessage, "Este_Es_Mi_Token");
        return result ? Ok() : BadRequest();  
    }
}
