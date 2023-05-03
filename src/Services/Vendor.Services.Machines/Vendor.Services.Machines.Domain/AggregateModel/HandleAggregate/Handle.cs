using Vendor.Services.Machines.Domain.SeedWork;

namespace Vendor.Services.Machines.Domain.AggregateModel.HandleAggregate;

public class Handle : Entity, IAggregateRoot
{
    public string MaintainerId { get; private set; }
    public int MachineId { get; private  set; }
    public DateTime HandleExpiry { get; private set; }

    public Handle(string maintainerId, int machineId, DateTime handleExpiry)
    {
        MaintainerId = maintainerId;
        MachineId = machineId;
        HandleExpiry = handleExpiry;
    }
}