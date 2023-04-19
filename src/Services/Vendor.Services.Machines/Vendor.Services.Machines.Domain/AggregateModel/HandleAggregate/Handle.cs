using Vendor.Services.Machines.SeedWork;

namespace Vendor.Services.Machines.AggregateModel.HandleAggregate;

public class Handle : Entity, IAggregateRoot
{
    public int MaintainerId { get; private set; }
    public DateTime HandleExpiry { get; private set; }

    public Handle(int maintainerId, DateTime handleExpiry)
    {
        MaintainerId = maintainerId;
        HandleExpiry = handleExpiry;
    }
}