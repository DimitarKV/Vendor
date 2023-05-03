using System.ComponentModel.DataAnnotations;

namespace Vendor.Gateways.Portal.Models;

public class LoginUserModel
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}