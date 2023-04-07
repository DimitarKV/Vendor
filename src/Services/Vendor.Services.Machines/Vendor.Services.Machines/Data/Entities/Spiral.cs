using Vendor.Domain.Entities;

namespace Vendor.Services.Machines.Data.Entities;

public class Spiral : Entity<int>
{
    public Vending Vending { get; set; }
    public string Name { get; set; }
    public int ProductId { get; set; }
    public Double Price { get; set; }
    public int Loads { get; set; }

    public Spiral()
    {
        Price = 0.0;
        Loads = 0;
    }
}