using System.Net;

namespace Vendor.Gateways.Portal.Wrappers.ResponseTypes;

public class ClientResponse<R>
{
    public R? Result { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public string? ReasonPhrase { get; set; }
    public bool IsSuccessful { get; set; }
}