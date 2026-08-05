using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Prodemos.Application.Dtos.UserGuests;
using Prodemos.Application.Exceptions;
using Prodemos.Application.Persistence;
using Prodemos.Application.Services.Interfaces;
using Prodemos.Domain;

namespace Prodemos.Application.Services.UserGuests.Queries;
public class GetUserGuestsByUserAndChampionshipIdCommand : IRequest<ICollection<UserGuestResponseDto>>
{
    public Guid ChampionshipId { get; set; }
}

public class GetUserGuestsByUserAndChampionshipIdHandler : IRequestHandler<GetUserGuestsByUserAndChampionshipIdCommand, ICollection<UserGuestResponseDto>>
{
    private readonly IUnitOfWork _unitOfWOrk;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IAuthService _authService;

    public GetUserGuestsByUserAndChampionshipIdHandler(IUnitOfWork unitOfWOrk, IMapper mapper, UserManager<User> userManager,
                                                        IAuthService authService)
    {
        _unitOfWOrk = unitOfWOrk;
        _mapper = mapper;
        _userManager = userManager;
        _authService = authService;
    }

    public async Task<ICollection<UserGuestResponseDto>> Handle(GetUserGuestsByUserAndChampionshipIdCommand request, CancellationToken cancellationToken)
    {
        if (!await _unitOfWOrk.Repository<Championship>().Exist(x => x.Id == request.ChampionshipId))
        {
            throw new BadRequestException($"Not exist a {nameof(Championship)} with id {request.ChampionshipId}");
        }

        var user = await _userManager.FindByEmailAsync(_authService.GetSessionUserEmail());
        Guid userId = Guid.Parse(user!.Id);

        Func<IQueryable<UserGuest>, IQueryable<UserGuest>> include = new(c =>
                c.Include(x => x.UserPlay).Include(y => y.Match).ThenInclude(z => z!.TeamA)
                .Include(y => y.Match).ThenInclude(z => z!.TeamB));

        var userGuests = await _unitOfWOrk.Repository<UserGuest>()
                                        .GetAsync(x => x.UserPlay!.ChampionshipId == request.ChampionshipId
                                                                && x.UserPlay!.UserId == userId, include: include);

        return _mapper.Map<ICollection<UserGuestResponseDto>>(userGuests);
    }
}
