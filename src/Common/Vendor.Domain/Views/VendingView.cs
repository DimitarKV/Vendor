using Vendor.Services.Machines.Views;

namespace Vendor.Domain.Views;

public class VendingView
{
    public string Title { get; set; }
    public Double Latitude { get; set; }
    public Double Longitude { get; set; }
    public Double Money { get; set; }
    public List<BanknoteView> Banknotes { get; set; }
    public List<ProductView> Products { get; set; }
}