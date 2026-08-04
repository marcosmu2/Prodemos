using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Prodemos.Application.Dtos.Matches;
using Prodemos.Application.Persistence;
using Prodemos.Domain;
using System.Linq.Expressions;

namespace Prodemos.Application.Services.Matches.Query;
public class GetMatchesByChampionshipIdQuery : IRequest<ICollection<MatchResponseDto>>
{
    public Guid ChampionshipId { get; set; }
}

public class GetMatchesByChampionshipIdQueryHandler : IRequestHandler<GetMatchesByChampionshipIdQuery, ICollection<MatchResponseDto>>
{
    private readonly IUnitOfWork _unitOfWOrk;
    private readonly IMapper _mapper;

    public GetMatchesByChampionshipIdQueryHandler(IUnitOfWork unitOfWOrk, IMapper mapper)
    {
        _unitOfWOrk = unitOfWOrk;
        _mapper = mapper;
    }

    public async Task<ICollection<MatchResponseDto>> Handle(GetMatchesByChampionshipIdQuery request, CancellationToken cancellationToken)
    {
        var includes = new List<Expression<Func<Match, object>>>();
        includes.Add(x => x.TeamA!);
        includes.Add(x => x.TeamB!);

        var matches = await _unitOfWOrk.Repository<Match>().GetAsync(x => x.ChampionshipId == request.ChampionshipId, includes: includes);

        return _mapper.Map<ICollection<MatchResponseDto>>(matches);
    }
}
