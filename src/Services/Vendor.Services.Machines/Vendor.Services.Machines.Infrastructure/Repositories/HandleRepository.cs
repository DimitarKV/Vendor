using Microsoft.EntityFrameworkCore;
using Vendor.Services.Machines.AggregateModel.HandleAggregate;
using Vendor.Services.Machines.SeedWork;

namespace Vendor.Services.Machines.Infrastructure.Repositories;

public class HandleRepository : IHandleRepository
{
    private readonly MachineDbContext _context;

    public HandleRepository(MachineDbContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;
    
    public Handle HandleMachine(int machineId, TimeSpan handleDuration)
    {
        var handle = new Handle(machineId, DateTime.Now.Add(handleDuration));
        return _context.Handles.Add(handle).Entity;
    }

    public async Task<bool> IsMachineNotHandled(int machineId)
    {
        return await _context.Handles.AnyAsync(h => h.MaintainerId == machineId && h.HandleExpiry > DateTime.Now);
    }
}