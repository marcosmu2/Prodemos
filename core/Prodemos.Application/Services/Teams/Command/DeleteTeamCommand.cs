using MediatR;
using Prodemos.Application.Exceptions;
using Prodemos.Application.Persistence;
using Prodemos.Domain;

namespace Prodemos.Application.Services.Teams.Command;
public class DeleteTeamCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}

public class DeleteTeamCommandHandler : IRequestHandler<DeleteTeamCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    public DeleteTeamCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
    {
        var team = await _unitOfWork.Repository<Team>().GetByIdAsync(request.Id);

        if (team == null)
        {
            throw new NotFoundException(nameof(Team), request.Id);
        }

        try
        {
            await _unitOfWork.Repository<Team>().DeleteAsync(team);
            return true;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
