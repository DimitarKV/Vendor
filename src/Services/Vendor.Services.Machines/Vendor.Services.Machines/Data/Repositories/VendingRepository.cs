using Vendor.Services.Machines.Data.Entities;
using Vendor.Services.Machines.Data.Persistence.Interface;
using Vendor.Services.Machines.Data.Repositories.Interfaces;

namespace Vendor.Services.Machines.Data.Repositories;

public class VendingRepository : IVendingRepository
{
    private readonly IMachineDbContext _context;

    public VendingRepository(IMachineDbContext context)
    {
        _context = context;
    }

    public async Task<Vending?> SaveVendingAsync(Vending vendor)
    {
        var result = _context.Vendings.Add(vendor);
        await _context.SaveChangesAsync();
        return result.Entity;
    }
}