using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Gateways.Portal.DTO;

namespace Vendor.Gateways.Portal.Services.Product;

public interface IProductService
{
    public Task<ApiResponse<ProductView>> GetProductByIdAsync(int id);
    public Task<ApiResponse<ProductView>> RegisterProductAsync(RegisterProductDto dto);
    public Task<ApiResponse<List<ProductView>>> FetchAllProducts();
}