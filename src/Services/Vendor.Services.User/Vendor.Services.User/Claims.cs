using System.Security.Claims;

namespace Vendor.Services.User;

public static class Claims
{
    public static readonly Dictionary<string, RoleClaim> RoleClaims = new()
    {
        { "Admin", new RoleClaim("Admin", 60 * 24 * 14) },
        { "Maintainer", new RoleClaim("Maintainer", 60 * 24) },
        { "User", new RoleClaim("User", 60 * 24 * 7) },
        { "Machine", new RoleClaim("Machine", 60 * 24 * 365 * 10) }
    };

    public class RoleClaim
    {
        public Claim Claim { get; }
        public int SessionExpiry { get; }

        /// <summary>
        /// Constructor for RoleClaim with sessionExpiry
        /// </summary>
        /// <param name="role">Set the role of the claim</param>
        /// <param name="sessionExpiry">Set the expiration time of the token which is going to be generated</param>
        public RoleClaim(string role, int sessionExpiry)
        {
            Claim = new Claim(ClaimTypes.Role, role);
            SessionExpiry = sessionExpiry;
        }
    }
}