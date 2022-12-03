namespace Vendor.Services.Machines.Data.Entities;

public class Vending : Machine
{
    public List<Tray> Products { get; set; }
    
    public Vending()
    {
        Products = new List<Tray>();
    }
}