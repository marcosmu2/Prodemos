using AutoMapper;
using MediatR;
using Prodemos.Application.Dtos.Team;
using Prodemos.Application.Persistence;
using Prodemos.Application.Services.Interfaces;
using Prodemos.Domain;

namespace Prodemos.Application.Services.Teams.Command;
public class CreateTeamCommand : IRequest<TeamResponseDto>
{
    public string Name { get; set; } = string.Empty;
    public string FlagUrl { get; set; } = string.Empty;
}

public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, TeamResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateTeamCommandHandler(IUnitOfWork unitOfWork, IAuthService authService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TeamResponseDto> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
    {
        var newTeam = new Team()
        {
            Name = request.Name.Trim(),
            FlagUrl = request.FlagUrl
        };

        newTeam = await _unitOfWork.Repository<Team>().AddAsync(newTeam);

        return _mapper.Map<TeamResponseDto>(newTeam);
    }
}
