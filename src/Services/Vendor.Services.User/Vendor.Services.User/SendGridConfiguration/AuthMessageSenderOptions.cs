namespace Vendor.Services.User.SendGridConfiguration;

public class AuthMessageSenderOptions
{
    public string? SendGridKey { get; set; }
    public string? OutboundAddress { get; set; }
}