using MediatR;
using Microsoft.AspNetCore.Authorization;
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

    [AllowAnonymous]
    [HttpPost("register")]
    //[ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<string>> Register(RegisterCommand registerCommand)
    {
        var response = await _mediator.Send(registerCommand);
        return response ? Ok("User created succesfully") : BadRequest("User cannot be created");
    }

    [HttpPost("updateUser")]
    public async Task<ActionResult<string>> UpdateUser(UpdateUserCommand updateUserCommand)
    {
        var response = await _mediator.Send(updateUserCommand);
        return response ? Ok("User updated succesfully") : BadRequest("User cannot be updated");
    }
}
