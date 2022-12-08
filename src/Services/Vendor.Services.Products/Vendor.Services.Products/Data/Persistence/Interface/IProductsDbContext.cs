using Microsoft.EntityFrameworkCore;
using Vendor.Domain.Entities;

namespace Vendor.Services.Products.Data.Persistence.Interface;

public interface IProductsDbContext
{
    public DbSet<Product> Products { get; set; }
}