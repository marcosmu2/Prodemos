using FluentValidation;
using Prodemos.Application.Services.UserPlays.Commands;

namespace Prodemos.Application.Validators.UserPlays;
public class CreateUserPlayCommandValidator : AbstractValidator<CreateUserPlayCommand>
{
    public CreateUserPlayCommandValidator()
    {
        RuleFor(x => x.ChampionshipId).NotNull().WithMessage("Championship Id is required.");
        RuleForEach(x => x.UserGuests).SetValidator(new UserGuestUserPlayRequestValidator());
    }
}
