using Microsoft.AspNetCore.Components.Forms;

namespace Vendor.Gateways.Portal.DTO;

public class RegisterProductDto
{
    public string Name { get; set; }
    public IBrowserFile Image { get; set; }
}