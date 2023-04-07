namespace Vendor.Domain.Views;

public class VendingView
{
    public string Title { get; set; } = "";
    public Double Latitude { get; set; }
    public Double Longitude { get; set; }
    public Double Money { get; set; }
    public string Image { get; set; } = "";
    public List<BanknoteView> Banknotes { get; set; } = new();
    public List<SpiralView> Spirals { get; set; } = new();
}