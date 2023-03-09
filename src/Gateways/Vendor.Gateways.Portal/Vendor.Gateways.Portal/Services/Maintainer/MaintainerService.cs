using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Gateways.Portal.Static;

namespace Vendor.Gateways.Portal.Services.Maintainer;

public class MaintainerService : IMaintainerService
{
    private readonly HttpClient _client;

    public MaintainerService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _client = httpClientFactory.CreateClient(configuration["Services:Machines:Client"]!);
    }
    
    // JS code for Haversine formula
    // const R = 6371e3; // metres
    // const φ1 = lat1 * Math.PI/180; // φ, λ in radians
    // const φ2 = lat2 * Math.PI/180;
    // const Δφ = (lat2-lat1) * Math.PI/180;
    // const Δλ = (lon2-lon1) * Math.PI/180;
    //
    // const a = Math.sin(Δφ/2) * Math.sin(Δφ/2) +
    // Math.cos(φ1) * Math.cos(φ2) *
    // Math.sin(Δλ/2) * Math.sin(Δλ/2);
    // const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1-a));
    //
    // const d = R * c; // in metres
    
    //TODO: Sort by proximity to maintainer
    public async Task<List<VendingView>> FetchEmptyMachines()
    {
        var result = await _client.GetAsync(Endpoints.QueryEmptyVendings);
        return (await result.Content.ReadFromJsonAsync<ApiResponse<List<VendingView>>>())!.Result;
    }

    public Task HandleMachine(string title)
    {
        throw new NotImplementedException();
    }
}