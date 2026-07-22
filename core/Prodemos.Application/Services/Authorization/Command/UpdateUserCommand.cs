using MediatR;
using Microsoft.AspNetCore.Identity;
using Prodemos.Application.Exceptions;
using Prodemos.Application.Services.Interfaces;
using Prodemos.Domain;

namespace Prodemos.Application.Services.Authorization.Command;
public class UpdateUserCommand : IRequest<bool>
{
    public string FullName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
{
    private readonly UserManager<User> _userManager;
    private readonly IAuthService _authService;

    public UpdateUserCommandHandler(UserManager<User> userManager, IAuthService authService)
    {
        _userManager = userManager;
        _authService = authService;
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var userByEmail = await _userManager.FindByEmailAsync(_authService.GetSessionUserEmail());
        if (userByEmail is null)
        {
            throw new BadRequestException($"Email {_authService.GetSessionUserEmail()} does not exist");
        }

        userByEmail.PhoneNumber = string.IsNullOrEmpty(request.PhoneNumber) ? userByEmail.PhoneNumber : request.PhoneNumber;
        userByEmail.FullName = string.IsNullOrEmpty(request.FullName) ? userByEmail.FullName : request.FullName;

        if (string.IsNullOrEmpty(request.Password))
        {
            var hashedNewPassword = _userManager.PasswordHasher.HashPassword(userByEmail, request.Password);
            userByEmail.PasswordHash = hashedNewPassword;
        }

        var resultado = await _userManager.UpdateAsync(userByEmail);

        if (!resultado.Succeeded)
        {
            throw new Exception("Cannot update the user");
        }

        return true;
    }
}
