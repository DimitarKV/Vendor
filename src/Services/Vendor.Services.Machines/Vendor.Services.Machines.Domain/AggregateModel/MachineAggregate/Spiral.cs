using Vendor.Services.Machines.Domain.Exceptions;
using Vendor.Services.Machines.Domain.SeedWork;

namespace Vendor.Services.Machines.Domain.AggregateModel.MachineAggregate;

public class Spiral : Entity
{
    public Vending Vending { get; private set; }
    public int ProductId { get; private set; }
    public Double Price { get; private set;  }
    public int Loads { get; private set; }

    public void SetVending(Vending vending)
    {
        Vending = vending;
    }

    public void SetProductId(int id)
    {
        ProductId = id;
    }

    public void SetPrice(Double price)
    {
        if (price < 0)
            throw new MachinesDomainException("Price cannot be negative!");
        Price = price;
    }

    public void SetLoads(int loads)
    {
        if (loads < 0)
            throw new MachinesDomainException("Loads cannot be negative!");
        Loads = loads;
    }

    public void DecrementLoads()
    {
        if (Loads <= 0)
            throw new MachinesDomainException("Spiral is empty!");
        Loads--;
    }

    public bool IsEmpty()
    {
        return Loads == 0;
    }

    public Spiral(int productId, double price, int loads)
    {
        ProductId = productId;
        Price = price;
        Loads = loads;
    }

    public Spiral(Vending vending, int productId, double price, int loads)
    {
        Vending = vending;
        ProductId = productId;
        SetPrice(price);
        SetLoads(loads);
    }

    public Spiral(Vending vending, int productId, double price)
    {
        Vending = vending;
        ProductId = productId;
        SetPrice(price);
        Loads = 0;
    }
}