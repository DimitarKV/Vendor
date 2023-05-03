using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Vendor.Domain.DTO.Requests;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Gateways.Portal.Providers;
using Vendor.Gateways.Portal.Static;
using Vendor.Gateways.Portal.Wrappers.HttpClientWrapper;

namespace Vendor.Gateways.Portal.Services.Maintainer;

public class MaintainerService : IMaintainerService
{
    private readonly HttpClient _client;
    private readonly IHttpClientWrapper _clientWrapper;

    public MaintainerService(IHttpClientFactory httpClientFactory, IConfiguration configuration,
        TokenAuthenticationStateProvider stateProvider, ILogger<HttpClientWrapper> logger)
    {
        _client = httpClientFactory.CreateClient(configuration["Services:Machines:Client"]!);
        _clientWrapper = new HttpClientWrapper(_client, stateProvider, logger);
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
    public async Task<ApiResponse<List<VendingView>>> FetchEmptyMachines()
    {
        var response = await _clientWrapper.SendAsJsonAsync<List<VendingView>>(
            Endpoints.QueryEmptyVendings,
            HttpMethod.Get);

        return response; 
    }

    public async Task<ApiResponse<VendingView>> CreateMachineAsync(CreateVendingRequestDto requestDto)
    {
        var response = await _clientWrapper.SendAsJsonAsync<VendingView, CreateVendingRequestDto>(
            Endpoints.CreateMachine,
            HttpMethod.Post,
            requestDto);

        return response;
    }

    public async Task<ApiResponse<VendingView>> SetMachineImageAsync(SetMachineImageDto requestDto)
    {
        var endpoint = Endpoints.SetMachineImageEndpoint + "/" + requestDto.MachineId;

        var response = await _clientWrapper.PostImageAsync<VendingView>(endpoint, HttpMethod.Post, requestDto.Image);

        return response;
    }

    public async Task<ApiResponse<HandleView>> HandleMachine(int id)
    {
        var response =
            await _clientWrapper.SendAsJsonAsync<HandleView>(
                Endpoints.HandleVending,
                HttpMethod.Get);
        return response;
    }

    public async Task<ApiResponse<VendingView>> FetchMachineById(int machineId)
    {
        var response =
            await _clientWrapper.SendAsJsonAsync<VendingView>(
                Endpoints.FetchMachineById + "/" + machineId,
                HttpMethod.Get);
        return response;
    }

    public async Task<ApiResponse<List<int>>> QueryMissingProducts(int machineId)
    {
        var response =
            await _clientWrapper.SendAsJsonAsync<List<int>>(
                Endpoints.QueryMissingProductsEndpoint + "/" + machineId,
                HttpMethod.Get);
        return response;
    }
}