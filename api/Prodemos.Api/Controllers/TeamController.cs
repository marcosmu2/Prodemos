using MediatR;
using Microsoft.AspNetCore.Mvc;
using Prodemos.Api.Attribute;
using Prodemos.Application.Dtos.Team;
using Prodemos.Application.Services.Teams.Command;
using Prodemos.Application.Services.Teams.Queries;

namespace Prodemos.Api.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class TeamController : ControllerBase
{
    private readonly IMediator _mediator;

    public TeamController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    [RequireAdmin]
    public async Task<ActionResult<TeamResponseDto>> CreateTeam(CreateTeamCommand createTeamCommand)
    {
        var response = await _mediator.Send(createTeamCommand);
        return Ok(response);
    }

    [HttpPut("update")]
    [RequireAdmin]
    public async Task<ActionResult<TeamResponseDto>> UpdateTeam(UpdateTeamCommand updateTeamCommand)
    {
        var response = await _mediator.Send(updateTeamCommand);
        return Ok(response);
    }

    [HttpDelete("delete/{id}")]
    [RequireAdmin]
    public async Task<ActionResult> DeleteTeam(Guid id)
    {
        var deleteTeamCommand = new DeleteTeamCommand() { Id = id };
        var response = await _mediator.Send(deleteTeamCommand);
        return response ? Ok() : BadRequest();
    }

    [HttpGet("getAll")]
    [RequireAdmin]
    public async Task<ActionResult<ICollection<TeamResponseDto>>> GetAllTeams()
    {
        return Ok(await _mediator.Send(new GetAllTeamsQuery()));
    }

    [HttpGet("getById/{id}")]
    public async Task<ActionResult<TeamResponseDto>> GetById(Guid id)
    {
        var query = new GetTeamByIdQuery()
        {
            Id = id
        };
        return Ok(await _mediator.Send(query));
    }
}
