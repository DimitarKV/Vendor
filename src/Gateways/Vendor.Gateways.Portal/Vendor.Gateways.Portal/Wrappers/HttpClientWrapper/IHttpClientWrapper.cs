using Vendor.Domain.Types;

namespace Vendor.Gateways.Portal.Wrappers.HttpClientWrapper;

public interface IHttpClientWrapper
{
    public Task<ApiResponse<R>> SendAsJsonAsync<R, S>(HttpClient client, string uri, HttpMethod method, S? body,
        IEnumerable<KeyValuePair<string, string>>? headers);
    public Task<ApiResponse<R>> SendAsJsonAsync<R, S>(HttpClient client, string uri, HttpMethod method, S? body);
    
    public Task<ApiResponse<R>> SendAsJsonAsync<R>(HttpClient client, string uri, HttpMethod method,
        IEnumerable<KeyValuePair<string, string>>? headers);
    
    public Task<ApiResponse<R>> SendAsJsonAsync<R>(HttpClient client, string uri, HttpMethod method);
}