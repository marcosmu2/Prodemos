using FluentValidation;
using Prodemos.Application.Services.Authorization.Command;

namespace Prodemos.Application.Validators.Authorizations;
public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
    }
}
