using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Prodemos.Application.Dtos.Championship;
using Prodemos.Application.Exceptions;
using Prodemos.Application.Persistence;
using Prodemos.Domain;

namespace Prodemos.Application.Services.Championships.Commands;
public class UpdateChampionshipCommand : IRequest<ChampionshipResponseDto>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public virtual ICollection<ChampionshipMatchRequestDto> Matches { get; set; } = new List<ChampionshipMatchRequestDto>();
}

public class UpdateChampionshipCommandHandler : IRequestHandler<UpdateChampionshipCommand, ChampionshipResponseDto>
{
    private readonly IUnitOfWork _unitOfWOrk;
    private readonly IMapper _mapper;

    public UpdateChampionshipCommandHandler(IUnitOfWork unitOfWOrk, IMapper mapper)
    {
        _unitOfWOrk = unitOfWOrk;
        _mapper = mapper;
    }

    public async Task<ChampionshipResponseDto> Handle(UpdateChampionshipCommand request, CancellationToken cancellationToken)
    {
        if (await _unitOfWOrk.Repository<Championship>().Exist(x => x.Name == request.Name))
        {
            throw new BadRequestException($"A {nameof(Championship)} with name {request.Name} have already exist");
        }

        var championshipToUpdate = await _unitOfWOrk.Repository<Championship>().GetByIdAsync(request.Id);

        if (championshipToUpdate is null)
        {
            throw new BadRequestException($"Not exist a {nameof(Championship)} with id {request.Id}");
        }

        championshipToUpdate.Name = string.IsNullOrWhiteSpace(request.Name) ? championshipToUpdate.Name : request.Name.Trim();
        _unitOfWOrk.Repository<Championship>().UpdateEntity(championshipToUpdate);

        if (request.Matches.Any())
        {
            await DeleteOldMatches(request);

            CreateMatches(request);
        }

        await _unitOfWOrk.Complete();

        championshipToUpdate = await GetChampionship(request.Id, championshipToUpdate);

        return _mapper.Map<ChampionshipResponseDto>(championshipToUpdate);
    }

    private void CreateMatches(UpdateChampionshipCommand request)
    {
        foreach (var match in request.Matches)
        {
            var newMatch = new Match
            {
                TeamAId = match.TeamAId,
                TeamBId = match.TeamBId,
                MatchStatus = MatchStatus.Upcoming,
                ChampionshipId = request.Id
            };

            _unitOfWOrk.Repository<Match>().AddEntity(newMatch);
        }
    }

    private async Task DeleteOldMatches(UpdateChampionshipCommand request)
    {
        var oldMatches = await _unitOfWOrk.Repository<Match>().GetAsync(x => x.ChampionshipId == request.Id);

        foreach (var oldMatch in oldMatches)
        {
            _unitOfWOrk.Repository<Match>().DeleteEntity(oldMatch);
        }
    }

    private async Task<Championship> GetChampionship(Guid newChampionshipId, Championship newChampionship)
    {
        var include = new Func<IQueryable<Championship>, IQueryable<Championship>>(c =>
        c.Include(x => x.Matches).ThenInclude(y => y.TeamA)
        .Include(x => x.Matches).ThenInclude(z => z.TeamB));

        newChampionship = await _unitOfWOrk.Repository<Championship>().GetEntityAsync(x => x.Id == newChampionshipId, include);
        return newChampionship;
    }
}
