using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Prodemos.Application.Dtos.UserPlays;
using Prodemos.Application.Exceptions;
using Prodemos.Application.Persistence;
using Prodemos.Application.Services.Interfaces;
using Prodemos.Domain;

namespace Prodemos.Application.Services.UserPlays.Commands;
public class CreateUserPlayCommand : IRequest<UserPlayResponseDto>
{
    public Guid? ChampionshipId { get; set; }
    public virtual ICollection<UserGuestUserPlayRequest> UserGuests { get; set; } = new List<UserGuestUserPlayRequest>();
}

public class CreateUserPlayCommandHandler : IRequestHandler<CreateUserPlayCommand, UserPlayResponseDto>
{
    private readonly IUnitOfWork _unitOfWOrk;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IAuthService _authService;

    public CreateUserPlayCommandHandler(IUnitOfWork unitOfWOrk, IMapper mapper, IAuthService authService,
                                        UserManager<User> userManager)
    {
        _unitOfWOrk = unitOfWOrk;
        _mapper = mapper;
        _authService = authService;
        _userManager = userManager;
    }

    public async Task<UserPlayResponseDto> Handle(CreateUserPlayCommand request, CancellationToken cancellationToken)
    {
        if (!await _unitOfWOrk.Repository<Championship>().Exist(x => x.Id == request.ChampionshipId))
        {
            throw new BadRequestException($"Not exist a {nameof(Championship)} with id {request.ChampionshipId}");
        }

        Guid userId = await GetUserId(request);

        var newUserPlayId = Guid.NewGuid();

        UserPlay newUserPlay = new()
        {
            Id = newUserPlayId,
            ChampionshipId = (Guid)request.ChampionshipId!,
            Points = 0,
            UserId = userId
        };

        _unitOfWOrk.Repository<UserPlay>().AddEntity(newUserPlay);

        foreach (var userGuest in request.UserGuests)
        {
            CreateUserGuests(newUserPlayId, userGuest);
        }

        await _unitOfWOrk.Complete();
        newUserPlay = await GetUserPlay(newUserPlayId, newUserPlay);

        return _mapper.Map<UserPlayResponseDto>(newUserPlay);
    }

    private async Task<UserPlay> GetUserPlay(Guid newUserPlayId, UserPlay newUserPlay)
    {
        Func<IQueryable<UserPlay>, IQueryable<UserPlay>> include = new (c =>
                c.Include(x => x.Championship).Include(x => x.UserGuests).ThenInclude(y => y.Match).ThenInclude(z => z!.TeamA)
                .Include(x => x.UserGuests).ThenInclude(y => y.Match).ThenInclude(z => z!.TeamB));

        newUserPlay = await _unitOfWOrk.Repository<UserPlay>().GetEntityAsync(x => x.Id == newUserPlayId, include);
        return newUserPlay;
    }

    private void CreateUserGuests(Guid newUserPlayId, UserGuestUserPlayRequest userGuest)
    {
        UserGuest newUserGuest = new()
        {
            MatchId = (Guid)userGuest.MatchId!,
            ScoreTeamAGuessed = (int)userGuest.ScoreTeamAGuessed!,
            ScoreTeamBGuessed = (int)userGuest.ScoreTeamBGuessed!,
            GuessStatus = GuessStatus.Pending,
            UserPlayId = newUserPlayId,
        };

        _unitOfWOrk.Repository<UserGuest>().AddEntity(newUserGuest);
    }

    private async Task<Guid> GetUserId(CreateUserPlayCommand request)
    {
        var user = await _userManager.FindByEmailAsync(_authService.GetSessionUserEmail());
        Guid userId = Guid.Parse(user!.Id);

        if (await _unitOfWOrk.Repository<UserPlay>().Exist(x => x.ChampionshipId == request.ChampionshipId && x.UserId == userId))
        {
            throw new BadRequestException($"A {nameof(UserPlay)} for the championship Id {request.ChampionshipId} has already exist");
        }

        return userId;
    }
}
