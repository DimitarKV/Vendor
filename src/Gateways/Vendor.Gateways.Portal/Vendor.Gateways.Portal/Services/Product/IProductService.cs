using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Gateways.Portal.DTO;

namespace Vendor.Gateways.Portal.Services.Product;

public interface IProductService
{
    public Task<ApiResponse<ProductView>> GetProductByIdAsync(QueryProductByIdDto dto);
    public Task<ApiResponse<ProductView>> RegisterProductAsync(RegisterProductDto dto);
}