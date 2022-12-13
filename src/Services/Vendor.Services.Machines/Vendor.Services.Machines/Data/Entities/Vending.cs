using Vendor.Domain.Entities;

namespace Vendor.Services.Machines.Data.Entities;

public class Vending : Machine
{
    public List<Spiral> Spirals { get; set; }
    
    public Vending()
    {
        Spirals = new List<Spiral>();
    }
}