using Vendor.Domain.Entities;

namespace Vendor.Services.Machines.Data.Entities;

public class Product : Entity<int>
{
    public string Name { get; set; }
    public string ImageUrl { get; set; }
}