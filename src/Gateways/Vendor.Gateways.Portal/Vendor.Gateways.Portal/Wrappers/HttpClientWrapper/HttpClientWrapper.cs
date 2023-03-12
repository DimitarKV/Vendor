using System.Net.Http.Json;
using Vendor.Domain.Types;
using Vendor.Gateways.Portal.Extensions;
using Vendor.Gateways.Portal.Providers;
using Vendor.Gateways.Portal.Wrappers.ResponseTypes;

namespace Vendor.Gateways.Portal.Wrappers.HttpClientWrapper;

public class HttpClientWrapper : IHttpClientWrapper
{
    private readonly TokenAuthenticationStateProvider _stateProvider;

    public HttpClientWrapper(TokenAuthenticationStateProvider stateProvider)
    {
        _stateProvider = stateProvider;
    }

    public async Task<ClientResponse<R>> SendAsJsonAsync<R, S>(HttpClient client, string uri, HttpMethod method,
        S? body,
        IEnumerable<KeyValuePair<string, string>>? headers)
    {
        var request = new HttpRequestMessage(method, uri);

        if (body is not null)
            request.Content = JsonContent.Create(body);

        if ((await _stateProvider.GetAuthenticationStateAsync()).User.IsAuthenticated())
            request.Headers.Add("Authorization", "Bearer " + await _stateProvider.GetTokenAsync());

        if (headers is not null)
            foreach (var header in headers)
                request.Headers.Add(header.Key, header.Value);

        var result = await client.SendAsync(request);

        return await ReadResultAsync<R>(result);
    }

    public async Task<ClientResponse<R>> SendAsJsonAsync<R>(HttpClient client, string uri, HttpMethod method,
        IEnumerable<KeyValuePair<string, string>>? headers)
    {
        var request = new HttpRequestMessage(method, uri);


        if ((await _stateProvider.GetAuthenticationStateAsync()).User.IsAuthenticated())
            request.Headers.Add("Authorization", "Bearer " + await _stateProvider.GetTokenAsync());

        if (headers is not null)
            foreach (var header in headers)
                request.Headers.Add(header.Key, header.Value);

        var result = await client.SendAsync(request);

        return await ReadResultAsync<R>(result);
    }

    public async Task<ClientResponse<R>> SendAsJsonAsync<R>(HttpClient client, string uri, HttpMethod method)
    {
        var request = new HttpRequestMessage(method, uri);

        if ((await _stateProvider.GetAuthenticationStateAsync()).User.IsAuthenticated())
            request.Headers.Add("Authorization", "Bearer " + await _stateProvider.GetTokenAsync());

        var result = await client.SendAsync(request);

        return await ReadResultAsync<R>(result);
    }

    private async Task<ClientResponse<R>> ReadResultAsync<R>(HttpResponseMessage response)
    {
        var clientResponse = new ClientResponse<R>()
        {
            StatusCode = response.StatusCode,
            IsSuccessful = response.IsSuccessStatusCode,
            ReasonPhrase = response.ReasonPhrase
        };
        
        try
        {
            clientResponse.Result = (await response.Content.ReadFromJsonAsync<R>())!;
        }
        catch (Exception e)
        {
            clientResponse.Result = default;
        }

        return clientResponse;
    }
}