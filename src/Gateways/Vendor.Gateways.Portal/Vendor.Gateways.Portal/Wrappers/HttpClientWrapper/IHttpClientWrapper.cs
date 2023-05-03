using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Vendor.Domain.Types;

namespace Vendor.Gateways.Portal.Wrappers.HttpClientWrapper;

public interface IHttpClientWrapper
{
    public Task<ApiResponse<R>> SendAsJsonAsync<R, S>(string uri, HttpMethod method, S? body,
        IEnumerable<KeyValuePair<string, string>>? headers);
    public Task<ApiResponse<R>> SendAsJsonAsync<R, S>(string uri, HttpMethod method, S? body);
    
    public Task<ApiResponse<R>> SendAsJsonAsync<R>(string uri, HttpMethod method,
        IEnumerable<KeyValuePair<string, string>>? headers);
    
    public Task<ApiResponse<R>> SendAsJsonAsync<R>(string uri, HttpMethod method);

    public Task<ApiResponse<R>> PostImageAsync<R>(string url, HttpMethod method, IBrowserFile body);
}