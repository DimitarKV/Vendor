using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Gateways.Portal.DTO;
using Vendor.Gateways.Portal.Extensions;
using Vendor.Gateways.Portal.Providers;
using Vendor.Gateways.Portal.Static;
using Vendor.Gateways.Portal.Wrappers.HttpClientWrapper;

namespace Vendor.Gateways.Portal.Services.Product;

public class ProductService : IProductService
{
    private readonly IHttpClientWrapper _clientWrapper;
    private readonly HttpClient _client;
    private readonly TokenAuthenticationStateProvider _stateProvider;

    public ProductService(IHttpClientFactory httpClientFactory, IConfiguration configuration,
        TokenAuthenticationStateProvider stateProvider, ILogger<HttpClientWrapper> logger)
    {
        _stateProvider = stateProvider;
        _client = httpClientFactory.CreateClient(configuration["Services:Products:Client"]!);
        _clientWrapper = new HttpClientWrapper(_client, stateProvider, logger);
    }

    public async Task<ApiResponse<ProductView>> GetProductByIdAsync(QueryProductByIdDto dto)
    {
        var response = await _clientWrapper.SendAsJsonAsync<ProductView, int>(
            Endpoints.QueryProductById, 
            HttpMethod.Get, 
            dto.Id);

        return response;
    }

    public async Task<ApiResponse<ProductView>> RegisterProductAsync(RegisterProductDto dto)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, Endpoints.RegisterProductEndpoint);
        
        if ((await _stateProvider.GetAuthenticationStateAsync()).User.IsAuthenticated())
            request.Headers.Add("Authorization", "Bearer " + await _stateProvider.GetTokenAsync());

        using (var multipartFormContent = new MultipartFormDataContent())
        {
            var fileStreamContent = new StreamContent(dto.Image.OpenReadStream());
            fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(dto.Image.ContentType);
            multipartFormContent.Add(fileStreamContent, name: "image", fileName: "filename");

            var nameContent = new StringContent(dto.Name);
            multipartFormContent.Add(nameContent, "name");
            request.Content = multipartFormContent;

            var response = await _client.SendAsync(request);

            return (await response.Content.ReadFromJsonAsync<ApiResponse<ProductView>>())!;
        }
    }
}