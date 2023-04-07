using System.ComponentModel.DataAnnotations;
using Vendor.Domain.Entities;

namespace Vendor.Services.Machines.Data.Entities;

public class Machine : Entity<int>
{
    public string Title { get; set; }
    public Double Latitude { get; set; }
    public Double Longitude { get; set; }
    public string ImageUrl { get; set; }
    public Double Money { get; set; }
    public List<Banknote> Banknotes { get; set; }

    public Machine()
    {
        Banknotes = new List<Banknote>();
    }
}