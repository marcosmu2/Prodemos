using MediatR;
using Microsoft.AspNetCore.Identity;
using Prodemos.Application.Dtos.Authorization;
using Prodemos.Application.Exceptions;
using Prodemos.Application.Services.Interfaces;
using Prodemos.Domain;

namespace Prodemos.Application.Services.Authorization.Command;
public class LoginUserCommand : IRequest<AuthResponseDto>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthResponseDto>
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IAuthService _authService;

    public LoginUserCommandHandler(UserManager<User> userManager,
                                   SignInManager<User> signInManager,
                                   RoleManager<IdentityRole> roleManager,
                                   IAuthService authService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _authService = authService;
    }

    public async Task<AuthResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new NotFoundException(nameof(User), request.Email);
        }

        if (!user.IsActive)
        {
            throw new Exception("Inactive user, please contact the administrator for further assistance");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
        {
            throw new BadRequestException("Invalid credential errors");
        }

        var roles = await _userManager.GetRolesAsync(user);

        var authResponse = new AuthResponseDto
        {
            Email = user.Email,
            FullName = user.FullName,
            Id = user.Id,
            PhoneNumber = user.PhoneNumber,
            Roles = roles,
            Token = _authService.CreateToken(user,roles)
        };

        return authResponse;
    }
}
