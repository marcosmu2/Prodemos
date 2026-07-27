using Microsoft.AspNetCore.Authorization;
using Prodemos.Application.Models.Authorization;

namespace Prodemos.Api.Attribute;

public class RequireAdminAttribute : AuthorizeAttribute
{
    public RequireAdminAttribute() : base()
    {
        Roles = Role.ADMIN;
    }
}
