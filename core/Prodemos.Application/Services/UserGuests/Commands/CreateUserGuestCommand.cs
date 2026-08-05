using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Prodemos.Application.Dtos.UserGuests;
using Prodemos.Application.Exceptions;
using Prodemos.Application.Persistence;
using Prodemos.Application.Services.Interfaces;
using Prodemos.Domain;

namespace Prodemos.Application.Services.UserGuests.Commands;
public class CreateUserGuestCommand : IRequest<UserGuestResponseDto>
{
    public Guid? UserPlayId { get; set; }
    public Guid? MatchId { get; set; }
    public int? ScoreTeamAGuessed { get; set; }
    public int? ScoreTeamBGuessed { get; set; }
}

public class CreateUserGuestCommandHandler : IRequestHandler<CreateUserGuestCommand, UserGuestResponseDto>
{
    private readonly IUnitOfWork _unitOfWOrk;
    private readonly IMapper _mapper;

    public CreateUserGuestCommandHandler(IUnitOfWork unitOfWOrk, IMapper mapper, UserManager<User> userManager, IAuthService authService)
    {
        _unitOfWOrk = unitOfWOrk;
        _mapper = mapper;
    }

    public async Task<UserGuestResponseDto> Handle(CreateUserGuestCommand request, CancellationToken cancellationToken)
    {
        if (await _unitOfWOrk.Repository<UserGuest>().Exist(x => x.UserPlayId == request.UserPlayId && x.MatchId == request.MatchId))
        {
            throw new BadRequestException($"There has already exist a {nameof(UserGuest)} for the userPlay id " +
                                            $"{request.UserPlayId} and the match id {request.MatchId}");
        }

        UserGuest newUserGuest = new()
        {
            UserPlayId = (Guid)request.UserPlayId!,
            MatchId = (Guid)request.MatchId!,
            ScoreTeamAGuessed = (int)request.ScoreTeamAGuessed!,
            ScoreTeamBGuessed = (int)request.ScoreTeamBGuessed!,
            GuessStatus = GuessStatus.Pending
        };

        newUserGuest = await _unitOfWOrk.Repository<UserGuest>().AddAsync(newUserGuest);

        Func<IQueryable<UserGuest>, IQueryable<UserGuest>> include = new (c =>
                c.Include(y => y.Match).ThenInclude(z => z!.TeamA)
                .Include(y => y.Match).ThenInclude(z => z!.TeamB));

        newUserGuest = await _unitOfWOrk.Repository<UserGuest>().GetEntityAsync(x => x.Id == newUserGuest.Id, include);

        return _mapper.Map<UserGuestResponseDto>(newUserGuest);
    }
}
