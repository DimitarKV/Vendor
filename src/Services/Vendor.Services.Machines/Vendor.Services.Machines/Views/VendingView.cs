using Vendor.Domain.Entities;
using Vendor.Services.Machines.Data.Entities;

namespace Vendor.Services.Machines.Views;

public class VendingView
{
    public string Title { get; set; }
    public Double Latitude { get; set; }
    public Double Longitude { get; set; }
    public Double Money { get; set; }
    public List<Banknote> Banknotes { get; set; }
    public List<ProductView> Products { get; set; }
}