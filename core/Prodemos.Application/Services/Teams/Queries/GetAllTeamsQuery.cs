using AutoMapper;
using MediatR;
using Prodemos.Application.Dtos.Team;
using Prodemos.Application.Persistence;
using Prodemos.Domain;

namespace Prodemos.Application.Services.Teams.Queries;
public class GetAllTeamsQuery : IRequest<ICollection<TeamResponseDto>>
{ }

public class GetAllTeamsQueryHandler : IRequestHandler<GetAllTeamsQuery, ICollection<TeamResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllTeamsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ICollection<TeamResponseDto>> Handle(GetAllTeamsQuery request, CancellationToken cancellationToken)
    {
        var teams = await _unitOfWork.Repository<Team>().GetAllAsync();
        return _mapper.Map<ICollection<TeamResponseDto>>(teams);
    }
}
