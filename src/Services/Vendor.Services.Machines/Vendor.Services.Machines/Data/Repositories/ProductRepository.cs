using Newtonsoft.Json;
using Vendor.Domain.Entities;
using Vendor.Domain.Types;
using Vendor.Services.Machines.Data.Repositories.Interfaces;

namespace Vendor.Services.Machines.Data.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly HttpClient _httpClient;

    public ProductRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResponse<Product>> GetProductAsync(int id)
    {
        var responseString = await _httpClient.GetStringAsync("products/QueryProduct?id=" + id);

        var response = JsonConvert.DeserializeObject<ApiResponse<Product>>(responseString)!;
        return response;
    }
}