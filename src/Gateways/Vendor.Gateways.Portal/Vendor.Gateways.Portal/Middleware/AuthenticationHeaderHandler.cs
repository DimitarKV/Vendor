using System.Net.Http.Headers;
using Vendor.Gateways.Portal.Providers;

namespace Vendor.Gateways.Portal.Middleware;

public class AuthenticationHeaderHandler : DelegatingHandler
{
    private readonly TokenAuthenticationStateProvider _stateProvider;

    public AuthenticationHeaderHandler(TokenAuthenticationStateProvider stateProvider)
    {
        _stateProvider = stateProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        
        var token = await _stateProvider.GetTokenAsync();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await base.SendAsync(request, cancellationToken);
    }
}