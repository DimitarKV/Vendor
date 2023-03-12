using System.Net;

namespace Vendor.Gateways.Portal.Wrappers.HttpClientWrapper;

public class WrapperResponse<R>
{
    public HttpStatusCode StatusCode { get; set; }
    public string? Reason { get; set; }
    public R? Result { get; set; }
    public bool Successful { get; set; }
}