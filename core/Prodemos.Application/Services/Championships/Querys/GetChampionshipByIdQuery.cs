using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Prodemos.Application.Dtos.Championship;
using Prodemos.Application.Persistence;
using Prodemos.Domain;
using System.Linq.Expressions;

namespace Prodemos.Application.Services.Championships.Querys;
public class GetChampionshipByIdQuery : IRequest<ChampionshipResponseDto>
{
    public Guid Id { get; set; }
}

public class GetChampionshipByIdQueryHandler : IRequestHandler<GetChampionshipByIdQuery, ChampionshipResponseDto>
{
    private readonly IUnitOfWork _unitOfWOrk;
    private readonly IMapper _mapper;

    public GetChampionshipByIdQueryHandler(IUnitOfWork unitOfWOrk, IMapper mapper)
    {
        _unitOfWOrk = unitOfWOrk;
        _mapper = mapper;
    }

    public async Task<ChampionshipResponseDto> Handle(GetChampionshipByIdQuery request, CancellationToken cancellationToken)
    {
        var include = new Func<IQueryable<Championship>, IQueryable<Championship>>(c => 
        c.Include(x => x.Matches).ThenInclude(y => y.TeamA)
        .Include(x => x.Matches).ThenInclude(z => z.TeamB));

        var response = await _unitOfWOrk.Repository<Championship>().GetEntityAsync(x => x.Id == request.Id, include);

        return _mapper.Map<ChampionshipResponseDto>(response);
    }
}
