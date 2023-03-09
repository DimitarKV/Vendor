using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Gateways.Portal.DTO;
using Vendor.Gateways.Portal.Static;

namespace Vendor.Gateways.Portal.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _client;

    public AuthenticationService(IHttpClientFactory factory, IConfiguration configuration)
    {
        _client = factory.CreateClient(configuration["Services:Users:Client"]!);
    }

    //Add user view to common
    public async Task<ApiResponse<UserView>> RegisterAsync(RegisterUserFormData createUserForm)
    {
        var content = new StringContent(JsonConvert.SerializeObject(createUserForm), Encoding.UTF8, "application/json");
        var result = await _client.PostAsync(Endpoints.RegisterUser, content);
        var stringContent = await result.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<ApiResponse<UserView>>(await result.Content.ReadAsStringAsync());
        
    }

    public async Task<ApiResponse<string>> LoginAsync(LoginUserFormData loginForm)
    {
        var content = new StringContent(JsonConvert.SerializeObject(loginForm), Encoding.UTF8, "application/json");
        var result = await _client.PostAsync(Endpoints.Login, content);
        var stringResult = await result.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<ApiResponse<string>>(stringResult);
    }
}