using System.Security.Claims;

namespace Vendor.Services.User.Authorization;

public static class Claims
{
    public static readonly Dictionary<string, Claim> RoleClaims = new()
    {
        {"Admin", new Claim(ClaimTypes.Role, "Admin")},
        {"Maintainer", new Claim(ClaimTypes.Role, "Maintainer")},
        {"User", new Claim(ClaimTypes.Role, "User")},
        {"Machine", new Claim(ClaimTypes.Role, "Machine")}
    };
}