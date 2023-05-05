using Vendor.Services.Machines.Domain.SeedWork;

namespace Vendor.Services.Machines.Domain.AggregateModel.MachineAggregate;

public class Servicing : Entity
{
    public int MachineId { get; private set; }
    public string MaintainerUserName { get; private set; }
    public DateTime Time { get; private set; }
    public string Filled { get; private set; }

    public Servicing(int machineId, string maintainerUserName, DateTime time, string filled)
    {
        MachineId = machineId;
        MaintainerUserName = maintainerUserName;
        Time = time;
        Filled = filled;
    }
}