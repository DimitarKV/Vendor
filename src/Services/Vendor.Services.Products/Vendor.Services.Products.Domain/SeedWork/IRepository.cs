namespace Vendor.Services.Products.Domain.SeedWork;

public interface IRepository
{
    IUnitOfWork UnitOfWork { get; }
}