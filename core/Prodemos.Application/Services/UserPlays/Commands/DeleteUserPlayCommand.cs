using MediatR;
using Prodemos.Application.Exceptions;
using Prodemos.Application.Persistence;
using Prodemos.Domain;

namespace Prodemos.Application.Services.UserPlays.Commands;
public class DeleteUserPlayCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}

public class DeleteUserPlayCommandHandler : IRequestHandler<DeleteUserPlayCommand, bool>
{
    private readonly IUnitOfWork _unitOfWOrk;

    public DeleteUserPlayCommandHandler(IUnitOfWork unitOfWOrk)
    {
        _unitOfWOrk = unitOfWOrk;
    }

    public async Task<bool> Handle(DeleteUserPlayCommand request, CancellationToken cancellationToken)
    {
        if (!await _unitOfWOrk.Repository<UserPlay>().Exist(x => x.Id == request.Id))
        {
            throw new BadRequestException($"Not exist a {nameof(UserPlay)} with id {request.Id}");
        }

        var userPlayToDelete = await _unitOfWOrk.Repository<UserPlay>().GetByIdAsync(request.Id);

        try
        {
            await _unitOfWOrk.Repository<UserPlay>().DeleteAsync(userPlayToDelete);
        }
        catch (Exception ex)
        {
            throw new DBException($"Something wrong ocurred when we trying to delete a {nameof(UserPlay)} " +
                $"with Id {request.Id}, {ex.Message}");
        }

        return true;
    }
}
