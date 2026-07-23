using FluentValidation;
using Prodemos.Application.Services.Authorization.Command;

namespace Prodemos.Application.Validators.Authorizations;
public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(x => x.FullName).NotEmpty().WithMessage("Full name is required");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
    }
}
