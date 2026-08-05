using MediatR;
using Microsoft.AspNetCore.Mvc;
using Prodemos.Api.Attribute;
using Prodemos.Application.Dtos.UserGuests;
using Prodemos.Application.Services.UserGuests.Commands;
using Prodemos.Application.Services.UserGuests.Queries;

namespace Prodemos.Api.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class UserGuestController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserGuestController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<ActionResult<UserGuestResponseDto>> CreateUserGuest(CreateUserGuestCommand createUserGuestCommand)
    {
        var response = await _mediator.Send(createUserGuestCommand);
        return Ok(response);
    }

    [HttpPut("update")]
    public async Task<ActionResult<UserGuestResponseDto>> UpdateUserGuest(UpdateUserGuestCommand updateUserGuestCommand)
    {
        var response = await _mediator.Send(updateUserGuestCommand);
        return Ok(response);
    }

    [HttpDelete("delete/{id}")]
    [RequireAdmin]
    public async Task<ActionResult<string>> DeleteUserGuest(Guid id)
    {
        DeleteUserGuestCommand deleteUserGuestCommand = new()
        {
            Id = id,
        };

        return await _mediator.Send(deleteUserGuestCommand) ? Ok("User Guest deleted successfully") : BadRequest();
    }

    [HttpGet("getByUserAndChampionshipId/{championshipId}")]
    public async Task<ActionResult<ICollection<UserGuestResponseDto>>> GetByUserAndChampionshipId(Guid championshipId)
    {
        GetUserGuestsByUserAndChampionshipIdCommand query = new()
        {
            ChampionshipId = championshipId
        };

        return Ok(await _mediator.Send(query));
    }
}
