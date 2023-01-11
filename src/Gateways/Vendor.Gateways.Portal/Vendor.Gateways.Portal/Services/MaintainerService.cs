using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Gateways.Portal.Static;

namespace Vendor.Gateways.Portal.Services;

public class MaintainerService : IMaintainerService
{
    private readonly HttpClient _client;

    public MaintainerService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _client = httpClientFactory.CreateClient(configuration["Services:Machines:Client"]!);
    }
    
    public async Task<List<VendingView>> FetchEmptyMachines()
    {
        var result = await _client.GetAsync(Endpoints.QueryEmptyVendings);
        return (await result.Content.ReadFromJsonAsync<ApiResponse<List<VendingView>>>())!.Result;
    }
}