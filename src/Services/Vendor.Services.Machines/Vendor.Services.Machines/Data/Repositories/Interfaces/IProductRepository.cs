using Vendor.Domain.Entities;
using Vendor.Domain.Types;

namespace Vendor.Services.Machines.Data.Repositories.Interfaces;

public interface IProductRepository
{
    public Task<ApiResponse<Product>> GetProductAsync(int id);
}