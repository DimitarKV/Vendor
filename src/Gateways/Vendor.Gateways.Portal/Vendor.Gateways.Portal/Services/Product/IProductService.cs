using Vendor.Domain.Types;
using Vendor.Domain.Views;

namespace Vendor.Gateways.Portal.Services.Product;

public interface IProductService
{
    public Task<ApiResponse<ProductView>> GetProductByIdAsync(int id);
}