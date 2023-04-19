using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Vendor.Services.User;

public class UserDbContext : IdentityDbContext<VendorUser>
{
    public UserDbContext()
    {
    }

    public UserDbContext(DbContextOptions options) : base(options)
    {
    }
}