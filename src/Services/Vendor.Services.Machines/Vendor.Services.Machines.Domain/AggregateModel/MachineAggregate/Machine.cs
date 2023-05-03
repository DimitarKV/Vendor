using Microsoft.IdentityModel.Tokens;
using Vendor.Services.Machines.Domain.Exceptions;
using Vendor.Services.Machines.Domain.SeedWork;

namespace Vendor.Services.Machines.Domain.AggregateModel.MachineAggregate;

public abstract class Machine : Entity, IAggregateRoot
{
    public string Title { get; private set; }
    public Double Latitude { get; private set; }
    public Double Longitude { get; private set; }
    public string ImageUrl { get; private set; }
    public Double Money { get; }
    public List<Banknote> Banknotes { get; }

    public void SetTitle(string title)
    {
        if (title.IsNullOrEmpty())
            throw new MachinesDomainException("Title cannot be empty!");
        Title = title.Trim();
    }
    
    public void SetLocation(Double latitude, Double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public void SetImageUrl(string imageUrl)
    {
        if (imageUrl.IsNullOrEmpty())
            throw new MachinesDomainException("Image url cannot be empty!");
        ImageUrl = imageUrl.Trim();
    }

    public Machine(string title, double latitude, double longitude, string imageUrl, List<Banknote> banknotes)
    {
        Title = title;
        Latitude = latitude;
        Longitude = longitude;
        ImageUrl = imageUrl;
        Banknotes = banknotes;

        Money = Banknotes.Aggregate(0.0, (sum, b) => sum + b.Value);
    }

    public Machine(string title, double latitude, double longitude, List<Banknote> banknotes)
    {
        Title = title;
        Latitude = latitude;
        Longitude = longitude;
        Banknotes = banknotes;

        Money = Banknotes.Aggregate(0.0, (sum, b) => sum + b.Value);
        ImageUrl = Constants.Constants.DefaultVendingImageUrl;
    }

    public Machine(string title, double latitude, double longitude)
    {
        Title = title;
        Latitude = latitude;
        Longitude = longitude;
        
        ImageUrl = Constants.Constants.DefaultVendingImageUrl;
        Banknotes = new List<Banknote>();
        Money = 0.0;
    }

    public Machine(string title, double latitude, double longitude, string imageUrl)
    {
        Title = title;
        Latitude = latitude;
        Longitude = longitude;
        ImageUrl = imageUrl;
        
        Banknotes = new List<Banknote>();
        Money = 0.0;
    }
}