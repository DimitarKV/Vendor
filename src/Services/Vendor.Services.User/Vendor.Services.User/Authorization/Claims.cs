using System.Security.Claims;

namespace Vendor.Services.User.Authorization;

public static class Claims
{
    public const string UserRole = "User";
    public const string MaintainerRole = "Maintainer";

    public static Claim User = new Claim(ClaimTypes.Role, UserRole);
    public static Claim Maintainer = new Claim(ClaimTypes.Role, MaintainerRole);

}