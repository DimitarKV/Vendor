using System.Net.Http.Json;
using Vendor.Domain.Types;
using Vendor.Gateways.Portal.Extensions;
using Vendor.Gateways.Portal.Providers;

namespace Vendor.Gateways.Portal.Wrappers.HttpClientWrapper;

public class HttpClientWrapper : IHttpClientWrapper
{
    private readonly TokenAuthenticationStateProvider _stateProvider;

    public HttpClientWrapper(TokenAuthenticationStateProvider stateProvider)
    {
        _stateProvider = stateProvider;
    }

    public async Task<ApiResponse<R>> SendAsJsonAsync<R, S>(HttpClient client, string uri, HttpMethod method, S? body,
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

    public async Task<ApiResponse<R>> SendAsJsonAsync<R>(HttpClient client, string uri, HttpMethod method,
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

    public async Task<ApiResponse<R>> SendAsJsonAsync<R>(HttpClient client, string uri, HttpMethod method)
    {
        var request = new HttpRequestMessage(method, uri);

        if ((await _stateProvider.GetAuthenticationStateAsync()).User.IsAuthenticated())
            request.Headers.Add("Authorization", "Bearer " + await _stateProvider.GetTokenAsync());

        var result = await client.SendAsync(request);

        return await ReadResultAsync<R>(result);
    }

    private async Task<ApiResponse<R>> ReadResultAsync<R>(HttpResponseMessage response)
    {
        try
        {
            return new ApiResponse<R>()
            {
                Result = (await response.Content.ReadFromJsonAsync<R>())!,
                Message = response.StatusCode.ToString()
            };
        }
        catch (Exception e)
        {
            return new ApiResponse<R>()
            {
                Result = default!,
                Errors = new List<string>() {response.ReasonPhrase!},
                Message = response.StatusCode.ToString()
            };
        }
    }
}