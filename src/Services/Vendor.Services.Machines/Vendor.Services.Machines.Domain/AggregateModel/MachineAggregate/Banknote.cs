using System.Globalization;
using Vendor.Services.Machines.Exceptions;
using Vendor.Services.Machines.SeedWork;

namespace Vendor.Services.Machines.AggregateModel.MachineAggregate;

public class Banknote : Entity
{
    public string ValueInString { get; private set; }
    public Double Value { get; private set; }

    public Banknote(Double value)
    {
        if (!BanknoteValues.Values.Contains(value))
            throw new MachinesDomainException("No such banknote exists!");
        Value = value;
        ValueInString = value.ToString("F", CultureInfo.InvariantCulture);
    }
}