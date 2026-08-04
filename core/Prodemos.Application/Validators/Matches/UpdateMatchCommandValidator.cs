using FluentValidation;
using Prodemos.Application.Services.Matches.Command;

namespace Prodemos.Application.Validators.Matches;
public class UpdateMatchCommandValidator : AbstractValidator<UdpateMatchCommand>
{
    public UpdateMatchCommandValidator()
    {
        RuleFor(x => x.TeamAId).NotNull().WithMessage("Team A is required");
        RuleFor(x => x.TeamBId).NotNull().WithMessage("Team B is required");
    }
}
