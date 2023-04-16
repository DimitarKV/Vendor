using Vendor.Services.Products.Domain.SeedWork;

namespace Vendor.Services.Products.Domain.AggregateModel.ProductAggregate;

public class Product : Entity, IAggregateRoot
{
    public string Name { get; private set; }
    public string ImageUrl { get; private set; }

    public Product(string name, string imageUrl)
    {
        Name = name;
        ImageUrl = imageUrl;
    }
}