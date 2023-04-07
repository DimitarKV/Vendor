using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Gateways.Portal.Wrappers.ResponseTypes;

namespace Vendor.Gateways.Portal.Services.Product;

public interface IProductService
{
    public Task<ClientResponse<ApiResponse<ProductView>>> GetProductByIdAsync(int id);
}