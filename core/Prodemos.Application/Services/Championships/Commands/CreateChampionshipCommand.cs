using AutoMapper;
using MediatR;
using Prodemos.Application.Dtos.Championship;
using Prodemos.Application.Exceptions;
using Prodemos.Application.Persistence;
using Prodemos.Domain;

namespace Prodemos.Application.Services.Championships.Commands;
public class CreateChampionshipCommand : IRequest<ChampionshipResponseDto>
{
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<ChampionshipMatchRequestDto>? Matches { get; set; }
}

public class CreateChampionshipCommandHandler : IRequestHandler<CreateChampionshipCommand, ChampionshipResponseDto>
{
    private readonly IUnitOfWork _unitOfWOrk;
    private readonly IMapper _mapper;

    public CreateChampionshipCommandHandler(IUnitOfWork unitOfWOrk, IMapper mapper)
    {
        _unitOfWOrk = unitOfWOrk;
        _mapper = mapper;
    }

    public async Task<ChampionshipResponseDto> Handle(CreateChampionshipCommand request, CancellationToken cancellationToken)
    {
        if (await _unitOfWOrk.Repository<Championship>().Exist(x => x.Name == request.Name))
        {
            throw new BadRequestException($"A {nameof(Championship)} with name {request.Name} have already exist");
        }

        var newChampionshipId = Guid.NewGuid();

        var newChampionship = new Championship()
        {
            Name = request.Name,
            Id = newChampionshipId
        };

        _unitOfWOrk.Repository<Championship>().AddEntity(newChampionship);

        var requestMatches = request.Matches;

        if (requestMatches is not null && requestMatches.Any())
        {
            foreach (var match in requestMatches)
            {
                var newMatch = new Match
                {
                    TeamAId = match.TeamAId,
                    TeamBId = match.TeamBId,
                    MatchStatus = MatchStatus.Upcoming,
                    ChampionshipId = newChampionshipId
                };

                _unitOfWOrk.Repository<Match>().AddEntity(newMatch);
            }
        }

        await _unitOfWOrk.Complete();

        newChampionship = await _unitOfWOrk.Repository<Championship>().GetByIdAsync(newChampionshipId);

        return _mapper.Map<ChampionshipResponseDto>(newChampionship);
    }
}
