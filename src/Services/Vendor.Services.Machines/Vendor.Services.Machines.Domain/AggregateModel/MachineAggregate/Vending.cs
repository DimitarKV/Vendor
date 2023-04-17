namespace Vendor.Services.Machines.AggregateModel.MachineAggregate;

public class Vending : Machine
{
    public List<Spiral> Spirals { get; }

    public void AddSpiral(Spiral spiral)
    {
        Spirals.Add(spiral);
    }

    public void AddSpirals(IEnumerable<Spiral> spirals)
    {
        Spirals.AddRange(spirals);
    }

    public Vending(string title, double latitude, double longitude, string imageUrl, List<Banknote> banknotes) : base(title, latitude, longitude, imageUrl, banknotes)
    {
        Spirals = new List<Spiral>();
    }

    public Vending(string title, double latitude, double longitude, List<Banknote> banknotes) : base(title, latitude, longitude, banknotes)
    {
        Spirals = new List<Spiral>();
    }

    public Vending(string title, double latitude, double longitude) : base(title, latitude, longitude)
    {
        Spirals = new List<Spiral>();
    }

    public Vending(string title, double latitude, double longitude, string imageUrl) : base(title, latitude, longitude, imageUrl)
    {
        Spirals = new List<Spiral>();
    }
}