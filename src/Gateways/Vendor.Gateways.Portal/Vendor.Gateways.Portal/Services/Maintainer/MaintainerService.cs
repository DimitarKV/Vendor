using Microsoft.Extensions.Configuration;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Gateways.Portal.Providers;
using Vendor.Gateways.Portal.Static;
using Vendor.Gateways.Portal.Wrappers.HttpClientWrapper;
using Vendor.Gateways.Portal.Wrappers.ResponseTypes;

namespace Vendor.Gateways.Portal.Services.Maintainer;

public class MaintainerService : IMaintainerService
{
    private readonly HttpClient _client;
    private readonly IHttpClientWrapper _clientWrapper;

    public MaintainerService(IHttpClientFactory httpClientFactory, IConfiguration configuration,
        TokenAuthenticationStateProvider stateProvider, IHttpClientWrapper clientWrapper)
    {
        _client = httpClientFactory.CreateClient(configuration["Services:Machines:Client"]!);
        _clientWrapper = clientWrapper;
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
    public async Task<ClientResponse<ApiResponse<List<VendingView>>>> FetchEmptyMachines()
    {
        var response = await _clientWrapper.SendAsJsonAsync<ApiResponse<List<VendingView>>>(_client,
            Endpoints.QueryEmptyVendings,
            HttpMethod.Get);

        return response; 
    }

    public async Task<ClientResponse<ApiResponse<HandleView>>> HandleMachine(int id)
    {
        var response =
            await _clientWrapper.SendAsJsonAsync<ApiResponse<HandleView>>(_client, Endpoints.HandleVending,
                HttpMethod.Get);
        return response;
    }
}