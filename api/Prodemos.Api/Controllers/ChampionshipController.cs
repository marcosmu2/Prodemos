using MediatR;
using Microsoft.AspNetCore.Mvc;
using Prodemos.Api.Attribute;
using Prodemos.Application.Dtos.Championship;
using Prodemos.Application.Services.Championships.Commands;
using Prodemos.Application.Services.Championships.Querys;

namespace Prodemos.Api.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class ChampionshipController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChampionshipController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get/{id}")]
    public async Task<ActionResult<ChampionshipResponseDto>> GetById(Guid id)
    {
        var query = new GetChampionshipByIdQuery()
        {
            Id = id
        };
        return await _mediator.Send(query);
    }

    [HttpPost("create")]
    [RequireAdmin]
    public async Task<ActionResult<ChampionshipResponseDto>> CreateChampionship(CreateChampionshipCommand createChampionshipCommand)
    {
        var response = await _mediator.Send(createChampionshipCommand);
        return Ok(response);
    }

    [HttpPut("update")]
    [RequireAdmin]
    public async Task<ActionResult<ChampionshipResponseDto>> UpdateChampionship(UpdateChampionshipCommand updateChampionshipCommand)
    {
        var response = await _mediator.Send(updateChampionshipCommand);
        return Ok(response);
    }

    [HttpDelete("delete/{id}")]
    [RequireAdmin]
    public async Task<ActionResult<string>> DeleteChampionship(Guid id)
    {
        var deletecommand = new DeleteChampioshipCommand()
        {
            Id = id,
        };

        return await _mediator.Send(deletecommand) ? Ok("Championship deleted successfully") : BadRequest();
    }
}
