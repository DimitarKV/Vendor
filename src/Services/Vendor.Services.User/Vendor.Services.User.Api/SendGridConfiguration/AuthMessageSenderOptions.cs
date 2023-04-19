namespace Vendor.Services.User.Api.SendGridConfiguration;

public class AuthMessageSenderOptions
{
    public string? SendGridKey { get; set; }
    public string? OutboundAddress { get; set; }
}