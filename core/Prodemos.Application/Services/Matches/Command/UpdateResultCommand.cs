using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Prodemos.Application.Dtos.Matches;
using Prodemos.Application.Exceptions;
using Prodemos.Application.Persistence;
using Prodemos.Domain;

namespace Prodemos.Application.Services.Matches.Command;
public class UpdateResultCommand : IRequest<MatchResponseDto>
{
    public Guid Id { get; set; }
    public int? ScoreTeamA { get; set; }
    public int? ScoreTeamB { get; set; }
    public MatchStatus? MatchStatus { get; set; }
}

public class UpdateResultCommandHandler : IRequestHandler<UpdateResultCommand, MatchResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateResultCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<MatchResponseDto> Handle(UpdateResultCommand request, CancellationToken cancellationToken)
    {

        if (!await _unitOfWork.Repository<Match>().Exist(x => x.Id == request.Id))
        {
            throw new BadRequestException($"Not exist a {nameof(Match)} with id {request.Id}");
        }

        var include = new Func<IQueryable<Match>, IQueryable<Match>>(c =>
        c.Include(x => x.TeamA).Include(x => x.TeamB));

        var matchToUpdate = await _unitOfWork.Repository<Match>().GetEntityAsync(x => x.Id == request.Id, include, disableTracking: false);

        matchToUpdate.MatchStatus = request.MatchStatus ?? matchToUpdate.MatchStatus;
        matchToUpdate.ScoreTeamA = request.ScoreTeamA ?? matchToUpdate.ScoreTeamA;
        matchToUpdate.ScoreTeamB = request.ScoreTeamB ?? matchToUpdate.ScoreTeamB;

        await _unitOfWork.Repository<Match>().UpdateAsync(matchToUpdate);

        return _mapper.Map<MatchResponseDto>(matchToUpdate);
    }
}
