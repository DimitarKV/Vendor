using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vendor.Services.User.Data.Entities;

namespace Vendor.Services.User.Data.Persistence;

public class UserDbContext : IdentityDbContext<VendorUser>
{
    public UserDbContext()
    {
    }

    public UserDbContext(DbContextOptions options) : base(options)
    {
    }
}