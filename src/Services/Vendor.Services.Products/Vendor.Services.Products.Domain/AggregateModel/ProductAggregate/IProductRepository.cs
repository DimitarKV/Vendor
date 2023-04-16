using Vendor.Services.Products.Domain.SeedWork;

namespace Vendor.Services.Products.Domain.AggregateModel.ProductAggregate;

public interface IProductRepository : IRepository
{
    public Task<Product?> FindProductByIdAsync(int id);
    public Task<List<Product>> FindProductsByMatchingNameAsync(string name);
    public Task<Product?> FindProductByExactNameAsync(string name);
    public void AddProduct(Product product);
}