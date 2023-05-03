using Microsoft.AspNetCore.Components.Forms;

namespace Vendor.Gateways.Portal.Models;

public class RegisterProductModel
{
    public string Name { get; set; }
    public IBrowserFile Image { get; set; }
}