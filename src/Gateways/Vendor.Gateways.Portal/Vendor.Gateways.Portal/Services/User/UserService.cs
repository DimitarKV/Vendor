using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Vendor.Domain.DTO.Requests;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Gateways.Portal.Providers;
using Vendor.Gateways.Portal.Static;
using Vendor.Gateways.Portal.Wrappers.HttpClientWrapper;

namespace Vendor.Gateways.Portal.Services.User;

public class UserService : IUserService
{
    private readonly IHttpClientWrapper _clientWrapper;

    public UserService(IHttpClientFactory httpClientFactory, IConfiguration configuration,
        TokenAuthenticationStateProvider stateProvider, ILogger<HttpClientWrapper> logger)
    {
        _clientWrapper = new HttpClientWrapper(httpClientFactory.CreateClient(configuration["Services:Users:Client"]!),
            stateProvider, logger);
    }

    public async Task<ApiResponse<List<UserView>>> QueryUsersAsync(QueryUsersRequestDto requestDto)
    {
        return await _clientWrapper.SendAsJsonAsync<List<UserView>, QueryUsersRequestDto>(
            Endpoints.QueryUsers,
            HttpMethod.Get,
            requestDto
        );
    }

    public async Task<ApiResponse<List<string>>> QueryAvailableRolesAsync()
    {
        return await _clientWrapper.SendAsJsonAsync<List<string>>(
            Endpoints.QueryAvailableRoles,
            HttpMethod.Get);
    }
}