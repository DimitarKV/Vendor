using Microsoft.EntityFrameworkCore;
using Vendor.Services.Machines.Data.Entities;
using Vendor.Services.Machines.Data.Persistence.Interface;

namespace Vendor.Services.Machines.Data.Persistence;

public class MachineDbContext : DbContext, IMachineDbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Vending> Vendings { get; set; }
    public DbSet<Banknote> Banknotes { get; set; }

    public MachineDbContext()
    {
        
    }

    public MachineDbContext(DbContextOptions<MachineDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Banknote>()
            .HasIndex(b => b.ValueInString)
            .IsUnique();
    }
}