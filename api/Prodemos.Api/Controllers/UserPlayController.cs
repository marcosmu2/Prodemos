using MediatR;
using Microsoft.AspNetCore.Mvc;
using Prodemos.Api.Attribute;
using Prodemos.Application.Dtos.UserPlays;
using Prodemos.Application.Services.UserPlays.Commands;

namespace Prodemos.Api.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class UserPlayController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserPlayController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    [RequireAdmin]
    public async Task<ActionResult<UserPlayResponseDto>> CreateUserPlay(CreateUserPlayCommand createUserPlayCommand)
    {
        return Ok(await _mediator.Send(createUserPlayCommand));
    }
}
