using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Vendor.Domain.DTO.Requests;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Gateways.Portal.DTO;
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

    //TODO: Sort by proximity to maintainer
    public async Task<ApiResponse<List<VendingView>>> FetchEmptyMachines()
    {
        var response = await _clientWrapper.SendAsJsonAsync<List<VendingView>>(
            Endpoints.QueryEmptyVendings,
            HttpMethod.Get);

        return response; 
    }

    public async Task<ApiResponse<List<VendingView>>> FetchNonEmptyMachines()
    {
        var response = await _clientWrapper.SendAsJsonAsync<List<VendingView>>(
            Endpoints.QueryNonEmptyVendings,
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

    public async Task<ApiResponse<HandleView>> HandleMachineAsync(int id)
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

    public async Task<ApiResponse<List<int>>> QueryMissingProductsAsync(int machineId)
    {
        var response =
            await _clientWrapper.SendAsJsonAsync<List<int>>(
                Endpoints.QueryMissingProductsEndpoint + "/" + machineId,
                HttpMethod.Get);
        return response;
    }

    public async Task<ApiResponse<SpiralView>> LoadSpiralAsync(SpiralView spiral)
    {
        var response =
            await _clientWrapper.SendAsJsonAsync<SpiralView, SpiralView>(
                Endpoints.LoadSpiralEndpoint,
                HttpMethod.Post,
                spiral);
        return response;
    }

    public async Task<ApiResponse<List<SpiralView>>> LoadSpiralsAsync(List<SpiralView> spirals)
    {
        var response =
            await _clientWrapper.SendAsJsonAsync<List<SpiralView>, List<SpiralView>>(
                Endpoints.LoadSpiralsEndpoint,
                HttpMethod.Post,
                spirals);
        return response;
    }

    public async Task<ApiResponse<List<ServiceRecordDto>>> FetchServiceRecords(int machineId)
    {
        var response =
            await _clientWrapper.SendAsJsonAsync<List<ServiceRecordDto>>(
                Endpoints.FetchServiceRecordsEndpoint + "/" + machineId,
                HttpMethod.Get);
        return response;
    }

    public async Task<ApiResponse<VendingView>> ExtractMoney(ExtractMoneyDto dto)
    {
        var response =
            await _clientWrapper.SendAsJsonAsync<VendingView, ExtractMoneyDto>(
                Endpoints.ExtractMoneyEndpoint,
                HttpMethod.Post,
                dto);
        return response;
    }
}