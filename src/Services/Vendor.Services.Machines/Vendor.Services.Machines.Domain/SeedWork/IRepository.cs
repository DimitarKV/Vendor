namespace Vendor.Services.Machines.SeedWork;

public interface IRepository
{
    IUnitOfWork UnitOfWork { get; }
}