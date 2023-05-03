namespace Vendor.Services.Machines.Domain.SeedWork;

public interface IRepository
{
    IUnitOfWork UnitOfWork { get; }
}