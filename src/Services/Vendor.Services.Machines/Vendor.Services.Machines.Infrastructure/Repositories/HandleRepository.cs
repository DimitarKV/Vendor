using Microsoft.EntityFrameworkCore;
using Vendor.Services.Machines.Domain.AggregateModel.HandleAggregate;
using Vendor.Services.Machines.Domain.SeedWork;

namespace Vendor.Services.Machines.Infrastructure.Repositories;

public class HandleRepository : IHandleRepository
{
    private readonly MachineDbContext _context;

    public HandleRepository(MachineDbContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;

    public Handle HandleMachine(int machineId, string maintainerId, TimeSpan handleDuration)
    {
        var handle = new Handle(maintainerId, machineId, DateTime.Now.Add(handleDuration));
        return _context.Handles.Add(handle).Entity;
    }

    public async Task<bool> IsMachineNotHandled(int machineId)
    {
        return await _context.Handles.AnyAsync(h => h.MachineId == machineId && h.HandleExpiry > DateTime.Now);
    }
}