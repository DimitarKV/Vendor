using Vendor.Domain.Entities;

namespace Vendor.Services.Machines.Data.Entities;

public class Spiral : Entity<int>
{
    public Product Product { get; set; }
    public Double Price { get; set; }
    public int Loads { get; set; }
}