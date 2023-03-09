using System.ComponentModel.DataAnnotations;

namespace Vendor.Gateways.Portal.DTO;

public class LoginUserFormData
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}