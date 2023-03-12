using Vendor.Domain.Types;
using Vendor.Gateways.Portal.Wrappers.ResponseTypes;

namespace Vendor.Gateways.Portal.Wrappers.HttpClientWrapper;

public interface IHttpClientWrapper
{
    public Task<ClientResponse<R>> SendAsJsonAsync<R, S>(HttpClient client, string uri, HttpMethod method, S? body,
        IEnumerable<KeyValuePair<string, string>>? headers);
    
    public Task<ClientResponse<R>> SendAsJsonAsync<R>(HttpClient client, string uri, HttpMethod method,
        IEnumerable<KeyValuePair<string, string>>? headers);
    
    public Task<ClientResponse<R>> SendAsJsonAsync<R>(HttpClient client, string uri, HttpMethod method);
}