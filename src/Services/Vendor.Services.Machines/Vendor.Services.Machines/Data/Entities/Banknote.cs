using Vendor.Domain.Entities;

namespace Vendor.Services.Machines.Data.Entities;

public class Banknote : Entity<int>
{
    public string ValueInString { get; set; }
    public Double Value { get; set; }
}