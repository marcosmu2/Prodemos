using MediatR;
using Microsoft.AspNetCore.Identity;
using Prodemos.Application.Exceptions;
using Prodemos.Application.Models.Authorization;
using Prodemos.Application.Services.Interfaces;
using Prodemos.Domain;

namespace Prodemos.Application.Services.Authorization.Command;
public class RegisterCommand : IRequest<bool>
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, bool>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IAuthService _authService;

    public RegisterCommandHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IAuthService authService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _authService = authService;
    }

    public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existUserEmail = await _userManager.FindByEmailAsync(request.Email) is null ? false : true;
        if (existUserEmail)
        {
            throw new BadRequestException("Email registered");
        }

        var newUser = new User() { 
            FullName = request.FullName,
            Email = request.Email,
            UserName = request.Email,
            PhoneNumber = request.PhoneNumber,
        };

        var result = await _userManager.CreateAsync(newUser, request.Password);

        if (result.Succeeded) {

            await _userManager.AddToRoleAsync(newUser, Role.USER);

            return true;
        }

        throw new BadRequestException("User cannot be created.");
    }
}
