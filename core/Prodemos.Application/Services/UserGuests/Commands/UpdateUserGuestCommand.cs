using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Prodemos.Application.Dtos.UserGuests;
using Prodemos.Application.Exceptions;
using Prodemos.Application.Persistence;
using Prodemos.Domain;

namespace Prodemos.Application.Services.UserGuests.Commands;
public class UpdateUserGuestCommand : IRequest<UserGuestResponseDto>
{
    public Guid Id { get; set; }
    public int? ScoreTeamAGuessed { get; set; }
    public int? ScoreTeamBGuessed { get; set; }
    public GuessStatus? Status { get; set; }
}

public class UpdateUserGuestCommandHandler : IRequestHandler<UpdateUserGuestCommand, UserGuestResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateUserGuestCommandHandler(IUnitOfWork unitOfWOrk, IMapper mapper)
    {
        _unitOfWork = unitOfWOrk;
        _mapper = mapper;
    }

    public async Task<UserGuestResponseDto> Handle(UpdateUserGuestCommand request, CancellationToken cancellationToken)
    {
        if (!await _unitOfWork.Repository<UserGuest>().Exist(x => x.Id == request.Id))
        {
            throw new BadRequestException($"Not exist a {nameof(UserGuest)} with id {request.Id}");
        }

        Func<IQueryable<UserGuest>, IQueryable<UserGuest>> include = new(c =>
                c.Include(y => y.Match).ThenInclude(z => z!.TeamA)
                .Include(y => y.Match).ThenInclude(z => z!.TeamB));

        var userGuestToUpdate = await _unitOfWork.Repository<UserGuest>().GetEntityAsync(x => x.Id == request.Id, include);

        if (userGuestToUpdate == null)
        {
            throw new BadRequestException($"There is not an {nameof(UserGuest)} with id {request.Id}");
        }

        userGuestToUpdate.ScoreTeamAGuessed = request.ScoreTeamAGuessed ?? userGuestToUpdate.ScoreTeamAGuessed;
        userGuestToUpdate.ScoreTeamBGuessed = request.ScoreTeamBGuessed ?? userGuestToUpdate.ScoreTeamBGuessed;
        userGuestToUpdate.GuessStatus = request.Status ?? userGuestToUpdate.GuessStatus;

        await _unitOfWork.Repository<UserGuest>().UpdateAsync(userGuestToUpdate);

        return _mapper.Map<UserGuestResponseDto>(userGuestToUpdate);
    }
}
