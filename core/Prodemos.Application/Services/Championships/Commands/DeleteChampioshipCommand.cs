using AutoMapper;
using MediatR;
using Prodemos.Application.Exceptions;
using Prodemos.Application.Persistence;
using Prodemos.Domain;

namespace Prodemos.Application.Services.Championships.Commands;
public class DeleteChampioshipCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}

public class DeleteChampioshipCommandHandler : IRequestHandler<DeleteChampioshipCommand, bool>
{
    private readonly IUnitOfWork _unitOfWOrk;
    private readonly IMapper _mapper;

    public DeleteChampioshipCommandHandler(IUnitOfWork unitOfWOrk, IMapper mapper)
    {
        _unitOfWOrk = unitOfWOrk;
        _mapper = mapper;
    }

    public async Task<bool> Handle(DeleteChampioshipCommand request, CancellationToken cancellationToken)
    {
        var championship = await _unitOfWOrk.Repository<Championship>().GetByIdAsync(request.Id);

        if (championship == null)
        {
            throw new BadRequestException($"There is not a {nameof(Championship)} with id {request.Id}");
        }

        await _unitOfWOrk.Repository<Championship>().DeleteAsync(championship);

        return true;
    }
}
