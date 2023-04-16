using Microsoft.EntityFrameworkCore;
using Vendor.Services.Products.Domain.AggregateModel.ProductAggregate;
using Vendor.Services.Products.Domain.SeedWork;

namespace Vendor.Services.Products;

public class ProductsDbContext : DbContext, IUnitOfWork
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