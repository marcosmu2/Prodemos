using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prodemos.Application.Dtos.Authorization;
using Prodemos.Application.Services.Authorization.Command;
using System.Net;

namespace Prodemos.Api.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class AuthorizationController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthorizationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpGet("login", Name = "Login")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginUserCommand loginUserCommand)
    {
        var response = await _mediator.Send(loginUserCommand);
        return Ok(response);
    }
}
