using Vendor.Services.Machines.Data.Entities;

namespace Vendor.Services.Machines.Data.Repositories.Interfaces;

public interface IVendingRepository
{
    Task<Vending?> SaveVendingAsync(Vending vendor);
}