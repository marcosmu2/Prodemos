using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Prodemos.Application.Dtos.Matches;
using Prodemos.Application.Exceptions;
using Prodemos.Application.Persistence;
using Prodemos.Domain;

namespace Prodemos.Application.Services.Matches.Command;
public class UdpateMatchCommand : IRequest<MatchResponseDto>
{
    public Guid Id { get; set; }
    public Guid? TeamAId { get; set; } = null;
    public Guid? TeamBId { get; set; } = null;
}

public class UpdateMatchCommandHandler : IRequestHandler<UdpateMatchCommand, MatchResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateMatchCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<MatchResponseDto> Handle(UdpateMatchCommand request, CancellationToken cancellationToken)
    {
        if (!await _unitOfWork.Repository<Match>().Exist(x => x.Id == request.Id))
        {
            throw new BadRequestException($"Not exist a {nameof(Match)} with id {request.Id}");
        }

        var include = new Func<IQueryable<Match>, IQueryable<Match>>(c =>
        c.Include(x => x.TeamA).Include(x => x.TeamB));

        var matchToUpdate = await _unitOfWork.Repository<Match>().GetEntityAsync(x => x.Id == request.Id, include, disableTracking:false);

        matchToUpdate.TeamAId = request.TeamAId ?? matchToUpdate.TeamAId;
        matchToUpdate.TeamBId = request.TeamBId ?? matchToUpdate.TeamBId;

        await _unitOfWork.Repository<Match>().UpdateAsync(matchToUpdate);

        return _mapper.Map<MatchResponseDto>(matchToUpdate);
    }
}
