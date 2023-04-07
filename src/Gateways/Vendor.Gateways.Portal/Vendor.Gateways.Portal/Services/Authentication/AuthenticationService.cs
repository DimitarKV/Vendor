using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Gateways.Portal.DTO;
using Vendor.Gateways.Portal.Static;
using Vendor.Gateways.Portal.Wrappers.HttpClientWrapper;
using Vendor.Gateways.Portal.Wrappers.ResponseTypes;

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
    public async Task<ClientResponse<ApiResponse<UserView>>> RegisterAsync(RegisterUserFormData createUserForm)
    {
        var response =
            await _clientWrapper.SendAsJsonAsync<ApiResponse<UserView>, RegisterUserFormData>(_client,
                Endpoints.RegisterUser, HttpMethod.Post, createUserForm);
        return response;
    }

    public async Task<ClientResponse<ApiResponse<string>>> LoginAsync(LoginUserFormData loginForm)
    {
        var response =
            await _clientWrapper.SendAsJsonAsync<ApiResponse<string>, LoginUserFormData>(_client, Endpoints.Login,
                HttpMethod.Post, loginForm);

        return response;
    }

    public async Task<ClientResponse<ApiResponse>> ConfirmEmailAsync(ConfirmEmailDto dto)
    {
        var response =
            await _clientWrapper.SendAsJsonAsync<ApiResponse, ConfirmEmailDto>(_client, Endpoints.EmailConfirmation,
                HttpMethod.Get, dto);

        return response;
    }
}