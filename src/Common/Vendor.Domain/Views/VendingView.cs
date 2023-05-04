namespace Vendor.Domain.Views;

public class VendingView
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public Double Latitude { get; set; }
    public Double Longitude { get; set; }
    public Double Money { get; set; }
    public string ImageUrl { get; set; } = "";
    public List<BanknoteView> Banknotes { get; set; } = new();
    public List<SpiralView> Spirals { get; set; } = new();

    public int SpiralsPerRow { get; set; }
}