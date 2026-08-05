using MediatR;
using Prodemos.Application.Exceptions;
using Prodemos.Application.Persistence;
using Prodemos.Domain;

namespace Prodemos.Application.Services.UserGuests.Commands;
public class DeleteUserGuestCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}

public class DeleteUserGuestCommandHandler : IRequestHandler<DeleteUserGuestCommand, bool>
{
    private readonly IUnitOfWork _unitOfWOrk;

    public DeleteUserGuestCommandHandler(IUnitOfWork unitOfWOrk)
    {
        _unitOfWOrk = unitOfWOrk;
    }

    public async Task<bool> Handle(DeleteUserGuestCommand request, CancellationToken cancellationToken)
    {
        if (!await _unitOfWOrk.Repository<UserGuest>().Exist(x => x.Id == request.Id))
        {
            throw new BadRequestException($"Not exist a {nameof(UserGuest)} with id {request.Id}");
        }

        var userGuestToDelete = await _unitOfWOrk.Repository<UserGuest>().GetByIdAsync(request.Id);

        try
        {
            await _unitOfWOrk.Repository<UserGuest>().DeleteAsync(userGuestToDelete);
        }
        catch (Exception ex)
        {
            throw new DBException($"Something wrong ocurred when we trying to delete a {nameof(UserGuest)} " +
                $"with Id {request.Id}, {ex.Message}");
        }

        return true;
    }
}
