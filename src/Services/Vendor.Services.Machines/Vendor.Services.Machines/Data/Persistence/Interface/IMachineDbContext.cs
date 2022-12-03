using Microsoft.EntityFrameworkCore;
using Vendor.Services.Machines.Data.Entities;

namespace Vendor.Services.Machines.Data.Persistence.Interface;

public interface IMachineDbContext
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    public DbSet<Product> Products { get; set; }
    public DbSet<Vending> Vendings { get; set; }

    public DbSet<Banknote> Banknotes { get; set; }
}