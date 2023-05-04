using Vendor.Services.Machines.Domain.Exceptions;

namespace Vendor.Services.Machines.Domain.AggregateModel.MachineAggregate;

public class Vending : Machine
{
    public List<Spiral> Spirals { get; }
    public int SpiralsPerRow { get; private set; }

    public void AddSpiral(Spiral spiral)
    {
        Spirals.Add(spiral);
    }

    public void AddSpirals(IEnumerable<Spiral> spirals)
    {
        Spirals.AddRange(spirals);
    }

    public void SetSpiralsPerRow(int n)
    {
        if (n <= 0)
            throw new MachinesDomainException("Spirals per row must be at least 1!");
        SpiralsPerRow = n;
    }


    public Vending(string title, double latitude, double longitude, string imageUrl, List<Banknote> banknotes) : base(
        title, latitude, longitude, imageUrl, banknotes)
    {
        Spirals = new List<Spiral>();
        SpiralsPerRow = 1;
    }

    public Vending(string title, double latitude, double longitude, List<Banknote> banknotes) : base(title, latitude,
        longitude, banknotes)
    {
        Spirals = new List<Spiral>();
        SpiralsPerRow = 1;
    }

    public Vending(string title, double latitude, double longitude) : base(title, latitude, longitude)
    {
        Spirals = new List<Spiral>();
        SpiralsPerRow = 1;
    }

    public Vending(string title, double latitude, double longitude, string imageUrl) : base(title, latitude, longitude,
        imageUrl)
    {
        Spirals = new List<Spiral>();
        SpiralsPerRow = 1;
    }
}