using Microsoft.EntityFrameworkCore;
using Vendor.Domain.Entities;
using Vendor.Services.Products.Data.Persistence.Interface;

namespace Vendor.Services.Products.Data.Persistence;

public class ProductsDbContext : DbContext, IProductsDbContext
{
    public DbSet<Product> Products { get; set; }

    public ProductsDbContext()
    {
        
    }

    public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Name)
            .IsUnique();
    }
}