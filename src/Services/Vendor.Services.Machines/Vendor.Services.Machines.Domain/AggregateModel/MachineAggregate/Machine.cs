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
    public Decimal Money { get; private set; }

    public void AddMoney(Decimal value)
    {
        if (value < 0)
            throw new MachinesDomainException("Use the subtract method for this operation!");

        Money += value;
    }
    
    public void SubtractMoney(Decimal value)
    {
        if(value > Money)
            throw new MachinesDomainException("Not enough balance in the machine!");

        Money -= value;
    }
    
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

    public Machine(string title, double latitude, double longitude, string imageUrl)
    {
        Title = title;
        Latitude = latitude;
        Longitude = longitude;
        ImageUrl = imageUrl;
        Money = 0;
    }

    public Machine(string title, double latitude, double longitude)
    {
        Title = title;
        Latitude = latitude;
        Longitude = longitude;
        ImageUrl = Constants.Constants.DefaultVendingImageUrl;
        Money = 0;
    }
}