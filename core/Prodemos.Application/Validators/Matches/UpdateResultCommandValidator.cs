using FluentValidation;
using Prodemos.Application.Services.Matches.Command;

namespace Prodemos.Application.Validators.Matches;
public class UpdateResultCommandValidator : AbstractValidator<UpdateResultCommand>
{
    public UpdateResultCommandValidator()
    {
        RuleFor(x => x.MatchStatus).NotNull().WithMessage("MatchStatus is required");
        RuleFor(x => x.ScoreTeamA).NotNull().WithMessage("ScoreTeamA is required");
        RuleFor(x => x.ScoreTeamB).NotNull().WithMessage("ScoreTeamB is required");
    }
}
