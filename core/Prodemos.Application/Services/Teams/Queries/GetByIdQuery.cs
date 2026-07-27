using AutoMapper;
using MediatR;
using Prodemos.Application.Dtos.Team;
using Prodemos.Application.Exceptions;
using Prodemos.Application.Persistence;
using Prodemos.Domain;

namespace Prodemos.Application.Services.Teams.Queries;
public class GetTeamByIdQuery : IRequest<TeamResponseDto>
{
    public Guid Id { get; set; }
}

public class GetTeamByIdQueryHandler : IRequestHandler<GetTeamByIdQuery, TeamResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTeamByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TeamResponseDto> Handle(GetTeamByIdQuery request, CancellationToken cancellationToken)
    {
        var team = await _unitOfWork.Repository<Team>().GetByIdAsync(request.Id);

        if (team == null)
        {
            throw new NotFoundException(nameof(Team), request.Id);
        }

        return _mapper.Map<TeamResponseDto>(team);
    }
}
