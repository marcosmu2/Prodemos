using FluentValidation;
using Prodemos.Application.Services.Championships.Commands;

namespace Prodemos.Application.Validators.Championships;
public class CreateChampionshipCommandValidator : AbstractValidator<CreateChampionshipCommand>
{
    public CreateChampionshipCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is a required field");
    }
}
