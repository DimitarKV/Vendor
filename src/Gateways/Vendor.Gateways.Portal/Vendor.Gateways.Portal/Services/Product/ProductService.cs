using Microsoft.Extensions.Configuration;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Gateways.Portal.Static;
using Vendor.Gateways.Portal.Wrappers.HttpClientWrapper;

namespace Vendor.Gateways.Portal.Services.Product;

public class ProductService : IProductService
{
    private readonly IHttpClientWrapper _clientWrapper;
    private readonly  HttpClient _client;

    public ProductService(IHttpClientFactory httpClientFactory, IHttpClientWrapper clientWrapper, IConfiguration configuration)
    {
        _clientWrapper = clientWrapper;
        _client = httpClientFactory.CreateClient(configuration["Services:Products:Client"]!);
    }

    public async Task<ApiResponse<ProductView>> GetProductByIdAsync(int id)
    {
        var response = await _clientWrapper.SendAsJsonAsync<ProductView, int>(_client,
            Endpoints.QueryProductById, HttpMethod.Get, id);

        return response;
    }
}