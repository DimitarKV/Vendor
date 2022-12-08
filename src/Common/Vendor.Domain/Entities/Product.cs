namespace Vendor.Domain.Entities;

public class Product : Entity<int>
{
    public string Name { get; set; }
    public string ImageUrl { get; set; }
}