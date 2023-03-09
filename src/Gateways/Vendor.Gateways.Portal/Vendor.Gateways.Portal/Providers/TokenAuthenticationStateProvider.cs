using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Vendor.Gateways.Portal.Extensions;

namespace Vendor.Gateways.Portal.Providers;

public class TokenAuthenticationStateProvider : AuthenticationStateProvider
{
    private const string StorageKey = "authToken";
    private readonly ProtectedSessionStorage _sessionStorage;

    public TokenAuthenticationStateProvider(ProtectedSessionStorage sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }
    
    public async Task SetAuthenticationStateAsync(string? token)
    {
        if (token is null)
        {
            await _sessionStorage.DeleteAsync(StorageKey);
        }
        else
        {
            await _sessionStorage.SetAsync(StorageKey, token);
        }
        
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await GetTokenAsync();
        var identity = string.IsNullOrEmpty(token)
            ? new ClaimsIdentity()
            : new ClaimsIdentity(AuthenticationExtensions.ParseClaimsFromJwt(token), "jwt");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }
    
    public async Task<string?> GetTokenAsync()
    {
        var result = await _sessionStorage.GetAsync<string>(StorageKey);
        return result.Value;
    }
}