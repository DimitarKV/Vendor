using Microsoft.EntityFrameworkCore;
using Vendor.Services.Products.Domain.AggregateModel.ProductAggregate;
using Vendor.Services.Products.Domain.SeedWork;

namespace Vendor.Services.Products.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ProductsDbContext _context;

    public ProductRepository(ProductsDbContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;
    
    public async Task<Product?> FindProductByIdAsync(int id)
    {
        return await _context.Products.SingleOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Product>> FindProductsByMatchingNameAsync(string name)
    {
        if (name == "")
            return await _context.Products.ToListAsync();
        return await _context.Products.Where(p => p.Name.Contains(name)).ToListAsync();
    }

    public async Task<Product?> FindProductByExactNameAsync(string name)
    {
        return await _context.Products.SingleOrDefaultAsync(p => p.Name == name);
    }

    public void AddProduct(Product product)
    {
        _context.Products.Add(product);
    }
}