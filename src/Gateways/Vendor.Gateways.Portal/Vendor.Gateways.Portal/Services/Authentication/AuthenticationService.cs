using Microsoft.Extensions.Configuration;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Gateways.Portal.DTO;
using Vendor.Gateways.Portal.Static;
using Vendor.Gateways.Portal.Wrappers.HttpClientWrapper;

namespace Vendor.Gateways.Portal.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _client;
    private readonly IHttpClientWrapper _clientWrapper;

    public AuthenticationService(IHttpClientFactory factory, IConfiguration configuration,
        IHttpClientWrapper clientWrapper)
    {
        _clientWrapper = clientWrapper;
        _client = factory.CreateClient(configuration["Services:Users:Client"]!);
    }

    //Add user view to common
    public async Task<ApiResponse<UserView>> RegisterAsync(RegisterUserFormData createUserForm)
    {
        var response =
            await _clientWrapper.SendAsJsonAsync<UserView, RegisterUserFormData>(_client,
                Endpoints.RegisterUser, HttpMethod.Post, createUserForm);
        return response;
    }

    public async Task<ApiResponse<string>> LoginAsync(LoginUserFormData loginForm)
    {
        var response =
            await _clientWrapper.SendAsJsonAsync<string, LoginUserFormData>(_client, Endpoints.Login,
                HttpMethod.Post, loginForm);

        return response;
    }

    public async Task<ApiResponse> ConfirmEmailAsync(ConfirmEmailDto dto)
    {
        var response =
            await _clientWrapper.SendAsJsonAsync<ApiResponse, ConfirmEmailDto>(_client, Endpoints.EmailConfirmation,
                HttpMethod.Get, dto);

        return response;
    }
}