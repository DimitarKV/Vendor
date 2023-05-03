using Vendor.Services.Machines.Domain.SeedWork;

namespace Vendor.Services.Machines.Domain.AggregateModel.HandleAggregate;

public interface IHandleRepository : IRepository
{
    public Handle HandleMachine(int machineId, string maintainerId, TimeSpan handleDuration);
    public Task<bool> IsMachineNotHandled(int machineId);
}