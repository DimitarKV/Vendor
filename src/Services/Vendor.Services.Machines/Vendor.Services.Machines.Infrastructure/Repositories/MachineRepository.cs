using Microsoft.EntityFrameworkCore;
using Vendor.Domain.Views;
using Vendor.Services.Machines.Domain.AggregateModel.MachineAggregate;
using Vendor.Services.Machines.Domain.Exceptions;
using Vendor.Services.Machines.Domain.SeedWork;

namespace Vendor.Services.Machines.Infrastructure.Repositories;

public class MachineRepository : IMachineRepository
{
    private MachineDbContext _context;

    public MachineRepository(MachineDbContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;
    
    public Vending CreateVending(string title, int spiralCount, double latitude, double longitude)
    {
        var vending = new Vending(title, latitude, longitude);

        for (int j = 0; j < spiralCount; j++)
        {
            var spiral = new Spiral(vending, -1, 0.0);
            vending.Spirals.Add(spiral);
        }

        return _context.Vendings.Add(vending).Entity;
    }

    public async Task LoadSpiralAsync(int spiralId, int productId, int loads, double price)
    {
        var spiral = await _context.Spirals.SingleOrDefaultAsync(s => s.Id == spiralId);
        if (spiral is null)
            throw new MachinesDomainException("Spiral with id " + spiralId + " does not exist!");
        
        spiral.SetProductId(productId);
        spiral.SetLoads(loads);
        spiral.SetPrice(price);
    }

    public async Task DropProductAsync(int spiralId)
    {
        var spiral = await _context.Spirals.SingleOrDefaultAsync(s => s.Id == spiralId);
        if (spiral is null)
            throw new MachinesDomainException("Spiral with id " + spiralId + " does not exist!");

        spiral.DecrementLoads();
    }

    public async Task<List<Vending>> GetEmptyVendingsAsync()
    {
        var emptyVendings = await _context.Vendings.Include(v => v.Spirals)
            .Where(v => v.Spirals.Any(s => s.Loads == 0))
            .ToListAsync();
        return emptyVendings;
    }

    public async Task<Vending?> GetVendingByIdAsync(int id)
    {
        var vending = await _context.Vendings.SingleOrDefaultAsync(v => v.Id == id);
        return vending;
    }
    
    public async Task<Vending?> GetVendingAndSpiralsByIdAsync(int id)
    {
        var vending = await _context.Vendings.Include(v => v.Spirals).SingleOrDefaultAsync(v => v.Id == id);
        return vending;
    }

    public async Task<Spiral?> GetSpiralByIdAsync(int id)
    {
        var spiral = await _context.Spirals.SingleOrDefaultAsync(s => s.Id == id);
        return spiral;
    }

    public async Task<Spiral?> GetSpiralAndVendingByIdAsync(int id)
    {
        var spiral = await _context.Spirals.Include(s => s.Vending).SingleOrDefaultAsync(s => s.Id == id);
        return spiral;
    }

    public async Task<Vending?> SetVendingImageUrl(int machineId, string url)
    {
        var machine = await _context.Vendings.SingleOrDefaultAsync(v => v.Id == machineId);
        if (machine is null)
            return null;

        machine.SetImageUrl(url);
        return machine;
    }
}