using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Vendor.Domain.Types;
using Vendor.Gateways.Portal.Extensions;
using Vendor.Gateways.Portal.Providers;

namespace Vendor.Gateways.Portal.Wrappers.HttpClientWrapper;

public class HttpClientWrapper : IHttpClientWrapper
{
    private readonly TokenAuthenticationStateProvider _stateProvider;
    private HttpClient _client;
    private readonly ILogger<HttpClientWrapper> _logger;

    public HttpClientWrapper(HttpClient client, TokenAuthenticationStateProvider stateProvider,
        ILogger<HttpClientWrapper> logger)
    {
        _stateProvider = stateProvider;
        _logger = logger;
        _client = client;
    }

    public async Task<ApiResponse<R>> SendAsJsonAsync<R, S>(string uri, HttpMethod method,
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

        var result = await TrySendAsync(request);

        return await ReadResultAsync<R>(result);
    }

    public async Task<ApiResponse<R>> SendAsJsonAsync<R, S>(string uri, HttpMethod method,
        S? body)
    {
        var request = new HttpRequestMessage(method, uri);

        if (body is not null)
            request.Content = JsonContent.Create(body);

        if ((await _stateProvider.GetAuthenticationStateAsync()).User.IsAuthenticated())
            request.Headers.Add("Authorization", "Bearer " + await _stateProvider.GetTokenAsync());

        var result = await TrySendAsync(request);

        return await ReadResultAsync<R>(result);
    }

    public async Task<ApiResponse<R>> SendAsJsonAsync<R>(string uri, HttpMethod method,
        IEnumerable<KeyValuePair<string, string>>? headers)
    {
        var request = new HttpRequestMessage(method, uri);


        if ((await _stateProvider.GetAuthenticationStateAsync()).User.IsAuthenticated())
            request.Headers.Add("Authorization", "Bearer " + await _stateProvider.GetTokenAsync());

        if (headers is not null)
            foreach (var header in headers)
                request.Headers.Add(header.Key, header.Value);

        var result = await TrySendAsync(request);

        return await ReadResultAsync<R>(result);
    }

    public async Task<ApiResponse<R>> SendAsJsonAsync<R>(string uri, HttpMethod method)
    {
        var request = new HttpRequestMessage(method, uri);

        if ((await _stateProvider.GetAuthenticationStateAsync()).User.IsAuthenticated())
            request.Headers.Add("Authorization", "Bearer " + await _stateProvider.GetTokenAsync());

        var result = await TrySendAsync(request);

        return await ReadResultAsync<R>(result);
    }

    public async Task<ApiResponse<R>> PostImageAsync<R>(string url, HttpMethod method, IBrowserFile body)
    {
        var request = new HttpRequestMessage(method, url);
        using (var multipartFormContent = new MultipartFormDataContent())
        {
            var fileStreamContent = new StreamContent(body.OpenReadStream());
            fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(body.ContentType!);
            
            multipartFormContent.Add(fileStreamContent, name: "image", fileName: "filename");

            request.Content = multipartFormContent;
            var response = await TrySendAsync(request);
            return await ReadResultAsync<R>(response);
        }
    }

    private async Task<HttpResponseMessage> TrySendAsync(HttpRequestMessage request)
    {
        return await _client.SendAsync(request);
    }

    private async Task<ApiResponse<R>> ReadResultAsync<R>(HttpResponseMessage response)
    {
        return (await response.Content.ReadFromJsonAsync<ApiResponse<R>>())!;
    }
}