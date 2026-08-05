using FluentValidation;
using Prodemos.Application.Services.UserGuests.Commands;

namespace Prodemos.Application.Validators.UserGuests;
public class CreateUserGuestCommandValidator : AbstractValidator<CreateUserGuestCommand>
{
    public CreateUserGuestCommandValidator()
    {
        RuleFor(x => x.MatchId).NotNull().WithMessage("Match Id is required");
        RuleFor(x => x.UserPlayId).NotNull().WithMessage("UserPlay Id is required");
        RuleFor(x => x.ScoreTeamAGuessed).NotNull().WithMessage("Team A Guess is required");
        RuleFor(x => x.ScoreTeamBGuessed).NotNull().WithMessage("Team B Guess is required");
    }
}
