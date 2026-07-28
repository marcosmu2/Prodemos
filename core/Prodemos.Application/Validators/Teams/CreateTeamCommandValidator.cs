using FluentValidation;
using Prodemos.Application.Services.Teams.Command;

namespace Prodemos.Application.Validators.Teams;
public class CreateTeamCommandValidator : AbstractValidator<CreateTeamCommand>
{
    public CreateTeamCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is a required field");
    }
}
