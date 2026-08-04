using MediatR;
using Microsoft.AspNetCore.Mvc;
using Prodemos.Api.Attribute;
using Prodemos.Application.Dtos.Matches;
using Prodemos.Application.Services.Matches.Command;
using Prodemos.Application.Services.Matches.Query;

namespace Prodemos.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class MatchController : ControllerBase
{
    private readonly IMediator _mediator;

    public MatchController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("getByChampionshipId/{id}")]
    public async Task<ActionResult<ICollection<MatchResponseDto>>> GetByCHampionshipId(Guid id)
    {
        var query = new GetMatchesByChampionshipIdQuery()
        {
            ChampionshipId = id,
        };
        return Ok(await _mediator.Send(query));
    }

    [HttpPut("update")]
    [RequireAdmin]
    public async Task<ActionResult<MatchResponseDto>> UpdateMatch(UdpateMatchCommand updateMatchCommand)
    {
        var response = await _mediator.Send(updateMatchCommand);

        return Ok(response);
    }

    [HttpPut("updateResult")]
    [RequireAdmin]
    public async Task<ActionResult<MatchResponseDto>> UpdateResult(UpdateResultCommand updateResultCommand)
    {
        var response = await _mediator.Send(updateResultCommand);

        return Ok(response);
    }
}
