using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;

namespace Vendor.Gateways.Portal.Models;

public class RegisterProductModel
{
    [Required]
    public string Name { get; set; }
    [Required]
    public IBrowserFile Image { get; set; }
}