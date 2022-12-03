using Vendor.Domain.Entities;

namespace Vendor.Services.Machines.Data.Entities;

public class Tray : Entity<int>
{
    public List<Spiral> Spirals { get; set; }

    public Tray()
    {
        Spirals = new List<Spiral>();
    }
}