using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Prodemos.Application.Models.Authorization;
using Prodemos.Domain;

namespace Prodemos.Infrastructure.Persistence;
public class ProdemosDbContextData
{
    public static async Task LoadDataASync(
        ProdemosDbContext context,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        ILoggerFactory loggerFactory
        )
    {
        try
        {
            if (!roleManager.Roles.Any()) {
                await roleManager.CreateAsync(new IdentityRole(Role.ADMIN));
                await roleManager.CreateAsync(new IdentityRole(Role.USER));
            }

            if (!userManager.Users.Any()) {

                User userAdmin = new()
                {
                    FullName = "Admin",
                    Email = "admin@gmail.com",
                    UserName = "admin",
                    PhoneNumber = "1234567890",
                };

                await userManager.CreateAsync(userAdmin, "Nal17amj123456789=");
                await userManager.AddToRoleAsync(userAdmin, Role.ADMIN);

                User newUser = new()
                {
                    FullName = "Juan Perez",
                    Email = "juan_perez@gmail.com",
                    UserName = "juan.perez",
                    PhoneNumber = "1234567891",
                };

                await userManager.CreateAsync(newUser, "PasswordJuanPerez123$");
                await userManager.AddToRoleAsync(newUser, Role.USER);
                await context.SaveChangesAsync();
            }

        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<ProdemosDbContext>();
            logger.LogError(ex.Message);
        }
    }
}
