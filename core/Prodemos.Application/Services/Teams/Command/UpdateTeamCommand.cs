using AutoMapper;
using MediatR;
using Prodemos.Application.Dtos.Team;
using Prodemos.Application.Exceptions;
using Prodemos.Application.Persistence;
using Prodemos.Domain;

namespace Prodemos.Application.Services.Teams.Command;
public class UpdateTeamCommand : IRequest<TeamResponseDto>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string FlagUrl { get; set; } = string.Empty;
}

public class UpdateTeamCommandHandler : IRequestHandler<UpdateTeamCommand, TeamResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateTeamCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TeamResponseDto> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
    {
        var team = await _unitOfWork.Repository<Team>().GetByIdAsync(request.Id);

        if (team == null)
        {
            throw new NotFoundException(nameof(Team), request.Id);
        }

        team.Name = string.IsNullOrWhiteSpace(request.Name) ? team.Name : request.Name;
        team.FlagUrl = string.IsNullOrWhiteSpace(request.FlagUrl) ? team.FlagUrl: request.FlagUrl;

        var response = await _unitOfWork.Repository<Team>().UpdateAsync(team);
        return _mapper.Map<TeamResponseDto>(response);
    }
}
