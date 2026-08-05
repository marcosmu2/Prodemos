using FluentValidation;
using Prodemos.Application.Dtos.UserPlays;

namespace Prodemos.Application.Validators.UserPlays;
public class UserGuestUserPlayRequestValidator : AbstractValidator<UserGuestUserPlayRequest>
{
    public UserGuestUserPlayRequestValidator()
    {
        RuleFor(x => x.MatchId).NotNull().WithMessage("Match Id is required");
        RuleFor(x => x.ScoreTeamAGuessed).NotNull().WithMessage("Score Team A is required");
        RuleFor(x => x.ScoreTeamBGuessed).NotNull().WithMessage("Score Team B is required");
    }
}
