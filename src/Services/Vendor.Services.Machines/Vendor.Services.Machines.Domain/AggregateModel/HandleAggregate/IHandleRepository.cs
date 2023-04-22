using Vendor.Services.Machines.SeedWork;

namespace Vendor.Services.Machines.AggregateModel.HandleAggregate;

public interface IHandleRepository : IRepository
{
    public Handle HandleMachine(int machineId, string maintainerId, TimeSpan handleDuration);
    public Task<bool> IsMachineNotHandled(int machineId);
}