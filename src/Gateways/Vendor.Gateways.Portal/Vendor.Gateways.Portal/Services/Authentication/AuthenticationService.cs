using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Vendor.Domain.DTO.Requests;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Gateways.Portal.Models;
using Vendor.Gateways.Portal.Providers;
using Vendor.Gateways.Portal.Static;
using Vendor.Gateways.Portal.Wrappers.HttpClientWrapper;

namespace Vendor.Gateways.Portal.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IHttpClientWrapper _clientWrapper;

    public AuthenticationService(IHttpClientFactory factory, IConfiguration configuration,
        TokenAuthenticationStateProvider stateProvider, ILogger<HttpClientWrapper> logger)
    {
        _clientWrapper = new HttpClientWrapper(factory.CreateClient(configuration["Services:Users:Client"]!), stateProvider, logger);
    }

    
    public async Task<ApiResponse<UserView>> RegisterAsync(RegisterUserModel createUserForm)
    {
        var response =
            await _clientWrapper.SendAsJsonAsync<UserView, RegisterUserModel>(
                Endpoints.RegisterUser, 
                HttpMethod.Post, 
                createUserForm);
        return response;
    }

    public async Task<ApiResponse<string>> LoginAsync(LoginUserModel loginForm)
    {
        var response =
            await _clientWrapper.SendAsJsonAsync<string, LoginUserModel>(
                Endpoints.Login,
                HttpMethod.Post, 
                loginForm);

        return response;
    }

    public async Task<ApiResponse> ConfirmEmailAsync(ConfirmEmailRequestDto requestDto)
    {
        var response =
            await _clientWrapper.SendAsJsonAsync<ApiResponse, ConfirmEmailRequestDto>(
                Endpoints.EmailConfirmation,
                HttpMethod.Get, requestDto);

        return response;
    }
}